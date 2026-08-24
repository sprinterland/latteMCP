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

## 2026-08-23 20:16 — commit ea694ae (claude-project-framework) — Add missing `Source:` field values to module requirements template

### Project Reviewer

- Command: `/code-review high` — first pass scoped to the propagation commit in
  `claude-project-framework` (no `docs/modules/*` content in latteMCP itself needed changing, per
  the plan's own finding that existing usage already conformed); ran clean the first time.
  Re-review of the amended commit (see Framework Reviewer below) could not be run through the
  `/code-review` tool a second time — it returned "session limit · resets 10:20pm
  (Europe/Istanbul)" — so the second pass was a manual line-by-line read of the amended diff
  instead, checking specifically for the two failure modes the first pass had just found
  (Confirmed-phrasing reuse, inconsistent split-provenance tagging) plus general clarity. This is
  a logged, deliberate substitution for the tool on this one round, not a skipped review.
- Repo: latteMCP @ HEAD; `claude-project-framework` @ `ea694ae`
- Module(s): n/a — docs/framework-process change, no module code touched
- Framework version: `26.08.23:20.16.061`
- Outcome (first pass, tool): not clean — 3 findings.
  1. `_data/change_requests.jsonl`'s appended line for `CR-1787505166518-a99b` still had
     `status:"open"`/`resolution:null` even though the commit message called it resolved — fixed
     by appending a further, correctly `resolved` line (same `id`, per this repo's append-only
     convention).
  2. The new `Source:` value "Confirmed by implementation — `<ref>`" reused "Confirmed" phrasing
     for content no person had reviewed, contradicting `CLAUDE.md` Workflow Rule 5's explicit
     reservation of that phrasing for actual human sign-off, and overlapped in meaning with the
     pre-existing "Draft (inferred from code)" value — fixed by dropping the new value entirely
     and clarifying that "Draft (inferred from code — path)" already covers the already-built/
     discovered-by-Track-A case, with `Status:` (not `Source:`) recording later verification.
  3. The split-provenance example tagged its two clauses inconsistently (`VALUE (X)` vs.
     `Y VALUE`) — fixed to tag both clauses the same way (`VALUE (tag)`).
  All three fixed; the local, unpushed propagation commit was amended (`5f090f3` → `ea694ae`)
  rather than adding a follow-up commit, per Maintenance rule step 5.
- Outcome (second pass, manual): clean. Confirmed the amended `Source:` enum no longer uses
  "Confirmed" for anything a person hasn't reviewed; confirmed the split-provenance example is
  consistently tagged; confirmed `_data/change_requests.jsonl` and the other two `_data/*.jsonl`
  files still parse as valid JSON Lines
  (`python3 -c "import json; [json.loads(l) for l in open(f)]"` per file); separately caught (via
  my own re-read of the two latteMCP record-keeping files below, not part of this diff) that
  `docs/project/CHANGELOG.md` and `docs/framework-maintenance.md`'s sync pointer both still said
  "6 values" after the fix dropped the third value back out, leaving 5 — corrected both before
  this log entry.

### Framework Reviewer

- Command: same review as Project Reviewer above (this push touches `docs/framework-maintenance.md`
  directly, plus the propagated template file in `claude-project-framework`).
- Repo: latteMCP @ HEAD; `claude-project-framework` @ `ea694ae`
- Module(s): n/a — framework-level
- Framework version: `26.08.23:20.16.061`
- Outcome: fidelity check — `docs/modules/_module-template/requirements.md` stays
  placeholder/structural only (no framework-design rationale leaked into it, per `_design/
  requirements.md`'s `FRW-REQ-008`); `CLAUDE.md` / `CLAUDE.md.template` needed no edit since Rule
  5's prose already matched the added wording and neither file enumerates `Source:` values itself.
  Ambiguity check — the 3 findings above (Project Reviewer, first pass) are exactly the
  ambiguity/self-consistency gaps this lens exists to catch; none remaining after the fix.
  Enhancement-opportunity check — none additional noticed this round.
- Enhancement suggestions: none this round (the round's own subject, `CR-1787505166518-a99b`, is
  now resolved rather than newly suggested).

## 2026-08-23 22:53 — latteMCP: pending commit (this entry's own commit) / commit `6d367dd` → `a8c3a28` (claude-project-framework) — Reconcile ADR-0005's real-captured-samples mandate with Rule 10

### Project Reviewer

- Command: `/code-review high` (2 background finder agents: correctness/cross-file, cleanup/
  altitude/conventions), triggered by reviewing `docs/modules/latteAPI/interfaces/` and
  `docs/modules/latteMCP/interfaces/` against `CLAUDE.md`/`api-conventions.md`/ADR-0005 and
  finding a real secret leaked in two sample files.
- Repo: latteMCP @ HEAD (pending commit); `claude-project-framework` @ `a8c3a28`
- Module(s): `latteAPI` (`interfaces/post-auth-login.md`), `latteMCP`
  (`interfaces/post-login.md`); cross-cutting `docs/api-conventions.md`, `CLAUDE.md`
- Framework version: `26.08.23:22.41.679`
- Outcome: not clean, but every finding landed on the `claude-project-framework` side of the diff
  (see Framework Reviewer below) — none on latteMCP's own module content. The credential
  redaction and the two accuracy corrections (`api-conventions.md`'s `401`/`WWW-Authenticate`
  note; `post-login.md`'s `Content-Type` passthrough claim) were verified directly against
  `src/latteAPI/Program.cs` and `src/latteMCP/Program.cs:76` before being written, and the
  reviewer raised no issue with either.

### Framework Reviewer

- Command: same review as Project Reviewer above (full overlap — this push's substance is the
  `CLAUDE.md`/`CLAUDE.md.template` exception itself, not an incidental touch).
- Repo: latteMCP @ HEAD (pending commit); `claude-project-framework` @ `a8c3a28` (fix amended into
  `6d367dd`, review-log entries added in `a8c3a28`)
- Module(s): n/a — framework-level
- Framework version: `26.08.23:22.41.679`
- Outcome: not clean — 4 findings, all fixed before push.
  1. Fidelity: confirmed `CLAUDE.md.template`'s new exception clause matches `CLAUDE.md`'s
     verbatim except for the expected project-vs-generic substitution (`Waitresses` vs.
     `<ConfigKeyName>`) — no issue.
  2. Process-completeness (Maintenance rule step 6): no `update_history.jsonl` entry existed for
     the propagation commit — fixed by appending `UPD-0004`.
  3. Process-completeness (Maintenance rule step 5): no `push_reviews.jsonl` entry reviewed the
     propagation commit itself (the only new entry, `REV-0004`, documented an earlier,
     already-pushed commit) — fixed by appending `REV-0005` (this entry's `claude-project-framework`
     counterpart).
  4. Altitude: the new exception and the pre-existing "Seed / Example Data" secrets carve-out
     solved the same Rule 10 conflict with two unlinked strategies, and the redaction example said
     the literal words "config key" instead of naming one — fixed by cross-referencing both
     carve-outs and correcting the example to name an actual/placeholder key.
  Separately, `CR-1787513864537-a3b8` was left `status:"open"` in the propagation commit despite
  being the finding that commit fixed — appended a `resolved` line. Also noted, not fixed: a
  pre-existing, unrelated `push_reviews.jsonl` id gap (no `REV-0003` anywhere in history) —
  renumbering a published append-only log would misrepresent history worse than the gap does, so
  it was logged instead as `CR-1787514778001-f4e2` for a future session to decide.
- Enhancement suggestions: `CR-1787514778001-f4e2` (the `REV-0003` gap, this round); plus two
  lower-priority findings from the interfaces-file review that motivated this whole round, not
  acted on — `CR-1787513864547-86c1` (ADR-0005/`CLAUDE.md` doesn't say whether an MCP tool surface
  is exempt from the OpenAPI mandate) and `CR-1787513864557-2d8f` (no shared-conventions home for
  the repeated MCP tool error-message format, unlike the HTTP side's `api-conventions.md`).

## 2026-08-23 23:25 — latteMCP: pending commit (this entry's own commit) / commit `1c44c0d` (claude-project-framework) — Add `moderate` severity tier and `affected_entities` field to `change_requests.jsonl`'s schema

### Project Reviewer

- Command: `/code-review high`, three passes (initial edit, after amending in the enum-gap/date-cutoff/architecture-ambiguity fixes, after amending in two wording fixes)
- Repo: latteMCP @ HEAD (pending commit); `claude-project-framework` @ `1c44c0d`
- Module(s): n/a — framework-level (`docs/framework-maintenance.md`'s "Framework activity logs" schema description)
- Framework version: `26.08.23:23.20.557`
- Outcome: not clean on first two passes; findings fixed each round; two low-severity findings from
  the final pass logged as deferred rather than fixed (see Framework Reviewer below).

### Framework Reviewer

- Command: same review as Project Reviewer above (full overlap — this push's entire substance is
  the schema description itself, in both `docs/framework-maintenance.md` copies).
- Repo: same as above.
- Module(s): n/a — framework-level.
- Framework version: `26.08.23:23.20.557`
- Outcome: not clean — 5 findings across two passes, 3 fixed, 2 deferred.
  1. Fidelity: confirmed both `docs/framework-maintenance.md` copies stayed verbatim in sync
     throughout, and the version/commit pointer line matched the propagation commit at each step —
     no issue.
  2. Ambiguity: the initial `affected_entities` enum had no value for framework/meta artifacts
     (`CLAUDE.md`, `review_log.md`, `CHANGELOG.md`, `PLAN.md`, `00-index.md`, `discovery/*`, the
     `_data/*.jsonl` schemas themselves) even though most real historical entries concern exactly
     those — fixed by adding an open-ended `process` catch-all value.
  3. Ambiguity: `architecture` collided between a module's own `architecture.md` and the top-level
     `docs/architecture/overview.md` — fixed by splitting into `architecture` and
     `architecture-overview`.
  4. Correctness: the "entries logged before 2026-08-23 predate this schema" cutoff was date-level
     while every existing entry is also dated 2026-08-23, so it couldn't actually distinguish
     anything — fixed by switching to "identify by field absence" instead of a date.
  5. Completeness: `_frw/_design/domain-model.md`'s own lookup copy of this schema (kept for
     `_design/`'s self-sufficiency, per that file's own "if the two disagree, `docs/framework-
     maintenance.md` wins and this table should be corrected to match" clause) was not updated in
     the same commit — fixed by syncing it.
  A second review pass then found the `process` catch-all's own example list read as closed/
  exhaustive rather than open-ended (missing a module's own `plan.md` and future top-level docs
  like a hypothetical `docs/security.md`) — fixed by rewording it as an explicit catch-all. A third
  pass found two more issues, deferred rather than fixed given diminishing returns on a
  single-field schema change: the new `affected_entities` enum values collide by name with
  `_frw/_design/`'s own file names (`domain-model`, `architecture`, etc.) with no disambiguation —
  logged as `CR-1787517912972-b7e1`; and the new `moderate` severity tier has no documented
  selection criteria, unlike the `affected_entities` enum's careful disambiguation — logged as
  `CR-1787517912973-c92a`.
- Enhancement suggestions: `CR-1787517912972-b7e1`, `CR-1787517912973-c92a` (both this round, both
  deferred rather than acted on).

## 2026-08-24 00:25 — latteMCP: commit `5371b6e` / commit `1374628` (claude-project-framework) — Add a minor-propagation fast lane to the Maintenance rule (`FRW-ADR-0009`)

### Project Reviewer

- Command: `/code-review high`, 3 invocations spawning 6 parallel doc-consistency/correctness finder agents in total (single-file ambiguity; flow-ordering; ADR/wording logic; altitude/completeness; reuse/simplification/altitude/conventions; correctness angles A+B+C)
- Repo: latteMCP @ `5371b6e`; `claude-project-framework` @ `1374628`
- Module(s): n/a — framework-level (`docs/framework-maintenance.md`'s Maintenance rule, steps 1-2)
- Framework version: `26.08.24:00.06.658`
- Outcome: not clean on any invocation; every real finding fixed in place (commit amends
  `cc6acc9` → `dc009a5` → `7a7b227` → `84043e9` → `1374628`); two design-level concerns deferred
  (see Framework Reviewer below).

### Framework Reviewer

- Command: same review as Project Reviewer above (full overlap — this push's entire substance is
  the Maintenance-rule change and its `_design/` self-description, in both repos).
- Repo: same as above.
- Module(s): n/a — framework-level.
- Framework version: `26.08.24:00.06.658`
- Outcome: not clean — 10 findings across the 6 agents, 8 fixed, 2 deferred as `change_requests.jsonl`
  entries. Several later-reporting agents found issues that an earlier amend had already fixed
  (stale drift against a mid-review commit state) — noted below where relevant, not double-counted.
  1. Ambiguity: the "confined to a single file" criterion didn't say whether a project doc and its
     identical `_frw` mirror count as one file or two — exactly the pair this fast lane is meant to
     cover — fixed by stating the mirror pair counts as one (and, in a later round, that a lone
     `_frw`-only file with no project-side counterpart, e.g. one `_design/` doc, likewise counts as
     one).
  2. Fidelity: `_design/architecture.md`'s compressed step-1/2 summary put the user ask in step 1,
     but the canonical rule has no ask until step 2 — fixed by rewriting the summary to match.
  3. Fidelity: `_design/requirements.md` and `test-spec.md` each dropped two of the four
     minor-propagation criteria ("comparably small self-contained edit"; "does not change an
     existing rule's meaning") when condensing the canonical wording — fixed by restoring all four,
     then superseded by finding 6 below.
  4. Correctness: `_design/decisions/0009-*.md`'s Consequences claimed the fast lane would help
     "the majority of past propagations, by inspection" — checking that claim against
     `_data/update_history.jsonl` showed only `UPD-0003` actually qualifies under the ADR's own
     criteria — fixed by replacing the claim with the checked citation.
  5. Fidelity: `_design/domain-model.md`'s "Maintenance rule" glossary entry still described the
     old unconditional ask-then-plan flow — fixed to describe the two-tier split.
  6. Ambiguity: `FRW-ADR-0009`'s Decision prose said "four criteria" but read as five
     comma-separated clauses — fixed by reformatting as an explicit four-item list.
  7. Completeness: neither the minor-propagation criteria nor Maintenance-rule step 3 addressed a
     change confined entirely to `_frw`-internal files with no project-side counterpart (e.g. a
     lone `_design/` doc) — fixed by extending the "single file" criterion and adding a step-3
     no-op note for that case.
  8. Cosmetic: `FRW-ADR-0008`'s `Status:` field extended past the README's documented fixed-enum
     values with an inline explanation — fixed by moving the explanation to a separate "Superseded
     note" bullet; that bullet's own convention wasn't documented anywhere, so
     `_design/decisions/README.md`'s legend was updated to permit and describe it.
  9. Reuse/altitude: the four criteria were restated near-verbatim in five places
     (`docs/framework-maintenance.md`, `_design/requirements.md`, `test-spec.md`, `architecture.md`,
     `decisions/0009-*.md`) instead of pointing to one canonical source — and had already drifted
     within this same change (`architecture.md`'s copy lagged the other four after finding 1's later
     refinement) — fixed by converting `requirements.md`, `test-spec.md`, and `architecture.md` to
     point at `docs/framework-maintenance.md`'s Maintenance rule instead of restating it (the ADR's
     own restatement is left as-is, consistent with other ADRs' self-contained Decision sections).
  10. Design (deferred): the "single file" criterion is calibrated to the narrowest historical
      propagation (only `UPD-0003` of 5 past entries would qualify), and the criteria have no guard
      against a large change being split into several sequential "minor" propagations that
      collectively achieve what one large change would be blocked from — both real considerations,
      neither acted on since they'd mean redesigning already-approved criteria or adding back
      ceremony unilaterally; logged as `CR-1787520515394-219f` and `CR-1787520659261-b488`.
- Enhancement suggestions: `CR-1787520515394-219f`, `CR-1787520659261-b488`.

## 2026-08-24 15:31 — latteMCP: pending commit (this entry's own commit) / commit `6b9b07d` (claude-project-framework) — Add `copy_me/` boundary and 8 process skills (`FRW-ADR-0010`)

This was a **Full framework change** (new top-level structure, 8 new files — past the
minor-propagation criteria), so it went through the full ask-then-written-plan sequence (Plan
Mode), including one round of user correction on where skills should live before the plan was
approved.

### Project Reviewer

- Command: `/code-review high`, 3 separate invocations across the change (one after the initial
  restructuring, one after fixing its findings, one final independent multi-agent pass — the third
  itself spawned 4 parallel finder/verifier agents).
- Repo: `claude-project-framework` @ `a7ff48a` → `fd0496d` → `1d00092` → `b2fa084` → `6b9b07d`
  (amended four times — restructuring, review fixes, the `push_reviews.jsonl`/`update_history.jsonl`
  entries below, then correcting their self-referenced commit hash to the actual final one — still
  local/unpushed); latteMCP (this commit, pending).
- Module(s): n/a — framework-level (`copy_me/` restructuring, 8 new skills, `_design/`
  self-description, `docs/framework-maintenance.md`)
- Framework version: `26.08.24:15.00.729`
- Outcome: not clean on the first two invocations; every finding fixed in place via commit amends
  (safe — still local/unpushed). Findings and fixes:
  1. Fidelity: the `copy_me/` restructuring moved `docs/`, `CLAUDE.md.template`, `PLAN.md.template`
     out from under the bundle root, but left `_design/00-index.md`, `architecture.md`,
     `domain-model.md`, `requirements.md`, and `test-spec.md` — the bundle's own reflexive
     self-description — still describing the old flat layout, plus one genuinely broken relative
     link (`_design/decisions/README.md`'s `../../docs/decisions/TEMPLATE.md`, now
     `../../copy_me/docs/decisions/TEMPLATE.md`) — fixed across all six files; `FRW-REQ-002/003`
     widened to state the invariant covers `_design/`'s own cross-references too.
  2. Correctness: `new-module` and `new-api-operation` (two of the new skills) instructed copying a
     project-local `docs/modules/_module-template/`, but the bootstrap flow *renames* that folder
     away on first use rather than copying it — so both skills would fail on every module after the
     first, the common case — fixed to fall back to the shared `_frw` clone's permanent copy
     (`copy_me/docs/modules/_module-template/`) when no local template survives.
  3. Altitude: `log-change-request`'s own text warned the `change_requests.jsonl` schema "has grown
     before" and told the author to re-read the `affected_entities` enum live, then hardcoded the
     `severity` enum two lines later — fixed to defer to a live re-read for both fields.
  4. Altitude: `push-review-gate` restated Rule 16's current trigger-path list verbatim inside a
     step framed as "read the current rules live" — fixed to describe the classification step
     without restating the list.
  5. Clarity: `docs/framework-maintenance.md`'s bundle-contents tree showed two differently-scoped
     `.claude/skills/` folders (bundle-root, framework-dev-only vs. `copy_me/`-nested,
     project-usage) with the same unqualified label — fixed by naming the root one explicitly.
  6. Reuse (deferred): the new 6-skill roster is enumerated verbatim in four separate index files
     (`README.md`, `copy_me/docs/framework-maintenance.md`, and both `.claude/skills/README.md`
     copies) with no single source of truth — the same duplication class `copy_me/` itself was just
     introduced to eliminate for the bootstrap allowlist — not acted on now since fixing it would
     mean designing a generation/single-source mechanism for markdown index content, larger than
     the propagation in flight; logged as `CR-1787573917472-b195`.

### Framework Reviewer

- Command: reused the Project Reviewer's invocations above (full overlap — this push's entire
  substance is framework-level).
- Repo: same as above.
- Module(s): n/a — framework-level.
- Framework version: `26.08.24:15.00.729`
- Outcome: same findings/fixes as Project Reviewer above, read through the fidelity/ambiguity/
  enhancement lens — finding 1 is a fidelity gap (bundle self-description contradicting its own
  fresh content), finding 5 is an ambiguity fix, finding 6 is the enhancement opportunity.
- Enhancement suggestions: `CR-1787573917472-b195`.

## 2026-08-24 17:30 — latteMCP: pending commit (this entry's own commit) / commit `95c2c27` (claude-project-framework) — Restrict downstream-project writes into `_frw` to `_data/`; add `sync-framework-updates` (`FRW-ADR-0011`)

This was a **Full framework change** (new rule content, changes an existing rule's meaning,
architectural implication), so it went through the full ask-then-written-plan sequence (Plan
Mode).

### Project Reviewer

- Command: `/code-review high`. First invocation mis-scoped (defaulted to latteMCP's own repo and
  reviewed an unrelated already-pushed commit instead of the `_frw` clone) and was discarded;
  redone via a general-purpose agent given an explicit `git -C /Users/sprn/claudework/newFrw show
  HEAD` scope. A second, separately-scoped `/code-review high` ran correctly against this
  project's own diff (`CLAUDE.md`, `docs/framework-maintenance.md`, `docs/project/CHANGELOG.md`,
  `.claude/skills/README.md`, the new `.claude/skills/sync-framework-updates/`).
- Repo: `claude-project-framework` @ `e485363` → `54aa332` → `78f817e` → `d6c75a1` (pushed) →
  `5d22495` → `e2c1d51` → `b27a02d` → `fd486ae` (this last amend was already-pushed history —
  caught before force-pushing; recovered via `git reset --hard origin/main` back to `4721d99`,
  then fixed forward with two new commits, `8b9a0bb` and `95c2c27` — see the correctness note
  below); latteMCP (this commit, pending).
- Module(s): n/a — framework-level (Maintenance rule restructuring, new project-usage skill,
  `_design/` updates)
- Framework version: `26.08.24:17.26.719`
- Outcome: not clean on the `_frw` side across two rounds; every finding fixed via commit amends
  or, once one round was mistakenly amended after already being pushed, via forward-only commits
  instead (see `_frw/_data/push_reviews.jsonl` `REV-0009`/`REV-0010`/`REV-0011` for the full
  detail). Findings and fixes:
  1. Ambiguity: `_design/requirements.md` spliced the new `FRW-REQ-009`/`FRW-REQ-010` in before
     the pre-existing `FRW-REQ-008`, producing a confusing `007→009→010→008` reading order — fixed
     by moving `009`/`010` to after `008`, restoring numeric order.
  2. Ambiguity: `copy_me/CLAUDE.md.template`'s rewritten Standing-rule paragraph juxtaposed "ask
     the user first" directly against the `change_requests.jsonl` write description, readable as
     requiring approval before logging — contradicting Rule 17's "log it immediately... don't
     wait" elsewhere in the same file — fixed by splitting the sentence so proposing (logging) is
     explicitly never gated on asking, only actually making the change is. Mirrored into
     latteMCP's own `CLAUDE.md`.
  3. Altitude (fixed, not deferred): `propagate-framework-change/SKILL.md`'s own procedure used 6
     steps while `framework-maintenance.md`'s "Framework propagation" flow it orchestrates uses 5
     — merged the skill's "log and push" and "review" steps to match.
  4. Altitude, caught in a follow-up minor propagation: the new `sync-framework-updates` skill
     shipped with 7 steps instead of the 5-step "Inbound sync" flow it mirrors — same class as
     finding 3, missed in the first round because the skill was newly authored rather than edited.
     Fixed and propagated separately (commit `4721d99`, approved as a minor propagation per
     `FRW-ADR-0009`).
  5. Correctness (process, not content): while fixing finding 4, a `git commit --amend` was run
     against `4721d99` *after* it had already been pushed, producing a rejected non-fast-forward
     push. Caught immediately — no force-push was attempted. Recovered by `git reset --hard
     origin/main` (discarding only the unpushed, easily-reproducible amend) and re-applying the
     intended change (a missed `VERSION` bump) as a new forward-only commit (`8b9a0bb`), then
     logging that recovery (`8b9a0bb`'s own review note, commit `95c2c27`).

### Framework Reviewer

- Command: reused the Project Reviewer's `_frw`-side invocations above (full overlap — this
  push's entire substance is framework-level) for `CLAUDE.md`/`docs/framework-maintenance.md`;
  the `.claude/skills/README.md` and `docs/project/CHANGELOG.md` changes are Project-Reviewer-only
  (not Rule-16 paths).
- Repo: same as above.
- Module(s): n/a — framework-level.
- Framework version: `26.08.24:17.26.719`
- Outcome: same `_frw`-side findings/fixes as Project Reviewer above, read through the
  fidelity/ambiguity/enhancement lens — findings 1 and 2 are ambiguity fixes; findings 3 and 4 are
  fidelity gaps (a skill's own procedure drifting from the doc flow it claims to mirror); no new
  enhancement opportunities beyond what was already fixed in place.
- Enhancement suggestions: none.

## 2026-08-24 20:38 — latteMCP: pending commit (this entry's own commit) / commit `2e1ca85` (claude-project-framework) — Clarify FRW-ADR-0011's session-boundary wording for the multi-working-directory case (`FRW-ADR-0012`)

Resolving a proposed proactive inbound-sync trigger (`CR-1787588514684-954f`) surfaced a deeper
ambiguity in `FRW-ADR-0011`'s absolute session-separation wording (it never addressed this tool's
multi-working-directory single-session case). This escalated mid-task from a minor propagation to
a **Full framework change** (Plan Mode) once two independent `/code-review high` Framework
Reviewer passes flagged the same contradiction on two different wording attempts.

### Project Reviewer

- Command: `/code-review high`, run iteratively against the `_frw` clone across six rounds as
  findings surfaced (fix/amend/re-run each time), then once more against latteMCP's own merge diff.
- Repo: `claude-project-framework` @ `a9ba107` → `1741372` → `72f5b92` → `5d12718` → `2e1ca85`
  (pushed); latteMCP (this commit, pending).
- Module(s): n/a — framework-level (new `FRW-ADR-0012`, `FRW-ADR-0011` superseded-note,
  `framework-maintenance.md` reworded, plus five more files carrying the same claim).
- Framework version: `26.08.24:20.29.178`
- Outcome: not clean until the sixth `_frw`-side round; see `_frw/_data/push_reviews.jsonl`
  `REV-0012` for the full per-round detail. Findings and fixes, summarized:
  1. Round 1: the natural Inbound-sync trigger wording ("after this project's own outbound push")
     contradicted `FRW-ADR-0011`'s absolute "never as part of a task in this or any other
     downstream project" language — reverted, escalated to a full framework change instead of a
     second inline patch attempt.
  2. Round 2 (first `FRW-ADR-0012` draft): the reworded clause still described propagation as
     something that "can happen in the very same session as a task in this project," which read as
     sanctioning the exact scenario `FRW-ADR-0011` forbade, plus an unresolved "here" pronoun
     reference and a "write boundary" term not used anywhere else in the file's vocabulary.
  3. Round 3: the same stale absolute wording this ADR was clarifying was found duplicated,
     unfixed, in four more files — `propagate-framework-change/SKILL.md`, `_design/requirements.md`
     (`FRW-REQ-009`), `_design/architecture.md`, `_design/domain-model.md`'s glossary — updated all
     four. Also fixed two accuracy issues inside `FRW-ADR-0012` itself: the quoted phrase was
     misattributed to `FRW-ADR-0011`'s own Decision text (it's actually the implementing doc's
     wording, not the ADR's); and the "unenforceable by Claude itself" rationale for rejecting a
     stricter alternative didn't hold, since working-directory info is visible in a session's own
     environment context — reworded to the real reason (a self-report is no stronger a signal than
     the check the ADR settles on, so it buys no extra safety for the added friction).
  4. Round 4: `FRW-ADR-0011`'s own new Superseded note repeated the same misattribution as finding
     3; `FRW-ADR-0012`'s Decision condition (a) read "entered *after* the classify/approve step,"
     temporally incoherent since classify/approve are steps 1-2 of the very flow being entered;
     `SKILL.md`'s own "has changed shape before" ADR citation list was missing `FRW-ADR-0012`; and
     a third Decision condition ("narrated distinctly to the user") was asserted in the ADR's prose
     but never actually operationalized in any doc a session would read while acting — fixed by
     folding it into condition (a) as a concrete "say so explicitly" instruction, then adding that
     instruction to `framework-maintenance.md` step 2 and `SKILL.md` step 2 for real.
  5. Round 5: the diagram-spec file (`_diagrams/framework_update_flow/framework-update-flow.md`)
     and `_design/domain-model.md`'s Inbound-sync glossary row were missing the new same-session
     sync trigger that every other narration site had gained — added to both; then a further check
     found `_design/architecture.md`'s own Inbound-sync flow (flow 3) still missing it too —
     fixed.
  6. Round 6: clean pass across all ten in-scope files, plus one out-of-scope landmine caught and
     fixed anyway (`.claude/skills/README.md` still carried the pre-`FRW-ADR-0012` absolute
     wording) since it's the same false-positive risk class.
  7. latteMCP-side merge (this commit): confirmed word-for-word merge fidelity against the upstream
     `2e1ca85` diff (no defects); found the new `CHANGELOG.md` bullet pointed to a `review_log.md`
     entry that didn't exist yet — this entry resolves that. Also flagged, not fixed locally: the
     "Framework propagation (inside `_frw`'s own repo only)" section heading is stale relative to
     its own reworded body, but that heading is inherited unchanged from upstream — fixing it only
     in latteMCP's copy would itself create drift, so it was logged instead (see Framework
     Reviewer, Enhancement suggestions below).

### Framework Reviewer

- Command: reused the Project Reviewer's invocations above in full (the entire push is
  framework-level — every changed file is either inside `_frw`'s clone or this project's own
  `docs/framework-maintenance.md` mirror of it).
- Repo: same as above.
- Module(s): n/a — framework-level.
- Framework version: `26.08.24:20.29.178`
- Outcome: same findings/fixes as Project Reviewer above, read through the
  fidelity/ambiguity/enhancement lens — rounds 1-2 are fidelity contradictions (the wording
  didn't match the ADR it was supposed to implement); rounds 3-5 are fidelity gaps (the same claim
  left stale in files that weren't the primary edit target) and one ambiguity fix (the temporally
  incoherent "entered after" phrasing); round 6 closed the loop with no new findings.
- Enhancement suggestions: `CR-1787592628846-4d63` (a downstream-projects registry in `_frw`, so a
  propagation can surface which projects still need to sync — deliberately deferred, not folded
  into this change, per the user's explicit choice), `CR-1787592628857-e01a` (the `FRW-ADR-0012`
  claim itself is independently restated across six-plus files with no single source of truth,
  which is the actual mechanism that produced rounds 3-5's repeated drift findings), and
  `CR-1787593149632-eabe` (the stale "Framework propagation (inside `_frw`'s own repo only)"
  section heading, found during the latteMCP-side merge, inherited unchanged from upstream so not
  fixed locally).

## 2026-08-24 21:05 — latteMCP: pending commit (this entry's own commit) / commit `aaf829b` → `108742e` (cosmetic) (claude-project-framework) — Cap review-fix iteration at 2 rounds; grep proactively on duplicated findings

Triggered directly by the immediately preceding push: the `FRW-ADR-0012` propagation ran 6
unbounded `/code-review high` rounds for a small doc-wording fix, costing roughly half an hour and
a large amount of tokens. User asked for either an optimization or a defined stop/escalate
procedure. Logged as `CR-1787593416032-6ff2`, then applied the same session on explicit "do it
now" — small, self-contained wording addition, executed directly rather than through a full
Plan-Mode round-trip given the user's stated preference to reduce process overhead, per Rule 4.

### Project Reviewer

- Command: `/code-review high`, 2 rounds against the `_frw` clone (the cap this change itself
  defines), then one round against latteMCP's own merge diff.
- Repo: `claude-project-framework` @ `db4158a` → `93522ef` → `aaf829b` (pushed, reviewed) →
  `108742e` (cosmetic line-wrap fix only, not separately reviewed — no content change); latteMCP
  (this commit, pending).
- Module(s): n/a — framework-level (`framework-maintenance.md` Framework-propagation step 4,
  `push-review-gate/SKILL.md` step 7, plus 3 cross-referencing files).
- Framework version: `26.08.24:21.01.945`
- Outcome: not clean until round 2 on the `_frw` side; see `_frw/_data/push_reviews.jsonl`
  `REV-0013` for full detail. Round 1: proactively grepped the repo for the same "fix findings...
  re-run" pattern before editing (applying the very rule being added) and found 5 files carrying
  it, not just the 2 originally planned — fixed all 5 in one pass, making `framework-maintenance.md`
  step 4 canonical and having the other 4 cross-reference it instead of restating it (directly
  addressing `CR-1787592628857-e01a`'s duplication concern). Round 2: found round 1's own
  escape-hatch fix was itself wrong — it misdirected deferred findings to `_data/change_requests.jsonl`
  based on an overly literal reading of that file's schema description, when `push_reviews.jsonl`'s
  own `enhancement_suggestions` field and 3 prior entries (`REV-0005`/`0006`/`0008`) already
  established the correct pattern (log to `change_requests.jsonl`, cross-reference by id) — reverted
  to the established pattern without a 3rd review round, since the fix was unambiguous and
  well-precedented (Rule 4). The latteMCP-side merge found two cosmetic line-wrap mismatches against
  the cited upstream commit (a stray missing line break introduced during round-2's edit, and one
  more in the same section) — fixed by hand to byte-match upstream exactly, and one commit-citation
  error in the CHANGELOG (attributed the reviewed content to `108742e`, an unreviewed cosmetic-only
  follow-up commit, instead of the actually-reviewed `aaf829b`) — corrected.

### Framework Reviewer

- Command: reused the Project Reviewer's invocations above in full (the entire push is
  framework-level).
- Repo: same as above.
- Module(s): n/a — framework-level.
- Framework version: `26.08.24:21.01.945`
- Outcome: same findings/fixes as Project Reviewer above, read through the
  fidelity/ambiguity/enhancement lens — round 1's file-duplication findings are fidelity gaps;
  round 2's escape-hatch reversal is a fidelity fix (matching established schema usage); the
  latteMCP-side line-wrap and citation fixes are fidelity/ambiguity fixes ensuring the local mirror
  byte-matches its cited upstream commit.
- Enhancement suggestions: none.

## 2026-08-25 00:30 — latteMCP: commit `20680f3` / commit `392d6e3` (claude-project-framework) — Sync FRW-ADR-0013 (diff-verification substitution for byte-identical sync merges)

Immediately after the iteration-cap fix (`CR-1787593416032-6ff2`), user asked whether the update
still felt slow and requested further enhancement suggestions. Diagnosed that every framework
change pays for a full `/code-review high` pass at least twice — once in `_frw`, again in the
consuming project's `push-review-gate` for the sync merge — even when the second pass re-reviews
already-reviewed, byte-identical content. `FRW-ADR-0013` adds a diff-verification substitution for
that specific case. This entry logs the push that pulled it into latteMCP — the first real use of
the new substitution, on itself.

### Project Reviewer

- Command: `/code-review high`, scoped to the two Rule-15-only files this push touches (`CLAUDE.md`
  and `docs/framework-maintenance.md` are Rule-16 scope, handled below).
- Repo: `claude-project-framework` @ `db4158a` → `93522ef` → `aaf829b` → `108742e` →
  `c391598` → `392d6e3` (pushed, 2 review rounds — the cap — 3 round-2 findings fixed directly
  without a 3rd round per Rule 4, see `_frw/_data/push_reviews.jsonl` `REV-0014`); latteMCP @
  `20680f3`.
- Module(s): n/a — framework-level (`.claude/skills/push-review-gate/SKILL.md`,
  `docs/project/CHANGELOG.md`).
- Framework version: `26.08.24:21.35.195`
- Outcome: `push-review-gate/SKILL.md` confirmed byte-identical to its upstream counterpart at
  `392d6e3`; `CHANGELOG.md`'s new entry verified accurate against the shared clone (commit exists,
  `CR-1787595516088-a898` resolved, `FRW-ADR-0013` exists) — except its forward reference to this
  review_log.md entry, which didn't exist yet at review time (the entry had not yet been appended
  when the review ran, mid-way through this same push) — now resolved by this entry's own
  existence. One out-of-scope finding, not fixed here: `push-review-gate/SKILL.md` step 6's
  diff-verification instructions don't state the `copy_me/`-prefix / `.template`-suffix path-
  mapping needed to find a file's upstream counterpart — logged as `CR-1787608885289-5e2c` for a
  future `_frw` propagation rather than forked locally, since this skill file must stay
  byte-identical to the synced template.

### Framework Reviewer

- Command: diff-verification substitution (`FRW-ADR-0013`) — `CLAUDE.md`'s Rule 16 section
  confirmed byte-identical to `copy_me/CLAUDE.md.template` at commit `392d6e3`; the only change to
  `docs/framework-maintenance.md` was its own "Bootstrapped from / last synced at" line (the
  explicitly excluded, always-project-filled-in placeholder) — both checks in Rule 16's
  substitution condition confirmed (commit `20680f3` appears in `git log origin/main..HEAD`; the
  cited `_frw` commit matches the "Bootstrapped from" line this same commit just updated). No
  findings possible by construction.
- Repo: same as above.
- Module(s): n/a — framework-level.
- Framework version: `26.08.24:21.35.195`
- Outcome: substitution applied cleanly — first real use of `FRW-ADR-0013` since it was added.
- Enhancement suggestions: `CR-1787608885289-5e2c` (path-mapping clarity in
  `push-review-gate/SKILL.md` step 6, found during the Project Reviewer pass above).
