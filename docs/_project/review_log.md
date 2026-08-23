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
Reviewer), the `_frw/VERSION` value at run time, and the outcome (findings count, how many were
fixed vs. logged as a deliberate deferral, or "clean"). The Framework Reviewer sub-entry also
records any enhancement suggestions raised (or "none").

<!-- Entries are added going forward from 2026-08-23 (when Rule 15/16 logging started) — no
     retroactive entries were fabricated for reviews that ran before this file existed. -->
