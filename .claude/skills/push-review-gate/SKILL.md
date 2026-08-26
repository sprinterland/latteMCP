---
name: push-review-gate
description: Run this project's pre-push secondary-review gate (CLAUDE.md Rules 15/16 — Project Reviewer + Framework Reviewer) and log the run to docs/project/review_log.md. Use before any git push, when asked to "run the review gate", "review before pushing", "run Rule 15/16", or "log the push review". Does not push by itself.
---

# Push Review Gate

Runs this project's two-reviewer gate before a `git push` and logs it. **Do not treat the steps
below as the rule itself** — `CLAUDE.md` Rules 15/16 and `docs/dev-practices.md`'s "Secondary
Review Before Push" section are the source of truth and have changed shape before; re-read them
live every time this skill runs rather than trusting this summary.

## Procedure

1. **Check the gate is active.** Read `docs/dev-practices.md`'s "Secondary Review Before Push"
   section. If it says no dedicated gate runs, stop and say so — this skill is then a no-op.
2. **Scope the push.** Run `git status` and `git diff <upstream>...HEAD` (or against the relevant
   base branch) to get the full list of files and the diff this push would carry.
3. **Read the current rules live.** Re-read `CLAUDE.md` Rules 15 and 16 in full — do not rely on a
   remembered list of which paths trigger Rule 16; that list has changed shape before and this step
   exists specifically so a stale copy of it never substitutes for the real rule text.
4. **Classify.** Per Rule 15/16's current text: Rule 15 covers everything Rule 16 doesn't claim.
   Does the diff from step 2 touch any path Rule 16 currently names? Note yes/no and why.
5. **Run the Project Reviewer.** Invoke `/code-review high` scoped per Rule 15.
6. **Run the Framework Reviewer, if step 4 was yes.** First check Rule 16's diff-verification
   substitution — confirm both: (a) the `sync-framework-updates` merge commit(s) are part of this
   push (`git log <this project's own remote>..HEAD` includes them, not an older already-pushed
   sync), and (b) the shared `_frw` clone commit to diff against is the one
   `docs/framework-maintenance.md`'s "Bootstrapped from / last synced at" line was just updated to
   cite. If both hold, `diff` each touched file against its counterpart in the `_frw` clone at that
   commit — every line must match exactly *except* lines that are always project-filled-in and can
   never match `_frw`'s generic template (the "Bootstrapped from" line itself,
   `docs/dev-practices.md`'s `Selected:` values, and any other explicit fill-in-the-blank
   placeholder), which are excluded from the comparison by definition. If everything else matches,
   that stands in for the full review (log it as such, don't also run `/code-review high`).
   Otherwise, either reuse step 5's invocation
   (re-read through Rule 16's fidelity / ambiguity / enhancement-opportunity lens) when the two
   scopes substantially overlap, or run a second `/code-review high` scoped to the Rule-16 paths —
   follow Rule 16's own text for which applies, and default to the full pass whenever it's unclear
   the diff is genuinely byte-identical. If step 4 was no, the sub-entry is "not run: push touched
   no framework paths."
7. **Resolve every finding.** Fix it, or explicitly ask the user (per Rule 2 — a finding that's
   itself ambiguous, hard-to-reverse, or touches security/architecture/business rules is never
   just logged and pushed past) and log a deliberate deferral (e.g. in `PLAN.md` or
   `docs/project/completed_plan.md`) if they decline to fix it now. If fixing a finding requires
   re-running review, cap it at 2 rounds on the same push — announce the round count out loud at
   every invocation past the first ("this is round N since the last check-in"), the same
   discipline as the Maintenance rule's Framework-propagation step 4. A 3rd round due means stop
   and offer the user that same step's check-in menu instead of continuing silently; if they choose
   to keep fixing, the same 2-round cap re-arms before the next check-in — never run more than 2
   rounds without asking. If a round flags a claim/pattern as duplicated elsewhere, `grep` the
   repo for every occurrence before the next round rather than letting review rediscover files one
   at a time.

   Persist the round-since-check-in count the same way the Maintenance rule's Framework-propagation
   step 4 does (see `docs/framework-maintenance.md`) — a small scratch state file (e.g.
   `_tmp/review_checkin_state.json`, or this project's own scratch convention), anchored to this
   project's own repo root instead of `_frw`'s, with the same scope-matched-existence-means-resume /
   announce-before-the-round-starts / increment-after-verified-clean / reset-on-keep-fixing /
   delete-on-loop-end lifecycle (deleted as part of step 10 below). This is what makes it safe to
   clear conversation context between rounds (right after a round's fixes are verified clean, never
   mid-fix), not just at larger push/phase boundaries.
8. **Log any enhancement suggestion** from step 6's lens (c) using the `log-change-request` skill
   — record the resulting `change_requests.jsonl` id(s) (or "none") for step 10.
9. **Resolve the framework version.** Read `docs/framework-maintenance.md`'s "Versioning" section
   for how; read `_frw`'s `VERSION` live from the shared clone, falling back to this project's own
   static "bootstrapped from" line if the clone isn't reachable.
10. **Append the log entry.** Read the most recent entries in `docs/project/review_log.md` to match
    its exact current format, then append a new entry with the same shape: a heading
    (`## <timestamp> — commit <sha> (<short description>)`), a `### Project Reviewer` sub-entry and
    a `### Framework Reviewer` sub-entry (always both, per that file's own stated invariant), each
    recording: command + scope, repo/commit, module(s) touched (or "n/a — framework-level"),
    framework version, and outcome in prose (what was found, what was fixed, what was deferred —
    not a strict tally). The Framework Reviewer sub-entry also cites the change-request id(s) from
    step 8. If step 6's diff-verification substitution applied, say so explicitly instead of citing
    a `/code-review` command (e.g. "substituted diff-verification for a byte-identical sync merge
    against commit `<sha>`, no findings possible by construction"). If step 7 used the persisted
    round-check-in counter, delete its scratch state file now — the loop it belonged to is over.
11. **Stop here.** This skill logs and gates; it does not run `git push` itself.
