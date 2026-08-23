# Review Log

Log of every `CLAUDE.md` Rule 15/16 secondary-review pass run before a `git push`. Kept separate
from `CHANGELOG.md`, which logs *what changed*; this file logs *how it was checked* — useful for
later investigation (e.g. "was this reviewed, by which reviewer, under which framework version").
One entry per push; each entry always has both a **Project Reviewer** sub-entry and a
**Framework Reviewer** sub-entry — the format never depends on whether Rule 16 actually fired.
When the push didn't touch a Rule-16 path (see `CLAUDE.md` Rule 16 for the exact trigger), the
Framework Reviewer sub-entry's content is simply "not run: push touched no framework paths"
instead of a command and outcome. This is a historical record — don't edit a past entry; if a
past review needs revisiting, add a new entry and link back to it.

Each sub-entry records: the command run and its scope (see `CLAUDE.md` Rule 15/16 for the exact,
current scope of each reviewer — not restated here, to avoid a second copy that can drift out of
sync), the repo and commit(s) being pushed, the module(s) the push touched (or "n/a —
framework-level" for the Framework Reviewer), the `_frw/VERSION` value at run time (format in
`docs/framework-maintenance.md`'s "Versioning" section), and the outcome (findings count, how
many were fixed vs. logged as a deliberate deferral, or "clean"). The Framework Reviewer
sub-entry also records any enhancement suggestions raised (or "none").

<!-- Entries are added going forward from 2026-08-23 (when Rule 15/16 logging started) — no
     retroactive entries were fabricated for reviews that ran before this file existed. -->

## 2026-08-23 14:42 — commit 8e46a87 (Add `_frw/VERSION`, split reviewer into Project + Framework)

### Project Reviewer

- Command: `/code-review high` (this commit's full diff — entirely within Rule 15's scope:
  `PLAN.md`, `docs/00-index.md`, `docs/dev-practices.md`, `docs/_project/CHANGELOG.md`,
  `docs/_project/review_log.md`)
- Repo: latteMCP @ `8e46a87`
- Module(s): n/a — docs/framework-process change, no module code touched
- Framework version: `26.08.23:14.28.879`
- Outcome: 10 findings across 4 sub-agents; 8 fixed (stale "secondary review" wording in 3 spots,
  broken `_frw/VERSION` "header" pointer, `_frw/docs/dev-practices.md`/`review_log.md` lowercase
  `plan.md`, duplicated/drifting scope-path lists, Rule 15/16 not covering future top-level docs,
  Framework Reviewer missing the Rule 2 disclaimer, diagram column misalignment, ambiguous `MM`
  format string, contradictory "skipped" note location); 2 deferred as noted enhancement
  suggestions below

### Framework Reviewer

- Command: `/code-review high` — reused the Project Reviewer's same invocation per Rule 16's
  overlap-reuse clause (this commit is ~entirely framework-scoped, so a separate invocation
  wouldn't have added coverage), re-read through the fidelity/ambiguity/enhancement lens; a
  second explicit invocation additionally ran and returned 2 more sub-agents' worth of findings
  (Conventions/ADR-precedent check, diff line-by-line scan) covering the same commit
- Repo: latteMCP @ `8e46a87`
- Module(s): n/a — framework-level
- Framework version: `26.08.23:14.28.879`
- Outcome: fidelity/ambiguity findings folded into the Project Reviewer count above (this early
  in the rule's life, the project/framework split of *this specific* diff was near-total overlap,
  so findings weren't worth double-bookkeeping); all confirmed fidelity/ambiguity findings fixed
- Enhancement suggestions: (1) millisecond-precision `_frw/VERSION` timestamps may be more
  granular than needed for a manually-bumped, never-colliding marker — not changed, since the
  user explicitly specified this format; (2) whether a framework-level process change like this
  one should get an ADR rather than only a CHANGELOG entry — not changed, follows this project's
  existing precedent (the TDD-adoption and original single-reviewer-gate decisions also skipped
  ADRs); (3) `dev-practices.md`'s Yes/No setting can't express "Project Reviewer only, no
  Framework Reviewer" as a partial selection — not changed, judged unnecessary complexity while
  Rule 16 already naturally goes dormant for a project with no `_frw/`.
