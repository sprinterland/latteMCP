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
records the `_frw/_data/change_requests.jsonl` `id`(s) of any enhancement suggestions raised (or
"none") — the id, not the suggestion's text, per `CLAUDE.md` Rule 16.

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

## 2026-08-23 17:55 — commit a83a01c (Flatten `_frw`'s internal `_frw/` wrapper up to the framework repo's own root)

### Project Reviewer

- Command: `/code-review high`, one invocation covering the full diff (`CLAUDE.md`,
  `docs/00-index.md`, `docs/_project/CHANGELOG.md`, `docs/dev-practices.md`,
  `docs/framework-maintenance.md`, and `docs/_project/review_log.md`'s live format description).
- Repo: latteMCP @ `a83a01c`
- Module(s): n/a — docs/framework-process change, no module code touched
- Framework version: `26.08.23:17.49.988` (live-read from the shared external `_frw` clone at
  `/Users/sprn/claudework/newFrw/`, commit `a644991` in `claude-project-framework`)
- Outcome: clean — a thorough multi-angle pass (line-by-line, removed-behavior audit, cross-file/
  cross-repo tracing, and external verification against the actual `claude-project-framework` repo
  contents) found no correctness bugs, no dropped invariants, and no broken cross-references. Two
  purely cosmetic candidates (minor line-wrap width inconsistencies in `CLAUDE.md` and
  `docs/dev-practices.md`) were considered and judged not worth a finding.

### Framework Reviewer

- Command: same invocation as Project Reviewer above, re-read through the Rule 16 lens (this push
  touches `CLAUDE.md` and `docs/framework-maintenance.md`, and the two scopes substantially
  overlap for a framework-focused change like this one).
- Repo: latteMCP @ `a83a01c`
- Module(s): n/a — framework-level
- Framework version: `26.08.23:17.49.988`
- Outcome: fidelity check — re-confirmed `CLAUDE.md`'s "Reusable Framework Template" section and
  Rules 15/16 are still word-for-word identical to `_frw`'s `CLAUDE.md.template` after this
  round's terminology cleanup; confirmed no file in `_frw` carries any latteMCP-specific fact.
  Ambiguity check — none found.
- Enhancement suggestions: none this round.

## 2026-08-23 18:28 — commit 306a0cb (Rename docs/_discovery and docs/_project to docs/discovery and docs/project)

### Project Reviewer

- Command: `/code-review high`, one invocation covering the full diff (`CLAUDE.md`, `PLAN.md`,
  `docs/00-index.md`, `docs/decisions/0005-api-docs-openapi-per-operation-samples.md`,
  `docs/dev-practices.md`, `docs/framework-maintenance.md`, and the `docs/_discovery/` →
  `docs/discovery/` / `docs/_project/` → `docs/project/` renames themselves).
- Repo: latteMCP @ `306a0cb`
- Module(s): n/a — docs/framework-process change, no module code touched
- Framework version: `26.08.23:18.23.266` (live-read from the shared external `_frw` clone at
  `/Users/sprn/claudework/newFrw/`, commit `9512282` in `claude-project-framework`)
- Outcome: clean — verified every live/prescriptive reference to the old paths was updated, no
  stray `docs/_discovery`/`docs/_project` directories or references remained, the external `_frw`
  clone matched what `docs/framework-maintenance.md` and the CHANGELOG claimed, `CLAUDE.md`'s
  "Reusable Framework Template" section and Rules 15/16 stayed word-for-word identical to
  `_frw`'s `CLAUDE.md.template`, and relative links from moved files (e.g.
  `docs/discovery/debt_log.md` → `../project/CHANGELOG.md`) resolved correctly. No findings.

### Framework Reviewer

- Command: same invocation as Project Reviewer above, re-read through the Rule 16 lens (this push
  touches `CLAUDE.md` and `docs/framework-maintenance.md`, and the two scopes substantially
  overlap for a framework-focused change like this one).
- Repo: latteMCP @ `306a0cb`
- Module(s): n/a — framework-level
- Framework version: `26.08.23:18.23.266`
- Outcome: fidelity check — confirmed `CLAUDE.md`'s "Reusable Framework Template" section and
  Rules 15/16 are still word-for-word identical to `_frw`'s `CLAUDE.md.template` after the rename;
  confirmed no file in `_frw` (including the new `_data/README.md`) carries any latteMCP-specific
  fact. Ambiguity check — none found; the new `_data/` folder's purpose and copy-exclusion are
  documented in both `_frw/README.md` and this project's `docs/framework-maintenance.md`.
