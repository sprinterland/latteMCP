# GET /menu

- Implements: API-REQ-001
- Auth: none (anonymous) — see API-RULE-004
- Status: Confirmed — verified 2026-08-22

## Request

No body.

## Response

`200 OK` with `{ items: [{ id, name, description, basePrice }, ...], sizeSurcharge: { Small,
Medium, Large: decimal } }`. Actual current values are in `../domain-model.md`'s
"Seed / Example Data" section — keep this doc and that one pointing at the same source of truth
(`src/latteAPI/Data/MenuCatalog.cs`) rather than duplicating the numbers a third time here.

## Errors

None expected.

## Sample Requests & Responses

Captured 2026-08-22 against a local `dotnet run` instance.

### Success

```http
GET /menu HTTP/1.1
```

```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8

{"items":[{"id":1,"name":"Latte","description":"Espresso with steamed milk","basePrice":4.25},{"id":2,"name":"Cappuccino","description":"Espresso with steamed milk foam","basePrice":4.25},{"id":3,"name":"Americano","description":"Espresso with hot water","basePrice":3.25},{"id":4,"name":"Mocha","description":"Espresso with chocolate and steamed milk","basePrice":4.75},{"id":5,"name":"Espresso","description":"A double shot, no milk","basePrice":2.75}],"sizeSurcharge":{"Small":0,"Medium":0.60,"Large":1.20}}
```
