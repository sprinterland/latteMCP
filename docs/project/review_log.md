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
