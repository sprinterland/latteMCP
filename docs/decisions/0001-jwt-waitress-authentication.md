# ADR-0001: Waitress identity via JWT, issued by latteAPI, accounts and signing key in settings

- Date: 2026-08-22
- Status: Accepted
- Modules affected: latteAPI, latteMCP, latteMCPclient (system-wide)
- Context: Every order needs to be traceable to the waitress who placed it, all the way from
  the MCP client through to the stored order. There is no database or secrets manager anywhere
  in this project — orders themselves live in an in-memory store — so whatever identity
  mechanism is chosen has to fit that same low-ceremony posture without blocking the
  demonstration of the end-to-end flow.
- Options considered:
  - **Real user database + hashed passwords + externally managed signing key.** Correct for
    production, but disproportionate: there is no persistence layer elsewhere in the system to
    justify adding one just for login, and no secret store currently in scope.
  - **No identity at all — orders are anonymous.** Simplest, but defeats the actual requirement
    (know which waitress placed which order).
  - **JWT issued by `latteAPI`, waitress accounts and signing key held in configuration.**
    Chosen. Gives real per-waitress identity and a standard, well-understood token mechanism
    (`AddJwtBearer`) without inventing new infrastructure.
- Decision: `latteAPI` owns identity. Waitress accounts (username, password, display name) are
  a hardcoded list in `appsettings.json`, bound via `IOptions<T>`; no self-registration. It
  exposes `POST /auth/login`, which checks credentials against that list and issues a JWT
  signed with a key also held in configuration, carrying the waitress's identity as a claim.
  Tokens expire after 4 hours — long enough to cover one shift and one continuous client
  session without needing a refresh flow, short enough to still be a bounded credential.
  Order-mutating/reading endpoints require this token (`[Authorize]`); `GET /menu`,
  `POST /auth/login`, and `GET /health` stay anonymous.
- Consequences: Login works end-to-end with zero new infrastructure, and every order can be
  attributed to a real waitress identity. The explicit trade-off: this is a dev-only posture.
  Passwords sit in plaintext in configuration and the signing key is not rotated or externally
  managed — this must not be carried into any non-local deployment as-is. If that need arises,
  it should be its own ADR (moving accounts to a real store with hashed passwords, and the
  signing key to a secret manager or environment-injected value) rather than a silent change to
  this one.
