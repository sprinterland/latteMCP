---
name: analyst-plan-task
description: Classify a filed Task Record as minor or major and draft its plan.md, the Analyst step of the Analyst/Developer/Reviewer/Tester/Auditor pipeline. Use to "plan TASK-...", "classify this task", "run the Analyst on this task", or when a task's status.jsonl is still "new" and needs a plan before Developer can start. Never implements anything itself.
---

# Analyst — Classify and Plan a Task Record

Claims a filed Task Record, classifies it minor/major against `dev-practices.md`'s live criteria,
drafts `plan.md` per the ceremony table, and gets it approved before Developer can start. **Re-read
`docs/project/tasks/README.md` and `docs/dev-practices.md`'s "Task Classification (Analyst)"
section live before acting** — both are the single source of truth this skill defers to; never
freeze a remembered copy of either.

Every `status.jsonl` line this skill appends carries the full schema per
`docs/project/tasks/README.md`'s "Precise artifacts" section: `ts`, `role`, `event`, `status`, and
(where noted) `notes` — never omit `ts`, even though only `role`/`event`/`status`/`notes` are called
out by name below. `task_log.md`'s Status/Owner-role columns are kept in sync at every step below,
not just at the end, so a concurrent run scanning it never sees a stale "unclaimed" row for a task
this skill is actively holding.

## Procedure

1. **Generate this run's agent id.** `.claude/skills/_lib/append_jsonl.py --gen-id session` (no
   file I/O — just prints an id). Reuse the same value for every claim/release below; never
   regenerate mid-run.
2. **Identify the task directory** `docs/project/tasks/TASK-<id>-<slug>/` — from the id/slug given,
   or, if none was given, the first `task_log.md` row whose Owner-role is `—` (unclaimed) and whose
   Status is `new`, `planning`, or `awaiting-approval`, **or** whose Status is `blocked` and whose
   `status.jsonl` latest line has `role: "analyst"` (this role's own rejected plan, not another
   role's blocker — `blocked` is a pipeline-wide status per `docs/project/tasks/README.md`'s status
   enum, not Analyst-specific; a task another role blocked on isn't this skill's to touch).
