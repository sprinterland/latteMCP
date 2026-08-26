# Changelog

Dated log of material changes to requirements, architecture, or accepted decisions. Not a commit
log — only entries that change what a reader of `docs/` would believe about the system. At
scale, split into `CHANGELOG-<year>.md` once this file gets unwieldy, keeping this file as the
current year only and linking older ones from the top.

## 2026-08-26

- Ran `sync-framework-updates` (skill), pulling `claude-project-framework` from commit `392d6e3`
  (version `26.08.24:21.35.195`) to commit `1a7e51f` (version `26.08.26:08.36.235`) — the Phase
  0-3 rollout of the new Analyst/Developer/Reviewer/Tester/Auditor Task Record pipeline. Merged in:
  three new pipeline skills (`file-task`, `analyst-plan-task`, `developer-work-task`), the
  `.claude/skills/_lib/` shared scripts (`append_jsonl.py` — safe append-only JSONL writer plus
  self-generated-id helper; `claim_lock.py` — atomic claim/lock helper for Task Record folders),
  `docs/project/tasks/README.md` (the pipeline's file-layout/schema source of truth),
  `docs/project/task_log.md` and `docs/project/completed_tasks.md` (open/closed Task Record
  indices), a new `docs/dev-practices.md` "Task Classification (Analyst)" section (left as an
  unfilled placeholder — this project hasn't adopted the pipeline yet, so no `Selected:` value was
  invented), `docs/00-index.md` entries for the three new files, and — the fix that triggered this
  sync — the persisted round-check-in counter for the Framework-propagation and `push-review-gate`
  review-fix loops (a scratch state file survives a conversation-context clear between rounds
  instead of silently losing count of "how many rounds since the last check-in", per updated
  `docs/framework-maintenance.md` step 4 and `push-review-gate/SKILL.md` step 7). All sixteen
  changed `copy_me/` files merged cleanly with no ambiguity — the changes were net-new files or
  additive process/structure edits that didn't collide with this project's own customizations
  (`dev-practices.md`'s existing `Selected:` values, `docs/project/`'s own history, the
  "Bootstrapped from" line's prior value). Updated `docs/framework-maintenance.md`'s "Bootstrapped
  from / last synced at" line to cite the new commit/version above.

## 2026-08-24

- Framework change, requested directly by the user: even with the review-fix cap in place, a
  small framework change still felt slow — diagnosed that every framework change structurally
  pays for a full `/code-review high` pass at least twice (once in `_frw` during propagation,
  again in the consuming project's `push-review-gate` for the sync merge), often redundant since
  the second pass re-reviews content already reviewed once, byte-identical to upstream. Rule 16
  (`CLAUDE.md`) now substitutes a plain `diff` against the cited upstream `_frw` commit for the
  full Framework Reviewer pass when a push's entire Rule-16-scoped diff is a verified,
  unambiguous `sync-framework-updates` merge — excluding lines that are always project-filled-in
  placeholders (the "Bootstrapped from" line, `dev-practices.md`'s `Selected:` values) from the
  comparison, since those can never match upstream's generic template by design. Any merge
  needing local adaptation, or a non-sync-driven edit, still gets the full pass. Documented as
  `FRW-ADR-0013`. Propagated into `claude-project-framework` (commit `392d6e3`, 2 review rounds —
  the cap — with 3 mechanical round-2 findings fixed directly without a 3rd round per Rule 4; see
  `docs/project/review_log.md`'s entry below). Resolved `CR-1787595516088-a898`.
- Framework change, requested directly by the user: capped the review-fix loop at 2 rounds
  before checking back in (re-applied at every subsequent check-in, never silently exceeded),
  with a proactive-grep instruction so a duplicated-claim finding gets fixed everywhere in one
  pass instead of one file per round. Added to `docs/framework-maintenance.md`'s
  Framework-propagation step 4 (canonical wording) and `push-review-gate`'s step 7 (independent
  copy, for this project's own pushes); 3 other files that previously restated step 4's text
  (`propagate-framework-change/SKILL.md`, `_design/architecture.md`, the diagram spec) now
  cross-reference it by name instead. Triggered by the immediately preceding FRW-ADR-0012 change
  running 6 unbounded review rounds for a small doc-wording fix (~30 minutes, high token cost) —
  logged as `CR-1787593416032-6ff2`, then applied the same session on explicit request. Propagated
  into `claude-project-framework` (commit `aaf829b`, 2 review rounds — the cap this change itself
  defines — the 2nd round catching a regression in the 1st round's own fix, corrected without a
  3rd round since it matched well-established prior practice; see `docs/project/review_log.md`'s
  entry below), then a cosmetic line-wrap-only follow-up (commit `108742e`, unreviewed — no
  content change). Resolved `CR-1787593416032-6ff2`.
- Framework change, requested directly by the user: while resolving a proposed proactive
  inbound-sync trigger (`CR-1787588514684-954f`), two `/code-review high` Framework Reviewer
  passes found the natural wording for that trigger contradicted FRW-ADR-0011's absolute
  "never as part of a task in this or any other downstream project" language for Framework
  propagation — an ambiguity FRW-ADR-0011 never addressed: this tool supports a single
  conversation holding several working directories at once, so a downstream-project task can
  legitimately `cd` into the local `_frw` clone (an additional working directory) and run
  Framework propagation directly. FRW-ADR-0012 clarifies (does not reverse) FRW-ADR-0011: the
  working-directory requirement is about the cwd of the propagation commands themselves, not
  about the conversation having no other directory open, provided propagation is only ever
  entered through the Maintenance rule's own classify/approve steps. Reworded
  `docs/framework-maintenance.md`'s "Since FRW-ADR-0011" paragraph and the Framework propagation
  flow's opening/step-2 text accordingly, and added the originally-proposed Inbound-sync trigger
  now that the contradiction is resolved: sync can run immediately after this same conversation
  completes a Framework-propagation push, instead of only on demand/periodically. Propagated into
  `claude-project-framework` (commit `2e1ca85`, after six Framework Reviewer passes — see
  `docs/project/review_log.md`'s entry below); this project's own `docs/framework-maintenance.md`
  synced to match and its "Bootstrapped from / last synced at" line updated. Resolved
  `CR-1787588514684-954f`; deferred two related enhancement ideas surfaced along the way as
  `CR-1787592628846-4d63` (a downstream-projects registry in `_frw`, so a propagation can surface
  which projects still need to sync) and `CR-1787592628857-e01a` (the FRW-ADR-0012 wording itself
  is independently restated across six-plus files with no single source of truth, which is what
  produced the repeated drift findings across review passes).
- Framework change, requested directly by the user (who asked which recurring `CLAUDE.md`
  processes could become Claude Code skills for faster, more consistent execution): consolidated
  everything a bootstrapped project receives under one new `copy_me/` folder in the shared `_frw`
  repo (`CLAUDE.md.template`, `PLAN.md.template`, `docs/`, moved via `git mv`) instead of an
  implicit allowlist, and added 8 process skills encoding this framework's own repeated
  procedures. 6 project-usage skills (`push-review-gate`, `log-change-request`,
  `discovery-iteration`, `new-module`, `new-adr`, `new-api-operation`) now live at
  `copy_me/.claude/skills/` in `_frw` and are installed in this project at `.claude/skills/`; 2
  framework-development skills (`bootstrap-project`, `propagate-framework-change`) live only at
  `_frw`'s own root `.claude/skills/`, never copied into a project. Each skill re-reads its own
  source-of-truth rule/doc live at invocation time rather than freezing a copy of the procedure.
  `docs/framework-maintenance.md`'s "What `_frw` is" section updated to describe the new split;
  `FRW-ADR-0010` added. Propagated into `claude-project-framework` (commit `6b9b07d`, version
  `26.08.24:15.00.729`). Three Rule 15/16 review passes (one after the initial restructuring, one
  after fixing its findings, one independent multi-agent pass) found and fixed: `_design/`'s own
  self-description (`00-index.md`, `architecture.md`, `domain-model.md`, `requirements.md`,
  `test-spec.md`, a broken relative link in `decisions/README.md`) left describing the old flat
  layout; the `new-module`/`new-api-operation` skills assuming a local module template that
  bootstrapping actually renames away after first use (fixed to fall back to the shared `_frw`
  clone's copy); `log-change-request` hardcoding the `severity` enum against its own stated
  live-re-read principle; `push-review-gate` restating Rule 16's trigger-path list inside a
  "re-read live" step; and an ambiguous tree label where two differently-scoped `.claude/skills/`
  folders were both shown unqualified. One simplification idea (the skill roster is now listed in
  four separate index files with no single source of truth) deferred as
  `CR-1787573917472-b195`. See `docs/project/review_log.md`'s entry below and `_frw/_data/
  push_reviews.jsonl` for the full list.

- Framework fix, requested directly by the user: the Maintenance rule's step 1-2 approval (asking
  about a framework-level change, then drafting and getting sign-off on a written plan) split into
  two tiers. A **minor propagation** — confined to a single file (a project doc and its identical
  `_frw` mirror count as one; a lone `_frw`-only file with no project-side counterpart, e.g. a
  single `_design/` doc, likewise counts as one), a wording clarification/single field/comparably
  small self-contained edit, no new rule/section/file or change to an existing rule's meaning, and
  no architectural/security/business-rule implication — now combines the ask and the plan into one
  message (the diff itself, one yes/no) instead of two separate round-trips. Anything else, or any
  doubt about the classification, still goes through the full ask-then-written-plan sequence.
  Steps 3-6 (apply, propagate + version bump, Rule 15/16 review, logging) are unchanged. Propagated
  into `claude-project-framework`'s `docs/framework-maintenance.md` (commit `1374628`, version
  `26.08.24:00.06.658`); `FRW-ADR-0009` added and `FRW-ADR-0008` marked superseded (its own text
  left as-written per `_design/`'s never-edit-a-past-ADR rule); `_design/requirements.md`,
  `architecture.md`, `test-spec.md`, and `domain-model.md` updated to match. A Rule 15/16 review
  (6 parallel finder agents across 3 invocations) found and fixed real gaps, and deferred two
  design-level concerns as logged change requests — see `docs/project/review_log.md`'s entry below
  and `_frw/_data/push_reviews.jsonl` `REV-0007` for the full list.
- Framework change, requested directly by the user (who asked whether latteMCP tracks `_frw`'s
  version, then whether it gets updated on each framework push, then asked to restrict outbound
  writes and add a missing inbound-sync mechanism): the Maintenance rule let a project's session
  write anywhere in the shared `_frw` repo (`copy_me/`, `VERSION`, push) as part of one
  project-driven procedure, and had no way for a project to pull in a framework change it didn't
  originate. Split the rule into three named flows — Outbound (a project's only write into `_frw`
  is now `_data/change_requests.jsonl`, nothing else), Framework propagation (runs only inside
  `_frw`'s own repo, never orchestrated from a project session), Inbound sync (new: the
  `sync-framework-updates` skill pulls `copy_me/*` and merges structural changes into a project's
  own `CLAUDE.md`/`docs/`/`.claude/skills/`, logging a change request **and** stopping to ask the
  user on any ambiguous merge rather than guessing). The old "apply to the requesting project's own
  files first" step is gone — a project now always receives a framework change via its own inbound
  sync, whether or not it originated it. Propagated into `claude-project-framework` (commit
  `95c2c27`, version `26.08.24:17.26.719`); `FRW-ADR-0011` added; `_design/requirements.md`,
  `architecture.md` (new Inbound-sync flow, renumbered push-gate flow), `domain-model.md`, and
  `test-spec.md` updated to match. A review (redone after an initial `/code-review` invocation
  mis-scoped to latteMCP's own repo instead of `_frw`) found and fixed a confusing requirement
  reading-order splice, a Standing-rule sentence readable as gating change-request logging on
  asking first, and a step-count mismatch between `propagate-framework-change` and the doc it
  orchestrates — see `docs/project/review_log.md`'s entry below and
  `_frw/_data/push_reviews.jsonl` `REV-0009`. latteMCP mirrored the `CLAUDE.md`/
  `docs/framework-maintenance.md` changes and added `sync-framework-updates` to its own
  `.claude/skills/`. A follow-up minor propagation (commit `4721d99`) then fixed
  `sync-framework-updates` itself, which had shipped with a 7-step procedure instead of the
  5-step "Inbound sync" flow it mirrors — same class of drift as the `propagate-framework-change`
  fix above, caught by a targeted review. During that recovery, an attempted amend of the
  already-pushed `4721d99` was caught before force-pushing and fixed forward instead (`8b9a0bb`
  bumped the `VERSION` that commit had missed; `95c2c27` logged the review entry) — no history was
  rewritten on the shared remote.

## 2026-08-23

- Added `_design/` to the shared `claude-project-framework` repo: the framework's own design
  rationale (`requirements.md`, `domain-model.md`, `architecture.md`, framework-level ADRs
  `FRW-ADR-0001`–`0008` in `decisions/`, an annotated rule-by-rule spec for `CLAUDE.md.template`,
  an annotated section-by-section spec for `PLAN.md.template`, and a manual verification
  checklist in `test-spec.md`) — enough, per its own `FRW-REQ-005`, to recreate the bundle from
  scratch if every template file were lost. Like `_data/`, `_design/` is never copied into a
  bootstrapped project; `README.md` and `docs/framework-maintenance.md`'s file-tree listings
  (both repos) were updated to mention it. Also formalized a new Maintenance-rule step 2 requiring
  a written, user-approved plan before any framework-level change is implemented (`FRW-ADR-0008`),
  renumbering the rule's later steps (old 2–5 → new 3–6) in both `docs/framework-maintenance.md`
  copies and fixing every "Maintenance rule step N" cross-reference accordingly. `_frw` synced to
  commit `01ae9df`, version `26.08.23:19.50.336`.
- Resolved the ADR-for-framework-changes question flagged (but never actually asked) in
  `review_log.md`'s 2026-08-23 14:42 entry: asked the user directly — no, a framework-level
  process change does **not** get its own ADR; `docs/framework-maintenance.md`,
  `docs/project/CHANGELOG.md`, and `docs/project/review_log.md` already fully capture what
  changed, why, and how it was reviewed, so a fourth record would just duplicate that. Encoded as
  a carve-out in `CLAUDE.md`/`CLAUDE.md.template` Rule 11 (mirrored, word-for-word identical
  except the project-specific confirmation-date parenthetical, per the usual fidelity check).
  Also fixed an unresolvable instruction the original Rule 17 review had flagged (Simplification
  finding #2) and the first fix-up round missed: `docs/framework-maintenance.md` said to "mark a
  stale entry resolved" while also declaring the three `_data/*.jsonl` logs append-only/never-
  rewritten. Defined the mechanism: "resolved" means appending a **new** line with the same `id`
  and updated `status`/`resolution`/`resolved_at`, never editing the original line — a reader
  takes the latest line per `id`. Resolved `change_requests.jsonl`'s own
  `CR-1787501279766-s9t0` entry (the ADR question above) using exactly this mechanism, as a live
  test of the new rule. Committed to both repos as `claude-project-framework` commit `461bd5e` /
  latteMCP commit (this entry's own), bumping `VERSION` to `26.08.23:19.14.972`.
- Added three append-only JSON-Lines activity logs under `claude-project-framework`'s `_data/`
  folder (framework-level change, applies to future projects too — user-requested, not proposed
  by Claude): `update_history.jsonl` (one record per completed framework update),
  `push_reviews.jsonl` (one record per Rule 15/16 review run immediately before an `_frw` push),
  and `change_requests.jsonl` (a continuous backlog of framework-enhancement ideas, logged the
  moment they're noticed during *any* activity in *any* project, per new `CLAUDE.md` Workflow
  Rule 17 — not just during a Rule 16 review). Documented all three schemas in
  `docs/framework-maintenance.md`'s new "Framework activity logs" section (mirrored into `_frw`'s
  own copy) and restructured the Maintenance rule so the pre-push Rule 15/16 review happens
  between the local propagation commit and the actual push, appending to `push_reviews.jsonl`
  along the way; step 5 (was step 4) now also appends to `update_history.jsonl`. Added `CLAUDE.md`
  Rule 17 (mirrored into `CLAUDE.md.template`, kept word-for-word identical per the usual
  fidelity check) and updated Rule 16's enhancement-suggestions handling to cite
  `change_requests.jsonl` `id`s from `review_log.md` instead of restating suggestion text, so the
  two logs can't drift apart. Committed to both repos as `claude-project-framework` commit
  `0808624` / latteMCP commit `dfebaec`, bumping `VERSION` to `26.08.23:18.59.204`.
  - A follow-up `/code-review high` pass (8 finder agents) on that same addition — run before
    pushing, per Rule 15/16 — found the design had real gaps, ironic given what it was building:
    `change_requests.jsonl`'s read-last-line-then-increment ID scheme was justified as safe by
    "never written concurrently," which Rule 17 itself directly contradicts (it fires from any
    project at any time); the Maintenance rule's review and push were split into two steps with no
    guidance for the amend-and-re-review case, letting a reviewed-clean commit sit unpushed
    indefinitely; the Versioning section still said VERSION is bumped "as step 3... and commit +
    push" after step 3 was rewritten (same commit) to commit-only; `review_log.md`'s live preamble
    wasn't updated for Rule 16's new id-citation requirement; `CLAUDE.md`'s Standing Rule restated
    the Maintenance rule's steps in prose — exactly the kind of restatement that just went stale;
    and `_data/` kept its leading underscore right after `discovery/`/`project/` lost theirs for
    "hidden/system" reasons, with no reconciling note. Fixed all six: `change_requests.jsonl` ids
    are now self-generated (`CR-<epoch-ms>-<hex>`, no coordination needed) while
    `update_history.jsonl`/`push_reviews.jsonl` keep plain counters (genuinely one-at-a-time,
    written mid-Maintenance-rule); merged the review and push back into one Maintenance-rule step
    with amend guidance; fixed the Versioning section, `review_log.md`'s preamble (both repos), and
    `CLAUDE.md`'s Standing Rule (now points at `docs/framework-maintenance.md` as the sole source
    of truth instead of restating it); and documented `_data/`'s underscore as meaning "excluded
    from bootstrap copying," a different, still-live reason than the one the rename removed. Two
    lower-severity findings — Rule 16(c)/17's circular cross-references, and a
    previously-flagged-but-never-actually-asked question (`review_log.md`, 2026-08-23 14:42 entry)
    about whether framework-level process changes need an ADR — were logged **open** in
    `change_requests.jsonl` rather than resolved unilaterally. Fixes committed as
    `claude-project-framework` commit `9cea998` / latteMCP commit `738ee4b`, bumping `VERSION` to
    `26.08.23:19.09.808`. First real entries logged in all three `_data/*.jsonl` files for this
    update per the newly-added process (see `_frw/_data/update_history.jsonl` id `UPD-0001`,
    `push_reviews.jsonl` id `REV-0001`) — the folder-rename update earlier this same day predates
    this logging system and wasn't backfilled, per the user's own framing that this update could be
    the first record.
- Renamed `docs/_discovery/` to `docs/discovery/` and `docs/_project/` to `docs/project/`
  (framework-level change, applies to future projects too), dropping the leading underscore from
  both docs subfolders so they read as ordinary content folders rather than hidden/system ones.
  Added `_data/` at the `claude-project-framework` bundle's root (a new top-level folder, sibling
  to `docs/`, not nested inside it) to hold `_frw`'s own framework-update notes — per the "What's
  project-specific vs. framework, precisely" section, explicitly excluded from what gets copied
  into a bootstrapped project (see `README.md`'s bootstrap step 2 and "What's in this bundle"
  tree). Renamed the two folders with `git mv` in both this repo and `claude-project-framework`
  (commit `9512282` in `claude-project-framework`, bumping `VERSION` to `26.08.23:18.23.266`), and
  updated every forward-looking reference across both repos to the new paths: `CLAUDE.md` /
  `CLAUDE.md.template` (Documentation Structure tree, Rules 2/6/7/10/15), `PLAN.md` /
  `PLAN.md.template`, `docs/00-index.md`, `docs/dev-practices.md`, `docs/framework-maintenance.md`
  (including the "Bootstrapped from / last synced" fact, bumped to commit `9512282`),
  `docs/decisions/0005-api-docs-openapi-per-operation-samples.md`, `docs/discovery/debt_log.md`,
  and `claude-project-framework`'s own `README.md`. Left this file's, `review_log.md`'s, and
  `completed_plan.md`'s own past dated entries untouched (in both repos) — they're historical
  record of what was true when written, per this file's own "don't edit a past entry" convention;
  only the physical file rename (`git mv`) applies retroactively, not the path text inside old
  entries.
- Flattened `_frw`'s internal `_frw/` wrapper folder up to the `claude-project-framework` repo's
  own root (framework-level change, applies to future projects too): that repo's root now **is**
  the bundle (`README.md`, `VERSION`, `CLAUDE.md.template`, `PLAN.md.template`, `docs/` all live
  directly at its root) instead of being nested one level down inside a `_frw/` subfolder —
  redundant now that the repo is entirely dedicated to being the framework bundle, with no
  surrounding project `docs/` to distinguish it from the way there was when this same content
  lived inside a project repo. Moved every file with `git mv` (commit `a644991` in
  `claude-project-framework`, bumping `VERSION` to `26.08.23:17.49.988`) and deleted the now-empty
  `_frw/` folder. Re-evaluated and fixed every literal `_frw/<path>` slash-notation reference
  across both repos, since none of those paths exist any more (nothing lives under a `_frw/`
  subfolder anywhere): `CLAUDE.md`'s "Reusable Framework Template" section and Rules 15/16,
  `docs/framework-maintenance.md` (full rewrite — the "What `_frw` is" tree diagram un-nested, the
  local-clone note updated, the "Bootstrapped from / last synced" fact bumped to commit `a644991`),
  `docs/dev-practices.md`, `docs/00-index.md`, and `docs/_project/review_log.md`'s live format
  description (not its past dated entries, which are historical record of what was true when
  written and were left untouched per this file's own "don't edit a past entry" convention).
  Mirrored the same fixes into the framework bundle's own copies of these files
  (`claude-project-framework`'s `README.md`, `CLAUDE.md.template`, `docs/framework-maintenance.md`,
  `docs/00-index.md`, `docs/_project/review_log.md`, `docs/dev-practices.md`) — confirmed
  `CLAUDE.md`'s "Reusable Framework Template" section and Rules 15/16 stay word-for-word identical
  to `_frw`'s `CLAUDE.md.template` after the edits. Kept `_frw` itself as the informal name this
  project's docs use for the shared framework repo — that naming convention is a project-doc
  choice, unaffected by the framework repo's own internal file layout; only the slash-path
  notation implying a subfolder was retired.
- Moved `_frw/` out of this repo entirely (framework-level change, applies to future projects
  too): it now lives in its own public GitHub repo,
  [`sprinterland/claude-project-framework`](https://github.com/sprinterland/claude-project-framework)
  (initial commit `243a06b` carried `_frw/VERSION` `26.08.23:14.54.273` unchanged from the last
  in-repo state; a follow-up commit `a1c86e4` then updated the template docs themselves — see
  below — bumping `VERSION` to `26.08.23:17.26.865`), rather than being vendored into any one
  project's repo and later copied-then-deleted at bootstrap time. This lets multiple projects on
  this machine bootstrap from, and propagate framework-level changes into, the same shared copy
  instead of each keeping (or not keeping) its own drifting copy. Removed `_frw/` from this repo.
  Rewrote `docs/framework-maintenance.md`'s "What `_frw/` is" and "Versioning" sections to describe
  the external repo/clone model (live-read `_frw/VERSION` from the local clone at review time,
  static "bootstrapped from" fact as fallback — recorded here as commit `a1c86e4`, version
  `26.08.23:17.26.865`, 2026-08-23) and updated the Maintenance rule's step 3 to commit + push
  instead of just commit. Updated `CLAUDE.md`'s "Reusable Framework Template" section, Rule 15
  (VERSION lookup no longer keyed to whether a project "still keeps its own `_frw/`" — it never
  does now; keyed to whether the shared clone is reachable instead), and Rule 16 (dropped "anything
  under `_frw/`" from its push-trigger list, since no push from this repo can touch a location
  outside it; added a note that a propagation edit made directly in the shared `_frw/` repo is
  reviewed there, on its own terms, not as part of this repo's Rule 16). Updated `PLAN.md`'s
  `_frw/**` mention to name the actual framework doc paths instead, since `_frw/**` can no longer
  be part of a push touching this repo. Mirrored the same conceptual changes into the framework
  bundle itself: `_frw/README.md`'s bootstrap steps 1–3 (no longer "copy the whole `_frw/`
  directory in, then delete it" — now "copy just the individual template files out of the shared,
  external `_frw/`; never vendor the folder itself"), `_frw/CLAUDE.md.template` (same three edits
  as this project's `CLAUDE.md`, genericized), `_frw/docs/framework-maintenance.md` (same
  restructure, genericized with placeholder repo/commit/version fields for the next project to
  fill in at its own bootstrap time), `_frw/docs/00-index.md`, and
  `_frw/docs/_project/review_log.md` (both had conditional wording keyed to "if this project still
  keeps/spins off its own `_frw/`," now dropped since the shared-repo model applies unconditionally
  going forward). Rule 15's own secondary review of this change (`/code-review high`, logged in
  `docs/_project/review_log.md`) flagged that `docs/dev-practices.md`'s bare `_frw/` references
  (its own "no `_frw/` update needed" line, pointing to `_frw/docs/dev-practices.md` for the full
  settings menu) hadn't been touched to reflect the new external/shared model — fixed by noting,
  the first time `_frw/` is mentioned in that file, that it's the external framework clone/repo
  described in `docs/framework-maintenance.md`. The Framework Reviewer lens (Rule 16, same
  invocation, re-read for `_frw/CLAUDE.md.template` fidelity) caught one more: `CLAUDE.md`'s
  "Reusable Framework Template" section still said `_frw/` lives in a shared "external directory"
  with a "concrete external path" — leftover wording from before `claude-project-framework` existed
  as an actual GitHub repo — while `docs/framework-maintenance.md` and the template already said
  "repo"/"location"; reworded to match, so `CLAUDE.md` and `_frw/CLAUDE.md.template` are now
  word-for-word identical in this section as intended. Also found, on a closer pass while fixing
  the above: this project's own `docs/_project/review_log.md` (not just its `_frw/` template
  counterpart) still described the Framework Reviewer's `_frw/VERSION` fallback as keyed to
  whether "this project has since deleted its own `_frw/`" — fixed to match Rule 15's live-clone
  fallback wording.
- Applied Rule 15/16 to the commit that fixed their own first-review findings (`dbbc48d`), logged
  in `docs/_project/review_log.md`. That round's fixes hadn't fully landed: `docs/dev-practices.md`
  still re-enumerated Rule 16's path list in its Framework Reviewer bullet (only the Project
  Reviewer bullet had been de-duplicated), which also meant the prior CHANGELOG entry's claim of
  removing that duplication was inaccurate at the time — completed now, mirrored in
  `_frw/docs/dev-practices.md`. Also fixed: Rule 16 listed `docs/dev-practices.md` as an
  unconditional trigger, contradicting `CLAUDE.md`'s own framing that flipping a `Selected:` value
  is an ordinary change needing no `_frw/` involvement — narrowed to trigger only on a structural
  change to that file's policy description, not a plain setting toggle. Added the VERSION-fallback
  guidance (for a project that bootstrapped from this framework and later deleted its own `_frw/`)
  to `CLAUDE.md` Rule 15 itself, matching what the `_frw/` template already had. Fixed a dangling
  unmatched `)` in `docs/framework-maintenance.md`'s Versioning section (and its `_frw/` mirror)
  left over from an earlier edit. Bumped `_frw/VERSION` to `26.08.23:14.54.273` — the prior
  round's fixes had touched `_frw/` without bumping it, the exact drift the bump rule exists to
  prevent. Corrected the prior round's own `review_log.md` entry, which had internal
  inconsistencies (a wrong sub-agent count, a findings/fixed tally that didn't match its own
  enumerated list, and a self-contradictory claim of both reusing and separately re-running a
  review) — replaced the unverifiable precise counts with a plain description of what was found
  and fixed, and updated the entry field description to do the same going forward. Also revisited
  (rather than re-affirmed) the open "should this be an ADR" question from the prior round: the
  original "follows precedent" reasoning had overlooked that ADR-0005 was itself a documentation/
  process convention, so this is left as a genuinely open question the user may want to weigh in
  on, not a settled one.
- Applied the freshly-split Rule 15/16 review to the commit that created them (`8e46a87`), logged
  in `docs/_project/review_log.md`'s first real entry. Fixed what it found: Rule 15/16's scope
  was redefined from two separately-enumerated allowlists (which already disagreed with each
  other and would have left future top-level docs like a hypothetical `docs/security.md`
  unreviewed by either reviewer) to a complementary split — Rule 16 keeps its explicit path list,
  Rule 15 is now "everything else in the push," exhaustive by construction; added
  `docs/dev-practices.md` to Rule 16's trigger list (its own canonical description of this
  policy, so a change there can itself need `_frw/` propagation); gave Rule 16 the same "doesn't
  loosen Rule 2" disclaimer Rule 15 already had; added a reuse clause letting Rule 16 read Rule
  15's own invocation instead of mandating a second full pass when scopes overlap; fixed Rule
  15's pointer to a nonexistent "header" in `_frw/VERSION` (the format lives in
  `docs/framework-maintenance.md`'s "Versioning" section, repeated in `_frw/docs/dev-practices.md`
  with the same bug); resolved a contradiction over whether a skipped Framework Reviewer gets its
  own `review_log.md` sub-entry (it does now, always, stating "not run" and why); fixed leftover
  singular "a secondary review gates every push" wording in `CLAUDE.md`'s diagram and both
  `00-index.md` files that the original split commit missed; fixed a one-space diagram-column
  misalignment; clarified the `YY.MM.DD:HH.MM.FFF` format's two `MM` tokens (month vs. minute);
  fixed lowercase `plan.md` reintroduced in `_frw/docs/dev-practices.md`'s and
  `_frw/docs/_project/review_log.md`'s new text; and removed duplicated scope-path enumeration
  from `docs/dev-practices.md` and both `review_log.md` files in favor of pointing at `CLAUDE.md`
  Rule 15/16 as the single source of truth. Three lower-stakes findings were logged as enhancement
  suggestions rather than applied (see `review_log.md`'s Framework Reviewer sub-entry for that
  commit): `_frw/VERSION`'s millisecond precision, whether this kind of change should get an ADR,
  and whether `dev-practices.md` should support a partial (project-only) reviewer selection.
- Added `_frw/VERSION` (format `YY.MM.DD:HH.MM.FFF`, a last-changed timestamp rather than a
  semantic version — documented in `docs/framework-maintenance.md`'s new "Versioning" section)
  and split the single secondary-reviewer gate into two (framework-level change, applies to
  future projects too): `CLAUDE.md` Rule 15 (**Project Reviewer** — `/code-review high` scoped to
  module docs, ADRs, traceability, discovery/debt logs, `PLAN.md`, and source code; always runs)
  and new Rule 16 (**Framework Reviewer** — `/code-review high` scoped to `CLAUDE.md`, `_frw/**`,
  `docs/framework-maintenance.md`, `docs/migrations.md`; checks framework/template fidelity and
  ambiguity, and raises enhancement suggestions as proposals only, never auto-applied; runs only
  when the push touches those paths). Added `docs/_project/review_log.md` (and its `_frw/`
  template) so every reviewer run is logged separately with its command/scope, repo/commit,
  module(s) touched, and the `_frw/VERSION` value at run time, for later investigation. Updated
  `docs/dev-practices.md`'s "Secondary Review Before Push" setting to describe the two-reviewer
  gate (still **Yes**, adopted 2026-08-23, split 2026-08-23), mirrored generically into
  `_frw/docs/dev-practices.md`, and updated `CLAUDE.md`'s Documentation Structure diagram,
  "Development practices" section, `docs/00-index.md`/`_frw/docs/00-index.md`, and
  `_frw/README.md` (bundle listing plus a new bootstrap step recording the framework version a
  new project started from, since `_frw/VERSION` itself won't survive that project deleting its
  `_frw/` copy) accordingly.
- Applied Rule 15's own first secondary-reviewer pass (`/code-review high`) to the two unpushed
  commits below before pushing, and fixed what it found: `CLAUDE.md`/`_frw/CLAUDE.md.template`
  listed Rule 8 among the rules governed by `dev-practices.md` even though Rule 8 never
  references that file (corrected to "Rules 9, 14, and 15" in `CLAUDE.md`, `docs/dev-practices.md`,
  `_frw/docs/dev-practices.md`, and `_frw/README.md`); `_frw/docs/dev-practices.md`'s new
  Secondary Review setting had labeled **No** as the default when Rule 15 actually defaults to
  **Yes** (swapped); both `00-index.md` files were missing the new setting from their
  `dev-practices.md` summary bullet (added); `_frw/docs/modules/_module-template/interfaces/
  README.md` linked `CLAUDE.md.template` instead of the post-bootstrap `CLAUDE.md` filename
  (fixed); the CHANGELOG's own "Shrunk `CLAUDE.md`..." entry below misstated the line counts as
  372→269/-28% when the actual figures are 296→269/-9% (corrected); `docs/_discovery/
  discovery_plan.md` and its `_frw/` counterpart had silently dropped the "document dependencies
  just enough to unblock, come back for full depth later" instruction when Track A guidance was
  extracted from `CLAUDE.md`, replacing it with an unrelated `debt_log.md` criterion instead of
  alongside it (restored, keeping both conditions); and `CLAUDE.md`/`_frw/CLAUDE.md.template` used
  lowercase `plan.md` in ~10 prose references to the root plan file while every other doc in the
  tree (and the actual filename) uses `PLAN.md` (corrected, leaving the deliberately-lowercase
  `docs/modules/<module>/plan.md` module-local reference untouched). Also added one line to Rule
  15 clarifying it doesn't override Rule 2 — a review finding that's itself ambiguous, hard to
  reverse, or touches security/architecture/business rules still gets asked about, not logged and
  pushed past.
- Added a new "Secondary Review Before Push" setting to `docs/dev-practices.md` (framework-level
  change, applies to future projects too): before any `git push`, run `/code-review high` against
  the outgoing commits as an independent secondary pass distinct from the authoring work, and fix
  or explicitly log any findings first. Adopted for this project: **Yes, every push**,
  self-enforced (no pre-push git hook — procedural, matching the existing recommend-don't-block
  pattern). Added `CLAUDE.md` Workflow Rule 15 governing it, updated the "Development practices"
  section and Documentation Structure diagram's `dev-practices.md` blurb to mention it, and added
  a note to `PLAN.md` that completed-task entries in `docs/_project/completed_plan.md` should
  record the review outcome. Mirrored the setting (unselected, `Selected: <fill in>`) into
  `_frw/docs/dev-practices.md`, the same `CLAUDE.md` changes into `_frw/CLAUDE.md.template`, and
  added it to `_frw/README.md`'s bootstrap step 6 checklist of settings to decide before real work
  starts.
- Restructured the repo root so only `CLAUDE.md` and `PLAN.md` remain there: moved
  `CHANGELOG.md` and `completed_plan.md` into `docs/_project/`, updating every cross-reference
  (`CLAUDE.md`'s Documentation Structure/rules 7/10, `docs/00-index.md`, `docs/_discovery/
  debt_log.md`, `PLAN.md`'s links to `completed_plan.md`).
- Added a new `_frw/` at the repo root: a genericized, self-contained copy of the documentation
  philosophy for bootstrapping future projects — `CLAUDE.md.template`, `PLAN.md.template`, a
  `README.md` explaining how to use the bundle, and a `docs/` tree mirroring this project's
  structure with every file reduced to placeholders/process text (no latteMCP-specific facts),
  including a single `modules/_module-template/` in place of the three real modules. Added a new
  "Reusable Framework Template (`_frw/`)" section to `CLAUDE.md` defining the project-vs-
  framework split and the standing rule that any future *philosophy/framework*-level change
  (as opposed to an ordinary project-specific doc change) requires asking the user first, then
  propagating the approved change into both `CLAUDE.md` and `_frw/CLAUDE.md.template` together.
- Renamed `_docs/` to `_frw/` (both the directory and every reference to it in `CLAUDE.md` and
  `docs/_project/CHANGELOG.md`) to avoid confusion with the real `docs/` folder.
- Added `docs/dev-practices.md`: a new configurable-process-decisions doc (test-writing timing,
  whether automated tests gate `Confirmed`, local verification requirements), mirrored generically
  at `_frw/docs/dev-practices.md` with the full menu of options and no selection made. Adopted for
  this project: **TDD (test-first)**, **automated tests required for `Confirmed`**, and **local
  test suite run required whenever tests exist for the touched area** — all stricter than the
  test-after/manual-verification practice `latteAPI`/`latteMCP` (Phase 1/2) actually followed.
  Per the file's transition note, their existing `Confirmed` status is not retroactively revoked;
  Phase 3 (`latteMCPclient`) is the first module built under the new policy from the start (see
  `PLAN.md`). Updated `CLAUDE.md`: added `dev-practices.md` to the Documentation Structure diagram
  and a new "Development practices" subsection, amended Workflow Rule 9 to note the
  `dev-practices.md` override, and added Workflow Rule 14 governing test-timing/local-verification
  process. Mirrored all of these into `_frw/CLAUDE.md.template`, `_frw/docs/00-index.md`, and
  `_frw/README.md`.
- Shrunk `CLAUDE.md` from 296 to 269 lines (-9%) by extracting its three largest,
  rarely-relevant-per-task sections into dedicated docs, since `CLAUDE.md` (unlike `docs/`) is
  injected into every conversation's context regardless of what the task actually is, while
  `docs/` files are already read selectively per Workflow Rule 1 — no information was removed,
  only relocated to where it's read on demand instead of paid for on every turn:
  - Track A's full per-operation process (topology scan, module ordering rationale, the loop
    itself, batching guidance, ambiguity handling) moved into `docs/_discovery/discovery_plan.md`
    (merging with — not duplicating — what that file already tracked); `CLAUDE.md`'s Track A
    section is now a short pointer.
  - The `_frw/` structure diagram, the project-vs-framework distinction, and the 4-step
    maintenance procedure moved into new `docs/framework-maintenance.md`; `CLAUDE.md` keeps only
    the one-paragraph summary and the standing "ask first" rule inline, since that part must stay
    always-visible to be followed proactively.
  - The full Migrations & Rewrites process moved into new `docs/migrations.md`; `CLAUDE.md` keeps
    a two-sentence pointer.
  Mirrored `docs/_discovery/discovery_plan.md`'s merge, `docs/framework-maintenance.md`, and
  `docs/migrations.md` into `_frw/` (generic versions), added all three to both `00-index.md`
  files' Cross-cutting list and `_frw/README.md`'s bundle listing, and resynced
  `_frw/CLAUDE.md.template` — which now needs no genericization edits at all, since the two
  remaining latteMCP-specific mentions were both inside the extracted framework-template section.
  Further reduction (smarter per-task doc selection instead of reading whole module folders) is
  a separate follow-up, not addressed by this change.

## 2026-08-22

- Initial documentation structure created (v2: module-sharded, discovery-tracked).
- Replaced `example-module` with three real modules — `latteAPI`, `latteMCP`, `latteMCPclient`
  — each with a full requirements/domain-model/architecture/interfaces/test-spec set (all
  `Draft`, pending confirmation).
- Added ADR-0001 through ADR-0004 (all `Accepted`), covering: JWT-based waitress identity
  (accounts and signing key in settings, 4h expiry); login as a plain REST endpoint on
  `latteMCP` rather than an MCP tool; stateless bearer-token pass-through in `latteMCP` (no
  session cache); and `latteMCP`'s health check verifying `latteAPI` reachability.
- Updated `docs/architecture/overview.md` (As-Is component map and data flow for the three
  apps) and `docs/glossary.md` (Waitress, Menu Item, Order, Bearer Token, MCP Tool, MCP
  Session).
- Root `plan.md` (`PLAN.md`) rescoped to active work only — design rationale that previously
  lived there has moved into the module docs and ADRs above.
- Upgraded documentation scaffolding from v2 to v3 (`CLAUDE.md` and `docs/_discovery/`):
  replaced the v2 hard gate ("no planning/code for a module until its docs are `Confirmed`")
  with the Two Concurrent Tracks model — Track A (Discovery, backfill) and Track B
  (Development, non-blocking: flag `Draft` docs, but document whatever is touched regardless).
  Split `_discovery/coverage.md` (was a single reconstruction tracker) into three files:
  `coverage.md` (module-level rollup), `discovery_plan.md` (operation-level backlog — populated
  with a Phase 0 topology scan noting this repo has no implemented operations yet to
  reconstruct), and `debt_log.md` (new, currently empty — flags deferred during Track B work).
  Reshaped root `plan.md` into the v3 template's Status/In Progress/Up Next/Open Questions
  structure. Re-applied `docs/architecture/overview.md` and `docs/00-index.md`'s module table,
  which had reverted to template placeholders during the v3 file sync.
- Implemented Phase 1 (`latteAPI`): `GET /health`, `GET /menu`, `POST /auth/login`,
  `POST /orders`, `GET /orders/{id}`, `GET /orders` (API-REQ-001–006), JWT bearer auth per
  ADR-0001, `Order.CreatedBy` stamping (API-RULE-002), server-computed totals (API-RULE-001).
  Removed the weather-forecast template. All endpoints and error paths (401/400/404) verified
  manually; automated tests not yet written. Brought `docs/modules/latteAPI/requirements.md`,
  `domain-model.md`, `interfaces.md`, `architecture.md` from `Draft` to `Confirmed`;
  `test-spec.md` stays `Draft` pending automated tests. Tightened `interfaces.md`'s `GET /menu`
  response shape and noted JSON conventions (camelCase, string enums) now that a concrete
  implementation exists. Updated `docs/_discovery/coverage.md` (dropped the now-`Confirmed`
  `latteAPI` row) and `docs/00-index.md`'s module table.
- Added a "Seed / Example Data" section to `docs/modules/latteAPI/domain-model.md` (actual menu
  catalog and size-surcharge values, kept in sync with `Data/MenuCatalog.cs`, plus one
  clearly-labeled dummy waitress-account example) — closes a real fidelity gap where docs alone
  couldn't recreate the shop's actual seed data. Made this a standing rule in `CLAUDE.md`
  ("Seed / example data belongs in domain-model.md") rather than a one-off fix.
- Added ADR-0005 (`Accepted`, system-wide): every module with an HTTP API now (1) enables
  framework-generated OpenAPI (`AddOpenApi()`/`MapOpenApi()`) instead of a hand-maintained
  schema, (2) documents its contracts as an `interfaces/` folder — `README.md` index + one file
  per operation — instead of a single flat `interfaces.md` (a module with no operations of its
  own, e.g. `latteMCPclient`, keeps the flat file), and (3) captures real request/response
  samples per operation, especially valuable for `Draft`/low-confidence entries during Track A
  discovery. Added the new cross-cutting `docs/api-conventions.md` (JSON casing, error-body
  shape, auth header, health payload) so these don't repeat on every operation file. Updated
  `CLAUDE.md`'s Documentation Structure, "Don't duplicate an existing source of truth", Track A's
  per-operation loop, and the Migrations & Rewrites section accordingly. Restructured
  `docs/modules/latteAPI/interfaces.md` and `docs/modules/latteMCP/interfaces.md` into the new
  `interfaces/` folder (latteAPI's six operation files include real captured samples; latteMCP's
  are still `Draft` placeholders pending Phase 2 implementation); re-enabled
  `Microsoft.AspNetCore.OpenApi` in `latteAPI` and documented it in `architecture.md`. Updated
  `docs/00-index.md`, `docs/architecture/overview.md`, and `docs/_discovery/discovery_plan.md`
  for the new structure.
- Implemented Phase 2 (`latteMCP`): typed `HttpClient` (`LatteApiClient`) to `latteAPI`, `GET
  /health` (verifies `latteAPI` reachability, ADR-0004; `503` if down), `POST /login` (forwards
  to `latteAPI`'s `POST /auth/login` as-is, ADR-0002; `502` if `latteAPI` is unreachable), and
  the MCP tool surface (`get_menu`, `place_order`, `get_order`, `list_orders`) forwarding the
  caller's bearer token statelessly per request via `IHttpContextAccessor` (ADR-0003) and
  rejecting a missing token with a clear `McpException` before ever calling `latteAPI`
  (MCP-REQ-003). Removed the "Hello World!" placeholder. Resolved two contract details the
  forward spec had left open: the `/login`/`/health` "latteAPI unreachable" status codes are
  `502`/`503` respectively (now documented in `interfaces/post-login.md` and `get-health.md`).
  Also recorded a non-obvious SDK behavior in `architecture.md`: `MapMcp()`'s default route
  pattern is the root path, not `/mcp` — it must be passed explicitly as `MapMcp("/mcp")`. All
  tool calls, error pass-through (400/404 from `latteAPI`), missing-token rejection, and the
  health/login degraded paths verified manually 2026-08-22; automated tests not yet written.
  Brought `docs/modules/latteMCP/requirements.md`, `domain-model.md`, `architecture.md`, and
  `interfaces/*.md` (with real captured samples replacing the Phase 2 placeholders) from `Draft`
  to `Confirmed`; `test-spec.md` stays `Draft` pending automated tests. Updated
  `docs/_discovery/coverage.md` (dropped the now-`Confirmed` `latteMCP` row) and `docs/00-index.md`'s
  module table.
- Framework fix: `claude-project-framework`'s
  `docs/modules/_module-template/requirements.md` `Source:` field enum only listed 4 values, but
  `CLAUDE.md` Workflow Rule 5 itself prescribes a 5th wording never added to the template, and
  real usage in this project's own `docs/modules/latteAPI/requirements.md` (`API-REQ-004`/`005`,
  and `MCP-REQ-001`'s split-provenance case) needed guidance the template didn't give. Expanded
  the template's enum to 5 values (adding the missing Rule 5 wording) plus guidance for
  split-provenance entries; deliberately did **not** add a 6th "Confirmed by implementation"
  value for the already-built-when-discovered case — an earlier draft of the fix did, but
  `/code-review high` caught that it would launder never-human-reviewed content into
  Confirmed-sounding language, contradicting Rule 5's own reservation of that phrasing for actual
  human sign-off. Clarified instead that the existing "Draft (inferred from code)" value already
  covers that case. This project's own docs already conformed to the corrected pattern, so no
  `docs/modules/*` edits were needed here, only this entry and the `framework-maintenance.md` sync
  pointer. Logged and resolved as `CR-1787505166518-a99b` in `_frw/_data/change_requests.jsonl`.
- Fixed a real secret leaked in docs: `docs/modules/latteAPI/interfaces/post-auth-login.md` and
  `docs/modules/latteMCP/interfaces/post-login.md` embedded the literal working password
  `carla-2026` in their "Sample Requests & Responses" sections — a real seeded account from
  `src/latteAPI/appsettings.Development.json`, not an illustrative value, directly violating Rule
  10 and contradicting `domain-model.md`'s own dummy-account carve-out for the same rule. Found
  during a review of the interfaces files against the framework's design docs. Redacted the
  password field in both samples, keeping the rest of each sample real.
- Framework fix: ADR-0005's "real captured samples, not illustrative pseudo-examples" mandate
  directly conflicted with Rule 10 for this exact case — a real successful login response
  necessarily contains a real working password, and neither ADR-0005 nor `CLAUDE.md`'s API
  documentation section said which rule wins. Added an explicit exception to `CLAUDE.md`'s API
  documentation section (redact just the secret field, name the actual config key rather than the
  literal words "config key", keep everything else in the sample real) and cross-referenced it
  with the pre-existing analogous "Seed / Example Data" secrets carve-out so the same conflict
  isn't solved twice with two unlinked strategies. Propagated into `claude-project-framework`'s
  `CLAUDE.md.template` (commit `67df260`, amended to `6d367dd` after a Rule 15/16 review found and
  fixed 4 process gaps — see `docs/project/review_log.md`'s entry below). Resolved
  `CR-1787513864537-a3b8`.
- While reviewing the interfaces files for the above, also fixed two accuracy gaps verified
  directly against source: `docs/api-conventions.md`'s `401` error-body row claimed every `401`
  includes a `WWW-Authenticate: Bearer` header, which only holds for JWT-middleware-challenged
  endpoints, not `POST /auth/login`'s handler-returned `401` (`Results.Unauthorized()` in
  `src/latteAPI/Program.cs`) — clarified the distinction. `docs/modules/latteMCP/interfaces/post-login.md`
  claimed a strict "as-is" passthrough of `latteAPI`'s response including `Content-Type`, but
  `src/latteMCP/Program.cs:76` falls back to a hardcoded `application/json` when `latteAPI`'s
  response has none (e.g. its empty-bodied `401`) — corrected the prose to describe the real
  fallback instead of an inaccurate strict-passthrough claim. Two lower-priority findings from the
  same review (MCP-tool-vs-OpenAPI scope ambiguity in ADR-0005/`CLAUDE.md`; no shared-conventions
  home for the repeated MCP tool error-message format) were logged as `CR-1787513864547-86c1` and
  `CR-1787513864557-2d8f` but not acted on this round.
- Framework fix, requested directly by the user: `_frw/_data/change_requests.jsonl`'s `severity`
  field gained a `moderate` tier (`minor`/`moderate`/`major`, no migration needed since old
  entries' existing values stay valid), and a new `affected_entities` field (fixed enum:
  `requirements`/`domain-model`/`architecture`/`architecture-overview`/`interfaces`/`test-spec`/
  `decisions`/`glossary`/`api-conventions`/`dev-practices`/`process`) records which framework
  doc-entity type(s) a change request concerns, instead of leaving that to free-text `description`
  alone. Propagated into `claude-project-framework`'s `docs/framework-maintenance.md` and
  `_design/domain-model.md`'s lookup copy (commit `1c44c0d`). A Rule 15/16 review found and fixed
  gaps in the first draft (no value for framework/meta docs, `architecture` ambiguity, a
  non-distinguishing date-based cutoff) and deferred two lower-priority findings as
  `CR-1787517912972-b7e1` and `CR-1787517912973-c92a` — see `docs/project/review_log.md`'s entry
  below.
