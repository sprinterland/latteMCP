# latteMCPclient — Test Specification

Each entry maps to exactly one requirement or rule and should translate directly into one or
more automated tests. New tests are written FROM this document, not inferred from existing code.

## CLIENT-TEST-001 (implements CLIENT-REQ-001, happy path)

- Given: a valid waitress account exists and `latteMCP` is running.
- When: the client is run and the operator enters correct credentials.
- Then: the client obtains a token from `latteMCP`'s `POST /login` and proceeds past the login
  step.
- Status: Draft

## CLIENT-TEST-002 (implements CLIENT-REQ-001, failure path)

- Given: `latteMCP` is running.
- When: the operator enters an incorrect password.
- Then: the client reports the login failure clearly and does not proceed to open an MCP
  session.
- Status: Draft

## CLIENT-TEST-003 (implements CLIENT-REQ-002)

- Given: a token was obtained via login.
- When: the client opens its MCP connection and makes subsequent tool calls.
- Then: every MCP request carries `Authorization: Bearer <token>`.
- Status: Draft

## CLIENT-TEST-004 (implements CLIENT-REQ-003)

- Given: a successful MCP connection.
- When: the client lists tools.
- Then: `get_menu`, `place_order`, `get_order`, `list_orders` are printed.
- Status: Draft

## CLIENT-TEST-005 (implements CLIENT-REQ-004, end-to-end)

- Given: a logged-in client with an open MCP session.
- When: the scripted demo flow runs.
- Then: the menu is printed, an order is placed and printed (with a `CreatedBy` matching the
  logged-in waitress), that order is fetched by id and printed, and the order list (including
  it) is printed.
- Status: Draft
