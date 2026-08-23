# Review Log

Log of every `CLAUDE.md` Rule 15/16 secondary-review pass run before a `git push`. Kept separate
from `CHANGELOG.md`, which logs *what changed*; this file logs *how it was checked* — useful for
later investigation (e.g. "was this reviewed, by which reviewer, under which framework version").
One entry per push; each entry has a **Project Reviewer** sub-entry (always present) and a
**Framework Reviewer** sub-entry (present only when that push touched `CLAUDE.md`, `_frw/**`,
`docs/framework-maintenance.md`, or `docs/migrations.md` — otherwise note it was skipped). This
is a historical record — don't edit a past entry; if a past review needs revisiting, add a new
entry and link back to it.

Each sub-entry records: the command run (including its scope/target), the repo and commit(s)
being pushed, the module(s) the push touched (or "n/a — framework-level" for the Framework
Reviewer), the framework version at run time (`_frw/VERSION`'s value if this project still keeps
its own `_frw/`; otherwise the static "bootstrapped from" version recorded in this project's
`docs/framework-maintenance.md`), and the outcome (findings count, how many were fixed vs. logged
as a deliberate deferral, or "clean"). The Framework Reviewer sub-entry also records any
enhancement suggestions raised (or "none").

## <date/time — commit range being pushed>

### Project Reviewer

- Command: `/code-review <level>` (scope: `docs/modules/**`, `docs/decisions/**`,
  `docs/_discovery/**`, `docs/_project/**`, `docs/00-index.md`, `docs/dev-practices.md`,
  `plan.md`, source code)
- Repo: `<project>` @ `<commit sha or range>`
- Module(s): `<module names, or "docs-only">`
- Framework version: `<value>`
- Outcome: `<N findings, M fixed, K deferred — or "clean">`

### Framework Reviewer

- Command: `<same skill, scoped to CLAUDE.md/_frw/framework docs — or "not run: push touched no
  framework paths">`
- Repo: `<project>` @ `<commit sha or range>`
- Module(s): n/a — framework-level
- Framework version: `<value>`
- Outcome: `<...>`
- Enhancement suggestions: `<... or "none">`
