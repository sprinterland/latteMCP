# Documentation Debt Log

Raised whenever Track B (feature/fix work — see `../../CLAUDE.md`) touches code in an area whose
documentation is missing, still `Draft`, or contradicts what the code actually does, and the
decision is made to proceed without fully resolving it first. Not needed when the developer
instead chooses to bring the relevant docs to `Confirmed` as part of the same task — in that
case just update the module docs (and `coverage.md` / `discovery_plan.md`) directly; there's
nothing to defer.

| Date | Module / Operation | What's missing or mismatched | Flagged during (task/PR) | Decision | Reason for deferring | Follow-up |
|---|---|---|---|---|---|---|
| — | — | — | — | — | — | — |

No entries yet — no Track B implementation work has started under this contract. This table
stays empty as long as every module's docs are brought to `Confirmed` before or alongside the
code that implements them (the current plan for Phase 1–3 in `plan.md`); it starts filling in
the moment implementation reveals a gap that isn't resolved in the same task.

When an item is logged here, bump the priority/order of its module or operation row in
`discovery_plan.md` so the dedicated discovery track (Track A) picks it up sooner than it
otherwise would have. Remove a row once it's resolved — docs brought to `Confirmed` and matching
the code — and note the resolution in `CHANGELOG.md`.

This table is also a signal on its own: a module accumulating many rows here is being actively
worked but chronically under-documented, and is a good candidate to prioritize even above
Track A's normal foundational-module-first ordering.
