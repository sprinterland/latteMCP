# Architecture Decision Records — Index

One row per ADR. Keep this table current whenever an ADR is added or superseded — it's the
fast way to see what's been decided without opening every file.

| ID | Title | Status | Modules affected | Date |
|---|---|---|---|---|
| [ADR-0001](0001-jwt-waitress-authentication.md) | Waitress identity via JWT, issued by latteAPI, accounts and signing key in settings | Accepted | system-wide | 2026-08-22 |
| [ADR-0002](0002-login-as-rest-endpoint-not-mcp-tool.md) | Login is a plain REST endpoint on latteMCP, not an MCP tool | Accepted | latteMCP, latteMCPclient | 2026-08-22 |
| [ADR-0003](0003-stateless-bearer-token-passthrough.md) | latteMCP forwards bearer tokens statelessly; no server-side session cache | Accepted | latteMCP | 2026-08-22 |
| [ADR-0004](0004-mcp-health-check-verifies-upstream.md) | latteMCP's health check verifies latteAPI reachability, not just itself | Accepted | latteMCP | 2026-08-22 |
| [ADR-0005](0005-api-docs-openapi-per-operation-samples.md) | Generated OpenAPI + per-operation interface files + captured samples | Accepted | system-wide | 2026-08-22 |

Status values: `Proposed`, `Accepted`, `Superseded by ADR-XXXX`. Copy `TEMPLATE.md` for each new
ADR (see that file for numbering rules).
