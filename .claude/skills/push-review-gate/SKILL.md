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
6. **Run the Framework Reviewer, if step 4 was yes.** Either reuse step 5's invocation (re-read
   through Rule 16's fidelity / ambiguity / enhancement-opportunity lens) when the two scopes
   substantially overlap, or run a second `/code-review high` scoped to the Rule-16 paths — follow
   Rule 16's own text for which applies. If step 4 was no, the sub-entry is "not run: push touched
   no framework paths."
7. **Resolve every finding.** Fix it, or explicitly ask the user (per Rule 2 — a finding that's
   itself ambiguous, hard-to-reverse, or touches security/architecture/business rules is never
   just logged and pushed past) and log a deliberate deferral (e.g. in `PLAN.md` or
   `docs/project/completed_plan.md`) if they decline to fix it now. If fixing a finding requires
   re-running review, cap it at 2 rounds on the same push — a 3rd round due means stop and ask the
   user how to proceed instead of continuing silently (same cap as the Maintenance rule's
   Framework-propagation step 4, `docs/framework-maintenance.md`); if they choose to keep fixing,
   the same 2-round cap applies again before the next check-in — never run more than 2 rounds
   without asking. If a round flags a claim/pattern as duplicated elsewhere, `grep` the repo for
   every occurrence before the next
   round rather than letting review rediscover files one at a time.
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
    step 8.
11. **Stop here.** This skill logs and gates; it does not run `git push` itself.
