---
name: developer-work-task
description: Implement an approved Task Record's plan.md on its own git branch, the Developer step of the Analyst/Developer/Reviewer/Tester/Auditor pipeline. Use to "implement TASK-...", "work this task", "run the Developer on this task", or when a task's status.jsonl is "approved" and needs implementation before Reviewer can merge it. Never merges or pushes — that's Reviewer's exclusive act.
---

# Developer — Implement an Approved Task Record

Claims an approved Task Record, implements its `plan.md` on a dedicated `task/TASK-<id>-<slug>`
branch, and writes `dev-notes.md` before handing off to Reviewer. **Re-read `docs/project/tasks/
README.md`'s "Git isolation" and "Precise artifacts" sections and `docs/dev-practices.md` live
before acting** — both are the single source of truth this skill defers to; never freeze a
remembered copy of either.

**This skill never commits anything to `main`**, matching `docs/project/tasks/README.md`'s git-
isolation rule to the letter: "Developer works only on that branch." Every `status.jsonl` append,
every `task_log.md` edit, and `dev-notes.md` itself all land on the task branch and are committed
there — they only reach `main` once Reviewer squash-merges. The one exception is `.claim`, which is
gitignored and untracked, so claiming/releasing it never needs a commit and is unaffected by which
branch happens to be checked out. A consequence, not a bug: `main`'s own copy of `task_log.md`/
`status.jsonl` for a task Developer is actively working stays frozen at whatever Analyst last wrote
(`approved`) until Reviewer's eventual merge — Reviewer's own task-aware step is designed to check
out the branch directly (per `docs/project/tasks/README.md`), not discover work via a `main`-side
scan, so this is expected, not a gap.

Every `status.jsonl` line this skill appends carries the full schema per `docs/project/tasks/
README.md`'s "Precise artifacts" section: `ts`, `role`, `event`, `status`, and (where noted)
`notes`.

## Procedure

1. **Generate this run's agent id.** `.claude/skills/_lib/append_jsonl.py --gen-id session` (no
   file I/O — just prints an id). Reuse the same value for every claim/release below; never
   regenerate mid-run.
2. **Identify the task directory** `docs/project/tasks/TASK-<id>-<slug>/` — from the id/slug given,
   or, if none was given, the first `main` `task_log.md` row whose Owner-role is `—` and whose
   Status is `approved`. **Scope limit, by design:** under strict git isolation, `main`'s
   `task_log.md` can never show `changes-requested` or an in-progress `in-development` row for a
   task that already has a branch — those transitions live only on the branch until merge. Auto-pick
   therefore only ever discovers genuinely fresh, never-branched tasks; resuming a task that already
   has a branch (an interrupted run, or one Reviewer bounced back) requires the caller to name its
   id explicitly.
