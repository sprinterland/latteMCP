# System Architecture — Overview

## As-Is (current system)

latteMCP is a small coffee-shop ordering demo made of three independently-run .NET processes
that build on each other in a straight line — there is no fan-out or shared infrastructure
between them beyond that chain.

- System Components:
  - `latteAPI` — business API (menu, orders, waitress login) — module
    [`modules/latteAPI/`](../modules/latteAPI/) — runs standalone on `http://localhost:5019`
    (dev).
  - `latteMCP` — MCP server + thin REST wrapper — module
    [`modules/latteMCP/`](../modules/latteMCP/) — runs standalone on `http://localhost:5040`
    (dev).
  - `latteMCPclient` — interactive console demo client — module
    [`modules/latteMCPclient/`](../modules/latteMCPclient/) — not hosted; run manually per
    session.

- Service boundaries and communication:
  - `latteMCPclient` → `latteMCP`: two separate channels to the same process — plain HTTP for
    `POST /login`, and the MCP protocol (streamable HTTP transport) for everything else. Both
    synchronous request/response. See ADR-0002 for why login is split out this way.
  - `latteMCP` → `latteAPI`: plain HTTP, synchronous request/response, for both the `/login`
    wrapper and every MCP tool call. `latteMCP` never talks to anything other than `latteAPI` —
    see ADR-0003 for why it holds no state of its own about these calls.
  - Identity flows waitress → `latteMCPclient` → `latteMCP` → `latteAPI` as a bearer token
    obtained once (ADR-0001) and forwarded unchanged at each hop (ADR-0003) — no service holds
    or re-derives it in between.

- Deployment / infrastructure topology: three independent local processes for local
  development, no shared database, no message broker, no container/orchestration layer defined
  yet. `latteAPI` has no dependencies; `latteMCP` depends only on `latteAPI` being reachable
  (surfaced via its own health check, ADR-0004); `latteMCPclient` depends only on `latteMCP`.

- Cross-module integration points: see each module's interfaces for the actual endpoint/tool
  contracts — [`latteAPI`](../modules/latteAPI/interfaces/README.md),
  [`latteMCP`](../modules/latteMCP/interfaces/README.md),
  [`latteMCPclient`](../modules/latteMCPclient/interfaces.md) (no operations of its own, see
  ADR-0005). Shared API conventions live in [`../api-conventions.md`](../api-conventions.md).

## To-Be (target, only during an active migration/rewrite)

Not applicable — no migration or rewrite is underway. This project is being built forward from
an empty implementation, not replacing an existing system.
