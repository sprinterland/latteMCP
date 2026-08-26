#!/usr/bin/env python3
"""Atomic claim/lock helper for Task Record folders (docs/project/tasks/<id>/.claim).

A role (Analyst/Developer/Reviewer/Tester/Auditor) must successfully create a task's .claim file
before acting on it, and remove it when done. Every successful claim -- via `claim`, or via any
branch of `reclaim` -- goes through the *same* single atomic-create primitive (`_try_create`), so
there is exactly one place in this script where "I won" is ever decided; no code path can report
success without having actually won that one primitive. `release` and the ABA-mismatch branch of
`reclaim` apply the same discipline in reverse: neither ever deletes or overwrites content by path
alone -- both consume it via rename, verify what they actually captured, and put back anything that
turns out not to be theirs to touch. See docs/project/tasks/README.md for the full mechanism (ships
in a later phase of the same rollout this script is the first phase of).

Usage:
  claim_lock.py claim <task-dir> --agent-id ID --role ROLE
      Atomically create <task-dir>/.claim. Fails (exit 1) if a claim already exists.

  claim_lock.py release <task-dir> --agent-id ID
      Remove <task-dir>/.claim. Fails if the claim is owned by a different agent-id (use `reclaim`
      instead in that case).

  claim_lock.py check <task-dir> --timeout-min N
      Print the current claim's contents (if any), its age, and whether it's past the given
      timeout. Never modifies anything -- used by list-blocked-tasks to surface candidates.

  claim_lock.py reclaim <task-dir> --agent-id ID --role ROLE --timeout-min N
      Take over an abandoned claim: only succeeds if the existing claim is past the timeout.
      Removes the old claim and creates a new one. The caller is responsible for appending a
      "claim-reclaimed" status.jsonl line afterward, so the takeover is visible, not silent.

Every command fails cleanly (a message on stderr, exit 1) rather than raising if <task-dir> doesn't
exist -- this script never creates it; that's the caller's job (the task folder itself must already
exist before anything claims it).
"""
import argparse
import json
import os
import random
import sys
from datetime import datetime

CLAIM_FILENAME = ".claim"


def _claim_path(task_dir: str) -> str:
    return os.path.join(task_dir, CLAIM_FILENAME)


def _now_iso() -> str:
    return datetime.now().astimezone().isoformat(timespec="seconds")


def _require_task_dir(task_dir: str) -> bool:
    if not os.path.isdir(task_dir):
        print(f"error: task directory does not exist: {task_dir}", file=sys.stderr)
        return False
    return True


def _read_claim(task_dir: str):
    path = _claim_path(task_dir)
    # Open directly and catch the failure rather than os.path.exists() then a separate open() --
    # that would leave its own gap where a concurrent rename/remove between the two calls could
    # make exists() true (or false) for a file that's no longer (or not yet) at that state by the
    # time open() actually runs.
    try:
        with open(path, "r", encoding="utf-8") as f:
            data = json.load(f)
    except FileNotFoundError:
        return None
    # A hand-edited or corrupted .claim holding valid JSON that isn't an object (a list, string,
    # number...) is treated the same as unreadable here, so every caller's existing.get(...)/
    # existing[...] access downstream can assume a dict without its own isinstance guard.
    return data if isinstance(data, dict) else None


def _age_minutes(claimed_at: str) -> float:
    claimed = datetime.fromisoformat(claimed_at)
    now = datetime.now(claimed.tzinfo) if claimed.tzinfo else datetime.now()
    return (now - claimed).total_seconds() / 60.0


def _try_create(path: str, payload: dict) -> bool:
    """Atomically create `path` with `payload`'s content if nothing is there. Returns True on
    success, False if something already exists at `path` (never raises for that reason -- a
    missing parent directory still raises FileNotFoundError, which callers guard against via
    `_require_task_dir` before ever reaching this).

    Writes the complete content to a temp file *first*, then publishes it via os.link(), which --
    unlike os.open(O_CREAT|O_EXCL) followed by a separate write() -- fails atomically with
    FileExistsError if the destination already exists, with no window in between where the
    destination could be observed to exist but not yet hold the full content. A plain
    open(O_CREAT|O_EXCL)-then-write() sequence has exactly that window: the file exists (0 bytes)
    the instant O_CREAT succeeds, and a concurrent _read_claim() landing in that gap sees an empty
    file and a JSONDecodeError instead of a clean 'no claim yet'/'claim present' result."""
    data = json.dumps(payload).encode("utf-8")
    tmp_path = path + f".tmp-{os.getpid()}-{random.randint(0, 0xffff):04x}"
    with open(tmp_path, "wb") as f:
        f.write(data)
    try:
        os.link(tmp_path, path)
        return True
    except FileExistsError:
        return False
    finally:
        os.remove(tmp_path)


