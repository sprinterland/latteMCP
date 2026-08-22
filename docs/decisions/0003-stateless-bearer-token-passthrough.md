# ADR-0003: latteMCP forwards bearer tokens statelessly; no server-side session cache

- Date: 2026-08-22
- Status: Accepted
- Modules affected: latteMCP
- Context: Once ADR-0002 moved login out of the MCP tool set, `latteMCP` still needs a way for
  its tools (`place_order`, `get_order`, `list_orders`) to know which waitress is calling, so
  that identity reaches `latteAPI`.
- Options considered:
  - **Session-scoped token cache.** Cache the token server-side, keyed by MCP session id (set
    during a `login` tool call, in the design ADR-0002 rejected) or some other correlation
    mechanism, and have tools look it up implicitly. Keeps tool call signatures clean (no auth
    parameter needed), but requires `latteMCP` to hold per-session state, which complicates
    scaling/restarts (session affinity, cache invalidation, a token that could go stale in the
    cache independent of its own expiry) and only works cleanly if login itself happens inside
    the same session — which ADR-0002 ruled out anyway.
  - **Stateless pass-through.** Chosen. Each MCP request carries its own
    `Authorization: Bearer <token>` header, set by the caller (obtained once from `POST /login`,
    reused for the rest of the conversation — see `latteMCPclient`'s `CLIENT-REQ-002`). Every
    tool simply forwards whatever header arrived on that specific request straight to `latteAPI`
    and does not remember it afterward.
- Decision: `latteMCP` keeps no server-side session or token state at all. Identity flows purely
  through the `Authorization` header on each individual MCP request, pass-through only. A tool
  call arriving without that header fails immediately with a clear "not logged in" error,
  without ever reaching `latteAPI`.
- Consequences: `latteMCP` instances can be freely restarted or scaled horizontally with no
  session-affinity requirement, since nothing is kept between requests. The token's validity is
  governed entirely by its own JWT expiry (ADR-0001), with no separate cache that could drift out
  of sync with it. The trade-off: the client (not `latteMCP`) is responsible for holding the
  token and attaching it to every request — there's no protocol-level convenience for "you're
  already logged in on this connection."
