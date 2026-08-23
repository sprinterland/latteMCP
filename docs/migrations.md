# Migrations & Rewrites — docs-first replacement of legacy code

A separate, deliberate initiative from Track B's day-to-day flow (see `CLAUDE.md`'s "Two
Concurrent Tracks") — a decision to fully replace a module's implementation, not just change it.
When the goal is to replace existing code with a new implementation (possibly on a different
stack) using the now-confirmed documentation as the spec:

- Keep the legacy system's `architecture/overview.md` and each module's `architecture.md` as the
  **As-Is** record — don't overwrite them, since the reasoning in `decisions/` often refers back
  to them.
- Describe the new system in a **To-Be** section (or a clearly separated new revision once the
  legacy system is fully retired) rather than editing As-Is in place.
- `requirements.md`, `domain-model.md`, `interfaces/` (the contract content, not the generated
  spec it links to), and `test-spec.md` for a `Confirmed` module should not need to change for a
  rewrite — if the rewrite forces a change to one of these, that's a sign the new system's
  behavior is diverging from the confirmed spec, and it's worth a deliberate decision (and likely
  an ADR) rather than an incidental edit.
- New implementation work goes through `plan.md` and normal Workflow Rules in `CLAUDE.md`, same
  as any other task.
