# latteAPI — Architecture (current implementation)

Describes the *current* build of this module. Expected to change if this module is ported to a
different stack — see `../../decisions/` for why current choices were made.

## Internal Structure

- ASP.NET Core Minimal API, single project (`src/latteAPI`), endpoints defined directly in
  `Program.cs` (no separate controller layer at this scale).
- `Data/MenuCatalog.cs` — static, hardcoded menu + size-surcharge table.
- `Data/OrderStore.cs` — in-memory order store (`ConcurrentDictionary<Guid, Order>`),
  registered as a DI singleton.
- JWT signing/issuer/audience/expiry configuration — bound from `appsettings.json` via
  `IOptions<JwtSettings>` (see ADR-0001). Waitress accounts — bound once at startup from the
  `Waitresses` config section into a `List<WaitressAccount>` registered as a DI singleton
  (config key values in `appsettings.Development.json`; see ADR-0001's dev-only-secrets caveat).
  No dedicated data-access layer since there's no database.

## Tech Stack

- Language / runtime / version: C# / .NET 10.
- Framework(s): ASP.NET Core Minimal APIs, ASP.NET Core JWT Bearer authentication
  (`Microsoft.AspNetCore.Authentication.JwtBearer`).
- Key libraries and why each was chosen:
  - JWT Bearer middleware — standard ASP.NET Core building block for issuing/validating tokens,
    avoids hand-rolling token validation. See ADR-0001 for the identity design overall.
  - `Microsoft.AspNetCore.OpenApi` (`AddOpenApi()` + `MapOpenApi()`, Development-only) — built-in
    generated OpenAPI document, served at `/openapi/v1.json`. Not hand-maintained, so it can't
    drift from the endpoint definitions in `Program.cs`; see ADR-0005 for why this is now
    required rather than optional, and `docs/modules/latteAPI/interfaces/` for the per-operation
    docs that link to it.

## Deployment

- Standalone Kestrel process, run via `dotnet run` in `src/latteAPI`. Dev URL:
  `http://localhost:5019` (`https://localhost:7181`) per `Properties/launchSettings.json`.
- Part of the `latteMCP.slnx` solution alongside `latteMCP` and `latteMCPclient`, but deployed
  and run as an independent process — see `../../architecture/overview.md` for how the three
  apps relate.

## Dependencies

- None on other modules in this system — `latteAPI` is the base of the dependency chain
  (`latteMCP` depends on it, not the other way around).
- No external services or databases in this phase (see `API-NFR-001` in `requirements.md`).
