---
name: new-api-operation
description: Document a new or changed HTTP/MCP operation as a per-operation interfaces file, per ADR-0005 (generated OpenAPI + per-operation files + real captured samples). Use for "document this endpoint", "new API operation doc", "add an interfaces file", or "write the interface doc for this MCP tool".
---

# New API Operation Doc

Documents one operation per `CLAUDE.md`'s "API documentation" section and ADR-0005. **Re-read both
live** — the naming convention, required sections, and the Rule 10 redaction carve-out are the
substantive rule and can change; this skill only speeds up the mechanical scaffolding.

## Procedure

1. **Confirm the module's interfaces shape.** If this module has a flat `interfaces.md` (no
   operations of its own) rather than an `interfaces/` folder, this operation is likely evidence it
   now needs the folder split — check `CLAUDE.md`'s rule for when that split pays for itself.
2. **Name the file**: `<method>-<path-with-dashes>.md` for a REST operation (e.g.
   `get-orders-id.md` for `GET /orders/{id}`), or `mcp-tool-<name>.md` for a non-REST operation like
   an MCP tool.
3. **Copy the template**: `interfaces/TEMPLATE-operation.md` (in this module's own `interfaces/`
   folder) → the new filename. If this module's `interfaces/` has no `TEMPLATE-operation.md` of its
   own (common — it only survives if this happens to be the module created straight from
   `_module-template`), get it from the shared `_frw` clone instead:
   `<_frw clone>/copy_me/docs/modules/_module-template/interfaces/TEMPLATE-operation.md` (find the
   clone path in this project's own `docs/framework-maintenance.md`).
4. **Fill in**: `Implements: <PREFIX>-REQ-XXX`, `Auth`, `Status` (`Draft` until implemented and
   verified), Request, Response, Errors — relying on `docs/api-conventions.md` for anything shared
   (JSON casing, error body shape, auth header, health payload) rather than repeating it here.
5. **Capture a real "Sample Requests & Responses" section** — actual captured traffic (status,
   headers, body), not an illustrative pseudo-example; this matters most while the entry is still
   `Draft`/low-confidence.
6. **Apply the Rule 10 exception if the sample would contain a secret** (e.g. a login request body
   with a real working password): redact just that field, name the real config key it comes from
   (not the literal words "config key"), note inline that it was redacted, and keep everything else
   in the sample real. Do not fall back to a fully illustrative example just because one field
   needs redacting.
7. **Register the operation**: add a row to the module's `interfaces/README.md` Operations table
   (Method, Path, Implements, Auth, File) and, if it calls a new external service or reads a new
   config key, add those to that same README's External Services / Configuration Keys sections.
