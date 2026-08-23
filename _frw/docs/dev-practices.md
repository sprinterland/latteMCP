# Development Practices

Configurable process decisions for how this project writes and verifies code — the process
counterpart to `api-conventions.md` (which fixes API *contract* conventions, and can be deleted
if the project has no HTTP API; this file cannot be deleted — every project makes these choices
implicitly even if it never writes them down). `CLAUDE.md` Workflow Rules 9, 14, and 15 read
the settings below.

**How to use this file:** for each setting, delete the options you didn't pick and keep only the
selected one under "Selected:", or leave all options listed with the chosen one marked — either
is fine as long as it's unambiguous which one is in force. Record the date adopted. Changing a
setting later is a normal process decision (confirm per Workflow Rule 2, update this file, then
follow it) — it does not require touching `CLAUDE.md` itself.

## Test-Writing Timing

Selected: _\<fill in\>_

- **Test-after** (lowest friction to start; risk: tests never get backfilled) — implement first,
  verify manually, write/backfill automated tests afterward as a tracked follow-up in `plan.md`;
  doesn't block the task or `Confirmed`.
- **TDD / test-first** (highest rigor; slower per task, catches design problems earlier) — write
  the failing automated test from the relevant `test-spec.md` entry before writing the
  implementation that makes it pass. A task is not done until: the test exists, it initially
  fails for the right reason, and the implementation makes it pass.
- **Test-alongside** (middle ground) — implementation and its automated tests written in the same
  task/commit, no strict ordering, but both required before the task is marked done — no
  "pending" state allowed.

## Automated Tests Gate `Confirmed` Status

Selected: _\<fill in\>_

- **No** (default assumed by `CLAUDE.md` Workflow Rule 9 if this file is absent or unset) —
  manual verification is enough for `Confirmed`; automated tests can lag as a tracked follow-up,
  independent of the rest of the module's status.
- **Yes, stricter** — a module/operation cannot reach `Status: Confirmed` until its
  `test-spec.md` entries have real, passing automated tests backing them. Pairs naturally with
  TDD above, but is an independent setting — e.g. test-after work can still require tests before
  `Confirmed` is declared, it just means `Confirmed` comes later than the implementation does.

## Local Verification After Implementation

Selected: _\<fill in\>_

- **Yes, whenever tests exist for the touched area** — run the build and the relevant automated
  test suite locally before considering an implementation task done. Manual smoke testing (e.g.
  `curl` / a running instance) remains useful in addition, but doesn't substitute for the
  automated run where tests exist.
- **No fixed requirement** (default assumed by `CLAUDE.md` Workflow Rule 14 if this file is
  absent or unset) — manual smoke testing alone is acceptable on its own.

## Secondary Review Before Push

Selected: _\<fill in\>_

- **Yes — `/code-review <level>` before every push** (default assumed by `CLAUDE.md` Workflow
  Rule 15 if this file is absent or unset; recommended level: `high`) — before any `git push`,
  run the code-review skill against the commits being pushed that aren't yet on the remote, as an
  independent secondary pass distinct from the authoring work already done. Its findings must be
  fixed, or explicitly acknowledged and logged as a deliberate deferral (e.g. in the relevant
  `plan.md`/`completed_plan.md` entry), before the push proceeds.
- **No** — no dedicated secondary review before push; rely on whatever review happens at PR/merge
  time only.

Enforcement, either way, is procedural by default (the person/Claude follows the rule) — no
pre-push git hook is set up automatically. A project wanting a mechanical guarantee can add a
Claude Code `PreToolUse` hook on `git push` (see the `update-config` skill) on top of whichever
option is selected above; that's a separate, stronger decision from picking "Yes" here.

## Adding further categories

This file is meant to grow as a project's needs become clear — e.g. code style/linting
enforcement, commit granularity, required code review before merge, CI gating. Add a new
`##`-level section per category, following the same "Selected: / other options documented"
shape, and reference it from a new or existing `CLAUDE.md` Workflow Rule so it's actually read
rather than just aspirational.
