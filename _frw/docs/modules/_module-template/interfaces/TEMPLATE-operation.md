# <METHOD> <path>

Copy this file to `<method>-<path-with-dashes>.md` (e.g. `get-orders-id.md` for
`GET /orders/{id}`) or `mcp-tool-<name>.md` for a non-REST operation like an MCP tool.

- Implements: `<PREFIX>-REQ-XXX`
- Auth: <none | Bearer token | ...>
- Status: Draft | Confirmed — <verified date, once implemented>

## Request

<path/query/body parameters>

## Response

<success shape and status code(s)>

## Errors

- `<status>` — <condition> — see `../../../api-conventions.md` for the shared error body shape.

## Sample Requests & Responses

Captured <date> against <environment>. Use **real captured** traffic, not illustrative
pseudo-examples — this matters most while the entry is still `Draft`/low-confidence.

### Success

```http
<method> <path> HTTP/1.1
```

```http
HTTP/1.1 200 OK

<real response body>
```
