# Discovery Plan — Operation-Level Backlog

The granular work queue behind `coverage.md`. One row = one bounded unit of work sized to fit a
single focused iteration (small enough that reading its code doesn't strain context; see the
"Bootstrapping Documentation for an Existing Codebase" section in `../../CLAUDE.md` for the full
process this table drives). Update rows as you go — this file is what makes reconstruction
resumable across many short sessions on a large codebase instead of needing one giant pass.

## Phase 0 — Repo Topology Scan (one-time, cheap, metadata only)

- [x] Enumerate top-level packages/services from build manifests — `latteMCP.slnx` lists three
      projects: `src/latteAPI`, `src/latteMCP`, `src/latteMCPclient`.
- [x] Locate existing machine-readable specs — none exist (no OpenAPI/proto/GraphQL schema in
      the repo).
- [x] Inventory existing legacy docs — none existed before this documentation pass.
- [x] Grep for route/handler/consumer/job-definition patterns — result at the time of this scan
      (2026-08-22, before Phase 1/2): **no operations to index**. `src/latteAPI/Program.cs` was
      still the unmodified weather-forecast template (no real endpoints), and
      `src/latteMCP/Program.cs` / `src/latteMCPclient/Program.cs` were still unmodified
      "Hello World" templates. The only pre-existing implementation was `latteAPI`'s domain
      model/data files (`Models/`, `Data/`), which had no request-handling entry points of their
      own to treat as operations. Since then, `latteAPI` (Phase 1) and `latteMCP` (Phase 2) have
      both been implemented via Track B, each operation written from its pre-existing forward
      spec in `docs/modules/*/` (not reverse-engineered) — see their `requirements.md`/
      `interfaces/` for the now-`Confirmed` result. `latteMCPclient` remains unimplemented.
- [x] Populate `docs/00-index.md` and `coverage.md` with the first-draft module list.

Conclusion: this repo had essentially nothing for Track A to reconstruct at the time of this
scan. The Operation Backlog below is intentionally near-empty — `latteAPI` and `latteMCP`'s
operations were specified as forward requirements/interfaces in `docs/modules/*/` and then
implemented from those specs (Track B's docs-first flow, `CLAUDE.md` rule 6), not
reverse-engineered from pre-existing code, so neither generated rows here. `latteMCPclient`'s
operations remain specified but unimplemented (Phase 3). Re-run Phase 0 only if code is ever
found to have landed *ahead* of its doc for any of the three apps.

## Module Ordering

Foundational/shared modules generally go first since later modules' docs will reference them —
unless a specific module needs to be replanned first for business reasons, or `debt_log.md` has
accumulated several deferred flags against a module (a sign it's being actively touched by
feature work and worth prioritizing above the default ordering).

| Order | Module | Why this order |
|---|---|---|
| 1 | latteAPI | Owns the domain model and identity (ADR-0001); `latteMCP` and `latteMCPclient` both depend on it. |
| 2 | latteMCP | Wraps `latteAPI`; `latteMCPclient` depends on it. |
| 3 | latteMCPclient | Depends on both of the above; nothing depends on it. |

## Operation Backlog

Grain: one HTTP endpoint / queue consumer / scheduled job / CLI command / significant internal
library entry point = one operation = one row.

Empty by design (see Phase 0 conclusion above) — no implemented operations exist yet to
reconstruct. Populate this table only if an operation is ever implemented *without* first being
specified in the relevant module's `requirements.md`/`interfaces/` (or flat `interfaces.md` for
a module with no operations of its own — see ADR-0005), i.e. code arrives ahead of its doc
rather than the other way around.

| Op ID | Module | Type | Entry point (file:line) | Complexity | Batch | Status | Confidence | Notes / open questions |
|---|---|---|---|---|---|---|---|---|
| — | — | — | — | — | — | — | — | — |

- Type: `HTTP endpoint` / `queue consumer` / `scheduled job` / `CLI command` / `internal boundary`
- Complexity: `Simple` (uniform/CRUD-like, safe to batch with others) / `Complex` (business
  rules, branching, or ambiguous — needs its own isolated iteration)
- Batch: which iteration this row was (or will be) processed in — lets a run resume at the right
  spot instead of restarting
- Status: `Not scanned` → `In progress` → `Drafted` → `Confirmed` (rolls up into `coverage.md`)

## Per-Operation Process (one iteration)

1. Read only the entry point + up to ~2 hops of direct calls (handler → service → repository;
   go one hop deeper only if a business rule still isn't resolved).
2. Draft or extend the relevant entries in the module's `requirements.md`, `domain-model.md`,
   `interfaces/<operation>.md`, `test-spec.md` — write immediately, tag `Source` and
   `Status: Draft`. Capture a real sample request/response for the operation in the same file if
   one can be produced (a running instance, logs, a trace) — see ADR-0005: a concrete example is
   often faster to get right than prose for an operation that's still low-confidence.
3. Update this row: `Status → Drafted`, set `Confidence`, add any open question.
4. Move to the next row without re-reading code already covered, unless revisiting an open
   question raised earlier.

## Batching Guidance

- Batch 3–6 `Simple` operations from the same module into one iteration.
- Give each `Complex` operation its own isolated iteration.
- Prefer finishing a whole module (all its rows `Drafted`) before starting the next, so
  `coverage.md` reaches a clean `Draft` state one module at a time and can be handed to a person
  for review as a coherent unit rather than in fragments.
