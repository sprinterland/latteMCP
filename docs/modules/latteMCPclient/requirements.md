# latteMCPclient — Requirements

Technology-agnostic. Should read the same regardless of implementation language/framework.
IDs are prefixed with the module name and are never reused or renumbered — mark removed items
`Deprecated` instead of deleting them.

## Functional Requirements

### CLIENT-REQ-001: Interactive login at startup

- Description: On startup, the client prompts the operator for a username and password
  (password input not echoed to the console), then obtains a token via `latteMCP`'s
  `POST /login` (`MCP-REQ-004`) before doing anything else.
- Rationale: This client exists to demonstrate/exercise the waitress-identity flow end-to-end;
  it needs a real identity before it can call any tool that requires one.
- Source: Confirmed by user in conversation on 2026-08-22
- Status: Draft

### CLIENT-REQ-002: Authenticated MCP session

- Description: After login, the client opens its MCP connection to `latteMCP` with
  `Authorization: Bearer <token>` attached to every request for the rest of the run.
- Rationale: `latteMCP` keeps no server-side session state (`MCP-RULE-001`), so the client is
  responsible for holding and resending the token itself on every call.
- Source: Confirmed by user in conversation on 2026-08-22
- Status: Draft

### CLIENT-REQ-003: List available tools

- Description: After connecting, the client lists the tools `latteMCP` exposes and prints them.
- Rationale: Basic sanity check that the MCP connection and handshake worked before running the
  scripted demo.
- Source: Draft (proposed by Claude, pending confirmation)
- Status: Draft

### CLIENT-REQ-004: Scripted end-to-end demo

- Description: The client runs a fixed sequence — fetch the menu, place an order, fetch that
  order's status, list all orders — printing the result of each step.
- Rationale: Proves the whole chain (`latteMCPclient` → `latteMCP` → `latteAPI`) works, and that
  the resulting order is attributed to the logged-in waitress (`API-RULE-002`).
- Source: Draft (proposed by Claude, pending confirmation)
- Status: Draft

## Business Rules

None specific to this module — it enforces no business rules of its own; it only exercises rules
owned by `latteAPI` (see `../latteAPI/requirements.md`) through `latteMCP`.

## Non-Functional Requirements

None identified yet — this is a demo/test client, not a production-facing component.
