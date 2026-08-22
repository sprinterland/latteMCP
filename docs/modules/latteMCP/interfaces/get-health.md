# GET /health

- Implements: MCP-REQ-005
- Auth: none (anonymous)
- Status: Confirmed — verified 2026-08-22

## Request

No body.

## Response

`200 OK` with `{ "status": "ok" }` only if both `latteMCP` itself is up **and** its call to
`latteAPI`'s `GET /health` succeeds. Doesn't follow the plain "always 200 if the process is up"
convention in `../../../api-conventions.md` — this endpoint's whole purpose is to also verify the
upstream (ADR-0004), so its success condition is stricter than the shared default.

## Errors

`503 Service Unavailable` with `{ "status": "unhealthy" }` if the call to `latteAPI`'s
`GET /health` fails (non-2xx response or `latteAPI` unreachable) — reflects the upstream being
down as an unhealthy result, not a hidden failure.

## Sample Requests & Responses

Captured 2026-08-22 against a local `dotnet run` instance.

### Success — latteAPI reachable

```http
GET /health HTTP/1.1
```

```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8

{"status":"ok"}
```

### Failure — latteAPI unreachable

```http
GET /health HTTP/1.1
```

```http
HTTP/1.1 503 Service Unavailable
Content-Type: application/json; charset=utf-8

{"status":"unhealthy"}
```
