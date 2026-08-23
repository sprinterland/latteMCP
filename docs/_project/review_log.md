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
framework-level" for the Framework Reviewer), the value of `_frw`'s `VERSION` file at run time
(format in `docs/framework-maintenance.md`'s "Versioning" section; read live from the shared
external `_frw` clone if reachable, or the static "bootstrapped from" version recorded in this
project's own `docs/framework-maintenance.md` otherwise), and the outcome (a description of what
was found and
what happened to it, not a strict tally — a full multi-agent review run's findings often overlap
across agents, so a precise "N found, M fixed" count is more likely to go stale/wrong than useful;
say what was fixed and what was deferred, in words). The Framework Reviewer sub-entry also
records any enhancement suggestions raised (or "none").

<!-- Entries are added going forward from 2026-08-23 (when Rule 15/16 logging started) — no
     retroactive entries were fabricated for reviews that ran before this file existed. -->

## 2026-08-23 14:42 — commit 8e46a87 (Add `_frw/VERSION`, split reviewer into Project + Framework)

### Project Reviewer

- Command: `/code-review high`, one invocation covering this commit's full diff. `PLAN.md`,
  `docs/00-index.md`, and `docs/_project/CHANGELOG.md`/`review_log.md` fall under Rule 15;
  `docs/dev-practices.md` fell under *both* rules for this commit, since the edit changed its
  "Secondary Review Before Push" section's own policy description (a structural change, not a
  `Selected:` toggle) — see Rule 16's later refinement of that distinction.
- Repo: latteMCP @ `8e46a87`
- Module(s): n/a — docs/framework-process change, no module code touched
- Framework version: `26.08.23:14.28.879`
- Outcome: the single invocation above spawned 6 sub-agents, covering both this lens and the
  Framework Reviewer's (see below); most-valuable fixes applied: stale "a secondary review gates
  every push" wording left in 3 spots, a broken `_frw/VERSION` "header" pointer (no header exists;
  the format lives in `docs/framework-maintenance.md`), reintroduced lowercase `plan.md` in two
  `_frw/` files, Rule 15/16's scope redefined from two independent allowlists (already disagreeing
  with each other) to a complementary split so future top-level docs can't fall through a gap,
  Rule 16 given the same Rule-2 disclaimer Rule 15 already had, a reuse clause added so
  overlapping-scope pushes don't need two full passes, a diagram column misalignment, and an
  ambiguous `MM`-used-for-both-month-and-minute format string clarified. A few lower-value
  findings were deferred — see the Framework Reviewer's enhancement suggestions below.

### Framework Reviewer

- Command: reused the Project Reviewer's single `/code-review high` invocation above, re-read
  through the fidelity/ambiguity/enhancement lens, per Rule 16's overlap-reuse clause — this
  commit was almost entirely framework-scoped, so a separate invocation wouldn't have added
  coverage.
- Repo: latteMCP @ `8e46a87`
- Module(s): n/a — framework-level
- Framework version: `26.08.23:14.28.879`
- Outcome: fidelity/ambiguity findings pooled with the Project Reviewer's outcome above rather
  than double-counted, since one invocation served both lenses; all confirmed fidelity/ambiguity
  findings from it were fixed.
- Enhancement suggestions: (1) millisecond-precision `_frw/VERSION` timestamps may be more
  granular than needed for a manually-bumped, never-colliding marker — not changed, since the
  user explicitly specified this format; (2) whether a framework-level process change like this
  one should get an ADR rather than only a CHANGELOG entry — reconsidered rather than dismissed:
  the initial "follows precedent" reasoning was incomplete (it ignored that ADR-0005 was itself a
  documentation/process convention, not a pure architecture decision), so the honest position is
  this is a genuinely open question rather than a settled one; left as a CHANGELOG-only entry for
  now, flagged to the user as a place to redirect if they'd rather this class of change get an
  ADR going forward; (3) `docs/dev-practices.md`'s Yes/No setting can't express "Project Reviewer
  only, no Framework Reviewer" as a partial selection — not changed, judged unnecessary complexity
  while Rule 16 already naturally goes dormant for a project with no `_frw/`.

## 2026-08-23 14:54 — commit dbbc48d (Apply Rule 15/16's own first review to the commit that created them)

### Project Reviewer

- Command: `/code-review high`, one invocation covering this commit's full diff (all within Rule
  15's scope: `docs/_project/review_log.md`, `docs/_project/CHANGELOG.md`).
- Repo: latteMCP @ `dbbc48d`
- Module(s): n/a — docs/framework-process change, no module code touched
- Framework version: `26.08.23:14.28.879` (at the time this review ran; bumped afterward — see
  Framework Reviewer sub-entry below)
- Outcome: this invocation spawned 4 sub-agents. No project-specific (non-framework) findings of
  substance; everything of value it surfaced was framework-scoped and is covered below.

