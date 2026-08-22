# latteAPI — Requirements

Technology-agnostic. Should read the same regardless of implementation language/framework.
IDs are prefixed with the module name and are never reused or renumbered — mark removed items
`Deprecated` instead of deleting them.

## Functional Requirements

### API-REQ-001: Browse the menu

- Description: Any caller (no login required) can retrieve the list of drinks the shop sells,
  each with its name, description, and base price, plus the price surcharge for each available
  size.
- Rationale: A waitress needs to see what's on offer before taking an order, and this is public
  information with no reason to gate it behind login.
- Source: Draft (inferred from existing code — `src/latteAPI/Data/MenuCatalog.cs`)
- Status: Confirmed — implemented as `GET /menu` in `src/latteAPI/Program.cs`, verified 2026-08-22.

### API-REQ-002: Waitress login

- Description: A waitress can authenticate with a username and password and receive a token that
  identifies her for subsequent requests.
- Rationale: Every order must be traceable to the waitress who took it (see API-RULE-002). Login
  is how that identity enters the system.
- Source: Confirmed by user in conversation on 2026-08-22
- Status: Confirmed — implemented as `POST /auth/login` in `src/latteAPI/Program.cs`, verified 2026-08-22.

### API-REQ-003: Place an order

- Description: An authenticated waitress can submit an order consisting of one or more lines
  (menu item, size, quantity). The system validates the menu items referenced, computes the
  total price, records who placed it, and returns the created order.
- Rationale: Core purpose of the shop's ordering system.
- Source: Draft (inferred from existing code — `src/latteAPI/Models/Order.cs`,
  `src/latteAPI/Data/OrderStore.cs`)
- Status: Confirmed — implemented as `POST /orders` in `src/latteAPI/Program.cs`, verified 2026-08-22.

### API-REQ-004: Look up a single order

- Description: An authenticated waitress can retrieve one previously placed order by its id.
- Rationale: Needed to check status/details of an order already taken.
- Source: Confirmed by implementation — `GET /orders/{id}` in `src/latteAPI/Program.cs`.
- Status: Confirmed — verified 2026-08-22.

### API-REQ-005: List all orders

- Description: An authenticated waitress can retrieve the full list of orders placed so far.
- Rationale: Needed for a shift overview / demo scenario (see `latteMCPclient`
  `CLIENT-REQ-004`).
- Source: Confirmed by implementation — `GET /orders` in `src/latteAPI/Program.cs`.
- Status: Confirmed — verified 2026-08-22.

### API-REQ-006: Report service health

- Description: Any caller can check whether the API process is up and able to serve requests.
- Rationale: Standard operability requirement; also consumed by `latteMCP`'s own health check
  (`MCP-REQ-005`) to determine whether its upstream is reachable.
- Source: Confirmed by user in conversation on 2026-08-22
- Status: Confirmed — implemented as `GET /health` in `src/latteAPI/Program.cs`, verified 2026-08-22.

## Business Rules

### API-RULE-001: Order total is computed by the server

- Rule: An order's total price is always computed server-side from the current menu price and
  size surcharge at the time the order is placed; it is never accepted as client input.
- Applies to: `API-REQ-003` (place an order).
- Source: Draft (inferred from existing code — `src/latteAPI/Data/MenuCatalog.cs`
  `SizeSurcharge`)
- Status: Confirmed — enforced server-side in the `POST /orders` handler, verified 2026-08-22.

### API-RULE-002: Every order records the waitress who placed it

- Rule: Every order stores the identity of the authenticated waitress who submitted it
  (`CreatedBy`). This is the entire reason login exists in this system.
- Applies to: `API-REQ-003`.
- Source: Confirmed by user in conversation on 2026-08-22
- Status: Confirmed — `Order.CreatedBy` set from the authenticated waitress's claims, verified 2026-08-22.

### API-RULE-003: Order status has no transition mechanism (deferred)

- Rule: An order's status is fixed at creation (`Received`) — there is no supported way, in this
  phase, to advance it to `Preparing` / `Ready` / `Completed` via the API, even though the model
  has room for those states.
- Applies to: `API-REQ-003`, `API-REQ-004`, `API-REQ-005`.
- Rationale: Scope was deliberately narrowed to "browse → order → look up," the minimum slice
  needed to prove the waitress-identity flow end-to-end across all three apps, before adding
  status-tracking as a further increment.
- Source: Confirmed by user in conversation on 2026-08-22
- Status: Confirmed — verified 2026-08-22 (no status-transition endpoint exists).

### API-RULE-004: Menu browsing and health checks don't require login

- Rule: `API-REQ-001` (menu) and `API-REQ-006` (health) are reachable without a token. Every
  other functional requirement requires a valid, unexpired token from `API-REQ-002`.
- Applies to: all functional requirements above.
- Source: Confirmed by user in conversation on 2026-08-22
- Status: Confirmed — `RequireAuthorization()` applied to `/orders*`, anonymous access confirmed
  for `/menu`, `/health`, `/auth/login`, verified 2026-08-22.

## Non-Functional Requirements

### API-NFR-001: Order data durability

- Requirement: Orders only need to survive for the lifetime of the running process — no
  durable/persistent storage is required in this phase.
- Measured by: N/A (explicit non-requirement, not a target to verify).
- Source: Draft (inferred from existing code — `OrderStore` uses an in-memory
  `ConcurrentDictionary`)
- Status: Confirmed — unchanged in this phase, verified 2026-08-22.

### API-NFR-002: Token lifetime

- Requirement: Issued tokens remain valid for 4 hours from issuance, long enough to cover one
  waitress shift and one continuous client session without requiring a refresh flow.
- Measured by: Token expiry claim inspection / an integration test asserting a token issued at
  T is still accepted at T+3h59m and rejected at T+4h01m.
- Source: Confirmed by user in conversation on 2026-08-22
- Status: Confirmed — `Jwt:ExpiryHours` = 4, verified via `exp` claim inspection 2026-08-22;
  no automated test yet (see API-TEST-011, still `Draft`).
