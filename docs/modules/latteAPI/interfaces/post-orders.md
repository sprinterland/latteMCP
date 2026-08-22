# POST /orders

- Implements: API-REQ-003, API-RULE-001, API-RULE-002
- Auth: Bearer JWT required (see ADR-0001)
- Status: Confirmed — verified 2026-08-22

## Request

`{ items: [{ menuItemId, size, quantity }, ...] }`.

## Response

`201 Created` with the full created order, including server-computed `total` and `createdBy`
(the authenticated waitress's display name — see `../domain-model.md`'s `Order` entity).
`Location` response header points at `/orders/{id}` for the new order.

## Errors

- `400 Bad Request` — see `../../../api-conventions.md` for the error body shape — if `items` is
  empty or any `menuItemId` doesn't exist. No order is stored in either case.
- `401 Unauthorized` — token missing/invalid/expired.

## Sample Requests & Responses

Captured 2026-08-22 against a local `dotnet run` instance with `../domain-model.md`'s current
seed menu.

### Success

```http
POST /orders HTTP/1.1
Content-Type: application/json
Authorization: Bearer {token from POST /auth/login}

{"items":[{"menuItemId":1,"size":"Medium","quantity":2},{"menuItemId":3,"size":"Large","quantity":1}]}
```

```http
HTTP/1.1 201 Created
Content-Type: application/json; charset=utf-8
Location: /orders/08637884-10ce-465b-8049-6d3b3fbf9f58

{"id":"08637884-10ce-465b-8049-6d3b3fbf9f58","items":[{"menuItemId":1,"size":"Medium","quantity":2},{"menuItemId":3,"size":"Large","quantity":1}],"status":"Received","createdAt":"2026-08-22T17:03:47.843665+00:00","total":14.15,"createdBy":"Carla"}
```

### Failure — unknown menu item

```http
POST /orders HTTP/1.1
Content-Type: application/json
Authorization: Bearer {token from POST /auth/login}

{"items":[{"menuItemId":999,"size":"Medium","quantity":1}]}
```

```http
HTTP/1.1 400 Bad Request
Content-Type: application/json; charset=utf-8

{"error":"Unknown menu item id: 999"}
```

### Failure — no token

```http
POST /orders HTTP/1.1
Content-Type: application/json

{"items":[{"menuItemId":1,"size":"Medium","quantity":1}]}
```

```http
HTTP/1.1 401 Unauthorized
WWW-Authenticate: Bearer
```
