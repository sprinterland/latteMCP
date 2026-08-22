# GET /orders

- Implements: API-REQ-005
- Auth: Bearer JWT required
- Status: Confirmed — verified 2026-08-22

## Request

No body.

## Response

`200 OK`, array of all orders (same per-order shape as `POST /orders`'s response), most recent
first. Not paginated in this phase (no requirement calls for it — `../requirements.md` has no
`API-NFR` on result size).

## Errors

`401 Unauthorized` — token missing/invalid/expired.

## Sample Requests & Responses

Captured 2026-08-22 against a local `dotnet run` instance, after two prior orders were placed.

### Success

```http
GET /orders HTTP/1.1
Authorization: Bearer {token from POST /auth/login}
```

```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8

[{"id":"b0dba06c-0fb2-46a9-ac79-399beac7c418","items":[{"menuItemId":4,"size":"Small","quantity":1}],"status":"Received","createdAt":"2026-08-22T17:03:47.872526+00:00","total":4.75,"createdBy":"Carla"},{"id":"08637884-10ce-465b-8049-6d3b3fbf9f58","items":[{"menuItemId":1,"size":"Medium","quantity":2},{"menuItemId":3,"size":"Large","quantity":1}],"status":"Received","createdAt":"2026-08-22T17:03:47.843665+00:00","total":14.15,"createdBy":"Carla"}]
```

### Failure — no token

```http
GET /orders HTTP/1.1
```

```http
HTTP/1.1 401 Unauthorized
WWW-Authenticate: Bearer
```
