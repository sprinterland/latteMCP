# latteMCPclient — Architecture (current implementation)

Describes the *current* build of this module. Expected to change if this module is ported to a
different stack — see `../../decisions/` for why current choices were made.

## Internal Structure

- Console application (`src/latteMCPclient`), single `Program.cs` entry point.
- Startup sequence: prompt for credentials → call `latteMCP`'s `POST /login` → open MCP
  connection with the token attached → list tools → run the scripted demo flow
  (`CLIENT-REQ-001` through `CLIENT-REQ-004`).

## Tech Stack

- Language / runtime / version: C# / .NET 10.
- Framework(s): none (plain console app).
- Key libraries and why each was chosen:
  - `ModelContextProtocol` (client SDK) — official MCP client library for .NET, used to open the
    MCP connection and call tools without hand-rolling the protocol.
  - A plain `HttpClient` call for `POST /login`, since that's a normal REST call outside the MCP
    protocol (see ADR-0002).

## Deployment

- Run via `dotnet run` in `src/latteMCPclient`; not a hosted service — an interactive CLI
  process, started manually against a running `latteMCP` (and, transitively, `latteAPI`).

## Dependencies

- `latteMCP`, via:
  - Plain HTTP for `POST /login`.
  - The MCP protocol (streamable HTTP transport) for tool calls.
- No direct dependency on `latteAPI` — all business operations go through `latteMCP`.
