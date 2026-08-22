# Changelog

Dated log of material changes to requirements, architecture, or accepted decisions. Not a commit
log — only entries that change what a reader of `docs/` would believe about the system. At
scale, split into `CHANGELOG-<year>.md` once this file gets unwieldy, keeping this file as the
current year only and linking older ones from the top.

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