### Framework Reviewer

- Command: reused the same invocation above, per Rule 16's overlap-reuse clause (this commit,
  like the one before it, was almost entirely framework-scoped).
- Repo: latteMCP @ `dbbc48d`
- Module(s): n/a — framework-level
- Framework version: `26.08.23:14.28.879` (at run time)
- Outcome: fixed — `docs/dev-practices.md`'s Framework Reviewer bullet still fully re-enumerated
  Rule 16's path list after the prior round's fix only touched the Project Reviewer bullet
  (mirrored in `_frw/docs/dev-practices.md`); this same gap made the prior round's CHANGELOG claim
  of having "removed duplicated scope-path enumeration... from `docs/dev-practices.md`"
  inaccurate at the time it was written (now true, since this round completed that removal); Rule
  16 listed `docs/dev-practices.md` as an unconditional trigger, contradicting `CLAUDE.md`'s own
  framing that an ordinary `Selected:` toggle needs no `_frw/` involvement — narrowed to trigger
  only on a structural change to that file, not a plain setting toggle; Rule 15 had no guidance
  for a project that bootstrapped from this framework and later deleted its own `_frw/` (no live
  `_frw/VERSION` to cite) — added the same static-fallback pointer the `_frw/` template already
  had; a dangling unmatched `)` from a prior edit in `docs/framework-maintenance.md`'s Versioning
  section (and its `_frw/` mirror) — fixed; `_frw/VERSION` itself had gone stale — this round
  touched `_frw/` again without bumping it, exactly the drift the Versioning section's "no
  exceptions" bump rule exists to prevent — bumped to `26.08.23:14.54.273` as part of this same
  fix pass; and the prior round's own log entry above had internal inconsistencies (a claimed "4
  sub-agents" when it was actually 6, a "findings/fixed" tally that didn't match its own
  enumerated list, and a self-contradictory claim of both reusing an invocation and running a
  separate one) — corrected to describe plainly what happened instead of a precise count that
  couldn't be verified.
- Enhancement suggestions: none new this round — this round's findings were all fixed rather than
  deferred, since they were direct follow-through on gaps the prior round's own design left
  behind, not new open questions.

## 2026-08-23 17:30 — commit 07e20dc (Move `_frw/` to shared external `claude-project-framework` repo)

### Project Reviewer

- Command: `/code-review high`, one invocation covering the full diff (CLAUDE.md, PLAN.md,
  docs/framework-maintenance.md, docs/dev-practices.md, docs/_project/CHANGELOG.md, and the
  removal of `_frw/` from this repo).
- Repo: latteMCP @ `07e20dc`
- Module(s): n/a — docs/framework-process change, no module code touched
- Framework version: `26.08.23:17.26.865` (live-read from the shared external `_frw/` clone at
  `/Users/sprn/claudework/newFrw/_frw/`, commit `a1c86e4` in `claude-project-framework`)
- Outcome: one finding, fixed — `docs/dev-practices.md` still described `_frw/` as something
  local to this repo (bare references to `_frw/docs/dev-practices.md`) without reflecting that
  it's now the external shared clone/repo described in `docs/framework-maintenance.md`; fixed by
  clarifying the first mention. A closer pass while fixing it turned up two more of the same
  class, applied in the same commit: `docs/_project/review_log.md`'s own format description (this
  file, the paragraph above the entries) still keyed the `_frw/VERSION` fallback to whether "this
  project has since deleted its own `_frw/`" — reworded to match Rule 15's live-clone-with-fallback
  wording; and `CLAUDE.md`'s "Reusable Framework Template" section still called the shared location
  an "external directory" with a "concrete external path," leftover from before
  `claude-project-framework` existed as an actual GitHub repo, breaking word-for-word fidelity
  with `_frw/CLAUDE.md.template` (which already said "repo"/"location") — reworded to match.

### Framework Reviewer

- Command: same invocation as Project Reviewer above, re-read through the Rule 16 lens (this push
  touches `CLAUDE.md` and `docs/framework-maintenance.md`, and the two scopes substantially
  overlap for a framework-focused change like this one).
- Repo: latteMCP @ `07e20dc`
- Module(s): n/a — framework-level
- Framework version: `26.08.23:17.26.865`
- Outcome: fidelity check — confirmed `CLAUDE.md`'s "Reusable Framework Template" section and
  Rules 15/16 are now word-for-word identical to `_frw/CLAUDE.md.template`'s corresponding
  sections (after the wording fix above); confirmed no file under `_frw/` carries any
  latteMCP-specific fact. Ambiguity check — none found beyond the dev-practices.md/review_log.md
  gaps already listed under Project Reviewer above (same review pass, same fixes).
- Enhancement suggestions: none this round.