- Enhancement suggestions: none this round.

## 2026-08-23 19:09 — commits dfebaec → 738ee4b (Add CLAUDE.md Rule 17 and `_frw/_data/`'s three activity logs, then fix what review found)

### Project Reviewer

- Command: `/code-review high`, one invocation (8 finder agents: simplification, removed-behavior,
  altitude, cross-file/external-repo tracer, reuse/duplication, efficiency, CLAUDE.md conventions,
  line-by-line diff) covering the full diff of commit `dfebaec` (`CLAUDE.md`,
  `docs/framework-maintenance.md`) and its `claude-project-framework` mirror `0808624`
  (`CLAUDE.md.template`, `README.md`, `docs/framework-maintenance.md`, and the three new
  `_data/*.jsonl` files).
- Repo: latteMCP @ `738ee4b` (reviewed at `dfebaec`, fixes landed here); `claude-project-framework`
  @ `9cea998` (reviewed at `0808624`, fixes landed here)
- Module(s): n/a — docs/framework-process change, no module code touched
- Framework version: `26.08.23:19.09.808` (live-read from the shared external `_frw` clone at
  `/Users/sprn/claudework/newFrw/`, commit `9cea998` in `claude-project-framework`)
- Outcome: not clean — 9 distinct findings surfaced across the 8 agents (after de-duplicating
  overlapping reports of the same underlying issues): a collision-prone `change_requests.jsonl` ID
  scheme whose own "never concurrent" justification was directly contradicted by the very rule
  (17) it was written to support; a Maintenance-rule review/push split with no amend guidance and
  no forced ordering; a `Versioning` section left saying "commit + push" after the step it
  described was rewritten to commit-only in the same diff; `review_log.md`'s live preamble (this
  file, the one you're reading) not updated for Rule 16's new id-citation requirement — missed in
  both repos; `CLAUDE.md`'s Standing Rule restating Maintenance-rule steps in prose, which is
  exactly the kind of restatement that had just gone stale; and `_data/` keeping its leading
  underscore right after `discovery/`/`project/` lost theirs for "hidden/system" reasons, with no
  reconciling note. Also flagged, not fixed (logged open in `change_requests.jsonl` instead, since
  neither is a mechanical bug and the second is explicitly a question for the user): Rule 16(c) and
  Rule 17 having overlapping, cross-referencing scope; and a 2026-08-23 14:42-entry question about
  whether framework-level process changes need an ADR that was flagged then and never actually put
  to the user across four subsequent framework pushes. Fixed the six mechanical/design findings in
  commits `738ee4b`/`9cea998` — verified by re-reading every changed section, grepping both repos
  for stale step-numbering and leftover old phrasing, `diff`-ing `framework-maintenance.md` and
  `CLAUDE.md`/`CLAUDE.md.template` between the two repos for continued parity, and validating all
  three `_data/*.jsonl` files parse as JSON — rather than a second full 8-agent pass, since the
  first pass's findings were specific enough to verify directly against the fix.

### Framework Reviewer

- Command: same invocation as Project Reviewer above (this diff touches `CLAUDE.md` and
  `docs/framework-maintenance.md` directly, and is itself framework-level end to end).
- Repo: latteMCP @ `738ee4b`; `claude-project-framework` @ `9cea998`
- Module(s): n/a — framework-level
- Framework version: `26.08.23:19.09.808`
- Outcome: fidelity check — confirmed `CLAUDE.md`'s "Reusable Framework Template" section and
  Rules 15/16/17 are still word-for-word identical to `_frw`'s `CLAUDE.md.template` after both the
  original addition and the fix-up. Ambiguity check — the 9 findings above are, precisely, ambiguity
  and self-consistency gaps this lens exists to catch; see Project Reviewer outcome for the list.
- Enhancement suggestions: `CR-1787501279766-q7r8` (Rule 16(c)/17 overlap, open),
  `CR-1787501279766-s9t0` (ADR-for-framework-changes question, open) — the other seven findings
  from this round were fixed rather than logged as open suggestions; see
  `_frw/_data/change_requests.jsonl` `CR-1787501200100-c3d4` and the `CR-1787501279766-*` ids for
  the full itemized resolution record.

## 2026-08-23 19:14 — commit range 738ee4b → HEAD (Resolve ADR-for-framework-changes question; define append-only "resolved" semantics)

### Project Reviewer

