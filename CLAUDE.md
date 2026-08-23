# CLAUDE.md

## Purpose

This file defines how work proceeds on this project: how project knowledge is documented, when
to ask before deciding, and — for an existing codebase — how reconstructing missing
documentation runs *alongside* ongoing feature development, rather than blocking it.

The project must be fully recreatable — including on a different technology stack — by reading
`docs/`, `PLAN.md`, and this file alone, without needing to ask further questions about intent
or business behavior. This structure is designed to scale to a large, multi-module commercial
codebase, and to work whether the project starts from nothing or from years of existing code.

## Documentation Structure

```
CLAUDE.md
PLAN.md                     — current, active work only (cross-module or org-level)
docs/
  00-index.md                — map of all modules and cross-cutting docs; read this first
  glossary.md                — vocabulary shared across all modules (single source, avoids drift)
  api-conventions.md         — shared HTTP API conventions (JSON casing, error bodies, auth
                                header, health payload) — see "API documentation" below
  dev-practices.md           — configurable process decisions (test-writing timing, whether
                                automated tests gate `Confirmed`, local verification requirements,
                                whether the two-reviewer gate runs before every push) — see
                                "Development practices" below
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
  _project/
    CHANGELOG.md                — dated log of material changes to requirements/architecture/
                                   decisions
    completed_plan.md           — archive of tasks checked off in `PLAN.md` (see rule 12 below)
    review_log.md               — per-push log of Rule 15/16 secondary-review runs (command,
                                   repo/commit, module(s), framework version) — see rule 15/16
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

Only `CLAUDE.md` and `PLAN.md` live at the project root — everything else that describes or logs
the project belongs under `docs/`. `docs/_project/` holds files that are a running log *of this
specific project* (`CHANGELOG.md`, `completed_plan.md`) rather than reusable framework content —
see "Reusable framework template (`_frw/`)" below for the distinction this split is for.

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

### Development practices (`dev-practices.md`)

`docs/dev-practices.md` holds this project's configurable process decisions for *how* code gets
written and verified — test-writing timing (test-after / TDD test-first / test-alongside),
whether automated tests are required for a module to reach `Status: Confirmed`, whether the
automated test suite must be run locally before a task counts as done, and whether the two-
reviewer gate (project + framework) runs before every push. Workflow Rules 9, 14, 15, and 16
below read this file rather than hard-coding one policy, so a project bootstrapped from `_frw/`
can pick its own rigor level without editing this file. If `docs/dev-practices.md` doesn't exist
yet, the defaults are: test-after, manual verification sufficient for `Confirmed`, no fixed
local-run requirement (i.e. Rule 9's base text, unmodified), and Rules 15/16's own base text
(both reviewers, every push where applicable) in force unmodified. Changing a setting in
`dev-practices.md` is an ordinary process decision — confirm it per Workflow Rule 2, update the
file, then follow it; it does not require a `_frw/` update, since `_frw/docs/dev-practices.md`
already documents the full menu of options
generically.

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

## Reusable Framework Template (`_frw/`)

`_frw/` is a genericized, self-contained bootstrap kit for starting a different project with this
same documentation philosophy — `docs/` stays this project's real content, `_frw/` stays generic
(structure/placeholders only, no project facts). It lives **outside this repo**, in a shared
external repo that multiple projects bootstrap from and propagate framework-level changes into —
it is not vendored into any single project's own repo. See `docs/framework-maintenance.md` for the
full structure, the concrete location, and the exact project-vs-framework split.

**Standing rule:** a framework-level change (something meant to apply to future projects too, not
just a fact about this one) is never made unilaterally — ask the user first, then apply it to
`CLAUDE.md`/`docs/` and propagate the generic version into the shared `_frw/` together, and log it
in `docs/_project/CHANGELOG.md`. See `docs/framework-maintenance.md` for the full procedure.

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
right now, in small bounded units — never a whole repo, or even a whole file, in one pass — so
each iteration fits comfortably in context and progress is resumable across many short sessions.
See `docs/_discovery/discovery_plan.md` for the full process (topology scan, module ordering, the
per-operation loop, batching guidance, and how to flag ambiguity instead of guessing) and
`docs/_discovery/coverage.md` for the module-level rollup it drives.

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

A separate, deliberate initiative from Track B's day-to-day flow above — fully replacing a
module's implementation (possibly on a different stack) using its now-confirmed documentation as
the spec, rather than just changing it. See `docs/migrations.md` for the full As-Is/To-Be process
and what must not change during a rewrite.

## Workflow Rules

1. At the start of any task, read `docs/00-index.md`, then the relevant module folder(s), then
   `PLAN.md`. For an existing codebase with incomplete docs, check `docs/_discovery/coverage.md`
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
   never be allowed to drift apart. Log material changes in `docs/_project/CHANGELOG.md`.
8. Every acceptance criterion in a module's `test-spec.md` maps to at least one automated test,
   written from the spec — not reverse-engineered from existing code (except during the
   discovery pass itself, where the direction is necessarily code → doc).
9. By default, a module's own `Status: Confirmed` (in `requirements.md`, `00-index.md`, etc.) can
   be reached on the strength of manual verification alone and does not imply Rule 8 is satisfied
   — `test-spec.md`'s individual entries stay `Draft` under Rule 8 until real automated tests
   exist for them, independent of the rest of the module's status. Track that gap as a follow-up
   in `PLAN.md`, not as a blocker to reaching `Confirmed` elsewhere in the module. **This default
   can be overridden by `docs/dev-practices.md`** — if its "Automated Tests Gate `Confirmed`
   Status" setting is `Yes`, this carve-out does not apply and `Confirmed` requires real passing
   automated tests first.
10. Never write secrets, API keys, passwords, or tokens into any file under `docs/` (including
    `docs/_project/CHANGELOG.md`) or `PLAN.md`. Reference the config key name only.
11. New significant decisions get a new ADR in `docs/decisions/`, added to `docs/decisions/
    README.md`; never edit or delete a past one's decision/consequences — supersede it with a new
    ADR that references it.
12. Keep `PLAN.md` scoped to active work only. If multiple teams work different modules in
    parallel, a module MAY keep its own `docs/modules/<module>/plan.md` for module-local work;
    root `PLAN.md` then tracks only cross-module or org-level initiatives.
13. When a task touches a module below `Confirmed`, follow the Track B contract above: flag it,
    let the developer decide whether to pause, but document whatever is added or changed
    regardless of that decision.
14. Follow `docs/dev-practices.md` for *how* implementation work is sequenced and verified on
    every task — test-writing timing (test-after / TDD test-first / test-alongside) and whether
    the automated test suite must be run locally before a task counts as done. That file also
    governs the override to Rule 9 above. If the file doesn't exist yet (e.g. a project freshly
    bootstrapped from `_frw/` that hasn't filled it in), fall back to this file's defaults:
    test-after, manual verification sufficient for `Confirmed`, no fixed local-run requirement.
15. **Project Reviewer.** Before any `git push`, run `/code-review high` scoped to everything in
    the push *except* what Rule 16 claims below — by construction, this is every file Rule 16
    doesn't cover, so nothing new added to `docs/` (a future `docs/security.md`, another
    cross-cutting top-level doc, a new module) can fall through a gap between the two reviewers'
    scopes the way an incomplete allowlist could. In practice this means `docs/modules/**`,
    `docs/decisions/**` (ADRs), requirement/test-ID traceability, `docs/_discovery/**`,
    `docs/_project/**`, every other file under `docs/`, `PLAN.md`, and application source code —
    as a check independent of the authoring work already done, not a repeat of it. Always runs, on
    every push. Fix its findings, or explicitly acknowledge and log a deliberate deferral (e.g. in
    the relevant `PLAN.md`/`docs/_project/completed_plan.md` entry), before the push proceeds. Log
    the run in `docs/_project/review_log.md` as a **Project Reviewer** sub-entry — always present
    — with the command, repo/commit, module(s) touched, and the current `_frw/VERSION` value (the
    `YY.MM.DD:HH.MM.FFF` format is documented in `docs/framework-maintenance.md`'s "Versioning"
    section, not in `_frw/VERSION` itself, which is just the bare value; read live from the shared
    external `_frw/` — see `docs/framework-maintenance.md` for its location — falling back to the
    static "bootstrapped from" version recorded in this project's own
    `docs/framework-maintenance.md` if that shared location isn't reachable). Governed by
    `docs/dev-practices.md`'s "Secondary Review Before Push" setting; if that file doesn't set it,
    the default is this rule's own text as written — `/code-review high`, every push,
    self-enforced (no technical block, e.g. no pre-push git hook — same recommend-don't-block
    spirit as Track B rule 2 above). This rule doesn't loosen Rule 2: a finding that's itself
    ambiguous, hard to reverse, or touches security/architecture/business rules still gets asked
    about, not just logged and pushed past.
16. **Framework Reviewer.** Before any `git push` that touches `CLAUDE.md`,
    `docs/framework-maintenance.md`, or `docs/migrations.md` — or that changes
    `docs/dev-practices.md`'s menu/structure itself (its "Secondary Review Before Push" section is
    this two-reviewer policy's own canonical description, so a structural change there can itself
    be a framework-level change needing propagation into the shared `_frw/` — see
    `docs/framework-maintenance.md`) — also run `/code-review high` scoped to those paths. `_frw/`
    itself lives outside this repo (see "Reusable Framework Template" above) — a push from this
    repo never touches it directly, so it falls outside this rule's scope; propagating a change
    into it is a separate edit made directly in the shared `_frw/` location, reviewed there on its
    own terms. Merely flipping one of
    `docs/dev-practices.md`'s `Selected:` values (e.g. TDD to test-after) stays an ordinary,
    Rule-15-only change per the "Development practices" section above — it doesn't by itself
    trigger this rule, since it changes this project's choice, not the menu of choices or the
    policy's own description. Reuse Rule 15's own invocation output, re-read through the lens
    below, when the two scopes substantially overlap in the same push (as they typically do for a
    framework-focused change like this one); invoke separately when the push is mostly ordinary
    project work with only an incidental framework-path touch, so the framework lens gets its own
    focused pass rather than being buried in an unrelated diff. The lens: (a) *framework/project
    fidelity* — does `_frw/CLAUDE.md.template` (in the shared external location) still match
    `CLAUDE.md`'s genericizable content, does every file under `_frw/` stay free of
    project-specific facts; (b) *ambiguity* — is any
    new/changed rule or template file unclear, self-contradictory, or missing a cross-reference a
    reader would need; (c) *enhancement opportunities* — note anything generalizable worth
    proposing as a future framework improvement. This rule doesn't loosen Rule 2 either: a
    fidelity or ambiguity finding that's itself ambiguous, hard to reverse, or touches
    security/architecture/business rules still gets asked about, not just logged and pushed past.
    Enhancement suggestions specifically are proposals only: per `docs/framework-maintenance.md`'s
    standing rule, no framework change — including one this reviewer itself suggests — is applied
    without asking the user first. Fix confirmed fidelity/ambiguity findings, or log a deferral,
    before the push proceeds. Log the run in `docs/_project/review_log.md` as a **Framework
    Reviewer** sub-entry the same way Rule 15 does, plus any enhancement suggestions raised (or
    "none"). This sub-entry is always present in every push's log entry, even when this rule
    didn't apply — its content then simply states "not run: push touched no framework paths"
    instead of a command and outcome, so the log format never depends on whether this rule fired.
