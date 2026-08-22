# MCP tool: get_menu

- Implements: MCP-REQ-001
- Maps to (`latteAPI`): `GET /menu`
- Auth required: No
- Status: Confirmed — verified 2026-08-22

## Request / Response

Mirrors `latteAPI`'s `GET /menu` contract exactly — see
`../../latteAPI/interfaces/get-menu.md`. Takes no arguments. The tool result's text content is
the JSON-encoded `latteAPI` response body, unmodified.

## Errors

A tool call that reaches `latteAPI` and gets an error back surfaces as an MCP tool error
(`isError: true`) with a message starting `latteAPI returned <status> <reason>`, optionally
followed by `latteAPI`'s error body — the same status/meaning, not a generic failure.

## Sample Requests & Responses

Captured 2026-08-22 against a local `dotnet run` instance (`tools/call` request/result, shown as
the JSON-RPC `params`/`result` payloads — transport framing per the MCP Streamable HTTP spec).

### Success

Request:

```json
{"name":"get_menu","arguments":{}}
```

Result:

```json
{"content":[{"type":"text","text":"{\"items\":[{\"id\":1,\"name\":\"Latte\",\"description\":\"Espresso with steamed milk\",\"basePrice\":4.25},{\"id\":2,\"name\":\"Cappuccino\",\"description\":\"Espresso with steamed milk foam\",\"basePrice\":4.25},{\"id\":3,\"name\":\"Americano\",\"description\":\"Espresso with hot water\",\"basePrice\":3.25},{\"id\":4,\"name\":\"Mocha\",\"description\":\"Espresso with chocolate and steamed milk\",\"basePrice\":4.75},{\"id\":5,\"name\":\"Espresso\",\"description\":\"A double shot, no milk\",\"basePrice\":2.75}],\"sizeSurcharge\":{\"Small\":0,\"Medium\":0.60,\"Large\":1.20}}"}]}
```
