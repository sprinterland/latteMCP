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
commit `0808624`, version `26.08.23:18.59.204`** (2026-08-23 — added `_data/update_history.jsonl`,
`_data/push_reviews.jsonl`, and `_data/change_requests.jsonl`, the three append-only logs described
in "Framework activity logs" below, plus the Maintenance rule steps and `CLAUDE.md` Rule 17 that
govern them).

## What's project-specific vs. framework, precisely

- `docs/` (no underscore, at repo root) is this project's actual documentation — real
  requirements, real ADRs, real module docs for this project's own modules. It is never
  genericized and never copied elsewhere as-is.
- `_frw` is the framework: structure, process explanations, and placeholders only. It carries no
  fact about this project's business domain, stack choices, or decisions. Nothing in `_frw`
  should ever need `docs/`'s content to make sense, and nothing in `docs/` should ever link into
  `_frw`.

## Framework activity logs (`_data/`)

`_frw/_data/` holds three append-only JSON-Lines logs of framework-maintenance activity — kept
only in the shared `_frw` repo, never copied into a bootstrapped project (see "What's
project-specific vs. framework, precisely" above). Each file is one JSON object per line; new
entries are always appended, never rewritten or deleted — mark a stale entry `resolved` (or
similar) instead of removing it.

- **`update_history.jsonl`** — one record per completed framework update (Maintenance rule step 6
  below): `id`, `timestamp`, `project` (which project's need triggered the update), `summary`,
  `questions_asked` (the Rule 2 confirmations made while deciding it — `[]` if none needed),
  `decisions_made` (Rule 4 judgment calls made along the way, with the reasoning — `[]` if none),
  `final_summary`, `frw_commit`, `frw_version`, `status` (`completed`).
- **`push_reviews.jsonl`** — one record per Rule 15/16 review run immediately before pushing
  `_frw`'s own propagation commit (Maintenance rule step 4 below): `id`, `timestamp`, `project`,
  `frw_commit`, `command`, `scope`, `outcome`, `enhancement_suggestions` (this entry's own
  `change_requests.jsonl` ids, or `[]`), `status` (`completed`).
- **`change_requests.jsonl`** — a continuous backlog of framework-enhancement ideas, logged the
  moment they're noticed during *any* activity in *any* project — planning, coding, discovery,
  review, even the task that's about to fix the very thing being logged (see `CLAUDE.md` Workflow
  Rule 17): `id`, `timestamp`, `project`, `activity` (`planning`/`coding`/`discovery`/`review`/
  etc.), `description`, `severity` (`minor`/`major`), `status` (`open`/`resolved`), `resolution`,
  `resolved_at`.

IDs are simple per-file incrementing counters (`UPD-0001`, `REV-0001`, `CR-0001`, ...), assigned
by reading the last line of the file at append time; these logs are only ever written during a
deliberate framework-maintenance step, never concurrently from multiple projects at once, so a
counter race isn't a real risk in practice.

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
   current timestamp (see "Versioning" above), and commit the propagation in the local `_frw`
   clone — not pushed yet.
4. Run the Rule 15/16 review already required before any push (`CLAUDE.md` Rules 15/16) — for a
   framework-level change, this same pass also validates the propagated `_frw` copy (Rule 16's
   fidelity lens) — then append its outcome to `_frw/_data/push_reviews.jsonl` (see "Framework
   activity logs" above), before pushing either repo. Fix any findings, or log a deliberate
   deferral, first.
5. Push both this project's commit(s) and the `_frw` propagation commit to
   `claude-project-framework` on GitHub — every propagation is a version change and a push, no
   exceptions.
6. Note the framework change in `docs/project/CHANGELOG.md` same as any other material change,
   and append a record to `_frw/_data/update_history.jsonl` summarizing the update (see
   "Framework activity logs" above).
