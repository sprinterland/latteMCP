# latteMCP — Domain Model

Technology-agnostic. Concepts this module deals with, independent of how they are currently
stored or transmitted. Shared cross-module terms belong in `../../glossary.md` instead.

This module owns no business data of its own — no menu, order, or waitress record is ever
stored here. It deals only in the request/response shapes it passes through to and from
`latteAPI` (see `../latteAPI/domain-model.md` for the entities those shapes represent) and one
concept specific to how it exposes them:

## Entities

### Entity: MCP Tool

- Fields: `Name` — string, e.g. `get_menu`, `place_order` — `Description` — string, shown to the
  connecting agent — `InputSchema` — the parameters the tool accepts.
- Relationships: Each tool corresponds to exactly one `latteAPI` endpoint (see
  `interfaces/README.md`'s Operations table).
- Lifecycle: Registered at startup via `[McpServerToolType]` on `src/latteMCP/Tools/OrderingTools.cs`;
  stateless per call — see `MCP-RULE-001`.
- Source: Standard MCP SDK concept (`ModelContextProtocol.AspNetCore`), not something specific to
  this project's business domain.
- Status: Confirmed — implemented and verified 2026-08-22.
