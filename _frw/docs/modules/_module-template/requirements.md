# <ModuleName> — Requirements

Technology-agnostic. Should read the same regardless of implementation language/framework.
IDs are prefixed with the module name and are never reused or renumbered — mark removed items
`Deprecated` instead of deleting them.

## Functional Requirements

### <PREFIX>-REQ-001: <short title>

- Description: <what the system must do, from the user/caller's point of view>.
- Rationale: <why this exists>.
- Source: Draft (proposed by Claude, pending confirmation) | Draft (inferred from code — `path`)
  | Draft (from legacy docs) | Confirmed by user in conversation on <date>
- Status: Draft | Confirmed — <implementation reference once built, e.g. "implemented as
  `POST /things`, verified <date>">.

## Business Rules

### <PREFIX>-RULE-001: <short title>

- Rule: <the constraint, stated precisely>.
- Applies to: `<PREFIX>-REQ-XXX`.
- Source: <as above>
- Status: <as above>

## Non-Functional Requirements

### <PREFIX>-NFR-001: <short title>

- Requirement: <the target, stated measurably where possible>.
- Measured by: <how this would be verified — a test, an inspection, or "N/A" for an explicit
  non-requirement>.
- Source: <as above>
- Status: <as above>
