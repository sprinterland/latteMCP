# ADR-0002: Login is a plain REST endpoint on latteMCP, not an MCP tool

- Date: 2026-08-22
- Status: Accepted
- Modules affected: latteMCP, latteMCPclient
- Context: `latteMCP` needs some way to let a caller turn waitress credentials (ADR-0001) into a
  token it can use for the rest of an MCP session. The first design considered exposing this as
  an MCP tool (`login(username, password)`) callable like any other tool, since `latteMCP`
  already hosts an MCP tool surface for everything else.
- Options considered:
  - **`login` as an MCP tool**, caching the resulting token server-side keyed by MCP session id,
    so later tool calls in the same session wouldn't need the caller to resend it. Keeps every
    capability behind the same protocol. Downsides: credentials and tokens becoming part of the
    model-visible tool-call surface is undesirable when it can be avoided — an MCP tool's
    arguments and results are things a connected agent can see and potentially reason about or
    log; and it requires `latteMCP` to track session-scoped state, which the rest of this
    system's design (ADR-0003) is trying to avoid.
  - **`login` as a plain REST endpoint (`POST /login`)**, called over ordinary HTTP *before* the
    MCP connection is opened. Chosen.
- Decision: `latteMCP` exposes `POST /login` as a normal REST endpoint, separate from its `/mcp`
  tool surface. It forwards credentials to `latteAPI`'s `POST /auth/login` and returns that
  response as-is. The MCP tool set contains no `login` tool. A client authenticates via this
  plain HTTP call first, then opens its MCP connection and attaches the resulting bearer token
  to every subsequent MCP request itself (see ADR-0003).
- Consequences: Credentials and tokens never appear as MCP tool arguments/results. Authentication
  happens in its natural place — before the MCP session exists — rather than as the required
  first call inside one. This also removes any need for `latteMCP` to correlate a login call
  with a specific MCP session, which directly enables the stateless design in ADR-0003. The
  trade-off: a client now needs two separate connections/protocols (plain HTTP for `/login`, MCP
  for everything else) instead of one uniform MCP surface for the whole interaction.
