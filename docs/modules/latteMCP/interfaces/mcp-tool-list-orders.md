# MCP tool: list_orders

- Implements: MCP-REQ-001
- Maps to (`latteAPI`): `GET /orders`
- Auth required: Yes — caller attaches `Authorization: Bearer <token>` on the MCP request
  (`MCP-REQ-002`); fails with a clear "not logged in" tool error if absent (`MCP-REQ-003`)
- Status: Confirmed — verified 2026-08-22

## Request / Response

Takes no arguments. Maps to `GET /orders` on `latteAPI` — see
`../../latteAPI/interfaces/get-orders.md`. The tool result's text content is the JSON-encoded
array of orders, unmodified, forwarding the caller's bearer token unchanged (ADR-0003).

## Errors

- No `Authorization` header: MCP tool error, "Not logged in: ..." — the call never reaches
  `latteAPI` (`MCP-REQ-003`). Confirms `MCP-RULE-001`: a token attached on an earlier tool call in
  the same client session is not remembered for this one.
- Invalid/expired token: MCP tool error with a message starting `latteAPI returned 401
  Unauthorized`.

## Sample Requests & Responses

Captured 2026-08-22 against a local `dotnet run` instance (`tools/call` request/result payloads).

### Success

Request (with a valid token):

```json
{"name":"list_orders","arguments":{}}
```

Result:

```json
{"content":[{"type":"text","text":"[{\"id\":\"a8f10558-5827-46b8-8db3-146797ceb47e\",\"items\":[{\"menuItemId\":1,\"size\":\"Medium\",\"quantity\":2}],\"status\":\"Received\",\"createdAt\":\"2026-08-22T17:43:07.423012+00:00\",\"total\":9.70,\"createdBy\":\"Carla\"}]"}]}
```

### Failure — no Authorization header (MCP-TEST-006)

Request (no `Authorization` header, even though an earlier `place_order` call in the same client
session did carry one):

```json
{"name":"list_orders","arguments":{}}
```

Result:

```json
{"content":[{"type":"text","text":"An error occurred invoking 'list_orders': Not logged in: this request has no Authorization header. Call POST /login first and attach the returned token as this MCP request's 'Authorization: Bearer <token>' header."}],"isError":true}
```
