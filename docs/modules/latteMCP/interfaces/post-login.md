# POST /login

- Implements: MCP-REQ-004
- Auth: none (anonymous) — this endpoint's purpose is to obtain auth, mirroring `latteAPI`'s
  `POST /auth/login` (see ADR-0002)
- Status: Confirmed — verified 2026-08-22

## Request

`{ username, password }` — forwarded to `latteAPI`'s `POST /auth/login` unchanged.

## Response

`latteAPI`'s `POST /auth/login` response, returned as-is: same status code, body, and
`Content-Type` — see `../../latteAPI/interfaces/post-auth-login.md` for the authoritative shape.

## Errors

- Passes through `latteAPI`'s `401 Unauthorized` on bad credentials as-is.
- `502 Bad Gateway` with `{ "error": "latteAPI is unreachable." }` if `latteAPI` itself cannot be
  reached (connection failure) — distinct from a `401`, which means `latteAPI` *was* reached and
  rejected the credentials.

## Sample Requests & Responses

Captured 2026-08-22 against a local `dotnet run` instance. Credentials shown are dev-only example
values, not real ones — see `../../latteAPI/domain-model.md`'s "Seed / Example Data" section.

### Success

```http
POST /login HTTP/1.1
Content-Type: application/json

{"username":"carla","password":"carla-2026"}
```

```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8

{"token":"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...(truncated)","expiresAt":"2026-08-22T21:41:27.072648+00:00"}
```

### Failure — wrong password

```http
POST /login HTTP/1.1
Content-Type: application/json

{"username":"carla","password":"wrong"}
```

```http
HTTP/1.1 401 Unauthorized
Content-Type: application/json
```

### Failure — latteAPI unreachable

```http
POST /login HTTP/1.1
Content-Type: application/json

{"username":"carla","password":"carla-2026"}
```

```http
HTTP/1.1 502 Bad Gateway
Content-Type: application/json; charset=utf-8

{"error":"latteAPI is unreachable."}
```
