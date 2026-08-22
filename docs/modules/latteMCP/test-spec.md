# latteMCP — Test Specification

Each entry maps to exactly one requirement or rule and should translate directly into one or
more automated tests. New tests are written FROM this document, not inferred from existing code.

Status note (2026-08-22): all scenarios below were exercised manually against the Phase 2
implementation (`curl` against the MCP Streamable HTTP `/mcp` endpoint and the plain `/login`,
`/health` endpoints of a running `dotnet run` instance, with `latteAPI` running alongside) and
passed. No automated tests exist yet — entries stay `Draft` per CLAUDE.md rule 8 until real tests
are written from this spec. Tracked as a follow-up in `PLAN.md`, alongside the equivalent Phase 1
gap for `latteAPI`.

## MCP-TEST-001 (implements MCP-REQ-001, tool discovery)

- Given: `latteMCP` is running.
- When: an MCP client sends `tools/list`.
- Then: `get_menu`, `place_order`, `get_order`, `list_orders` are all listed, and `login` is
  **not** among them (MCP-REQ-004).
- Status: Draft

## MCP-TEST-002 (implements MCP-REQ-004, login wrapper happy path)

- Given: a valid waitress account exists in `latteAPI`.
- When: a client calls `POST /login` on `latteMCP` with correct credentials.
- Then: `200 OK` is returned with the same token shape `latteAPI`'s `POST /auth/login` would
  return.
- Status: Draft

## MCP-TEST-003 (implements MCP-REQ-004, login wrapper failure path)

- Given: a waitress account exists.
- When: a client calls `POST /login` with a wrong password.
- Then: `401 Unauthorized` is returned, matching `latteAPI`'s response.
- Status: Draft

## MCP-TEST-004 (implements MCP-REQ-002, token pass-through happy path)

- Given: a valid token obtained from `POST /login`.
- When: an MCP client calls `place_order` with that token attached as `Authorization: Bearer`.
- Then: `latteAPI` receives the same token, creates the order, and the tool call returns the
  created order with the correct `CreatedBy`.
- Status: Draft

## MCP-TEST-005 (implements MCP-REQ-003, missing token)

- Given: no token attached to the MCP request.
- When: an MCP client calls `place_order`, `get_order`, or `list_orders`.
- Then: `latteMCP` returns a tool error indicating the caller is not logged in, without a call
  ever reaching `latteAPI`.
- Status: Draft

## MCP-TEST-006 (implements MCP-RULE-001, no session memory)

- Given: a client successfully calls `place_order` with a valid token on one MCP request.
- When: the same client, in the same MCP session, calls `list_orders` on a subsequent request
  **without** attaching the token.
- Then: the call fails as "not logged in" (MCP-TEST-005) — the earlier token is not remembered.
- Status: Draft

## MCP-TEST-007 (implements MCP-REQ-001, get_menu needs no auth)

- Given: `latteMCP` is running.
- When: an MCP client calls `get_menu` with no `Authorization` header.
- Then: the call succeeds and returns the menu, matching `latteAPI`'s `GET /menu`.
- Status: Draft

## MCP-TEST-008 (implements MCP-REQ-005, health reflects upstream)

- Given: `latteAPI` is reachable.
- When: a client calls `latteMCP`'s `GET /health`.
- Then: `200 OK` is returned.
- Status: Draft

## MCP-TEST-009 (implements MCP-REQ-005, health reflects upstream down)

- Given: `latteAPI` is stopped/unreachable.
- When: a client calls `latteMCP`'s `GET /health`.
- Then: a non-2xx status is returned, indicating the upstream dependency is down — `latteMCP`
  does not report healthy while unable to serve any real tool call.
- Status: Draft
