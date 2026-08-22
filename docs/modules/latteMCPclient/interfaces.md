# latteMCPclient — Interfaces & Contracts

No secrets or credential values — reference the config key name only; actual values live in the
secret store. This module exposes no endpoints of its own — it is only a caller.

## External Services

- **latteMCP** — the MCP server this client exercises — base URL config key
  `LatteMcp:BaseUrl` (not the value) — auth: waitress credentials, entered interactively at
  startup (`CLIENT-REQ-001`), then a bearer token attached per request (`CLIENT-REQ-002`). See
  `../latteMCP/interfaces/README.md` for the endpoints/tools called. This module has no
  operations of its own to document — it's a pure caller — so it keeps a single `interfaces.md`
  rather than the per-operation folder ADR-0005 uses for modules with an API surface.

## Configuration Keys

- `LatteMcp:BaseUrl` — base URL of the `latteMCP` instance to connect to; configurable per
  environment, mirroring `latteMCP`'s own `LatteApi:BaseUrl` pattern.
