# <ModuleName> — Domain Model

Technology-agnostic. Concepts this module deals with, independent of how they are currently
stored or transmitted. Shared cross-module terms belong in `../../glossary.md` instead.

## Entities

### Entity: <EntityName>

- Fields: `<Field>` — <type, meaning> — `<Field>` — <type, meaning>.
- Relationships: <references to/from other entities>.
- Lifecycle: <how it's created/changed/removed, or "static, no create/update/delete">.
- Source: <inferred from code (`path`) | Confirmed by implementation | Confirmed by user in
  conversation on <date>>

## Enumerations

### <EnumName>

- Values: `<Value1>`, `<Value2>`.
- Meaning: <what the values represent and how they're used>.
- Source: <as above>

## Seed / Example Data

For any entity documented as fixed/seed data (a static catalog, a lookup table, reference
values) — not just its schema — include the actual current values here, kept in sync with the
code that defines them. Delete this section if the module has no such fixed data.

### <Catalog name> (`<source>`)

| <Field> | <Field> |
|---|---|
| <value> | <value> |

- Source: Confirmed by implementation — `<path>`, verified <date>.

Never include real secret values (passwords, keys, tokens) here — see `CLAUDE.md` rule 10. Where
an entity mixes a non-secret identifier with a secret credential, include one clearly-labeled
dummy row illustrating the *shape* only:

| <Field> | <SecretField> |
|---|---|
| `example` | `example-only-not-a-real-value` |

- Source: Illustrative example — not derived from actual configured values.