3. **Claim, sync, then check eligibility** — claiming is the atomic step that actually serializes
   concurrent runs, so it must come before any decision is made on what's read, not after. Both race
   conditions below (3.1 and 3.3) get the **same recovery policy**: if the caller named a specific
   task id, stop and report immediately — they asked for that task, not a substitute. If step 2
   auto-picked (no id given), skip this task and retry step 2's scan for the next eligible one
   instead of ending the whole run on the first collision.
   1. `.claude/skills/_lib/claim_lock.py claim <task-dir> --agent-id <session-id> --role analyst`.
      On failure (a live claim already exists), report the existing claim's contents from the
      command's own error output, then apply the recovery policy above.
   2. Now that the claim is held, set `task_log.md`'s Owner-role for this task to `analyst`
      immediately (Status is updated separately at each step below as the run progresses) — this is
      what keeps the index accurate for the full duration the claim is held, not just at the end.
   3. Read `status.jsonl` and take the latest line's `status` and `role`. Proceed only if `status`
      is `new`, `planning`, or `awaiting-approval`, **or** `status` is `blocked` with `role:
      "analyst"` on that same line (this role's own rejected plan). Any other case — `approved`,
      `in-development`, `merged`, …, or a `blocked` line logged by a different role — means the task
      is already past this step or owned by someone else's blocker, not this skill's to touch:
      release the claim, set `task_log.md`'s Owner-role back to `—`, report the actual status (and
      the blocking role, if that's why), then apply the same recovery policy above.
4. **Log the start** — only if the latest status was `new` or `blocked` (first or re- entry into
   planning). Append `status.jsonl` (`role: "analyst"`, `event: "planning-started"`,
   `status: "planning"`) via `.claude/skills/_lib/append_jsonl.py <task-dir>/status.jsonl --stdin`
   (pipe the JSON in — heredoc or a scratch file, never an interpolated `--json '<...>'` argument,
   since apostrophes in prose routinely break single-quoting), and update `task_log.md`'s Status to
   `planning` at the same time. Skip this step entirely if the latest status was already `planning`
   or `awaiting-approval` — the task is already there.
5. **Resuming from `awaiting-approval`?** A `plan.md` already exists and is fully drafted — skip
   straight to step 9, re-reading it fresh rather than reclassifying or redrafting (which would
   silently discard it). Otherwise (`new`, `blocked`, or `planning`), continue to step 6.
6. **Read `ask.md` and classify.** First confirm `docs/dev-practices.md`'s "Task Classification
   (Analyst)" section is actually filled in — if its `Selected:` line is still the unfilled
   placeholder, the project hasn't adopted this section yet per that doc's own precondition: release
   the claim, set `task_log.md`'s Owner-role back to `—`, tell the user it needs to be filled in
   first, and stop. Otherwise, check `ask.md` live against all four criteria. All four must hold for
   **minor**; any doubt on any one defaults to **major**. Write the result to `task_log.md`'s
   Classification column immediately — don't defer this to step 12, since step 7's major "not right
   now" bail-out stops before reaching it, and classification work already done shouldn't be lost if
   that bail-out fires.
7. **Draft `plan.md`** per `docs/project/tasks/README.md`'s ceremony table:
   - **Minor**: classification line + one-paragraph approach only — no acceptance-criteria section.
   - **Major**: first ask the user explicitly whether to proceed with drafting a full plan at all —
     a "not right now" answer here is not a rejection of the plan itself, so **release the claim**
     (and set `task_log.md`'s Owner-role back to `—`) and stop without writing `plan.md` or
     appending anything further, leaving `status.jsonl` at `planning` (already logged in step 4) so
     this task can be resumed later exactly where it left off — step 2/3 will find and re-claim it.
     Once told to proceed, draft the full plan (classification, scope, approach, files touched,
     acceptance criteria).
8. **Log the plan as drafted.** (Reached only via step 7 — step 5's resume path skips directly to
   step 9 instead, since this was already logged on the prior run.) Append `status.jsonl`
   (`event: "plan-drafted"`, `status: "awaiting-approval"`) and update `task_log.md`'s Status to
   `awaiting-approval` — this is the state a scan for "tasks waiting on a human decision" filters
   on, distinct from still-drafting `planning`.
9. **Get the decision and present it.**
   - **Minor**: present `ask.md`'s content together with `plan.md` as a single combined yes/no.
   - **Major**: present the full `plan.md` and get an explicit approve/reject decision.
   Either way, record the decision **verbatim** (the user's own words, not a paraphrase) in
   `plan.md` as the sign-off, whichever way it goes.

   **No decision available right now** (the human wants to defer, or this is an unattended/async
   pass with no one to answer yet) is a third, equally legitimate outcome here, the same as step 7's
   major "not right now" branch: **release the claim**, set `task_log.md`'s Owner-role back to `—`,
   and stop — leave `status.jsonl` at `awaiting-approval` (already logged in step 8) so a later
   invocation resumes cleanly via step 2/3 → step 5's resume path, rather than holding the claim
   indefinitely across an unbounded wait.
10. **Record the outcome.**
    - Approved: finalize `plan.md`, append `status.jsonl` (`event: "plan-approved"`,
      `status: "approved"`).
    - Rejected: append `status.jsonl` (`event: "plan-rejected"`, `status: "blocked"`,
      `notes: "<verbatim reason>"`). Don't guess at a revised plan unprompted — a later re-planning
      pass (step 2/3's `blocked` branch) is what re-enters this skill once the blocker is addressed.
11. **Release.** `.claude/skills/_lib/claim_lock.py release <task-dir> --agent-id <session-id>` —
    before touching `task_log.md`'s Owner-role, matching every other release point in this skill
    (steps 7 and 9): the claim must actually be gone before the index says so, never the reverse,
    or a concurrent scan can see a row marked unclaimed that `claim_lock.py` would still refuse to
    claim.
12. **Update `task_log.md`.** Set the task's row: Status (the `status.jsonl` value just appended),
    Owner-role `—`. Classification was already written in step 6.
13. **Report** the task id, classification, and outcome. If approved, note it's ready for Developer
    (`developer-work-task`).
