# Framework Maintenance — `_frw/` and the project/framework split

Full definition of how `_frw/` relates to this project's own `docs/`, and the standing process
for evolving the documentation philosophy itself. `CLAUDE.md`'s "Reusable Framework Template"
section is the short pointer to this file for everyday reading; this is the actual content.

## What `_frw/` is

`_frw/` at the repo root is a **genericized, self-contained copy** of this documentation
philosophy — a bootstrap kit for starting a *different* project with the same approach. It
contains:

```
_frw/
  README.md              — how to use this bundle to bootstrap a new project
  VERSION                 — this bundle's version (see "Versioning" below)
  CLAUDE.md.template      — copy of `../CLAUDE.md` (already tech/project-agnostic)
  PLAN.md.template        — minimal starter plan.md
  docs/                   — mirrors this project's docs/ tree, but every file is a template:
                            process and structure only, no project-specific facts. `modules/`
                            holds one `_module-template/` folder instead of real modules.
```

## Versioning

`_frw/VERSION` holds a single line: the bundle's version, stamped `YY.MM.DD:HH.MM.FFF` — in
field order, 2-digit year, month, day, then a 24h hour, minute, and milliseconds (the second `MM`
is minute, not a repeat of the month field three groups earlier) — the moment the bundle was last
changed, not a
semantic version; there's no meaningful "major/minor" axis for a doc-template bundle, only "as of
when"). It exists so anything that references "the framework version" — most concretely, each
`docs/_project/review_log.md` entry's Framework Reviewer sub-entry (see `CLAUDE.md` Workflow Rule
16) — can cite a precise, unambiguous point in `_frw/`'s history rather than a vague "recent."

Bump it (regenerate the timestamp, overwrite the file) as step 3 of the Maintenance rule below,
every time `_frw/` actually changes — never on an ordinary `docs/`/`CLAUDE.md` change that doesn't
propagate. A project that bootstraps from this bundle and then deletes its own `_frw/` copy (the
normal case per `_frw/README.md` step 3) has no live `VERSION` file to read afterward; that
project should instead record the version it started from as a static fact in its own
`docs/framework-maintenance.md` at bootstrap time, so its own review log has something permanent
to cite even without keeping `_frw/` around.

## What's project-specific vs. framework, precisely

- `docs/` (no underscore, at repo root) is this project's actual documentation — real
  requirements, real ADRs, real module docs for this project's own modules. It is never
  genericized and never copied elsewhere as-is.
- `_frw/` is the framework: structure, process explanations, and placeholders only. It carries no
  fact about this project's business domain, stack choices, or decisions. Nothing in `_frw/`
  should ever need `docs/`'s content to make sense, and nothing in `docs/` should ever link into
  `_frw/`.

## Maintenance rule

`_frw/` is a deliberate snapshot, not a live mirror. An ordinary change to `docs/` or `CLAUDE.md`
for *this project's* reasons (a new ADR, a new requirement, a routine process clarification) does
**not** get propagated to `_frw/`. `_frw/` only changes when there's a genuine decision to evolve
the underlying documentation *philosophy/framework itself* (something meant to apply to future
projects too, not just a fact about this one) — and per standing instruction, that decision is
never made unilaterally:

1. When a change looks like a framework-level change (not just a project fact), stop and ask the
   user whether to make it, before touching either `CLAUDE.md` or `_frw/`.
2. If approved, apply it to this project's own files first (`CLAUDE.md`, and `docs/` if the
   change affects process there too), same as any other confirmed decision (Workflow Rule 6).
3. Then propagate the equivalent generic version of the same change into `_frw/` (including
   `_frw/CLAUDE.md.template` if `CLAUDE.md` itself changed) so the template bundle stays current
   with the philosophy it's meant to hand off, and bump `_frw/VERSION` to the current timestamp
   (see "Versioning" above) — every propagation is a version change, no exceptions.
4. Note the framework change in `docs/_project/CHANGELOG.md` same as any other material change.
