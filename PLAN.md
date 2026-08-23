# Implementation Plan (current)

Scope: active/near-term, cross-module or org-level work only. Anything settled belongs in
`docs/`, not here. Module-local work for a team owning a single module MAY live instead in
`docs/modules/<module>/plan.md` — see rule 12 in `CLAUDE.md`.

Work against a module whose docs are below `Confirmed` is allowed — see "Track B — Development"
in `CLAUDE.md`. When that happens, note it here (or link the `docs/_discovery/debt_log.md`
entry) so it's visible that this task is proceeding ahead of full documentation on purpose, not
just falling through the cracks.

Note: `latteMCPclient` is currently `Draft`. `latteAPI` reached `Confirmed` on 2026-08-22 (Phase 1
implementation, Track B) and `latteMCP` reached `Confirmed` on 2026-08-22 (Phase 2 implementation,
Track B) — each has one remaining gap (automated tests, see Phase 1/Phase 2 below); nothing has
been deferred to `docs/_discovery/debt_log.md` since those gaps are tracked here instead. See
`docs/00-index.md` for the module map and `docs/decisions/` (ADR-0001–0004) for the reasoning
behind the design choices reflected in the tasks below.

`docs/dev-practices.md` (adopted 2026-08-23) now requires TDD (test-first) and passing automated
tests before `Confirmed` for all work going forward — Phase 3 below follows it from the start.
Per that file's transition note, `latteAPI`/`latteMCP`'s existing `Confirmed` status from Phase
1/2 is not retroactively revoked; their open automated-test items below now also serve to satisfy
the new stricter policy, not just close out a nice-to-have. That file also now requires a
two-reviewer gate before every `git push` — Project Reviewer always, Framework Reviewer when
`CLAUDE.md`/framework docs (`docs/framework-maintenance.md`, `docs/migrations.md`) are touched
(see "Secondary Review Before Push") — applies
from this point forward regardless of phase; each run is logged in
`docs/_project/review_log.md`, and a completed task's entry in `docs/_project/completed_plan.md`
should note the review outcome the same way it already notes manual-verification dates.

## Status

- Last updated: 2026-08-22
- Phase 1 (`latteAPI`) and Phase 2 (`latteMCP`) implemented and manually verified; automated
  tests still pending for both (see below). Phase 3 (`latteMCPclient`) is next up.

## In Progress

_Nothing yet — Phase 3 below is next up._

## Up Next

### Phase 1 — `latteAPI` (implemented 2026-08-22; automated tests pending)

Implements: API-REQ-001 through API-REQ-006 (see
[`docs/modules/latteAPI/requirements.md`](docs/modules/latteAPI/requirements.md)). All tasks but
the last were completed 2026-08-22 and moved to
[`docs/_project/completed_plan.md`](docs/_project/completed_plan.md#phase-1--latteapi-completed-2026-08-22-except-automated-tests--see-planmd).

- [ ] Write automated tests for `docs/modules/latteAPI/test-spec.md` (API-TEST-001–012); those
      entries stay `Draft` until then per `CLAUDE.md` rule 8.

**Exit criteria:** see `docs/modules/latteAPI/requirements.md` — all functional requirements
demonstrably met (done, verified manually); `docs/modules/latteAPI/test-spec.md` entries passing
via automated tests (not yet done — the one open item above).

### Phase 2 — `latteMCP` (implemented 2026-08-22; automated tests pending)

Implements: MCP-REQ-001 through MCP-REQ-005 (see
[`docs/modules/latteMCP/requirements.md`](docs/modules/latteMCP/requirements.md)). All tasks but
the last were completed 2026-08-22 and moved to
[`docs/_project/completed_plan.md`](docs/_project/completed_plan.md#phase-2--lattemcp-completed-2026-08-22-except-automated-tests--see-planmd).

- [ ] Write automated tests for `docs/modules/latteMCP/test-spec.md` (MCP-TEST-001–009); those
      entries stay `Draft` until then per `CLAUDE.md` rule 8.

**Exit criteria:** see `docs/modules/latteMCP/requirements.md` — all functional requirements
demonstrably met (done, verified manually); `docs/modules/latteMCP/test-spec.md` entries passing
via automated tests (not yet done — the one open item above).

### Phase 3 — `latteMCPclient`

Implements: CLIENT-REQ-001 through CLIENT-REQ-004 (see
[`docs/modules/latteMCPclient/requirements.md`](docs/modules/latteMCPclient/requirements.md)).
Follows `docs/dev-practices.md` (TDD): write each task's automated test from
`docs/modules/latteMCPclient/test-spec.md` first, confirm it fails for the right reason, then
implement.

- [ ] Interactive credential prompt (password not echoed) → `POST /login` on `latteMCP`
      (CLIENT-TEST-001/002 first).
- [ ] Open MCP connection with the token attached to every request (CLIENT-TEST-003 first).
- [ ] List tools (CLIENT-TEST-004 first).
- [ ] Scripted demo: menu → place order → get order → list orders (CLIENT-TEST-005 first).
- [ ] Remove the "Hello, World!" placeholder.
- [ ] Bring `docs/modules/latteMCPclient/*` from `Draft` to `Confirmed` once implementation
      matches spec and its automated tests pass (required for `Confirmed` per
      `docs/dev-practices.md`).

**Exit criteria:** see `docs/modules/latteMCPclient/requirements.md`, plus
`docs/modules/latteMCPclient/test-spec.md` entries passing via automated tests (required for
`Confirmed` under `docs/dev-practices.md`, unlike Phase 1/2 which predate that policy).

## Open Questions

None currently — all decisions needed to start Phase 1 are settled (see ADR-0001–0004).
