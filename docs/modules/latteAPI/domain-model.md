# latteAPI — Domain Model

Technology-agnostic. Concepts this module deals with, independent of how they are currently
stored or transmitted. Shared cross-module terms belong in `../../glossary.md` instead.

## Entities

### Entity: MenuItem

- Fields: `Id` — integer, stable identifier — `Name` — string — `Description` — string —
  `BasePrice` — decimal, price for the smallest size before surcharge.
- Relationships: Referenced by `OrderLine.MenuItemId`.
- Lifecycle: Static catalog for this phase — no create/update/delete operations exist; the
  catalog is fixed data, not something a waitress or customer changes.
- Source: inferred from code (`src/latteAPI/Models/MenuItem.cs`,
  `src/latteAPI/Data/MenuCatalog.cs`)

### Entity: Waitress

- Fields: `Username` — string, unique, used to log in — `Password` — string, credential checked
  at login (not exposed after login) — `DisplayName` — string, optional, human-readable
  identity used e.g. as `Order.CreatedBy`.
- Relationships: A waitress creates zero or more `Order`s.
- Lifecycle: Fixed set configured at deploy time (see `API-RULE-004` in `requirements.md` and
  ADR-0001) — no self-registration or lifecycle transitions in this phase.
- Source: Confirmed by implementation — `src/latteAPI/Models/WaitressAccount.cs`, bound from the
  `Waitresses` config section (`src/latteAPI/appsettings.Development.json`), verified 2026-08-22.

### Entity: Order

- Fields: `Id` — GUID — `Items` — list of `OrderLine` — `Status` — `OrderStatus` enum, see below
  — `CreatedAt` — timestamp — `CreatedBy` — the waitress identity from `API-RULE-002`, taken from
  the authenticated token's `displayName` claim (falling back to `username`) — `Total` —
  decimal, computed per `API-RULE-001`.
- Relationships: Composed of one or more `OrderLine`; associated with exactly one `Waitress`
  (the one who placed it).
- Lifecycle: Created once via `API-REQ-003`. Per `API-RULE-003`, status starts and stays at
  `Received` in this phase — no transitions are exposed.
- Source: inferred from code (`src/latteAPI/Models/Order.cs`); `CreatedBy` confirmed by
  implementation 2026-08-22.

### Entity: OrderLine

- Fields: `MenuItemId` — references `MenuItem.Id` — `Size` — `DrinkSize` enum — `Quantity` —
  integer.
- Relationships: Belongs to one `Order`; references one `MenuItem`.
- Lifecycle: Immutable once the containing `Order` is created.
- Source: inferred from code (`src/latteAPI/Models/Order.cs`)

## Enumerations

### DrinkSize

- Values: `Small`, `Medium`, `Large`.
- Meaning: Determines the surcharge added to a `MenuItem.BasePrice` (see
  `MenuCatalog.SizeSurcharge`).
- Source: inferred from code (`src/latteAPI/Models/Order.cs`)

### OrderStatus

- Values: `Received`, `Preparing`, `Ready`, `Completed`.
- Meaning: Intended lifecycle of an order's fulfillment. Per `API-RULE-003`, only `Received` is
  reachable in this phase — the other three values exist on the model for a future increment but
  have no code path that sets them yet.
- Source: inferred from code (`src/latteAPI/Models/Order.cs`)

## Seed / Example Data

Ordinary business data (not secrets — see the separate dummy-credentials note below) that a
from-docs reconstruction of this module needs and that no other section captures. Values below
match the current implementation exactly; if the catalog changes, update both together.

### Menu catalog (`MenuCatalog.Items`)

| Id | Name | Description | BasePrice |
|---|---|---|---|
| 1 | Latte | Espresso with steamed milk | 4.25 |
| 2 | Cappuccino | Espresso with steamed milk foam | 4.25 |
| 3 | Americano | Espresso with hot water | 3.25 |
| 4 | Mocha | Espresso with chocolate and steamed milk | 4.75 |
| 5 | Espresso | A double shot, no milk | 2.75 |

### Size surcharge (`MenuCatalog.SizeSurcharge`)

| DrinkSize | Surcharge |
|---|---|
| Small | 0.00 |
| Medium | 0.60 |
| Large | 1.20 |

- Source: Confirmed by implementation — `src/latteAPI/Data/MenuCatalog.cs`, verified 2026-08-22.

### Waitress accounts — illustrative shape only, NOT real credentials

Per `CLAUDE.md` rule 10, actual account values must never appear in `docs/` — only the config key
name (`Waitresses`, see `interfaces/README.md`). The row below is a dummy example showing the *shape* a
seeded account takes; it does not match any account in `appsettings.Development.json`, and using
it against a running instance will not authenticate.

| Username | Password | DisplayName |
|---|---|---|
| `jane.doe` | `example-only-not-a-real-password` | Jane Doe |

- Source: Illustrative example, added 2026-08-22 — not derived from actual configured accounts.
