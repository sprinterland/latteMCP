# latteMCP — Interfaces (index)

One file per operation in this folder (see ADR-0005). Shared conventions (JSON casing, error
body shape, auth header, health payload) live in `../../../api-conventions.md` — not repeated
here. No secrets or credential values in any file below — reference the config key name only.

Implemented and manually verified 2026-08-22 (Phase 2, see `../../../../PLAN.md`); every file
below is `Status: Confirmed` with real captured samples. Generated OpenAPI is enabled for the two
plain REST endpoints (`GET /health`, `POST /login`) per ADR-0005 — dev URL
`http://localhost:5040/openapi/v1.json`. The MCP tools on `/mcp` are covered by the MCP
protocol's own tool-discovery mechanism instead, not OpenAPI.

## Operations

| Method / Kind | Path or tool name | Implements | Auth | File |
|---|---|---|---|---|
| GET | `/health` | MCP-REQ-005 | none | [`get-health.md`](get-health.md) |
| POST | `/login` | MCP-REQ-004 | none | [`post-login.md`](post-login.md) |
| MCP tool | `get_menu` | MCP-REQ-001 | No | [`mcp-tool-get-menu.md`](mcp-tool-get-menu.md) |
| MCP tool | `place_order` | MCP-REQ-001 | Yes | [`mcp-tool-place-order.md`](mcp-tool-place-order.md) |
| MCP tool | `get_order` | MCP-REQ-001 | Yes | [`mcp-tool-get-order.md`](mcp-tool-get-order.md) |
| MCP tool | `list_orders` | MCP-REQ-001 | Yes | [`mcp-tool-list-orders.md`](mcp-tool-list-orders.md) |

All four MCP tools require the caller to attach `Authorization: Bearer <token>` on the MCP
request (obtained beforehand from `POST /login`); each forwards that header to `latteAPI`
unchanged (`MCP-REQ-002`) and fails with a clear "not logged in" tool error if it's absent
(`MCP-REQ-003`). Request/response shape for each tool mirrors the corresponding `latteAPI`
endpoint's contract — see `../../latteAPI/interfaces/` for the authoritative shapes; this module
doesn't redefine them, only carries them through.

## External Services

- **latteAPI** — the business API this module wraps — base URL config key
  `LatteApi:BaseUrl` (not the value) — auth: none between `latteMCP` and `latteAPI` itself
  (the bearer token is the *caller's* credential, forwarded, not a service-to-service secret).

## Configuration Keys

- `LatteApi:BaseUrl` — base URL of the `latteAPI` instance to call; configurable per environment
  via `appsettings.{Environment}.json` (see ADR-0002 discussion / `../../../architecture/overview.md`).
