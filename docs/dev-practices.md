# Development Practices

Configurable process decisions for how this project writes and verifies code — the process
counterpart to `api-conventions.md` (which fixes API *contract* conventions). `CLAUDE.md`
Workflow Rules 9, 14, 15, and 16 read the settings below. Changing a setting here is a project
process decision like any other — confirm it per Workflow Rule 2, update this file, then follow
it — it does **not** require touching `CLAUDE.md` itself or the shared `_frw/` (see
`_frw/docs/dev-practices.md`, in the external framework clone/repo described in
`docs/framework-maintenance.md`, for the full menu this project's choices were drawn from, kept
generic for future projects).

## Test-Writing Timing

**Selected: TDD / test-first** (adopted 2026-08-23)

Write the failing automated test from the relevant `test-spec.md` entry before writing the
implementation that makes it pass. A task is not done until: the test exists, it initially fails
for the right reason (not a compile error), and the implementation makes it pass.

Other options (not selected, documented for reference — see `_frw/docs/dev-practices.md` for full
descriptions):
- Test-after — implement first, backfill automated tests afterward as a tracked follow-up.
- Test-alongside — implementation and its tests written in the same task, no strict ordering.

## Automated Tests Gate `Confirmed` Status

**Selected: Yes, stricter** (adopted 2026-08-23)

A module/operation cannot reach `Status: Confirmed` (in its own `requirements.md`/`test-spec.md`
and in `00-index.md`) until its `test-spec.md` entries have real, passing automated tests backing
them. This supersedes the "manual verification alone is enough" default described in `CLAUDE.md`
Workflow Rule 9's base text — see that rule's note pointing here.

Other option (not selected): No — manual verification is enough for `Confirmed`; automated tests
can lag as a tracked follow-up. This was this project's actual practice before this file existed
— see the transition note below.

## Local Verification After Implementation

**Selected: Yes, whenever tests exist for the touched area** (adopted 2026-08-23)

Run the build and the relevant automated test suite locally before considering an implementation
task done, whenever automated tests exist for the code touched. Manual smoke testing (e.g. via
`.http` files or `curl` against a running instance) remains useful in addition for end-to-end
sanity checks, but no longer substitutes for the automated run where tests exist.

Other option (not selected): No fixed requirement — manual smoke testing alone is acceptable,
without also running an automated suite.

## Secondary Review Before Push

**Selected: Yes — two-reviewer gate before every push** (adopted 2026-08-23; split into two
reviewers 2026-08-23)

Before any `git push`, two independent reviewer passes run — independent of the authoring work
already done, and of each other:

1. **Project Reviewer** (`CLAUDE.md` Rule 15) — `/code-review high` scoped to everything the push
   touches except what the Framework Reviewer claims (see Rule 15 for the exact, exhaustive-by-
   construction scope — it's "everything else," not a fixed allowlist, so nothing new added to
   `docs/` can fall between the two reviewers). Always runs.
2. **Framework Reviewer** (`CLAUDE.md` Rule 16) — `/code-review high` scoped to the paths Rule 16
   names (not restated here, to avoid a second copy that can drift out of sync), checking
   framework/template fidelity, ambiguity, and noting enhancement opportunities (proposals only —
   never applied without asking first, per `docs/framework-maintenance.md`). May reuse the
   Project Reviewer's same invocation when the two scopes substantially overlap in one push. Runs
   only when the push touches those paths; otherwise its `review_log.md` sub-entry is still
   present, stating "not run" and why.

Findings must be fixed, or explicitly acknowledged and logged as a deliberate deferral (e.g. in
the relevant `../PLAN.md`/`_project/completed_plan.md` entry), before the push proceeds. Both
runs are logged in `docs/_project/review_log.md` — command, repo/commit, module(s) touched, and
the `_frw/VERSION` value at run time (format in `docs/framework-maintenance.md`'s "Versioning"
section) — for later investigation. Applies to every push, on any branch — this project has no
separate "protected branch" concept to carve out an exemption for.

Self-enforced per `CLAUDE.md` Workflow Rules 15–16 — no technical block (e.g. no pre-push git
hook). Consistent with this project's existing recommend-don't-block pattern (Track B rule 2 in
`CLAUDE.md`): the discipline is procedural, not mechanically enforced.

Other option (not selected): No dedicated secondary review before push — rely on whatever review
happens at PR/merge time only.

## Transition Note

This policy was adopted 2026-08-23, after `latteAPI` and `latteMCP` had already reached
`Confirmed` under the previous (test-after, manual-verification-sufficient) practice — see Phase
1/Phase 2 in `_project/completed_plan.md`. Per the "coverage only ever moves forward" principle
in `CLAUDE.md`'s "Two Concurrent Tracks" section, their existing `Confirmed` status is **not**
retroactively revoked by this change — a policy change is not the same as Track B touching their
code. Their still-open automated-test follow-up (tracked in `../PLAN.md`) now exists to satisfy
this stricter policy going forward; its current absence doesn't undo work already confirmed under
the rules in force at the time. `latteMCPclient` (Phase 3, not yet implemented) is the first
module this policy applies to from the start of its implementation.
