# Framework Maintenance — `_frw` and the project/framework split

Full definition of how `_frw` relates to this project's own `docs/`, and the standing process
for evolving the documentation philosophy itself. `CLAUDE.md`'s "Reusable Framework Template"
section is the short pointer to this file for everyday reading; this is the actual content.

## What `_frw` is

`_frw` is this project's name for a **genericized, self-contained copy** of this documentation
philosophy — a bootstrap kit for starting a *different* project with the same approach. It lives
**outside this repo**, in its own GitHub repo — [`sprinterland/claude-project-framework`](https://github.com/sprinterland/claude-project-framework)
(public) — rather than being vendored into any one project, and not nested under a literal
`_frw/` subfolder anywhere: that repo's own root **is** the bundle. The canonical copy is that
GitHub repo; the local clone at `/Users/sprn/claudework/newFrw/` (a sibling of this and every
other project directory under `~/claudework/`) is what a project on this machine actually reads
from and bumps `VERSION` in before pushing back to GitHub. Multiple projects bootstrap from, and
propagate framework-level changes into, this same shared repo; it is not copied into a project
and then deleted the way an earlier, single-project version of this setup worked.

It contains:

```
README.md              — how to use this bundle to bootstrap a new project
VERSION                 — this bundle's version (see "Versioning" below)
.claude/skills/         — (bundle root — distinct from copy_me/.claude/skills/ below)
                          framework-development skills (bootstrap-project,
                          propagate-framework-change); run directly in the shared repo, never
                          copied into a bootstrapped project
_data/                  — `_frw`'s own framework-update notes; never copied into a bootstrapped
                          project
_design/                — `_frw`'s own design rationale — why the bundle is shaped this way, enough
                          to recreate it from scratch; like `_data/`, never copied into a project
copy_me/                — the entire copy boundary (see FRW-ADR-0010): everything a bootstrapped
                          project receives lives here, and only here
  CLAUDE.md.template      — a tech/project-agnostic copy of a project's `CLAUDE.md`
  PLAN.md.template        — minimal starter plan.md
  docs/                   — mirrors a project's docs/ tree, but every file is a template: process
                          and structure only, no project-specific facts. `modules/` holds one
                          `_module-template/` folder instead of real modules.
  .claude/skills/         — project-usage skills (push-review-gate, log-change-request,
                          discovery-iteration, new-module, new-adr, new-api-operation,
                          sync-framework-updates); copied verbatim into the bootstrapped
                          project's own `.claude/skills/` — this project's own copy lives at
                          `latteMCP/.claude/skills/`
```

## Versioning

`_frw`'s `VERSION` file holds a single line: the bundle's version, stamped `YY.MM.DD:HH.MM.FFF` —
in field order, 2-digit year, month, day, then a 24h hour, minute, and milliseconds (the second
`MM` is minute, not a repeat of the month field three groups earlier) — the moment the bundle was
last changed, not a semantic version; there's no meaningful "major/minor" axis for a doc-template
bundle, only "as of when." It exists so anything that references "the framework version" — most
concretely, each `docs/project/review_log.md` entry's Framework Reviewer sub-entry (see
`CLAUDE.md` Workflow Rule 16) — can cite a precise, unambiguous point in `_frw`'s history rather
than a vague "recent."