def _consume(path: str, normalize: bool = True):
    """Atomically take whatever currently sits at `path` out of play: rename it to a private temp
    name, read+parse its content, remove the temp file, and return it. Raises FileNotFoundError if
    nothing was at `path`; callers decide what that means for them.

    Shared by cmd_release and cmd_reclaim, both of which need to safely capture-then-decide rather
    than reading and separately deleting/replacing by path, which would race against a concurrent
    change to the same path between the read and the delete/replace -- but the two callers want
    different things back. `normalize=True` (cmd_release's case) folds non-dict JSON -- a
    hand-edited or corrupted claim -- to `{}`, same as `_read_claim`, so a caller that only ever
    needs `.get()`/equality doesn't need its own isinstance guard. `normalize=False` (cmd_reclaim's
    ABA-mismatch restore path) returns the parsed value exactly as captured, byte-for-byte,
    because that path's job is to put back whatever it actually took -- silently folding a non-dict
    interloper to `{}` there would discard real (if corrupted) content instead of restoring it."""
    taken_path = path + f".taken-{os.getpid()}-{random.randint(0, 0xffff):04x}"
    os.rename(path, taken_path)  # raises FileNotFoundError if nothing is there; let it propagate
    with open(taken_path, "r", encoding="utf-8") as f:
        data = json.load(f)
    os.remove(taken_path)
    if normalize:
        return data if isinstance(data, dict) else {}
    return data


def _report_claimed(args) -> None:
    print(f"claimed {args.task_dir} as {args.role}/{args.agent_id}")


def _report_already_claimed(task_dir: str) -> None:
    existing = _read_claim(task_dir)
    print(f"error: already claimed: {json.dumps(existing)}", file=sys.stderr)


def cmd_claim(args) -> int:
    if not _require_task_dir(args.task_dir):
        return 1
    path = _claim_path(args.task_dir)
    payload = {"agent_id": args.agent_id, "role": args.role, "claimed_at": _now_iso()}
    if not _try_create(path, payload):
        _report_already_claimed(args.task_dir)
        return 1
    _report_claimed(args)
    return 0


def cmd_release(args) -> int:
    if not _require_task_dir(args.task_dir):
        return 1
    path = _claim_path(args.task_dir)
    existing = _read_claim(args.task_dir)
    if existing is None:
        print("error: no claim present", file=sys.stderr)
        return 1
    if existing.get("agent_id") != args.agent_id:
        print(
            f"error: claim owned by a different agent_id ({existing.get('agent_id')}); use reclaim",
            file=sys.stderr,
        )
        return 1

    # Consume whatever is currently at the path via the same atomic capture `cmd_reclaim` uses for
    # a stale takeover, rather than a plain os.remove() by path -- a bare remove would blindly
    # delete whatever's there *now*, even if a concurrent reclaim already replaced this claim with
    # a different, still-valid one between the read above and this call.
    try:
        captured = _consume(path)
    except FileNotFoundError:
        print("error: no claim present", file=sys.stderr)
        return 1
    if captured.get("agent_id") != args.agent_id:
        # A different claim replaced ours in between -- it isn't ours to discard. Put it back if
        # the slot is still open; if a third caller has since legitimately claimed it, their claim
        # is current and wins outright (never silently overwritten).
        if not _try_create(path, captured):
            print(
                "error: claim changed underneath this release, and a third caller has since "
                "claimed the slot; nothing restored (their claim is current)",
                file=sys.stderr,
            )
            return 1
        print(
            f"error: claim owned by a different agent_id ({captured.get('agent_id')}); use reclaim",
            file=sys.stderr,
        )
        return 1
    print(f"released {args.task_dir}")
    return 0


def cmd_check(args) -> int:
    if not _require_task_dir(args.task_dir):
        return 1
    existing = _read_claim(args.task_dir)
    if existing is None:
        print("no claim present")
        return 0
    claimed_at = existing.get("claimed_at")
    try:
        age = _age_minutes(claimed_at)
    except (KeyError, TypeError, ValueError) as e:
        # A malformed claim (hand-edited, or written by a future schema this version doesn't
        # know) shouldn't crash a read-only introspection command that a scan across many task
        # folders (list-blocked-tasks) depends on -- report the anomaly instead.
        print(json.dumps({**existing, "malformed": True, "error": str(e)}))
        return 0
    stale = age > args.timeout_min
    print(json.dumps({**existing, "age_minutes": round(age, 1), "stale": stale}))
    return 0


