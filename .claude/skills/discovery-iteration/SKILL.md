---
name: discovery-iteration
description: Run one bounded Track A discovery iteration — reconstruct docs for the next operation(s) in docs/discovery/discovery_plan.md's backlog. Use for "run a discovery pass", "Track A", "reconstruct docs for this module/operation", or "continue the discovery backlog". Never processes a whole repo or file in one pass.
---

# Discovery Iteration (Track A)

Runs one bounded unit of Track A reconstruction work, per `CLAUDE.md`'s "Two Concurrent Tracks"
and `docs/discovery/discovery_plan.md`. **Re-read that file's "Per-Operation Process" and
"Batching Guidance" sections live before starting** — they are the source of truth for sizing and
sequencing; this skill only exists to make picking up the loop faster, not to replace it.

## Procedure

1. **Check Phase 0 is done.** If `docs/discovery/discovery_plan.md`'s "Phase 0 — Repo Topology
   Scan" hasn't run yet, do that first (one-time, metadata only — no function bodies) instead of
   picking a backlog row.
2. **Pick the next unit of work** from the Operation Backlog, honoring Module Ordering (finish a
   module's rows before starting the next, per that file) and Batching Guidance: batch 3–6
   `Simple` operations from the same module together, or take exactly one `Complex` operation
   alone.
3. **Read narrowly.** For each operation, read only its entry point plus up to ~2 hops of direct
   calls (handler → service → repository) — one hop deeper only if a business rule still isn't
   resolved.
4. **Draft immediately**, per operation, into the module's own files — don't hold drafts across
   operations:
   - `requirements.md`, `domain-model.md`, `interfaces/<op>.md` (or the module's flat
     `interfaces.md`), `test-spec.md`.
   - Tag provenance/status on every entry added or extended: `Draft (inferred from code)`,
     `Draft (from legacy docs)`, or leave existing `Confirmed` entries untouched.
   - Capture a real sample request/response in the interface file if one can be produced (see
     `../../api-conventions.md` and `CLAUDE.md`'s "API documentation" section) — this matters most
     while an entry is still low-confidence.
5. **Flag, don't guess.** If code looks unintentional or a legacy doc contradicts the code, record
   it as an open question in `docs/discovery/coverage.md`'s Open Questions Log and in the relevant
   module doc — do not silently pick an interpretation.
6. **Update the backlog row(s)**: `Status → Drafted`, set `Confidence`, note the batch and any open
   question.
7. **Roll up.** Once every row for a module is `Drafted`, mark it ready for review per "Marking a
   Module Ready for Review" and update `docs/discovery/coverage.md`. Move to the next module only
   after that, unless `docs/discovery/debt_log.md` has raised its priority.
