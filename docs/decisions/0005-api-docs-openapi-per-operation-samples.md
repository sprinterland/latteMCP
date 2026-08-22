# ADR-0005: Generated OpenAPI + per-operation interface files + captured samples

- Date: 2026-08-22
- Status: Accepted
- Modules affected: system-wide (documentation process — applies to any module with an HTTP API;
  `latteAPI` is the first to adopt it, `latteMCP` follows since it also exposes HTTP endpoints)
- Context: Two things surfaced while working on `latteAPI`. First,
  `docs/_discovery/latteAPI-reimplementation-audit.md` (a since-removed one-off exercise: delete
  the module, rebuild it from `docs/` alone) found that a single hand-written `interfaces.md`
  left real gaps a generated spec would have closed automatically — e.g. the `GET /health`
  response shape and the error-response body shape were both under-specified enough that two
  independent reconstructions disagreed with each other and both technically satisfied the doc.
  Second, as the API surface grows, a flat `interfaces.md` per module stops scaling the same way
  a flat `requirements.md` would — CLAUDE.md already splits docs by module for this reason; the
  same pressure applies one level down, per operation, once a module has more than a handful of
  endpoints. Separately: for a *less*-documented or legacy endpoint (Track A territory), a real
  captured request/response pair is often faster to produce and more trustworthy than prose —
  it's evidence, not interpretation — and is exactly the kind of artifact the existing
  per-operation discovery loop (`discovery_plan.md`) is already structured around.
- Options considered:
  - **Keep a single hand-written `interfaces.md` per module, do nothing else.** Status quo.
    Cheapest, but repeats the exact gap the audit found, and doesn't scale past a handful of
    endpoints per module.
  - **Adopt a full API-management toolchain (Swashbuckle + Swagger UI, contract testing, etc.).**
    Disproportionate for a three-module demo project with no external consumers; adds
    infrastructure with no one asking for it.
  - **Framework-generated OpenAPI document (source of truth for schemas) + split `interfaces.md`
    into one file per operation + require captured sample request/responses per operation.**
    Chosen. Uses what ASP.NET Core already ships (`AddOpenApi()`/`MapOpenApi()`, no new
    dependency), directly closes the shape-ambiguity gaps the audit found, and scales the same
    way module-splitting already does — one small, independently reviewable/confirmable file per
    operation, which maps onto the existing Track A per-operation loop instead of fighting it.
- Decision:
  1. Every module that exposes an HTTP API enables framework-generated OpenAPI
     (`builder.Services.AddOpenApi()` + `app.MapOpenApi()` in Development, per ASP.NET Core's
     built-in support) so the schema is regenerated from the actual endpoint definitions on every
     build and cannot drift the way a hand-copied one can. `interfaces/README.md` links to it
     rather than duplicating field-by-field schemas, per CLAUDE.md's existing
     "don't duplicate an existing source of truth" principle — now mandatory for API modules
     instead of conditional on one already existing.
  2. Each such module's `interfaces.md` is replaced by an `interfaces/` folder:
     `README.md` (index table of operations + non-endpoint content: external services, config
     keys, link to the live OpenAPI document) plus one file per operation, named
     `<method>-<path-with-dashes>.md` (e.g. `get-orders-id.md` for `GET /orders/{id}`,
     `mcp-tool-<name>.md` for an MCP tool). A module with no operations of its own (a pure
     caller, e.g. `latteMCPclient`) keeps a single flat `interfaces.md` — the split only pays for
     itself once there's something to split.
  3. Every operation file carries a "Sample Requests & Responses" section with real captured
     request/response pairs (headers/status/body), not illustrative pseudo-examples. Required to
     add or refresh whenever that operation is touched (Track B) or newly drafted (Track A);
     especially valuable while an entry is still `Draft` or low-confidence, since a real sample
     is evidence a reader can check against, ahead of (or instead of) fully-written prose.
  4. Conventions that would otherwise repeat on every operation file (JSON casing, the error
     response envelope, the auth header format) move to a new cross-cutting `docs/api-conventions.md`,
     referenced from each module's `interfaces/README.md` instead of restated per module.
- Consequences: More, smaller files per API module instead of one growing file — each
  independently reviewable and `Confirmed`-able, consistent with how `discovery_plan.md` already
  tracks progress per operation. Schemas can no longer silently drift from code, since they're
  generated, not hand-typed — but the generated document only covers structural shape, not
  business meaning/rules, so the per-operation files and `requirements.md` remain necessary, not
  redundant. Slightly more files to keep in sync when an operation's contract changes (the
  per-operation `.md` and, if the response shape actually changed, its sample), but each change
  is localized to one file instead of one section of a large shared one. `latteMCPclient` is
  exempt from the folder split (rule 2 above) since it has no operations of its own to split.