- Command: manual self-review (re-read every changed section, `diff`-checked `CLAUDE.md` against
  `CLAUDE.md.template` and `docs/framework-maintenance.md` between both repos for continued
  parity, validated all three `_data/*.jsonl` files still parse as JSON after the append) — no
  fresh multi-agent `/code-review` pass, since this round is a small, direct user-confirmed
  decision (Rule 11 carve-out) plus one narrowly-scoped mechanical fix (the append-only "resolved"
  definition), not new unreviewed design surface.
- Repo: latteMCP @ HEAD; `claude-project-framework` @ `461bd5e`
- Module(s): n/a — docs/framework-process change, no module code touched
- Framework version: `26.08.23:19.14.972`
- Outcome: clean — CLAUDE.md/CLAUDE.md.template Rule 11 differ only in the expected
  project-specific confirmation-date parenthetical; `change_requests.jsonl`'s
  `CR-1787501279766-s9t0` resolved via a new appended line (same id) rather than an in-place
  edit, as a live test of the mechanism just defined; all three `_data/*.jsonl` files remain
  valid JSON Lines.

### Framework Reviewer

- Command: same self-review as Project Reviewer above (this diff touches `CLAUDE.md` and
  `docs/framework-maintenance.md` directly).
- Repo: latteMCP @ HEAD; `claude-project-framework` @ `461bd5e`
- Module(s): n/a — framework-level
- Framework version: `26.08.23:19.14.972`
- Outcome: fidelity check — confirmed the Rule 11 carve-out is word-for-word identical between
  `CLAUDE.md` and `CLAUDE.md.template` apart from the one project-specific parenthetical; confirmed
  the append-only "resolved" definition is identical in both repos'
  `docs/framework-maintenance.md`. Ambiguity check — none found.

## 2026-08-23 19:52 — commit range HEAD~1 → HEAD (Add `_design/`; formalize written-plan Maintenance-rule step)

### Project Reviewer

- Command: `/code-review high docs/framework-maintenance.md` (this project's only changed file:
  the "Bootstrapped from" sync pointer and the new Maintenance-rule step-2 text with its
  renumbering).
- Repo: latteMCP @ HEAD; `claude-project-framework` @ `01ae9df`
- Module(s): n/a — docs/framework-process change, no module code touched
- Framework version: `26.08.23:19.50.336`
- Outcome: not clean — 2 findings, both fixed. (1) The "Previous sync" addition left an unclosed
  parenthetical, nesting the entire prior-sync description inside the new sync's parenthetical —
  fixed by closing the parenthetical after the new-sync description and dropping the "Previous
  sync" restatement in favor of a pointer to `CHANGELOG.md`/`update_history.jsonl`, per this same
  file's own "pointer, not restatement" principle. (2) Grepped the whole repo for stale
  `step N` cross-references after the renumbering — none found.

### Framework Reviewer

- Command: `/code-review high` scoped to `docs/framework-maintenance.md` (both repos) plus every
  new file under `claude-project-framework`'s `_design/` and its `README.md` edit — reused as one
  pass per Rule 16, since this push is entirely framework-focused (full scope overlap with Project
  Reviewer above, plus the propagated-copy validation Maintenance rule step 5 requires).
- Repo: latteMCP @ HEAD; `claude-project-framework` @ `01ae9df`
- Module(s): n/a — framework-level
- Framework version: `26.08.23:19.50.336`
- Outcome: not clean — 2 findings in the `_design/` propagation, both fixed before the (still
  local, unpushed) propagation commit was amended. (1) `_design/domain-model.md`'s "Propagation"
  glossary entry cited "Maintenance rule steps 3–4" but its definition text matches only step 4 —
  fixed to cite step 4 alone. (2) `_design/decisions/0008-...md` cited a
  "`CLAUDE.md.template`'s 'Executing actions with care' guidance" section that doesn't exist
  anywhere in that file (it conflated the harness's own operating instructions with the template's
  actual content) — fixed by rewriting the sentence without the false citation. Fidelity check —
  confirmed `docs/framework-maintenance.md`'s Maintenance-rule text and `_data/` file-tree bullet
  are word-for-word identical between `latteMCP` and `claude-project-framework` apart from the
  project-specific "Bootstrapped from" pointer, as expected. Ambiguity check — the 2 findings above
  are exactly the ambiguity/self-consistency gaps this lens exists to catch.
- Enhancement suggestions: none this round.
- Enhancement suggestions: none this round.
