# GET /health

- Implements: API-REQ-006
- Auth: none (anonymous)
- Status: Confirmed — verified 2026-08-22

## Request

No body.

## Response

`200 OK` — see `../../../api-conventions.md` for the shared health-payload shape. Absence of a
response (connection failure/timeout) is itself the negative signal, consumed by `latteMCP`'s
own health check (ADR-0004) — there is no "unhealthy but responding" case for this endpoint.

## Errors

None expected.

## Sample Requests & Responses

Captured 2026-08-22 against a local `dotnet run` instance.

### Success

```http
GET /health HTTP/1.1
```

```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8

{"status":"ok"}
```
