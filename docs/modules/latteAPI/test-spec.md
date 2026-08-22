# latteAPI — Test Specification

Each entry maps to exactly one requirement or rule and should translate directly into one or
more automated tests. New tests are written FROM this document, not inferred from existing code.

Status note (2026-08-22): all scenarios below were exercised manually against the Phase 1
implementation (`curl` against a running `dotnet run` instance) and passed. No automated tests
exist yet — entries stay `Draft` per CLAUDE.md rule 8 until real tests are written from this
spec. Tracked as a follow-up in `PLAN.md`.

## API-TEST-001 (implements API-REQ-001)

- Given: the API is running.
- When: a client calls `GET /menu` with no credentials.
- Then: `200 OK` is returned with the full menu catalog and size surcharges.
- Status: Draft

## API-TEST-002 (implements API-REQ-002, happy path)

- Given: a waitress account exists in configuration.
- When: a client calls `POST /auth/login` with the correct username/password.
- Then: `200 OK` is returned with a signed JWT whose claims identify that waitress.
- Status: Draft

## API-TEST-003 (implements API-REQ-002, failure case)

- Given: a waitress account exists in configuration.
- When: a client calls `POST /auth/login` with a wrong password (or an unknown username).
- Then: `401 Unauthorized` is returned, with no indication of whether the username itself
  existed.
- Status: Draft

## API-TEST-004 (implements API-REQ-003, API-RULE-001, API-RULE-002, happy path)

- Given: a valid waitress token, and a menu with known prices/surcharges.
- When: the client calls `POST /orders` with one or more valid order lines.
- Then: `201 Created` is returned; the order's `Total` equals the sum of
  (`BasePrice` + size surcharge) × quantity per line; `CreatedBy` matches the waitress from the
  token.
- Status: Draft

## API-TEST-005 (implements API-REQ-003, failure case — bad menu item)

- Given: a valid waitress token.
- When: the client calls `POST /orders` referencing a `menuItemId` that doesn't exist.
- Then: `400 Bad Request` is returned and no order is stored.
- Status: Draft

## API-TEST-006 (implements API-RULE-004, order endpoints require auth)

- Given: no token (or an expired/invalid one).
- When: the client calls `POST /orders`, `GET /orders`, or `GET /orders/{id}`.
- Then: `401 Unauthorized` is returned for each.
- Status: Draft

## API-TEST-007 (implements API-REQ-004)

- Given: an order was previously created with a known id.
- When: the client calls `GET /orders/{id}` with a valid token.
- Then: `200 OK` is returned with that exact order.
- Status: Draft

## API-TEST-008 (implements API-REQ-004, not-found case)

- Given: a valid token.
- When: the client calls `GET /orders/{id}` with an id that doesn't exist.
- Then: `404 Not Found` is returned.
- Status: Draft

## API-TEST-009 (implements API-REQ-005)

- Given: multiple orders have been created.
- When: the client calls `GET /orders` with a valid token.
- Then: `200 OK` is returned with all of them, most recent first.
- Status: Draft

## API-TEST-010 (implements API-RULE-003, no status transition)

- Given: an order was just created.
- When: its status is inspected via `GET /orders/{id}`.
- Then: `Status` is `Received`, and no endpoint exists in this phase to change it (there is
  nothing to call — this test documents the absence of the capability, not an error response).
- Status: Draft

## API-TEST-011 (implements API-NFR-002, token expiry)

- Given: a token issued at time T with a 4-hour expiry.
- When: the client calls an authenticated endpoint at T + 3h59m, then again at T + 4h01m.
- Then: the first call succeeds; the second returns `401 Unauthorized`.
- Status: Draft

## API-TEST-012 (implements API-REQ-006)

- Given: the API process is running.
- When: a client calls `GET /health`.
- Then: `200 OK` is returned with no auth required.
- Status: Draft
