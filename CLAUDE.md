# CLAUDE.md

## Purpose

This file defines how work proceeds on this project: how project knowledge is documented, when
to ask before deciding, and — for an existing codebase — how reconstructing missing
documentation runs *alongside* ongoing feature development, rather than blocking it.

The project must be fully recreatable — including on a different technology stack — by reading
`docs/`, `plan.md`, and this file alone, without needing to ask further questions about intent
or business behavior. This structure is designed to scale to a large, multi-module commercial
codebase, and to work whether the project starts from nothing or from years of existing code.

## Documentation Structure

```
CLAUDE.md
plan.md                     — current, active work only (cross-module or org-level)
CHANGELOG.md                — dated log of material changes to requirements/architecture/decisions
docs/
  00-index.md                — map of all modules and cross-cutting docs; read this first
  glossary.md                — vocabulary shared across all modules (single source, avoids drift)
  api-conventions.md         — shared HTTP API conventions (JSON casing, error bodies, auth
                                header, health payload) — see "API documentation" below
  architecture/
    overview.md               — system-wide component map, service boundaries, deployment topology
                                 (As-Is / To-Be — see "Migrations & Rewrites" below)
  decisions/
    README.md                 — index table of all ADRs (id, title, status, modules affected)
    0001-<title>.md            — one ADR per significant decision, globally numbered
  _discovery/
    coverage.md                — module-level rollup of reconstruction progress
    discovery_plan.md           — operation-level backlog that drives the rollup above
    debt_log.md                 — flags raised (and knowingly deferred) during feature work
  modules/
    <module-name>/
      requirements.md          — business rules, functional & non-functional reqs (tech-agnostic)
      domain-model.md          — entities, relationships, and seed/example data for this module
      architecture.md           — this module's components, current tech stack, internals
      interfaces/                — this module's API contracts, external deps, config keys —
                                    README.md (index) + one file per operation; a module with no
                                    operations of its own keeps a flat interfaces.md instead (see
                                    "API documentation" below)
      test-spec.md              — Given/When/Then acceptance criteria per requirement
```

Add one `docs/modules/<module-name>/` folder per bounded context / service / major subsystem,
named to match the actual code module or service directory where possible. This is what lets
the doc set scale with the codebase instead of becoming a handful of unmanageably long files —
splitting docs by module is the same principle as splitting code by module.

Add further cross-cutting top-level docs under `docs/` the same way `architecture/overview.md`
was added (e.g. `docs/security.md`, `docs/compliance.md`) only for concerns that genuinely span
every module; anything module-specific belongs inside that module's folder, not at the top level.

### Layering principle

`requirements.md`, `domain-model.md`, `interfaces/` (or a module's flat `interfaces.md` if it has
no operations of its own), and `test-spec.md` (per module) describe **what** the system does.
They are technology-agnostic and must survive a full rewrite onto a different stack unchanged —
except the *generated* OpenAPI document an `interfaces/` folder links to, which is tied to the
current stack like `architecture.md` is, and is expected to be regenerated fresh after a rewrite,
not carried over.

`architecture/overview.md`, each module's `architecture.md`, and `decisions/` describe **how**
it is currently built. They are expected to change if the stack changes. `decisions/` exists
specifically to preserve the reasoning behind past choices, so a future rewrite doesn't repeat
mistakes or re-litigate settled tradeoffs blindly.

### Don't duplicate an existing source of truth

If the codebase already has OpenAPI/GraphQL/protobuf schemas, a DB schema, or generated API
docs, `interfaces/` and `domain-model.md` should **link to and summarize the business meaning**
of those artifacts, not hand-transcribe their fields into markdown. Two sources of truth for the
same schema will drift; the generated/machine-checked one should win, and the markdown should
explain the *why*, not restate the *what* it already encodes.

### API documentation: generated spec, per-operation files, real samples (ADR-0005)

Every module that exposes an HTTP API:

