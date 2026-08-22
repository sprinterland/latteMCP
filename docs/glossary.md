# Glossary

Shared vocabulary across all modules. If a term means something different in two modules, that's
worth resolving explicitly — either it's genuinely two concepts that should have two names, or
the modules have drifted and should agree on one meaning. Module-specific jargon that doesn't
apply elsewhere can live in that module's `domain-model.md` instead.

- **Waitress** — the person operating `latteMCPclient` (or any MCP client) on behalf of the
  coffee shop; the identity every order is attributed to. Authenticates via `latteAPI`'s
  `POST /auth/login`. See `modules/latteAPI/domain-model.md`.
- **Menu item** — a drink the shop sells, with a name, description, and base price. See
  `modules/latteAPI/domain-model.md`.
- **Order** — a waitress's request for one or more menu items (as order lines), with a
  server-computed total and the waitress who placed it recorded against it. See
  `modules/latteAPI/domain-model.md`.
- **Bearer token** — the JWT a waitress receives from login and attaches to subsequent requests
  to prove her identity. Issued and validated only by `latteAPI` (ADR-0001); `latteMCP` only
  ever forwards it (ADR-0003).
- **MCP tool** — an operation an MCP client can discover and call on `latteMCP`'s `/mcp`
  endpoint (e.g. `place_order`). Distinct from a plain REST endpoint like `latteMCP`'s
  `POST /login`, which is deliberately *not* a tool — see ADR-0002.
- **MCP session** — one client's ongoing connection to `latteMCP`'s MCP endpoint, spanning
  multiple tool calls. `latteMCP` holds no state scoped to a session (ADR-0003) — every request
  within it must still carry its own bearer token.
