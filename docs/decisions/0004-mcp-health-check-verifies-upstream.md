# ADR-0004: latteMCP's health check verifies latteAPI reachability, not just itself

- Date: 2026-08-22
- Status: Accepted
- Modules affected: latteMCP
- Context: Both `latteAPI` and `latteMCP` need a `GET /health` endpoint. `latteAPI` has no
  external dependencies of its own (ADR-0001, `API-NFR-001`), so a plain liveness check ("the
  process is up") is sufficient for it. `latteMCP` is different: it does nothing on its own —
  every tool call and the `/login` wrapper depend entirely on `latteAPI` being reachable.
- Options considered:
  - **Plain liveness check** (mirroring `latteAPI`'s), reporting healthy whenever the `latteMCP`
    process itself is running, regardless of whether it can actually reach `latteAPI`.
    Simplest, but misleading: it would report "healthy" even while every real tool call is
    failing, which defeats the purpose of a health check for anything monitoring this service.
  - **Dependency health check.** Chosen. `latteMCP`'s `GET /health` also calls `latteAPI`'s
    `GET /health` and reports unhealthy if that fails.
- Decision: `latteMCP`'s `GET /health` succeeds only if both `latteMCP` itself is up and its
  call to `latteAPI`'s `GET /health` succeeds. `latteAPI`'s own `GET /health` remains a plain
  liveness check, since it has no dependencies to reflect.
- Consequences: `latteMCP`'s health status now genuinely reflects whether it can do useful work,
  which is the right signal for any monitoring/orchestration that uses it. Trade-off: `latteMCP`
  becomes momentarily unhealthy whenever `latteAPI` is slow or restarting, even though the
  `latteMCP` process itself is fine — this is intentional (see Context) but worth remembering
  if it's ever wired into aggressive auto-restart logic.
