# Framework Maintenance — `_frw` and the project/framework split

Full definition of how `_frw` relates to this project's own `docs/`, and the standing process
for evolving the documentation philosophy itself. `CLAUDE.md`'s "Reusable Framework Template"
section is the short pointer to this file for everyday reading; this is the actual content.

## What `_frw` is

`_frw` is this project's name for a **genericized, self-contained copy** of this documentation
philosophy — a bootstrap kit for starting a *different* project with the same approach. It lives
**outside this repo**, in its own GitHub repo — [`sprinterland/claude-project-framework`](https://github.com/sprinterland/claude-project-framework)
(public) — rather than being vendored into any one project, and not nested under a literal
`_frw/` subfolder anywhere: that repo's own root **is** the bundle. The canonical copy is that
GitHub repo; the local clone at `/Users/sprn/claudework/newFrw/` (a sibling of this and every
other project directory under `~/claudework/`) is what a project on this machine actually reads
from and bumps `VERSION` in before pushing back to GitHub. Multiple projects bootstrap from, and
propagate framework-level changes into, this same shared repo; it is not copied into a project
and then deleted the way an earlier, single-project version of this setup worked.

It contains:

```
README.md              — how to use this bundle to bootstrap a new project
VERSION                 — this bundle's version (see "Versioning" below)
CLAUDE.md.template      — a tech/project-agnostic copy of a project's `CLAUDE.md`
PLAN.md.template        — minimal starter plan.md
_data/                  — `_frw`'s own framework-update notes; never copied into a bootstrapped
                          project (unlike everything else above, which is)
docs/                   — mirrors a project's docs/ tree, but every file is a template: process
                          and structure only, no project-specific facts. `modules/` holds one
                          `_module-template/` folder instead of real modules.
```

## Versioning

`_frw`'s `VERSION` file holds a single line: the bundle's version, stamped `YY.MM.DD:HH.MM.FFF` —
in field order, 2-digit year, month, day, then a 24h hour, minute, and milliseconds (the second
`MM` is minute, not a repeat of the month field three groups earlier) — the moment the bundle was
last changed, not a semantic version; there's no meaningful "major/minor" axis for a doc-template
bundle, only "as of when." It exists so anything that references "the framework version" — most
concretely, each `docs/project/review_log.md` entry's Framework Reviewer sub-entry (see
`CLAUDE.md` Workflow Rule 16) — can cite a precise, unambiguous point in `_frw`'s history rather
than a vague "recent."

Bump it (regenerate the timestamp, overwrite the file) as step 3 of the Maintenance rule below,
every time `_frw` actually changes — never on an ordinary `docs/`/`CLAUDE.md` change that doesn't
propagate — and commit + push the bump to `claude-project-framework` on GitHub. Because `_frw` is
a shared external repo rather than a per-project copy, this project's review log normally reads
its `VERSION` file live from the local clone (kept up to date with `git pull`) at review time.
Record the version this project last synced against as a static fact here too, so `review_log.md`
still has something permanent to cite if the shared clone/repo is ever unreachable (different
machine, no network, repo moved) at review time:

**Bootstrapped from / last synced at [`claude-project-framework`](https://github.com/sprinterland/claude-project-framework)
commit `9512282`, version `26.08.23:18.23.266`** (2026-08-23 — `docs/_discovery/` and
`docs/_project/` were renamed to `docs/discovery/` and `docs/project/`, and a `_data/` folder was
added at the bundle root for `_frw`'s own framework-update notes, excluded from what gets copied
into a bootstrapped project).

## What's project-specific vs. framework, precisely

- `docs/` (no underscore, at repo root) is this project's actual documentation — real
  requirements, real ADRs, real module docs for this project's own modules. It is never
  genericized and never copied elsewhere as-is.
- `_frw` is the framework: structure, process explanations, and placeholders only. It carries no
  fact about this project's business domain, stack choices, or decisions. Nothing in `_frw`
  should ever need `docs/`'s content to make sense, and nothing in `docs/` should ever link into
  `_frw`.

## Maintenance rule

`_frw` is a deliberate snapshot, not a live mirror. An ordinary change to `docs/` or `CLAUDE.md`
for *this project's* reasons (a new ADR, a new requirement, a routine process clarification) does
**not** get propagated to `_frw`. `_frw` only changes when there's a genuine decision to evolve
the underlying documentation *philosophy/framework itself* (something meant to apply to future
projects too, not just a fact about this one) — and per standing instruction, that decision is
never made unilaterally:

1. When a change looks like a framework-level change (not just a project fact), stop and ask the
   user whether to make it, before touching either `CLAUDE.md` or the shared `_frw`.
2. If approved, apply it to this project's own files first (`CLAUDE.md`, and `docs/` if the
   change affects process there too), same as any other confirmed decision (Workflow Rule 6).
3. Then propagate the equivalent generic version of the same change into the local `_frw` clone
   (including its `CLAUDE.md.template` if `CLAUDE.md` itself changed) so the template bundle
   stays current with the philosophy it's meant to hand off, bump its `VERSION` file to the
   current timestamp (see "Versioning" above), and commit + push to `claude-project-framework` on
   GitHub — every propagation is a version change and a push, no exceptions.
4. Note the framework change in `docs/project/CHANGELOG.md` same as any other material change.
