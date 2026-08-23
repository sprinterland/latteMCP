# <ModuleName> — Interfaces (index)

One file per operation in this folder. Shared conventions (JSON casing, error body shape, auth
header, health payload) live in `../../../api-conventions.md` — not repeated here. No secrets or
credential values in any file below — reference the config key name only; actual values live in
the project's secret store / config.

If this module exposes no operations of its own (a pure caller of other modules), delete this
`interfaces/` folder and use a single flat `interfaces.md` instead — see
`../../../../CLAUDE.md`'s "API documentation" section for the rule this follows.

**Generated schema:** `<URL pattern once implemented, e.g. GET /openapi/v1.json>` on a running
instance is the source of truth for exact request/response *shape*. The files below explain what
each operation means and add real sample traffic; they don't re-transcribe the generated schema.

## Operations

| Method | Path | Implements | Auth | File |
|---|---|---|---|---|
| GET | `/example` | <PREFIX>-REQ-001 | none | [`TEMPLATE-operation.md`](TEMPLATE-operation.md) |

## External Services

- <services this module calls, or "None">.

## Configuration Keys

- `<Key:Name>` — <what it configures>. Mark **Secret** where applicable — value lives outside
  `docs/`, referenced by key name only.
