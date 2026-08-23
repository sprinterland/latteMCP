# Framework Template

This is a genericized, self-contained copy of this repo's documentation philosophy — a bootstrap
kit for starting a *different* project with the same approach (docs-first, module-sharded,
Discovery + Development tracks, ADRs, ID-namespaced requirements/tests). It carries no fact about
*this* project: no business domain, no stack choices, no decisions. Everything here is
structure, process explanation, and placeholders only.

See `CLAUDE.md.template`'s own "Reusable Framework Template (`_frw/`)" section for the rule
governing how this bundle relates to a real project's `docs/` and stays in sync with it over time.

## To bootstrap a new project with this kit

1. Copy this whole `_frw/` directory into the new project's repo root.
2. Rename `CLAUDE.md.template` → `CLAUDE.md` and `PLAN.md.template` → `PLAN.md`, both at the new
   project's root (not inside `docs/`).
3. Move the contents of this bundle's `docs/` subfolder up to the new project's own `docs/` at
   its root (i.e. `_frw/docs/*` → `<new-project>/docs/*`), then delete the now-empty `_frw/`
   copy from the new project — it was only a vehicle for the copy-paste, not something a working
   project keeps around unless it will itself spin off future projects the same way.
4. Rename `docs/modules/_module-template/` to your first real module's name, fill in its
   `requirements.md`/`domain-model.md`/`architecture.md`/`interfaces/`/`test-spec.md`, and add a
   row for it in `docs/00-index.md`.
5. Fill in `docs/glossary.md`, `docs/api-conventions.md` (if the project exposes an HTTP API —
   delete the file if it doesn't), and `docs/architecture/overview.md` as the project takes
   shape; leave `docs/decisions/`, `docs/_discovery/`, and `docs/_project/` empty until there's a
   real decision, a real discovery pass, or a real dated change to log in each.
6. Fill in `docs/dev-practices.md` — pick a setting for each category (test-writing timing,
   whether automated tests gate `Confirmed`, local verification requirements) before real
   implementation work starts, since `CLAUDE.md` Workflow Rules 8/9/14 read it from the first
   task onward. It's the one config file worth deciding early rather than leaving to "fill in
   later," since defaulting silently (test-after, `Confirmed` without tests) is itself a choice.
7. Read `CLAUDE.md` end to end before starting work — it is the actual rulebook, not this file.

## What's in this bundle

```
_frw/
  README.md              — this file
  CLAUDE.md.template      — the philosophy/process rulebook; copy to <new-project>/CLAUDE.md
  PLAN.md.template        — minimal starter plan; copy to <new-project>/PLAN.md
  docs/                   — template docs tree; copy contents to <new-project>/docs/
    00-index.md
    glossary.md
    api-conventions.md
    dev-practices.md
    framework-maintenance.md
    migrations.md
    architecture/overview.md
    decisions/README.md
    decisions/TEMPLATE.md
    _discovery/coverage.md
    _discovery/discovery_plan.md
    _discovery/debt_log.md
    _project/CHANGELOG.md
    _project/completed_plan.md
    modules/_module-template/
      requirements.md
      domain-model.md
      architecture.md
      interfaces/README.md
      interfaces/TEMPLATE-operation.md
      test-spec.md
```
