# POST /auth/login

- Implements: API-REQ-002
- Auth: none (anonymous) — this endpoint's purpose is to *grant* auth, see API-RULE-004
- Status: Confirmed — verified 2026-08-22

## Request

`{ username, password }`.

## Response

`200 OK` with `{ token, expiresAt }` on success; `token` is a signed JWT carrying the waitress's
identity as a claim (see ADR-0001).

## Errors

`401 Unauthorized` for an unknown username or wrong password — deliberately undifferentiated
(same status, empty body, no distinguishing detail) so a caller can't enumerate valid usernames
by comparing responses.

## Sample Requests & Responses

Captured 2026-08-22 against a local `dotnet run` instance. Credentials shown are dev-only
example values, not real ones — see `../domain-model.md`'s "Seed / Example Data" section.

### Success

```http
POST /auth/login HTTP/1.1
Content-Type: application/json

{"username":"carla","password":"carla-2026"}
```

```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8

{"token":"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...(truncated)","expiresAt":"2026-08-22T21:03:47.56283+00:00"}
```

### Failure — wrong password

```http
POST /auth/login HTTP/1.1
Content-Type: application/json

{"username":"carla","password":"wrong"}
```

```http
HTTP/1.1 401 Unauthorized
```
