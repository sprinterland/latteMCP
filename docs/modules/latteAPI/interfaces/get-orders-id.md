# GET /orders/{id}

- Implements: API-REQ-004
- Auth: Bearer JWT required
- Status: Confirmed — verified 2026-08-22

## Request

`id` (GUID) in the path. No body.

## Response

`200 OK` with the order (same shape as `POST /orders`'s response), or `404 Not Found` if `id`
doesn't exist. Any authenticated waitress can look up any order by id — this endpoint doesn't
restrict results to orders created by the caller (see `../requirements.md` — no such restriction
is specified; flagged here as an explicit design note in case that changes later).

## Errors

- `401 Unauthorized` — token missing/invalid/expired.
- `404 Not Found` — no order with that id — see `../../../api-conventions.md` for the (empty)
  error body shape.

## Sample Requests & Responses

Captured 2026-08-22 against a local `dotnet run` instance.

### Success

```http
GET /orders/b0dba06c-0fb2-46a9-ac79-399beac7c418 HTTP/1.1
Authorization: Bearer {token from POST /auth/login}
```

```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8

{"id":"b0dba06c-0fb2-46a9-ac79-399beac7c418","items":[{"menuItemId":4,"size":"Small","quantity":1}],"status":"Received","createdAt":"2026-08-22T17:03:47.872526+00:00","total":4.75,"createdBy":"Carla"}
```

### Failure — not found

```http
GET /orders/00000000-0000-0000-0000-000000000000 HTTP/1.1
Authorization: Bearer {token from POST /auth/login}
```

```http
HTTP/1.1 404 Not Found
```