def cmd_reclaim(args) -> int:
    if not _require_task_dir(args.task_dir):
        return 1
    path = _claim_path(args.task_dir)
    payload = {"agent_id": args.agent_id, "role": args.role, "claimed_at": _now_iso()}

    # First, try a plain atomic create. If the slot is genuinely open -- nothing has ever claimed
    # it, or a *different* in-flight reclaim just vacated it a moment ago -- this wins outright
    # through the exact same primitive `claim` uses. Routing this case through the shared
    # primitive (rather than a separate unguarded "existing is None" special case) is what closes
    # the race two concurrent reclaimers could otherwise win simultaneously: each observing a
    # momentarily-empty slot and claiming it without checking whether the *other* had already done
    # the same in between.
    if _try_create(path, payload):
        _report_claimed(args)
        return 0

    existing = _read_claim(args.task_dir)
    if existing is None:
        # Something was there a moment ago (the create above failed) but is gone again now --
        # another in-flight reclaim already vacated it and hasn't recreated it yet. Don't guess at
        # the staleness of content this call never actually saw; just retry the same atomic
        # create once more, through the same primitive as every other path here.
        if _try_create(path, payload):
            _report_claimed(args)
            return 0
        print("error: claim slot is contested by another caller; retry", file=sys.stderr)
        return 1

    try:
        age = _age_minutes(existing["claimed_at"])
    except (KeyError, TypeError, ValueError) as e:
        # Same malformed-claim guard as cmd_check -- can't verify staleness of content that
        # doesn't parse as expected, so refuse cleanly rather than crash.
        print(
            f"error: existing claim has a malformed claimed_at ({e}); refusing to reclaim -- "
            "fix or remove it manually",
            file=sys.stderr,
        )
        return 1
    if age <= args.timeout_min:
        print(
            f"error: existing claim is only {age:.1f} min old (timeout {args.timeout_min}); refusing to reclaim",
            file=sys.stderr,
        )
        return 1

    # existing is genuinely stale -- atomically consume it via the same capture primitive
    # cmd_release uses (only one concurrent caller's rename of the same source path can ever
    # succeed; every other caller's rename fails immediately with FileNotFoundError, unlike a
    # remove-then-create sequence, which has a window where a second reclaimer can remove a
    # *first* reclaimer's brand-new claim).
    #
    # Renaming alone isn't sufficient, though: it succeeds unconditionally on *whatever* is
    # currently at the path, not specifically the stale claim just validated above. A
    # late-arriving reclaimer that read `existing` before a *different* reclaimer already won and
    # replaced it could otherwise rename away that other reclaimer's fresh, valid claim -- an ABA
    # problem, not merely a lost race. So: after consuming, verify the content actually captured
    # is byte-identical to what was validated as stale; if a different claim beat this call to the
    # path in between, put it back and abort instead of proceeding.
    try:
        captured = _consume(path, normalize=False)
    except FileNotFoundError:
        print(
            "error: claim was taken by another caller before this reclaim completed; retry",
            file=sys.stderr,
        )
        return 1
    if captured != existing:
        # Renamed away someone else's already-fresh claim, not the stale one validated above.
        # Put it back through the same atomic-create primitive rather than a blind os.rename() --
        # a blind rename unconditionally replaces whatever is *currently* at the path, which would
        # silently clobber a third caller's own legitimate claim if one was created in the window
        # this rename just opened. _try_create only succeeds if the slot is still genuinely empty;
        # if a third caller beat this restore to it, their claim is current and stays untouched.
        if not _try_create(path, captured):
            print(
                "error: claim changed underneath this reclaim, and a third caller has since "
                "claimed the slot; nothing restored (their claim is current), retry",
                file=sys.stderr,
            )
            return 1
        print(
            "error: claim changed underneath this reclaim (a different claim beat it to the "
            "slot); restored the other claim untouched, retry",
            file=sys.stderr,
        )
        return 1

    # The slot is now genuinely empty and this call is the only one that has proven it consumed
    # the specific stale claim it validated -- but an *ordinary* `claim` (or another reclaim's
    # retry-on-None branch above) could still win the now-open slot first. Race for it through the
    # same shared primitive as every other path; losing here is exactly as valid an outcome as
    # losing any other `claim` race, not an error in this script.
    if not _try_create(path, payload):
        _report_already_claimed(args.task_dir)
        return 1
    _report_claimed(args)
    return 0


def main() -> int:
    parser = argparse.ArgumentParser(description="Atomic claim/lock helper for Task Record folders.")
    sub = parser.add_subparsers(dest="command", required=True)

    p_claim = sub.add_parser("claim")
    p_claim.add_argument("task_dir")
    p_claim.add_argument("--agent-id", required=True)
    p_claim.add_argument("--role", required=True)
    p_claim.set_defaults(func=cmd_claim)

    p_release = sub.add_parser("release")
    p_release.add_argument("task_dir")
    p_release.add_argument("--agent-id", required=True)
    p_release.set_defaults(func=cmd_release)

    p_check = sub.add_parser("check")
    p_check.add_argument("task_dir")
    p_check.add_argument("--timeout-min", type=float, default=30)
    p_check.set_defaults(func=cmd_check)

    p_reclaim = sub.add_parser("reclaim")
    p_reclaim.add_argument("task_dir")
    p_reclaim.add_argument("--agent-id", required=True)
    p_reclaim.add_argument("--role", required=True)
    p_reclaim.add_argument("--timeout-min", type=float, default=30)
    p_reclaim.set_defaults(func=cmd_reclaim)

    args = parser.parse_args()
    return args.func(args)


if __name__ == "__main__":
    raise SystemExit(main())
