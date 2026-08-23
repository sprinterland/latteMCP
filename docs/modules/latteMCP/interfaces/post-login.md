# POST /login

- Implements: MCP-REQ-004
- Auth: none (anonymous) — this endpoint's purpose is to obtain auth, mirroring `latteAPI`'s
  `POST /auth/login` (see ADR-0002)
- Status: Confirmed — verified 2026-08-22

## Request

`{ username, password }` — forwarded to `latteAPI`'s `POST /auth/login` unchanged.

## Response

`latteAPI`'s `POST /auth/login` response, returned as-is: same status code and body — see
`../../latteAPI/interfaces/post-auth-login.md` for the authoritative shape. `Content-Type` is
copied from `latteAPI`'s response when present, but falls back to `application/json` when it
isn't (e.g. `latteAPI`'s empty-bodied `401`, which carries no `Content-Type` at all) — see the
"Failure — wrong password" sample below, which has a `Content-Type` header `latteAPI`'s own
equivalent response does not.

## Errors

- Passes through `latteAPI`'s `401 Unauthorized` on bad credentials as-is.
- `502 Bad Gateway` with `{ "error": "latteAPI is unreachable." }` if `latteAPI` itself cannot be
  reached (connection failure) — distinct from a `401`, which means `latteAPI` *was* reached and
  rejected the credentials.

## Sample Requests & Responses

Captured 2026-08-22 against a local `dotnet run` instance. `password` is redacted below per
CLAUDE.md Rule 10 — the real request used a working seeded account's actual password, which must
not appear in `docs/`; see `../../latteAPI/domain-model.md`'s "Seed / Example Data" section for a
fabricated illustrative account shape instead. Status, headers, and the response body are
otherwise real and unmodified.

### Success

```http
POST /login HTTP/1.1
Content-Type: application/json

{"username":"carla","password":"<redacted — see Waitresses config key>"}
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
