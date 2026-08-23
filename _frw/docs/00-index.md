# Documentation Index

## Cross-cutting

- `glossary.md` — shared vocabulary across all modules
- `api-conventions.md` — shared HTTP API conventions (JSON casing, error bodies, auth header,
  health payload, generated-spec requirement) — delete this file if the project exposes no HTTP
  API
- `dev-practices.md` — configurable process decisions: test-writing timing, whether automated
  tests gate `Confirmed`, local verification requirements, whether a secondary review gates every
  push — fill in before starting real work
- `architecture/overview.md` — system-wide component map, service boundaries, deployment
- `decisions/README.md` — index of all architecture decisions (ADRs)
- `framework-maintenance.md` — how `_frw/` relates to this `docs/`, and the process for changing
  the documentation philosophy itself (read rarely — only for a framework-level change; delete
  if this project will never spin off its own `_frw/`)
- `migrations.md` — docs-first process for fully replacing a module's implementation (read
  rarely — only during an active migration/rewrite)
- `_discovery/coverage.md` — module-level rollup of reconstruction progress (Track A)
- `_discovery/discovery_plan.md` — operation-level backlog driving the rollup above
- `_discovery/debt_log.md` — flags raised (and knowingly deferred) during feature work (Track B)
- `_project/CHANGELOG.md` — dated log of material changes to requirements/architecture/decisions
- `_project/completed_plan.md` — archive of tasks checked off in `../PLAN.md`
- `_project/review_log.md` — per-push log of Rule 15/16 secondary-review runs (command, repo,
  module(s), framework version)

## Modules

One row per `docs/modules/<name>/` folder. Add a row here whenever a module folder is added.

| Module | Path | Doc status | Notes |
|---|---|---|---|
| <module-name> | [`modules/<module-name>/`](modules/_module-template/) | Draft | <one-line purpose> |

Doc status here should match `_discovery/coverage.md` while a module is still being
reconstructed from existing code, and can be dropped from tracking once `Confirmed` and stable.

## Reading order for a new module

1. `modules/<name>/requirements.md` — what it must do
2. `modules/<name>/domain-model.md` — its data/vocabulary (and seed/example data, see `CLAUDE.md`)
3. `modules/<name>/architecture.md` — how it's currently built
4. `modules/<name>/interfaces/README.md` — contracts at its boundaries, one file per operation
   (or a flat `modules/<name>/interfaces.md` for a module with no operations of its own)
5. `modules/<name>/test-spec.md` — how to verify it

Then `../PLAN.md` for current status (`_project/completed_plan.md` for its finished-task archive)
and `_project/CHANGELOG.md` for history.

See `../CLAUDE.md` for the rules governing how this documentation set is maintained, including
the process for reconstructing docs from an existing codebase.