3. **Claim, then read status from the right place, then check eligibility.**
   1. `.claude/skills/_lib/claim_lock.py claim <task-dir> --agent-id <session-id> --role developer`.
      A failure here means one of two different things — check the command's own error text before
      picking a response: `error: task directory does not exist: <path>` means a bad id/slug was
      given (from step 2's explicit-id path only; auto-pick never produces a nonexistent path) —
      report that directly and stop, this isn't a claim collision and the recovery policy below
      doesn't apply. Anything else (a live claim already exists):
      - `.claude/skills/_lib/claim_lock.py check <task-dir>` to see whether it's past the timeout.
      - **Past timeout:** `.claude/skills/_lib/claim_lock.py reclaim <task-dir> --agent-id
        <session-id> --role developer`. If `reclaim` itself fails (e.g. a third session reclaimed or
        claimed first in the window since `check` ran), treat it exactly like the "not past timeout"
        case just below — report and apply the recovery policy, don't proceed as if it had
        succeeded. On success, don't append the `claim-reclaimed` line yet — the correct
        `status.jsonl` copy to write it to (`main`'s, or the task branch's) isn't known until step
        3.2 below determines which one is authoritative, and 3.2 owns logging it, unconditionally,
        once that's known. Continue to 3.2.
      - **Not past timeout** (a genuinely live claim): report its contents, then apply the recovery
        policy below.
   2. Check whether `task/TASK-<id>-<slug>` already exists as a branch:
      - **No branch yet:** read status from `main`'s `status.jsonl`. Eligible only if the latest
        line's `status` is `approved`. This is always a fresh start (step 5) — if 3.1 reclaimed a
        stale claim to get here, fold a `claim-reclaimed` line (`status: "approved"`, unchanged) into
        step 5's fresh-start commit, right before its `dev-started` line, both on the new branch.
        (There's nowhere to log it before the branch exists — this skill never commits to `main`.)
      - **Branch already exists:** check it out, and read status from *its own* `status.jsonl`
        instead of `main`'s (which is frozen and no longer authoritative for this task). Eligible if
        the latest line's `status` is `approved` (the branch exists but no Developer line was ever
        committed to it — a prior run's claim died between branch-creation and its first commit),
        `changes-requested` (Reviewer's revision handoff), or `in-development` with
        `role: "developer"` on that line (this role's own genuinely interrupted work). Whichever of
        those three it is, if 3.1 reclaimed a stale claim to get here, append the `claim-reclaimed`
        line (`status:` whatever value was just read as eligible) right now, in its own small commit
        on this branch (e.g. `git add -A && git commit -m "TASK-<id>: claim reclaimed by developer"`) — this is
        the one and only place that commit happens for this sub-case; step 5 below never repeats it.
        Then proceed to step 5, which handles all three eligible sub-cases on their own terms
        (including the `approved`-with-no-Developer-commit-yet one, which step 5's fresh-start
        bullet treats as equivalent to a never-branched task, minus creating the branch again).
   3. Any other status found in either case above — `new`, `planning`, `awaiting-approval`,
      `in-review`, `ready-for-review`, `merged`, …, or an `in-development`/`changes-requested` line
      logged for a different role's own still-live work — means this task isn't ready for Developer,
      or belongs to someone else's step right now: release the claim (switch back to whatever branch
      this run started on first, if step 3.2 above checked out a task branch to read its status),
      report the actual status, then apply the recovery policy below.

   **Recovery policy** for both the claim failure above and the ineligibility case just above: if
   the caller named a specific task id, stop and report immediately — they asked for that task, not
   a substitute. If step 2 auto-picked, retry step 2's scan for the next eligible task, **excluding
   every task_dir already tried this run** (keep an in-memory list; don't just re-run the scan
   verbatim). This exclusion is required, not optional, specifically because `main`'s `task_log.md`
   never reflects an active claim or in-progress branch under this skill's design (see the file's
   opening note) — without it, a collision on an already-claimed task would look identical to a scan
   that had never tried it, and retrying would re-select the exact same task indefinitely instead of
   moving on.
4. **Refuse anything not eligible per step 3.** Step 3 already enforces this; this line just states
   the plan's own requirement explicitly — this skill only ever implements a task whose plan was
   actually approved, is a legitimate revision handoff from Reviewer, or is this role's own
   genuinely resumable prior work.
5. **Set up the branch and log the transition — entirely on the branch, never on `main`.**
   - **Fresh start** (no branch existed yet, or one existed but never got a Developer commit): if the
     branch doesn't exist yet, create `task/TASK-<id>-<slug>` from `main`'s current tip and check it
     out (if it already exists from a dead prior claim, it's already checked out from step 3.2). If
     3.1 reclaimed a stale claim to get here (only possible in the no-branch-yet case — step 3.2
     already logged it on the spot for the branch-already-exists case), append a `claim-reclaimed`
     line (`status: "approved"`, unchanged) first. Then append `status.jsonl` (`role: "developer"`,
     `event: "dev-started"`, `status: "in-development"`) and update `task_log.md`'s Status to
     `in-development` and Owner-role to `developer`, then stage and commit all of it together on the
     branch (e.g. `git add -A && git commit -m "TASK-<id>: developer started"`).
   - **Resuming genuinely interrupted `in-development` work:** already on the right branch from step
     3.2, with its own `dev-started` line already committed — don't re-append or re-commit it. Pick
     back up on the existing (possibly partial) implementation.
   - **Resuming from `changes-requested`:** already on the right branch from step 3.2. Append
     `status.jsonl` (`role: "developer"`, `event: "revision-started"`, `status: "in-development"`)
     and update `task_log.md`'s Status to `in-development` and Owner-role to `developer` (Reviewer's
     own review left Owner-role set to whatever it holds it as during review — reset it here just
     as the fresh-start bullet above does, so it doesn't read stale for the whole revision window),
     then stage and commit both on the branch (e.g. `git add -A && git commit -m "TASK-<id>:
     developer revising per review/round-N.md"`). Read the most recent
     `review/round-N.md` for what needs fixing (step 6 covers `ask.md`/`plan.md`; this is the
     additional input specific to a revision cycle).
6. **Read `ask.md` and `plan.md`.** For a major task, also its recorded approval sign-off; for
   minor, the collapsed classification + one-paragraph approach. Treat `plan.md` as the spec for
   this task — don't improvise beyond its stated scope/acceptance criteria without going through
   step 9's ask-or-defer branch below first.
7. **Implement, following the existing Track B contract** (`CLAUDE.md` Workflow Rules 6/7/9/14) and
   `docs/dev-practices.md`'s Test-Writing Timing / Automated Tests Gate `Confirmed` / Local
   Verification settings:
   - Check each touched module/operation's doc status first (Track B step 1). If it's missing,
     still `Draft`, or contradicts the code, that's a Track B step 2 recommendation, not a block —
     proceeding anyway is this role's own call to make (Workflow Rule 4: small, reversible, record
     the choice and mention it in `dev-notes.md`), *unless* the specific gap found is itself
     ambiguous, hard-to-reverse, or touches architecture/security/business rules, in which case
     Workflow Rule 2 applies and it's worth surfacing to the user before proceeding. **No answer
     available right now**, for a Rule-2-worthy gap on an async run with nobody to ask, is the same
     kind of bailout as step 9 below: commit whatever partial work exists on the branch, then follow
     step 9's own release procedure and stop.
   - Any newly added or materially changed code gets documented to at least `Draft (written
     alongside code)` before this task is done (Track B step 3) — non-negotiable regardless of the
     answer above.
   - If a doc actively contradicts the exact area just changed, fix the doc as part of this task
     (Track B step 4) and log the change in `docs/project/CHANGELOG.md` (Workflow Rule 7).
   - If the user declines to resolve a flagged gap from the first bullet, log it in
     `docs/discovery/debt_log.md` and bump its `discovery_plan.md` priority (Track B step 5) instead
     of silently dropping it.
   - Write or verify automated tests per `dev-practices.md`'s Test-Writing Timing setting
     (test-after backfill tracked in `plan.md`, TDD written before the implementation, or
     test-alongside with no "pending" state left open). Workflow Rule 14 gives this setting an
     explicit fallback if `dev-practices.md` is absent or still unfilled: test-after, manual
     verification sufficient. Apply that default rather than stopping — it's the framework's own
     documented behavior for this state, not a silent guess.
   - Similarly, Local Verification defaults to "no fixed local-run requirement" (Rule 14) if unset —
     apply that default (manual smoke verification suffices) rather than stopping; otherwise follow
     whatever `dev-practices.md` actually specifies.
   - Stage and commit implementation work to the branch as it progresses (`git add -A && git commit
     -m ...` — plain `git commit -am` never stages a newly created file, so a new file added this
     task would otherwise be silently left out) — Reviewer's `diff.patch` is scoped to this branch's
     full history against `main`, so nothing material should be left uncommitted when this skill
     finishes.
8. **Write `dev-notes.md`** — what was done, and every material deviation from `plan.md`
   cross-referenced to the specific section it departs from (per `docs/project/tasks/README.md`'s
   file-layout description — this cross-referencing is what lets Auditor later verify plan-drift
   without re-reading the whole diff). Minor tasks: short. Major tasks: full, covering every
   acceptance criterion in `plan.md`. Commit it on the branch along with the implementation.
9. **Blocked mid-implementation?** If something in `plan.md` turns out ambiguous, contradicts
   `ask.md`, or surfaces a decision that's itself ambiguous, hard-to-reverse, or touches
   architecture/security/business rules (Workflow Rule 2) — ask the user. **No answer available
   right now** is a legitimate outcome: set `task_log.md`'s Owner-role back to `—` (the claim is
   about to be released — an unclaimed row must never still show `developer`, the same invariant
   this skill's every other release point keeps), stage and commit that plus whatever partial work
   exists on the branch, release the claim (`.claude/skills/_lib/claim_lock.py release <task-dir>
   --agent-id <session-id>` — `.claim` is gitignored and untracked, so this itself needs no commit),
   switch back to `main` (so this working directory is left in the state every non-branching
   pipeline skill assumes — see step 12), and stop. `status.jsonl` stays at `in-development`
   (already committed in step 5) so a later invocation resumes via step 2/3, naming this task's id,
   checking out this same branch again and picking up where it left off.
10. **Log completion.** Finish and commit all implementation work on the branch. Append
    `status.jsonl` (`event: "dev-complete"`, `status: "ready-for-review"`, `notes: "implemented per
    plan.md; see dev-notes.md"` or a more specific equivalent) and update `task_log.md`'s Status to
    `ready-for-review` and Owner-role back to `—`, then stage and commit all of it together on the
    branch (e.g. `git add -A && git commit -m "TASK-<id>: ready for review"`).
11. **Release.** `.claude/skills/_lib/claim_lock.py release <task-dir> --agent-id <session-id>` —
    `.claim` is gitignored and untracked, so this needs no commit and works regardless of which
    branch is currently checked out.
12. **Switch back to `main`.** File-task, analyst-plan-task, and any other non-branching pipeline
    skill read `task_log.md`/`status.jsonl` assuming `main` is checked out; leaving the task branch
    checked out after this skill finishes would make those skills silently read the branch's own
    (still-`ready-for-review`-only-on-the-branch) copies instead. Reviewer's task-aware step checks
    out the branch itself when it actually starts reviewing (per `docs/project/tasks/README.md`), so
    it doesn't depend on this skill having left it checked out.
13. **Never merge or push.** That's Reviewer's exclusive act (`push-review-gate`'s task-aware step,
    once it lands in a later framework sync; until then, hand this branch to Reviewer by hand,
    following `docs/project/tasks/README.md`'s git-isolation section).
14. **Report** the task id and the branch name (`task/TASK-<id>-<slug>`) as ready for Reviewer — note
    explicitly that `main`'s own `task_log.md`/`status.jsonl` for this task still read `approved`
    and will keep doing so until Reviewer merges; that's expected, not a sign anything went wrong.
