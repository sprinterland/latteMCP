# latteAPI — Interfaces (index)

One file per operation in this folder (see ADR-0005). Shared conventions (JSON casing, error
body shape, auth header, health payload) live in `../../../api-conventions.md` — not repeated
here. No secrets or credential values in any file below — reference the config key name only;
actual values live in `appsettings.Development.json`.

**Generated schema:** `GET /openapi/v1.json` on a running instance (e.g.
`http://localhost:5019/openapi/v1.json` in dev) is the source of truth for exact request/response
*shape*. The files below explain what each operation means and add real sample traffic; they
don't re-transcribe the generated schema.

## Operations

| Method | Path | Implements | Auth | File |
|---|---|---|---|---|
| GET | `/health` | API-REQ-006 | none | [`get-health.md`](get-health.md) |
| GET | `/menu` | API-REQ-001 | none | [`get-menu.md`](get-menu.md) |
| POST | `/auth/login` | API-REQ-002 | none | [`post-auth-login.md`](post-auth-login.md) |
| POST | `/orders` | API-REQ-003, API-RULE-001, API-RULE-002 | Bearer JWT | [`post-orders.md`](post-orders.md) |
| GET | `/orders/{id}` | API-REQ-004 | Bearer JWT | [`get-orders-id.md`](get-orders-id.md) |
| GET | `/orders` | API-REQ-005 | Bearer JWT | [`get-orders.md`](get-orders.md) |

## External Services

- None — `latteAPI` has no outbound dependencies in this phase.

## Configuration Keys

- `Jwt:Issuer` — token issuer claim value.
- `Jwt:Audience` — token audience claim value.
- `Jwt:SigningKey` — symmetric key used to sign/validate tokens. **Secret** — dev-only value
  acceptable in `appsettings.Development.json` per ADR-0001; must move to a real secret store
  before any non-local deployment.
- `Jwt:ExpiryHours` — token lifetime; `4` per API-NFR-002.
- `Waitresses` — array of `{ username, password, displayName }` accounts. **Contains secrets**
  (passwords) — same storage caveat as `Jwt:SigningKey`. See `../domain-model.md`'s
  "Seed / Example Data" section for a dummy illustrative example (not a real account).
