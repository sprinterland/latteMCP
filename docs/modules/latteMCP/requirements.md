# latteMCP — Requirements

Technology-agnostic. Should read the same regardless of implementation language/framework.
IDs are prefixed with the module name and are never reused or renumbered — mark removed items
`Deprecated` instead of deleting them.

## Functional Requirements

### MCP-REQ-001: Expose ordering operations as MCP tools

- Description: An MCP client can discover and call tools to browse the menu, place an order,
  look up an order, and list orders — mirroring `latteAPI`'s corresponding operations
  (`API-REQ-001`, `API-REQ-003`, `API-REQ-004`, `API-REQ-005`).
- Rationale: This module's entire purpose is to make `latteAPI`'s ordering operations reachable
  by an MCP client/agent.
- Source: Confirmed by user in conversation on 2026-08-22 (module's purpose); tool boundaries
  Draft (proposed by Claude), implemented and verified 2026-08-22.
- Status: Confirmed — implemented as `get_menu`/`place_order`/`get_order`/`list_orders` MCP tools
  in `src/latteMCP/Tools/OrderingTools.cs`, verified 2026-08-22.

### MCP-REQ-002: Forward waitress identity to latteAPI

- Description: Every tool call that requires identity forwards the caller's bearer token to
  `latteAPI` unchanged, so the order recorded there is attributed to the correct waitress
  (`API-RULE-002`).
- Rationale: `latteMCP` does not own identity — `latteAPI` does (see ADR-0001) — so its job is
  purely to carry the token through, not to interpret or store it.
- Source: Confirmed by user in conversation on 2026-08-22
- Status: Confirmed — implemented via `LatteApiClient` forwarding the caller's `Authorization`
  header unchanged in `src/latteMCP/Tools/OrderingTools.cs`, verified 2026-08-22.

### MCP-REQ-003: Reject tool calls made without a token

- Description: If a tool that requires identity (everything except `get_menu`) is called without
  a bearer token, `latteMCP` surfaces a clear tool-call error rather than forwarding an
  unauthenticated request to `latteAPI`.
- Rationale: Fail fast with a clear message ("not logged in") instead of relying on `latteAPI`'s
  generic `401` to explain the problem.
- Source: Draft (proposed by Claude), implemented and verified 2026-08-22.
- Status: Confirmed — implemented via `OrderingTools.GetRequiredAuthorizationHeader`, throwing an
  `McpException` before any call reaches `latteAPI`, verified 2026-08-22.

### MCP-REQ-004: Login is a plain HTTP endpoint, not a tool

- Description: `latteMCP` exposes `POST /login`, a plain REST endpoint outside the MCP tool set,
  that forwards credentials to `latteAPI`'s `POST /auth/login` and returns its response as-is.
- Rationale: See ADR-0002 for why login is deliberately kept out of the MCP tool surface.
- Source: Confirmed by user in conversation on 2026-08-22
- Status: Confirmed — implemented as `POST /login` in `src/latteMCP/Program.cs`, verified
  2026-08-22.

### MCP-REQ-005: Health check reflects upstream availability

- Description: `latteMCP` exposes `GET /health`, which reports unhealthy if `latteAPI` is
  unreachable, not just if the `latteMCP` process itself is up.
- Rationale: See ADR-0004 — a proxy's health is meaningless without its dependency.
- Source: Confirmed by user in conversation on 2026-08-22
- Status: Confirmed — implemented as `GET /health` in `src/latteMCP/Program.cs`, calling
  `latteAPI`'s `GET /health` and returning `503` if it fails, verified 2026-08-22.

## Business Rules

### MCP-RULE-001: No server-side session token cache

- Rule: `latteMCP` never stores a caller's token on the server between requests. Each MCP
  request must carry its own `Authorization` header; nothing is remembered from a prior call in
  the same session.
- Applies to: `MCP-REQ-002`, `MCP-REQ-003`.
- Source: Confirmed by user in conversation on 2026-08-22 (see ADR-0003)
- Status: Confirmed — no token/session store exists anywhere in `src/latteMCP`; every tool reads
  the `Authorization` header fresh off the current request, verified 2026-08-22.

### MCP-RULE-002: latteMCP never validates credentials itself

- Rule: `latteMCP` has no knowledge of waitress accounts or the JWT signing key — credential
  validation happens exclusively in `latteAPI`, both for `POST /login` (a pure forward) and for
  every tool call (a pure pass-through, validated by `latteAPI` on receipt).
- Applies to: `MCP-REQ-002`, `MCP-REQ-004`.
- Source: Confirmed by user in conversation on 2026-08-22
- Status: Confirmed — `src/latteMCP` has no `Jwt`/`Waitresses` configuration and no credential
  logic; every path forwards to or through `latteAPI`, verified 2026-08-22.

## Non-Functional Requirements

### MCP-NFR-001: Statelessness

- Requirement: `latteMCP` instances can be scaled or restarted freely without losing any
  in-flight caller state, since none is kept (see `MCP-RULE-001`).
- Measured by: no test needed beyond the absence of a session store in the implementation —
  this is a design property, not a runtime behavior to assert.
- Source: Confirmed by user in conversation on 2026-08-22
- Status: Confirmed — see `MCP-RULE-001`.
