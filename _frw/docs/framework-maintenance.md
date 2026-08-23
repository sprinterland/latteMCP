# Framework Maintenance — `_frw/` and the project/framework split

Full definition of how a project's own `_frw/` (if it keeps one, to spin off future projects the
same way) relates to its `docs/`, and the standing process for evolving the documentation
philosophy itself. `CLAUDE.md`'s "Reusable Framework Template" section is the short pointer to
this file for everyday reading; this is the actual content. Delete this file if the bootstrapped
project has no plans to ever spin off its own framework bundle — it only matters once `_frw/`
exists again somewhere.

## What `_frw/` is

`_frw/` at a project's repo root is a **genericized, self-contained copy** of that project's
documentation philosophy — a bootstrap kit for starting a *different* project with the same
approach. It contains:

```
_frw/
  README.md              — how to use this bundle to bootstrap a new project
  CLAUDE.md.template      — copy of the project's own CLAUDE.md (already tech/project-agnostic)
  PLAN.md.template        — minimal starter plan.md
  docs/                   — mirrors the project's docs/ tree, but every file is a template:
                            process and structure only, no project-specific facts. `modules/`
                            holds one `_module-template/` folder instead of real modules.
```

## What's project-specific vs. framework, precisely

- `docs/` (no underscore, at repo root) is a project's actual documentation — real requirements,
  real ADRs, real module docs for that project's own modules. It is never genericized and never
  copied elsewhere as-is.
- `_frw/` is the framework: structure, process explanations, and placeholders only. It carries no
  fact about the project's business domain, stack choices, or decisions. Nothing in `_frw/`
  should ever need `docs/`'s content to make sense, and nothing in `docs/` should ever link into
  `_frw/`.

## Maintenance rule

`_frw/` is a deliberate snapshot, not a live mirror. An ordinary change to `docs/` or `CLAUDE.md`
for *the project's* reasons (a new ADR, a new requirement, a routine process clarification) does
**not** get propagated to `_frw/`. `_frw/` only changes when there's a genuine decision to evolve
the underlying documentation *philosophy/framework itself* (something meant to apply to future
projects too, not just a fact about this one) — and that decision should never be made
unilaterally:

1. When a change looks like a framework-level change (not just a project fact), stop and ask the
   user whether to make it, before touching either `CLAUDE.md` or `_frw/`.
2. If approved, apply it to the project's own files first (`CLAUDE.md`, and `docs/` if the change
   affects process there too), same as any other confirmed decision (Workflow Rule 6).
3. Then propagate the equivalent generic version of the same change into `_frw/` (including
   `_frw/CLAUDE.md.template` if `CLAUDE.md` itself changed) so the template bundle stays current
   with the philosophy it's meant to hand off.
4. Note the framework change in `docs/_project/CHANGELOG.md` same as any other material change.