- Enables framework-generated OpenAPI (e.g. ASP.NET Core's `AddOpenApi()`/`MapOpenApi()`) rather
  than hand-maintaining a schema — it's regenerated from the actual endpoint definitions on every
  build/run, so it can't drift the way a hand-written one can. This is the mandatory case of the
  "don't duplicate a source of truth" rule above, not merely the conditional one.
- Documents its contracts as `docs/modules/<name>/interfaces/`: a `README.md` index (operations
  table, external services, config keys, a link to the live generated spec) plus one file per
  operation, named `<method>-<path-with-dashes>.md` (e.g. `get-orders-id.md` for
  `GET /orders/{id}`) or `mcp-tool-<name>.md` for a non-REST operation like an MCP tool. A module
  with no operations of its own (a pure caller) keeps a single flat `interfaces.md` instead —
  the split only pays for itself once there's something to split.
- Gives every operation file a "Sample Requests & Responses" section with **real captured**
  request/response pairs (status, headers, body) — not illustrative pseudo-examples. Add or
  refresh this whenever the operation is touched (Track B) or newly drafted (Track A); it matters
  most while an entry is still `Draft` or low-confidence, since a real sample is evidence a
  reader can check, often faster to produce than getting the prose exactly right.
- Relies on `docs/api-conventions.md` for anything that would otherwise repeat on every
  operation file — JSON casing, the error-response body shape, the auth header format, the
  health-check payload shape. An operation file only documents what's specific to it.

### Seed / example data belongs in domain-model.md

For any entity documented as fixed/seed data (a static catalog, a lookup table, reference
values) — not just its schema — `domain-model.md` must also carry a "Seed / Example Data"
section with the actual current values, kept in sync with the code that defines them. Schema
alone isn't enough to recreate the module: a rewrite needs real business data to seed, and code
is otherwise the only place it lives.

This applies to ordinary business data, never to secrets (rule 10 below still governs those).
Where an entity mixes the two — e.g. an account record with both a non-secret identifier and a
secret credential — the seed-data section may include one clearly-labeled dummy row illustrating
the *shape* (field names, format) without using or resembling any real value; actual secret
values still never appear in `docs/`, referenced by config key name only.

### ID namespacing

Every requirement, rule, and test gets a stable ID prefixed with its module, e.g. `AUTH-REQ-001`,
`BILLING-RULE-004`, `AUTH-TEST-012`. IDs are never reused or renumbered — mark removed items
`Deprecated` instead of deleting them. ADRs are numbered globally (`ADR-0001`, `ADR-0002`, ...;
not per-module) since a single decision often affects multiple modules — each ADR lists which
modules it affects instead. Tests, code comments, and commit messages should reference these IDs
so any piece of the system can be traced back to the requirement or decision that justifies it.

## Two Concurrent Tracks: Discovery and Development

Two things happen at once on a project with existing code: a **Discovery track** reconstructs
missing documentation from the codebase, module by module; a **Development track** keeps
shipping features and fixes. Development never waits for Discovery to finish — pausing the
business to write docs isn't realistic. Instead both tracks read and write the same `docs/`
tree and pull it toward completeness from two directions at once: Discovery sweeps whatever
nobody happens to be touching, and Development guarantees that whatever *is* being touched
either gets documented or gets explicitly logged as deferred — never silently left dark.

### Track A — Discovery (backfill)

Dedicated, opportunistic reconstruction of documentation for modules nobody is actively changing
right now. Never process a whole repo, or even a whole file, in one pass — work in small,
bounded units so each iteration fits comfortably in context and progress is resumable across
many short sessions. `docs/_discovery/discovery_plan.md` is the operation-level backlog that
drives this; `docs/_discovery/coverage.md` is the module-level rollup of it.

#### Phase 0 — Topology scan (cheap, metadata only)

Before reading any implementation logic: enumerate modules/services from build manifests
(package.json workspaces, pom.xml modules, go.mod, monorepo config), locate any existing
machine-readable specs (OpenAPI/proto/GraphQL schema — these are a huge shortcut, use them
instead of reverse-engineering the same contract from code), and inventory existing legacy docs
per candidate module. This produces a first-draft module list (→ `docs/00-index.md`,
`coverage.md`) and an operation index — signatures only, found by grepping for
route/handler/consumer/job-definition patterns, without reading any function bodies yet
(→ `discovery_plan.md`).

#### Module boundaries first, then operations as the unit of depth

Don't choose between "endpoints first" and "modules first" — they answer different questions.
Module boundaries (from Phase 0) tell you *where a piece of documentation belongs*; you need
that, even approximately, before drafting anything, or operations get filed inconsistently.
Operations — one HTTP endpoint, one queue consumer, one scheduled job, one CLI command, one
significant internal library boundary — are the unit you go *deep* on, one at a time. A whole
module is too large to read in one bounded pass; a single operation plus 1–2 hops of its call
chain (handler → service → repository, going deeper only if a business rule still isn't
resolved) is exactly the size that fits one focused iteration.

Order modules before starting deep work: foundational/shared modules (auth, core domain
entities other modules depend on) generally go first, since later modules' docs will reference
them — unless a specific module needs to be replanned first for business reasons, in which case
document its direct dependencies just enough to unblock it and come back for their full depth
later. Record the chosen order and reasoning in `discovery_plan.md`.

#### Per-operation loop (the actual iteration)

1. Read only the operation's entry point + up to ~2 hops of direct calls.
2. Draft or extend the relevant entries in that module's `requirements.md`, `domain-model.md`,
   `interfaces/<operation>.md`, `test-spec.md` — write to the files immediately; don't hold
   drafts in conversation memory across operations. Capture a real sample request/response in
   the operation file if one can be produced (see "API documentation" above). Tag provenance and
   status on every entry:
   - `Status: Draft (inferred from code)` — reconstructed by reading implementation.
   - `Status: Draft (from legacy docs)` — pulled from existing but possibly outdated material.
   - `Status: Confirmed` — only set after a person has reviewed and approved it.
3. Update the operation's row in `discovery_plan.md` (`Status`, `Confidence`, any open question).
4. Move to the next operation without re-reading code already covered, unless revisiting an open
   question.

Batch several simple, uniform operations (e.g. near-identical CRUD endpoints) into one
iteration — reading them together adds little context cost. Give each complex operation
(non-trivial branching, business rules, or code that's ambiguous on its own) its own isolated
iteration so it isn't diluted by unrelated context. `discovery_plan.md` tracks batch membership
so an interrupted run resumes at the right spot instead of restarting.

#### Flag, don't guess, on ambiguity

If behavior in the code looks unintentional (dead code, possible bug), or a legacy doc
contradicts what the code actually does, record it as an open question in `discovery_plan.md`
and the relevant module doc — do not silently pick an interpretation.

A module is ready for review once every operation belonging to it is `Drafted` in
`discovery_plan.md`; once a person reviewing it marks it `Confirmed`, update `coverage.md` and
move to the next module. Drafting is Claude's best reconstruction, not verified truth — treat it
accordingly until someone has actually reviewed it.

### Track B — Development (features & fixes)

This is what happens most of the time: a task requires touching code in some module right now,
regardless of that module's documentation status. The contract:

1. Before touching code in a module/operation, check its status in `docs/00-index.md`,
   `docs/_discovery/coverage.md`, or the module's own docs.
2. If the relevant docs are missing, still `Draft`, or contradict what the code actually does,
   say so and recommend completing/confirming that documentation first. This is a
   recommendation, not a block — the developer may decide to proceed anyway, and that decision
   is theirs to make.
3. **Non-negotiable, regardless of that decision:** any code newly added or materially changed
   as part of this task must itself be documented before the task is done — bring the touched
   operation(s) to at least `Status: Draft (written alongside code)`, ideally `Confirmed` since
   the author just wrote it and knows what it does. This is what stops new debt from
   accumulating even while old debt persists elsewhere.
4. If the exact area being changed has a doc that actively contradicts the code — not just an
   unrelated gap elsewhere in the module — resolve that contradiction as part of the change.
   Don't leave a document that's provably wrong about the thing you just edited.
5. If the developer declines to resolve the flag from step 2, log it in
   `docs/_discovery/debt_log.md` (what's missing/mismatched, why deferred) and bump that
   module/operation's priority in `discovery_plan.md` so Track A picks it up sooner. Skip the
   log entirely if the developer instead brings the docs to `Confirmed` as part of the same
   task — there's nothing left to defer.

Coverage only ever moves forward under this contract: a module already `Confirmed` doesn't
regress because Track B touched one operation in it — that operation stays `Confirmed`, and
nothing else in the module needs re-verifying unless the change actually spilled into it.

## Migrations & Rewrites (docs-first replacement of legacy code)

This is a separate, deliberate initiative from Track B's day-to-day flow above — a decision to
fully replace a module's implementation, not just change it. When the goal is to replace
existing code with a new implementation (possibly on a different stack) using the now-confirmed
documentation as the spec:

- Keep the legacy system's `architecture/overview.md` and each module's `architecture.md` as the
  **As-Is** record — don't overwrite them, since the reasoning in `decisions/` often refers back
  to them.
- Describe the new system in a **To-Be** section (or a clearly separated new revision once the
  legacy system is fully retired) rather than editing As-Is in place.
- `requirements.md`, `domain-model.md`, `interfaces/` (the contract content, not the generated
  spec it links to), and `test-spec.md` for a `Confirmed` module should not need to change for a
  rewrite — if the rewrite forces a change to one of these, that's a sign the new system's
  behavior is diverging from the confirmed spec, and it's worth a deliberate decision (and likely
  an ADR) rather than an incidental edit.
- New implementation work goes through `plan.md` and normal Workflow Rules below, same as any
  other task.

## Workflow Rules

1. At the start of any task, read `docs/00-index.md`, then the relevant module folder(s), then
   `plan.md`. For an existing codebase with incomplete docs, check `docs/_discovery/coverage.md`
   and `debt_log.md` first to see what's already reconstructed or previously flagged.
2. Before a decision that is ambiguous, controversial, hard to reverse, or touches architecture,
   security, or business rules, ask whether the approach is acceptable — batch related questions,
   offer concrete options with tradeoffs rather than open-ended questions.
3. Rule 2 applies equally to a requirement/rule whose own `Source:` field reads `Draft (proposed
   by Claude, pending confirmation)`: that tag means the requirement's *content*, not just some
   implementation detail of it, was never actually confirmed by a person. Don't treat "it's
   already written in the doc" as equivalent to human sign-off just because Claude wrote it in an
   earlier session.
4. For small, reversible choices, use best judgment, record the choice and reasoning inline (or
   as a new ADR if consequential), and mention it in the summary instead of stopping to ask.
5. A `Draft (proposed by Claude, pending confirmation)` requirement can be implemented under Rule
   4, instead of asking per Rules 2–3, when the proposal itself is small/reversible enough — but
   say so explicitly in the summary (by requirement ID) so there's still a clear point for the
   person to redirect. Afterward, update its `Source` line to note it was implemented and
   verified (e.g. `Draft (proposed by Claude), implemented and verified <date>`) rather than
   rewriting it to `Confirmed by user` — that phrase is reserved for decisions a person actually
   approved; self-verification by implementation is a different, weaker kind of confirmation, and
   the doc should keep that distinction visible rather than launder one into the other.
6. Once a decision is confirmed, update the relevant doc(s) first, then implement.
7. If implementation reveals a doc was wrong, fix the doc before moving on — docs and code must
   never be allowed to drift apart. Log material changes in `CHANGELOG.md`.
8. Every acceptance criterion in a module's `test-spec.md` maps to at least one automated test,
   written from the spec — not reverse-engineered from existing code (except during the
   discovery pass itself, where the direction is necessarily code → doc).
9. A module's own `Status: Confirmed` (in `requirements.md`, `00-index.md`, etc.) can be reached
   on the strength of manual verification alone and does not imply Rule 8 is satisfied —
   `test-spec.md`'s individual entries stay `Draft` under Rule 8 until real automated tests exist
   for them, independent of the rest of the module's status. Track that gap as a follow-up in
   `plan.md` (see `latteAPI`/`latteMCP` Phase 1/2 for the pattern), not as a blocker to reaching
   `Confirmed` elsewhere in the module.
10. Never write secrets, API keys, passwords, or tokens into any file under `docs/`, `plan.md`, or
    `CHANGELOG.md`. Reference the config key name only.
11. New significant decisions get a new ADR in `docs/decisions/`, added to `docs/decisions/
    README.md`; never edit or delete a past one's decision/consequences — supersede it with a new
    ADR that references it.
12. Keep `plan.md` scoped to active work only. If multiple teams work different modules in
    parallel, a module MAY keep its own `docs/modules/<module>/plan.md` for module-local work;
    root `plan.md` then tracks only cross-module or org-level initiatives.
13. When a task touches a module below `Confirmed`, follow the Track B contract above: flag it,
    let the developer decide whether to pause, but document whatever is added or changed
    regardless of that decision.
