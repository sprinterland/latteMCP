# Documentation Discovery — Coverage Tracker (module-level rollup)

Tracks Track A (Discovery) progress while reconstructing documentation for an existing codebase
(see "Two Concurrent Tracks" in `../../CLAUDE.md`). This is the rollup view — one row per
module. The actual iteration work happens at the operation level in `discovery_plan.md`; update
this table by summarizing that file (a module reaches `Draft` once every operation belonging to
it is `Drafted` there). Once a module reaches `Confirmed` and stabilizes, it can be dropped from
this table (its status still lives in `docs/00-index.md`). See `debt_log.md` for gaps
surfaced by Track B (feature work) rather than by a dedicated discovery pass.

| Module | Source paths scanned | Status | Confidence | Reviewed by | Last reviewed | Open questions |
|---|---|---|---|---|---|---|
| — | — | — | — | — | — | — |

Status: `Not started` / `Draft (inferred from code)` / `Draft (from legacy docs)` / `Confirmed`

Confidence: `Low` / `Medium` / `High` — Claude's own estimate of how much of the draft is solid
inference vs. guesswork. Low-confidence entries should have specific open questions listed, not
just a general caveat.

## Open Questions Log

Anything found during reconstruction that couldn't be resolved from code or legacy docs alone —
ambiguous behavior, possible bugs, contradictions between code and legacy docs. Move each item
into the relevant module's `requirements.md` (as a flagged item) once resolved, and remove it
from here.

- <Module> — <question> — <why it matters / what was found>
