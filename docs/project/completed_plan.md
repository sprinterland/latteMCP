# Completed Plan Tasks

Archive of tasks checked off in `PLAN.md` and moved out of it to keep that file scoped to
active/near-term work only (see rule 12 in `../../CLAUDE.md`). Entries are grouped by the phase
heading they were completed under, in the order they were finished. This file is a historical
record — don't re-open or re-edit an entry here; if a completed item needs revisiting, add a new
task in `../../PLAN.md` instead and link back to the relevant entry below.

## Phase 1 — `latteAPI` (completed 2026-08-22, except automated tests — see `PLAN.md`)

Implements: API-REQ-001 through API-REQ-006 (see
[`docs/modules/latteAPI/requirements.md`](../modules/latteAPI/requirements.md)).

- [x] Register `OrderStore` in DI as a singleton.
- [x] `GET /health` (API-REQ-006).
- [x] `GET /menu` (API-REQ-001).
- [x] Waitress accounts + JWT settings bound from `appsettings.json` (ADR-0001).
- [x] `POST /auth/login` (API-REQ-002).
- [x] `POST /orders`, including `CreatedBy` stamping (API-REQ-003, API-RULE-001, API-RULE-002).
- [x] `GET /orders/{id}` (API-REQ-004).
- [x] `GET /orders` (API-REQ-005).
- [x] Remove the leftover weather-forecast template code.
- [x] Update `latteAPI.http` with real sample requests, including a login → authorized order
      call sequence.
- [x] Smoke-test manually (`dotnet run` + `.http` file/`curl`) — all endpoints and error paths
      (401/400/404) verified 2026-08-22.
- [x] Bring `docs/modules/latteAPI/*` from `Draft` to `Confirmed`; updated
      `docs/_discovery/coverage.md` and `docs/00-index.md` accordingly.

The one remaining Phase 1 item (automated tests for `test-spec.md`) is still open and stays in
`PLAN.md`, since that file's Phase 1 section isn't fully closed out yet.

## Phase 2 — `latteMCP` (completed 2026-08-22, except automated tests — see `PLAN.md`)

Implements: MCP-REQ-001 through MCP-REQ-005 (see
[`docs/modules/latteMCP/requirements.md`](../modules/latteMCP/requirements.md)).

- [x] Typed `HttpClient` to `latteAPI` (`LatteApiClient`), base URL from `LatteApi:BaseUrl` in
      `appsettings.Development.json`.
- [x] `GET /health`, verifying `latteAPI` reachability (ADR-0004) — `503` with
      `{"status":"unhealthy"}` if `latteAPI` is unreachable or itself unhealthy.
- [x] `POST /login` wrapper (ADR-0002) — passes through `latteAPI`'s response as-is; `502` with
      `{"error":"latteAPI is unreachable."}` if `latteAPI` can't be reached at all (a gap the
      original spec had left as "not yet decided" — resolved during implementation and reflected
      in `docs/modules/latteMCP/interfaces/post-login.md` and `get-health.md`).
- [x] `AddMcpServer().WithHttpTransport()` + `MapMcp("/mcp")` — the SDK's bare `MapMcp()` default
      pattern turned out to be the root path, not `/mcp`; the pattern must be passed explicitly
      (see `docs/modules/latteMCP/architecture.md`).
- [x] `[McpServerToolType]` (`Tools/OrderingTools.cs`) with `get_menu`, `place_order`,
      `get_order`, `list_orders`, each forwarding the caller's bearer token statelessly
      (ADR-0003, via `IHttpContextAccessor` bound per-request, never cached) and failing clearly
      with an `McpException` if it's missing (MCP-REQ-003), before ever reaching `latteAPI`.
- [x] Remove the "Hello World!" placeholder.
- [x] Smoke-test manually (`dotnet run` for both `latteAPI` and `latteMCP` + `curl` against the
      MCP Streamable HTTP `/mcp` endpoint and the plain `/login`/`/health` endpoints) — tool
      discovery, all four tools' happy paths, missing-token rejection, latteAPI error
      pass-through (400/404), and the health/login-unreachable paths (latteAPI killed) all
      verified 2026-08-22.
- [x] Bring `docs/modules/latteMCP/*` from `Draft` to `Confirmed`; updated
      `docs/_discovery/coverage.md` and `docs/00-index.md` accordingly.

The one remaining Phase 2 item (automated tests for `test-spec.md`) is still open and stays in
`PLAN.md`, since that file's Phase 2 section isn't fully closed out yet.
