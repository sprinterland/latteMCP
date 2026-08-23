# API Conventions

Cross-cutting conventions for every module that exposes an HTTP API. Per-module
`interfaces/README.md` files link here instead of restating these; a module's own
`interfaces/<operation>.md` files should only describe what's specific to that operation. Delete
this file if no module in the project exposes an HTTP API.

## Generated OpenAPI document

Every module with an HTTP API should enable framework-generated OpenAPI (e.g. ASP.NET Core's
`AddOpenApi()`/`MapOpenApi()`, or the equivalent for the stack in use) rather than
hand-maintaining a schema — it's regenerated from the actual endpoint definitions on every
build/run, so it's the structural source of truth for request/response *shape* and cannot drift
the way a hand-written doc can. It does **not** replace the per-operation `.md` files: the
generated doc only knows shape (property names/types), not business meaning, auth requirements in
plain language, or which requirement/rule an endpoint implements — that context stays in the
`.md` files, which should link to the live document rather than re-transcribing its fields.

- Dev URL pattern: `<fill in once a module implements this>`.

## JSON conventions

<Property naming (camelCase/snake_case), enum serialization, date/time format — fill in once the
stack is chosen and document any deviation from the framework's defaults.>

## Auth header

<e.g. `Authorization: Bearer <token>` — fill in once auth is designed.>

## Error response bodies

Status codes are documented per-operation; the body shape is standardized here so a client only
needs to learn it once:

| Status | Body | Notes |
|---|---|---|
| `400 Bad Request` | `<shape>` | |
| `401 Unauthorized` | `<shape>` | |
| `404 Not Found` | `<shape>` | |

## Health check payload

`GET /health` (or equivalent) returns `200 OK` with `<shape>` when healthy. A module whose health
check also verifies an upstream dependency documents its specific non-2xx failure behavior in its
own `interfaces/get-health.md` — this file only fixes the shared success-shape convention.
