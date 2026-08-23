# System Architecture — Overview

## As-Is (current system)

<One-paragraph summary of the system shape: how many components/services, whether they share
infrastructure, the overall topology.>

- System Components:
  - `<module-name>` — <one-line purpose> — module [`modules/<module-name>/`](../modules/_module-template/)
    — <how/where it runs>.

- Service boundaries and communication:
  - <which module calls which, over what protocol, sync/async>.

- Deployment / infrastructure topology: <processes, shared databases, message brokers,
  container/orchestration layer, or explicitly "none yet">.

- Cross-module integration points: see each module's interfaces for the actual endpoint/tool
  contracts. Shared API conventions live in [`../api-conventions.md`](../api-conventions.md).

## To-Be (target, only during an active migration/rewrite)

Not applicable until a migration/rewrite is deliberately underway — see "Migrations & Rewrites"
in `../../CLAUDE.md`.
