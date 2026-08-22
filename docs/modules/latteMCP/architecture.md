# latteMCP — Architecture (current implementation)

Describes the *current* build of this module (implemented and manually verified 2026-08-22).
Expected to change if this module is ported to a different stack — see `../../decisions/` for
why current choices were made.

## Internal Structure

- ASP.NET Core host, single project (`src/latteMCP`), exposing two distinct surfaces on the same
  process (see ADR-0002 for why login sits outside the MCP surface):
  - Plain REST endpoints (`POST /login`, `GET /health`) defined directly in `Program.cs`.
  - MCP tool endpoint (`/mcp`), mapped via `app.MapMcp("/mcp")` — the pattern must be passed
    explicitly; the SDK's own default for a bare `MapMcp()` call is the root path (`""`), not
    `/mcp` — and backed by the `[McpServerToolType]` class `Tools/OrderingTools.cs`, whose static
    methods implement `get_menu`, `place_order`, `get_order`, `list_orders` (see `interfaces/`).
- A typed `HttpClient` (`Services/LatteApiClient.cs`) configured to call `latteAPI` from
  `LatteApi:BaseUrl`, used by both surfaces — the REST `/login`/`/health` wrappers in
  `Program.cs` and every MCP tool that talks to `latteAPI`. It returns raw `HttpResponseMessage`s
  so callers can pass status codes through unchanged or translate them into MCP tool errors.
- Each tool reads the caller's `Authorization` header directly off the current request via
  `IHttpContextAccessor` (bound automatically as a tool-method parameter — MCP tool methods
  resolve any parameter type registered in DI from the request's service provider, so it never
  appears in the tool's JSON input schema) — see `MCP-RULE-001`. No token cache, no session store.
- `LatteApiJsonOptions.Default` (a copy of the MCP SDK's `McpJsonUtilities.DefaultOptions` with a
  `JsonStringEnumConverter` added) is shared by the outbound `HttpClient` calls to `latteAPI`
  *and* passed to `WithToolsFromAssembly(serializerOptions: ...)`, so tool schemas, tool
  arguments, and the JSON sent to/read from `latteAPI` all agree on enums-as-strings (matching
  `../../../api-conventions.md`). Building the options from scratch instead of copying
  `McpJsonUtilities.DefaultOptions` fails at startup — the reflection-based tool binder requires a
  populated `TypeInfoResolver` chain.

## Tech Stack

- Language / runtime / version: C# / .NET 10.
- Framework(s): ASP.NET Core, `ModelContextProtocol.AspNetCore` (MCP server hosting +
  streamable HTTP transport).
- Key libraries and why each was chosen:
  - `ModelContextProtocol.AspNetCore` — official MCP server SDK for .NET; provides
    `AddMcpServer().WithHttpTransport()` / `MapMcp()` and the `[McpServerTool]` attribute model,
    avoiding a hand-rolled MCP protocol implementation.

## Deployment

- Standalone Kestrel process, run via `dotnet run` in `src/latteMCP`. Dev URL:
  `http://localhost:5040` (`https://localhost:7284`) per `Properties/launchSettings.json`.
- Part of the `latteMCP.slnx` solution; deployed and run independently of `latteAPI` — see
  `../../architecture/overview.md`.

## Dependencies

- `latteAPI`, over HTTP — base URL configurable per environment (see `interfaces/README.md`'s
  Configuration Keys and ADR-0002's related discussion). This is the module's only outbound
  dependency; every tool call and the `/login` wrapper ultimately calls `latteAPI`.
