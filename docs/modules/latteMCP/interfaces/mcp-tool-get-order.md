# MCP tool: get_order

- Implements: MCP-REQ-001
- Maps to (`latteAPI`): `GET /orders/{id}`
- Auth required: Yes — caller attaches `Authorization: Bearer <token>` on the MCP request
  (`MCP-REQ-002`); fails with a clear "not logged in" tool error if absent (`MCP-REQ-003`)
- Status: Confirmed — verified 2026-08-22

## Request / Response

Tool arguments: `{ id }` (order id, a GUID). Maps to `GET /orders/{id}` on `latteAPI` — see
`../../latteAPI/interfaces/get-orders-id.md`. The tool result's text content is the JSON-encoded
order, unmodified, forwarding the caller's bearer token unchanged (ADR-0003).

## Errors

- No `Authorization` header: MCP tool error, "Not logged in: ..." — the call never reaches
  `latteAPI` (`MCP-REQ-003`).
- Unknown `id` or invalid/expired token: MCP tool error with a message starting
  `latteAPI returned <status> <reason>` (`404`/`401`), the same status/meaning `latteAPI` itself
  returned.

## Sample Requests & Responses

Captured 2026-08-22 against a local `dotnet run` instance (`tools/call` request/result payloads).

### Success

Request (with a valid token, `id` from a prior `place_order` call):

```json
{"name":"get_order","arguments":{"id":"a8f10558-5827-46b8-8db3-146797ceb47e"}}
```

Result:

```json
{"content":[{"type":"text","text":"{\"id\":\"a8f10558-5827-46b8-8db3-146797ceb47e\",\"items\":[{\"menuItemId\":1,\"size\":\"Medium\",\"quantity\":2}],\"status\":\"Received\",\"createdAt\":\"2026-08-22T17:43:07.423012+00:00\",\"total\":9.70,\"createdBy\":\"Carla\"}"}]}
```

### Failure — unknown id

Request (with a valid token):

```json
{"name":"get_order","arguments":{"id":"00000000-0000-0000-0000-000000000000"}}
```

Result:

```json
{"content":[{"type":"text","text":"An error occurred invoking 'get_order': latteAPI returned 404 Not Found"}],"isError":true}
```
