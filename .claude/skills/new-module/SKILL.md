---
name: new-module
description: Scaffold a new docs/modules/<name>/ folder from the module template and register it in docs/00-index.md and docs/discovery/coverage.md. Use for "new module", "scaffold a module", "add docs for module X", or "create the docs folder for this service".
---

# New Module Scaffold

Creates the standard per-module documentation folder described in `CLAUDE.md`'s "Documentation
Structure" section.

## Procedure

1. **Confirm the module boundary first.** Per `CLAUDE.md`, a module folder should match a real
   bounded context / service / major subsystem — check `docs/00-index.md` and the actual code
   layout before creating one, rather than creating a folder per file or class.
2. **Pick the module name** to match the real code module/service directory where possible.
3. **Copy the template.** A project's own `docs/modules/_module-template/` only survives locally
   until the first module is created — bootstrapping renames it away rather than copying it, so it
   is usually gone by the time a *second* module is needed. The durable source is the shared `_frw`
   clone's copy: `<_frw clone>/copy_me/docs/modules/_module-template/` (find the clone path in this
   project's own `docs/framework-maintenance.md`). Copy that → `docs/modules/<name>/`, keeping the
   full file set: `requirements.md`, `domain-model.md`, `architecture.md`, `test-spec.md`, and
   either `interfaces/README.md` + `interfaces/TEMPLATE-operation.md` (if this module exposes
   operations of its own) or a single flat `interfaces.md` (if it's a pure caller with none — see
   `CLAUDE.md`'s "API documentation" section for which applies). If a local
   `docs/modules/_module-template/` still happens to exist (nothing has renamed it yet), that's
   fine to use instead — same content, just closer to hand.
4. **Pick an ID prefix** for this module's requirements/rules/tests (e.g. `AUTH-REQ-001`). Read the
   prefixes already in use by existing modules (`docs/00-index.md`, or grep existing
   `requirements.md` files) and stay consistent with that convention rather than inventing a new
   style.
5. **Fill in the template's placeholders** — module name, ID prefix, initial `Status:` values
   (`Draft (proposed by Claude, pending confirmation)` for anything not yet confirmed by the user,
   per Workflow Rules 2–3).
6. **Register the module**: add a row to `docs/00-index.md`, and to `docs/discovery/coverage.md` if
   this module has existing code being reconstructed (Track A) rather than being specified forward
   (Track B).
7. **If this module has fixed/seed data** (a static catalog, lookup table, reference values), add
   the "Seed / Example Data" section to `domain-model.md` per `CLAUDE.md`'s rule — real current
   values, never secrets (a dummy row for any entity mixing secret and non-secret fields).