Bump it (regenerate the timestamp, overwrite the file) as step 3 of the Maintenance rule's
"Framework propagation" flow below, every time `_frw` actually changes — never on an ordinary
`docs/`/`CLAUDE.md` change that doesn't propagate — and commit the bump locally as part of that
step, run entirely inside `_frw`'s own repo; step 4 there reviews and pushes it. Because `_frw` is
a shared external repo rather than a per-project copy, this project's review log normally reads
its `VERSION` file live from the local clone (kept up to date with `git pull`) at review time.
The static "bootstrapped from / last synced at" fact below is updated separately, only when this
project runs its own inbound sync (see the Maintenance rule's "Inbound sync" flow below) — not
automatically every time `_frw`'s `VERSION` moves — so that citation stays a true record of what
this project has actually pulled in, and still has something permanent to cite if the shared
clone/repo is ever unreachable (different machine, no network, repo moved) at review time:

**Bootstrapped from / last synced at [`claude-project-framework`](https://github.com/sprinterland/claude-project-framework)
commit `2e1ca85`, version `26.08.24:20.29.178`** (2026-08-24 — clarified FRW-ADR-0011's
session-boundary wording for the multi-working-directory case and added an immediate-after-push
Inbound-sync trigger, requested directly by the user in latteMCP; see FRW-ADR-0012).
This line records only the current sync as a static fact, same as the rest of this file's
principle of pointing rather than restating — the full history of every prior sync already lives
in `docs/project/CHANGELOG.md` and `_frw/_data/update_history.jsonl`, not here.

## What's project-specific vs. framework, precisely

- `docs/` (no underscore, at repo root) is this project's actual documentation — real
  requirements, real ADRs, real module docs for this project's own modules. It is never
  genericized and never copied elsewhere as-is.
- `_frw` is the framework: structure, process explanations, and placeholders only. It carries no
  fact about this project's business domain, stack choices, or decisions. Nothing in `_frw`
  should ever need `docs/`'s content to make sense, and nothing in `docs/` should ever link into
  `_frw`.

## Framework activity logs (`_data/`)

`_frw/_data/` holds three append-only JSON-Lines logs of framework-maintenance activity — kept
only in the shared `_frw` repo, never copied into a bootstrapped project (see "What's
project-specific vs. framework, precisely" above). Unlike `discovery/` and `project/`'s rename
earlier this file's history (dropping their leading underscore because they're ordinary
bootstrapped content), `_data/` deliberately keeps its underscore — here it marks "excluded from
what gets copied into a project," not "hidden," which is a different, still-live reason to stand
out from `docs/`. Each log file is one JSON object per line; new entries are always appended,
never rewritten or deleted in place. "Marking an entry resolved" (e.g. a `change_requests.jsonl`
item, once decided or fixed) means **appending a new line with the same `id`**, the updated
`status`/`resolution`/`resolved_at`, and the rest of the fields unchanged — not editing the
original line. A reader reconstructing current state takes, per `id`, the line with the latest
`timestamp`/`resolved_at`; every earlier line for that `id` stays in the file as the historical
record of how the entry got there.

- **`update_history.jsonl`** — one record per completed framework update ("Framework propagation"
  flow step 5 below): `id`, `timestamp`, `project` (which project's need triggered the update),
  `summary`, `questions_asked` (the Rule 2 confirmations made while deciding it — `[]` if none
  needed), `decisions_made` (Rule 4 judgment calls made along the way, with the reasoning — `[]`
  if none), `final_summary`, `frw_commit`, `frw_version`, `changelog_ref` (the
  `docs/project/CHANGELOG.md` entry this summarizes, e.g. `"2026-08-23 — Add _data/ activity
  logs..."` — a pointer, so the two don't need to say the same thing twice), `status`
  (`completed`).
- **`push_reviews.jsonl`** — one record per review run immediately before pushing `_frw`'s own
  propagation commit ("Framework propagation" flow step 4 below): `id`, `timestamp`, `project`,
  `frw_commit`, `command`, `scope`, `outcome`, `review_log_ref` (the `docs/project/review_log.md`
  entry heading this corresponds to — a pointer, not a restatement, for the same reason as
  `changelog_ref` above), `enhancement_suggestions` (this entry's own `change_requests.jsonl` ids,
  or `[]`), `status` (`completed`).
- **`change_requests.jsonl`** — a continuous backlog of framework-enhancement ideas, logged the
  moment they're noticed during *any* activity in *any* project — planning, coding, discovery,
  review, even the task that's about to fix the very thing being logged (see `CLAUDE.md` Workflow
  Rule 17): `id`, `timestamp`, `project`, `activity` (`planning`/`coding`/`discovery`/`review`/
  `sync`/etc. — `sync` is a merge ambiguity the `sync-framework-updates` skill hit and logged
  rather than guessed at, see the Maintenance rule's "Inbound sync" flow below), `description`,
  `severity` (`minor`/`moderate`/`major`), `affected_entities` (array of one
  or more of `requirements`/`domain-model`/`architecture`/`architecture-overview`/`interfaces`/
  `test-spec`/`decisions`/`glossary`/`api-conventions`/`dev-practices`/`process` — which framework
  doc-entity type(s) the change request concerns; `architecture` is a module's own
  `architecture.md`, `architecture-overview` is the top-level `docs/architecture/overview.md`, and
  `process` is the open-ended catch-all for a framework/meta artifact not covered by any more
  specific value above — `CLAUDE.md` itself, `PLAN.md` (root or a module's own
  `docs/modules/<module>/plan.md` per Workflow Rule 12), `docs/00-index.md`, `docs/discovery/*`,
  `docs/project/*` (`CHANGELOG.md`, `review_log.md`, `completed_plan.md`), the `_data/*.jsonl`
  schemas described in this section, or a future cross-cutting top-level doc not yet given its own
  value — use it for anything the more specific values above don't fit, not only the examples
  named here), `status` (`open`/`resolved`), `resolution`, `resolved_at`. An entry with no
  `affected_entities` field predates this schema addition — identify by field absence, not by
  timestamp, since existing entries and this change share the same calendar date.

`change_requests.jsonl` is written far more often than the other two, and — per `CLAUDE.md` Rule
17 — from *any* project at *any* time, not only during a deliberate, one-at-a-time maintenance
step the way `update_history.jsonl`/`push_reviews.jsonl` are; two sessions in two different
projects could plausibly append within the same minute. So its IDs are **not** a read-last-line,
increment-by-one counter (which two concurrent writers could race and collide on) — each entry
self-generates its own `id` as `CR-<unix-epoch-milliseconds>-<4 random hex chars>` (e.g.
`CR-1755972123456-a1b2`), needing no coordination with any other writer. `update_history.jsonl`
and `push_reviews.jsonl` genuinely are one-at-a-time (each only gets written mid-way through the
Maintenance rule's "Framework propagation" flow below, which is itself a serial procedure), so a
plain per-file counter (`UPD-0001`, `REV-0001`, ...) is fine for those two.

Logging to `change_requests.jsonl` per Rule 17 means appending to the local `_frw` clone, not an
immediate standalone commit+push to `claude-project-framework` — an ordinary Track A/B task in a
downstream project shouldn't itself trigger ad-hoc pushes to the shared repo outside the Rule
15/16 review gate. The entry rides along next time `_frw`'s local clone is committed (a
Maintenance-rule propagation, most often), or a periodic sync commit if none happens for a while.

## Maintenance rule

`_frw` is a deliberate snapshot, not a live mirror. An ordinary change to `docs/` or `CLAUDE.md`
for *this project's* reasons (a new ADR, a new requirement, a routine process clarification) does
**not** get propagated to `_frw`. `_frw` only changes when there's a genuine decision to evolve
the underlying documentation *philosophy/framework itself* (something meant to apply to future
projects too, not just a fact about this one) — and per standing instruction, that decision is
never made unilaterally.

Since FRW-ADR-0011, an ordinary task in this project has exactly one write path into the shared
`_frw` repo — `_data/change_requests.jsonl` — and no others. The rule below is three separate
flows, not one procedure: **Outbound** is the only thing an ordinary task in this project ever
does; **Framework propagation** is a distinct, separately-approved procedure scoped to `_frw`'s
own working directory — never blended into or triggered as a side effect of a project task, even
when it runs within the same conversation as one (FRW-ADR-0012 clarifies this for the case where
one conversation has both `_frw` and a project open as separate working directories); **Inbound
sync** is how this project later receives whatever framework propagation decided, whether or not
this project was the one that proposed it, or ran it.

### Outbound: proposing a framework change (from this project)

The moment a possible framework-level enhancement is noticed — during planning, coding,
discovery, or a Rule 15/16 review, even one about to be fixed as part of the current task — log
it to `_frw/_data/change_requests.jsonl` immediately (Workflow Rule 17 / the `log-change-request`
skill). This is the entirety of what this project's session does toward evolving `_frw`: a
proposal, nothing more. It never writes `copy_me/`, `VERSION`, `README.md`, `_design/`, or `_frw`'s
own root `.claude/skills/` — those only ever change inside `_frw`'s own repo (see "Framework
propagation" below). A logged idea may still prompt an ordinary local change to this project's own
`CLAUDE.md`/`docs/` under Workflow Rule 6 if the situation calls for a stopgap — that's a normal
project-doc edit, not a write to `_frw`, and doesn't require any of this rule's approval ceremony.

### Framework propagation (inside `_frw`'s own repo only)

Turns an accepted framework-level idea into an actual bundle change. Its own commands run with a
working directory that is the shared `_frw` repo itself — most often triaging the
`change_requests.jsonl` backlog above, but, per FRW-ADR-0012, this can equally be `_frw` opened as
an additional working directory within a conversation that also has a downstream project's task
open, provided propagation is entered only through this flow's own classify/approve steps below,
**never** blended into or triggered as a side effect of that project's own task:

1. Classify the change against the criteria below before touching anything — every framework-level
   change still needs the user's approval before any edits begin; what differs is how much
   ceremony that approval takes.

   **Minor propagation** — all four must hold, or it's a full framework change instead:
   - confined to a single file — a lone `_frw`-only file counts as one file for this purpose;
     touching any additional file with different content does not
   - a wording clarification, a single field/value addition, or a comparably small self-contained
     edit
   - introduces no new rule, section, or file, and does not change an existing rule's meaning
   - carries no architectural, security, or business-rule implication of its own

   The fast lane is for genuinely small edits, not a way to avoid asking — if any criterion is
   doubtful, default to a full framework change.

2. Get the user's approval, scaled to the classification above. If this flow is being entered
   from within a conversation that also has a downstream project's task open (the FRW-ADR-0012
   same-conversation case), say so explicitly as part of the ask below — name it as a separate
   `_frw` propagation, distinct from the project task, not just another step of that task:

   - **Full framework change:** stop and ask the user whether to make it. Once approved, draft a
     written plan — scope, files touched, an outline of the content/edits — and get sign-off on
     that plan before making any edits. Plan Mode is the natural way to do this in an interactive
     session.
   - **Minor propagation:** combine the ask and the plan into one message — show the user the
     exact proposed edit (the diff itself, inline) and ask for a single yes/no. No separate plan
     document or Plan Mode round-trip is required; the diff *is* the plan.

   Only once approved (either path) does the rest of this flow proceed. (FRW-ADR-0008,
   FRW-ADR-0009)
3. Apply the generic edit directly to `copy_me/` (and `_design/` if the design rationale needs
   updating too), bump `VERSION` to the current timestamp (see "Versioning" above), and commit —
   in the `_frw` repo, not pushed yet. There is no "apply to a project's own files" step here — a
   project only receives this change later, via its own inbound sync (below).
4. Run `/code-review high` scoped to the changed `_frw` files, in the `_frw` repo itself (the
   Framework Reviewer lens — fidelity, ambiguity, enhancement opportunities — applied here since
   `_frw` carries no Rule 15/16 of its own). Fix findings or amend the still-local commit and
   re-run; once clean, append the outcome to `_frw/_data/push_reviews.jsonl` and push.
5. Append a record to `_frw/_data/update_history.jsonl` summarizing the update (see "Framework
   activity logs" above).

### Inbound sync: pulling framework updates into this project (pull, from this project)

The counterpart to framework propagation no longer touching a project's files directly — the only
way this project's own `CLAUDE.md`/`docs/`/`.claude/skills/` learn about a framework change,
whether or not this project was the one that proposed it. Runs on demand or periodically, via the
`sync-framework-updates` skill (`.claude/skills/sync-framework-updates/`, copied into this
project). It can also run immediately after this same conversation completes a Framework-
propagation push (the FRW-ADR-0012 same-session case above): check whether that push touched
`copy_me/` (e.g. `git show --stat <sha>`, or diff against the recorded "last synced" commit) and,
if so, tell the user and ask whether to sync now rather than waiting for the next on-demand or
periodic run; a push that touched no `copy_me/` files needs no sync. This doesn't change the write
path above — inbound sync stays read-only against `_frw` either way:

1. `git pull` the local `_frw` clone; compare its current `VERSION`/commit against this project's
   own recorded "Bootstrapped from / last synced at" line (below). If unchanged, stop — nothing to
   sync.
2. Diff `copy_me/*` between the last-synced commit and the clone's current `HEAD`, file by file.
3. For each changed file, merge the upstream structural/process edit into this project's own
   already-customized copy, preserving every project-specific fact (business content, filled-in
   placeholders, this project's own ADRs/modules/etc.) — apply directly when the merge is
   unambiguous (a wording clarification, an added section/rule that doesn't collide with anything
   project-specific).
4. Whenever a merge is ambiguous — the upstream change conflicts with something this project
   customized, or it's unclear how the two should combine — do not guess: log a
   `_data/change_requests.jsonl` entry describing the ambiguity (Workflow Rule 17) **and** stop to
   ask the user how to resolve that specific file/section before proceeding with it. Every other
   unambiguous file in the same sync may still go ahead while that one waits.
5. Once every file is resolved (merged, or deliberately deferred with the ambiguity logged),
   update this project's own "Bootstrapped from / last synced at" line (below) to the new
   commit/version/date, and log the sync in `docs/project/CHANGELOG.md`.

This flow is read-only against `_frw` except step 4's `_data/change_requests.jsonl` append (the
same Rule 17 write exception used everywhere else) — it never commits or pushes anything into the
shared `_frw` repo.
