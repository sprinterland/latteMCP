---
name: log-change-request
description: Append a framework-enhancement idea to the shared _frw clone's _data/change_requests.jsonl (CLAUDE.md Rule 17). Use whenever you notice a possible framework-level improvement during any activity — planning, coding, discovery, or review — even one you're about to fix as part of the current task. Logging is a proposal only; it never applies the change itself.
---

# Log a Framework Change Request

Appends one entry to the shared `_frw` clone's `_data/change_requests.jsonl` — the continuous,
append-only backlog described in `docs/framework-maintenance.md`'s "Framework activity logs"
section (Rule 17). **Re-read that section live before logging** — the schema has grown before
(e.g. `affected_entities` was added after the file already had entries) and this skill must not
freeze a stale field list.

## Procedure

1. **Locate the shared clone.** Read `docs/framework-maintenance.md` / this project's own record of
   where the shared `_frw` clone lives (do not hardcode a path here — it's a fact about this
   project's setup, not about the schema).
2. **Re-read the current schema** in `docs/framework-maintenance.md`'s "Framework activity logs"
   section — the exact field list, the current `activity` and `affected_entities` enums, and the
   current `severity` tiers.
3. **Generate the id.** `CR-<unix-epoch-milliseconds>-<4 random hex chars>` — self-generated, no
   coordination with any other writer needed (this is why `change_requests.jsonl` uses this scheme
   instead of a plain incrementing counter — see the doc section for why).
4. **Compose the entry** with today's fields: `id`, `timestamp` (ISO 8601, local offset), `project`
   (this project's name), `activity` (`planning` / `coding` / `discovery` / `review` / etc.),
   `description` (concrete — what's wrong or missing, and where), `severity` (from the current
   tiers — re-read step 2's schema, don't reuse a remembered list; it has grown before too, e.g.
   `moderate` was added after `minor`/`major`), `affected_entities` (array from the current enum,
   same reasoning), `status: "open"`, `resolution: null`, `resolved_at: null`.
5. **Append** the entry as one JSON line at the end of the file — never rewrite or delete existing
   lines. "Resolving" an existing entry later means appending a *new* line with the same `id` and
   updated `status`/`resolution`/`resolved_at`, not editing the original.
6. **Say so, and stop.** State what was logged and its id. This is a proposal only — per
   `docs/framework-maintenance.md`'s standing rule, no framework change is applied without asking
   the user first, whether or not it's logged here. Do not commit/push this on its own — it rides
   along with the next Maintenance-rule propagation commit or periodic sync.
