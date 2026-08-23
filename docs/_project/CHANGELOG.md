# Changelog

Dated log of material changes to requirements, architecture, or accepted decisions. Not a commit
log — only entries that change what a reader of `docs/` would believe about the system. At
scale, split into `CHANGELOG-<year>.md` once this file gets unwieldy, keeping this file as the
current year only and linking older ones from the top.

## 2026-08-23

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
