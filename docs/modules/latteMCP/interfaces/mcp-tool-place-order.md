# MCP tool: place_order

- Implements: MCP-REQ-001
- Maps to (`latteAPI`): `POST /orders`
- Auth required: Yes — caller attaches `Authorization: Bearer <token>` on the MCP request
  (`MCP-REQ-002`); fails with a clear "not logged in" tool error if absent (`MCP-REQ-003`)
- Status: Confirmed — verified 2026-08-22

## Request / Response

Tool arguments: `{ items: [{ menuItemId, size, quantity }, ...] }` — same shape as `latteAPI`'s
`POST /orders` body (`size` is one of `"Small"`/`"Medium"`/`"Large"`; see
`../../latteAPI/interfaces/post-orders.md`). The tool result's text content is the JSON-encoded
created order, unmodified, forwarding the caller's bearer token unchanged (ADR-0003).

## Errors

- No `Authorization` header on the MCP request: MCP tool error (`isError: true`), message "Not
  logged in: ..." — the call never reaches `latteAPI` (`MCP-REQ-003`).
- `latteAPI` rejects the request (e.g. unknown `menuItemId` → `400`, invalid/expired token →
  `401`): MCP tool error with a message starting `latteAPI returned <status> <reason>`, including
  `latteAPI`'s error body when present.

## Sample Requests & Responses

Captured 2026-08-22 against a local `dotnet run` instance (`tools/call` request/result payloads).

### Success

Request (with `Authorization: Bearer <token from POST /login>` on the MCP request):

```json
{"name":"place_order","arguments":{"items":[{"menuItemId":1,"size":"Medium","quantity":2}]}}
```

Result:

```json
{"content":[{"type":"text","text":"{\"id\":\"a8f10558-5827-46b8-8db3-146797ceb47e\",\"items\":[{\"menuItemId\":1,\"size\":\"Medium\",\"quantity\":2}],\"status\":\"Received\",\"createdAt\":\"2026-08-22T17:43:07.423012+00:00\",\"total\":9.70,\"createdBy\":\"Carla\"}"}]}
```

### Failure — no Authorization header

Request (no `Authorization` header on the MCP request):

```json
{"name":"place_order","arguments":{"items":[{"menuItemId":1,"size":"Medium","quantity":2}]}}
```

Result:

```json
{"content":[{"type":"text","text":"An error occurred invoking 'place_order': Not logged in: this request has no Authorization header. Call POST /login first and attach the returned token as this MCP request's 'Authorization: Bearer <token>' header."}],"isError":true}
```

### Failure — unknown menu item

Request (with a valid token):

```json
{"name":"place_order","arguments":{"items":[{"menuItemId":999,"size":"Medium","quantity":1}]}}
```

Result:

```json
{"content":[{"type":"text","text":"An error occurred invoking 'place_order': latteAPI returned 400 Bad Request: {\"error\":\"Unknown menu item id: 999\"}"}],"isError":true}
```
