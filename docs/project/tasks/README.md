# Task Records — the Analyst/Developer/Reviewer/Tester/Auditor pipeline

Canonical description of the Task Record pipeline's on-disk substrate: one folder per user ask
under `docs/project/tasks/`, deliberately distinct from `_data/change_requests.jsonl`'s "CR"
concept (a proposal backlog, not a unit of work). **Read this file live before any pipeline skill
acts** — it is the single source of truth for the file layout and schemas below; no skill should
freeze a stale copy of it.

Designed so each of the five roles can eventually run as a separate agent invocation with zero
shared conversational state: every handoff between roles happens strictly through the files
described here, never through memory of a prior turn.

## Roles

| Role | When it runs | Touches git? | Core question it answers |
|---|---|---|---|
| **Analyst** | Sync or async, right after a task is filed | No | Is this minor or major, and what's the plan? |
| **Developer** | After Analyst's plan is approved | Yes — own branch | Does the approved plan get implemented? |
| **Reviewer** | After Developer finishes | Yes — diffs the branch, merges, pushes | Is the diff correct, and should it ship? |
| **Tester** | Immediately after Reviewer merges+pushes — synchronous by default, async only if explicitly configured | Yes — deploys/runs on the **test** environment, post-merge | Does it actually work? |
| **Auditor** | Async, independent of push/test timing | No — stored artifacts only | Did the plan really get applied, and does it still fit the project's mission/architecture/domain-model? |

**Not every role's skill ships yet.** `file-task` (intake), `analyst-plan-task` (Analyst), and
`developer-work-task` (Developer) have landed; the remaining three role skills — the task-aware
step in `push-review-gate` (Reviewer), `tester-verify-task`, `auditor-audit-task` — land in later
framework syncs, the same "forward reference, not a claim it exists yet" precedent `_lib/README.md`
already set for this file. Until a given skill arrives, its role is carried out by hand, writing
the same files in the same shapes documented below.

## File layout

One folder per task, id `TASK-<epoch-ms>-<hex4>` (same self-generated, no-coordination-needed
scheme `_data/change_requests.jsonl` uses), folder and branch both suffix a human-readable slug
(`TASK-<id>-<slug>`) — the id *without* the slug is the canonical cross-reference everywhere else.

```
docs/project/tasks/TASK-<id>-<slug>/
  ask.md            — the raw request, verbatim, written once at intake, never edited after
  status.jsonl        — append-only transition log (schema below) — the single source of truth
                        for "whose turn it is"; latest line wins
  .claim              — ephemeral lock file, gitignored, NOT committed (schema below)
  plan.md             — Analyst's output: classification, scope, approach, acceptance criteria
                        (collapsed for minor — see ceremony table below)
  dev-notes.md         — Developer's report: what was done; deviations from plan.md, each
                        cross-referenced to the specific plan.md section it departs from
  review/
    round-1.md, ...     — Reviewer's findings per round that requested changes (skipped entirely
                          if the first round passes)
    diff.patch           — Reviewer's frozen, verified diff — captured only on the passing round,
                          `git diff $(git merge-base main <branch>)...<branch-head>`
    review.md            — Reviewer's verdict, round count, reference to diff.patch
  test/
    test-report.md        — Tester's report: what ran, pass/fail, acceptance-criteria coverage
  audit.md             — Auditor's verdict; if it opens a follow-up task, states the new TASK-id
```

`task_log.md` indexes **open** tasks only. `completed_tasks.md` archives **closed** tasks.

## Precise artifacts

**`status.jsonl` line** — one JSON object per line, appended via
`.claude/skills/_lib/append_jsonl.py <task-dir>/status.jsonl --stdin`:

```json
{"ts": "2026-08-25T14:32:07+03:00", "role": "developer", "event": "dev-complete", "status": "ready-for-review", "notes": "implemented per plan.md section 2; see dev-notes.md"}
```

`role` is one of the five roles above, or `intake` for `file-task`'s own filing line — the only
`status.jsonl` event no role has claimed yet, since `file-task` itself isn't a role.

`status` enum (closed set; `event` is free-text, not enumerated — it has grown before and may
again, so re-read this list live rather than trusting a remembered copy):

```
new | planning | awaiting-approval | approved | in-development | ready-for-review | in-review |
changes-requested | review-passed | merged | testing | tests-passed | tests-failed |
audit-pending | blocked | escalated | closed
```

**`.claim` file** — published atomically via `claim_lock.py` (write-to-temp-file then `os.link()`
the temp file to `.claim`, which fails with `FileExistsError` if a claim is already there and
never lets a concurrent reader observe a partially-written file — see "Claim / lock mechanism"
below), never committed:

```json
{"agent_id": "session-7f3a", "role": "developer", "claimed_at": "2026-08-25T14:10:00+03:00"}
```

**Follow-up linkage** (in a follow-up task's `ask.md`, added by whichever role — Tester or
Auditor — opens it): a `Follow-up of: TASK-<original-id>` line plus `Depth: <n>`. Depth 0 = an
ordinary task; a follow-up of a depth-N task is depth N+1, read off the ancestor's own `ask.md` —
a single value shared by construction, since both roles read the same field rather than tracking
their own count. **Max depth is 1**: before opening a follow-up that would be depth 2 (whether
Tester or Auditor is the one opening it), don't — append `status: escalated` naming both ancestor
ids and stop, instead of auto-spawning a third generation.

## Git isolation

Each task gets its own branch, `task/TASK-<id>-<slug>`, created from `main`'s current tip.
Developer works only on that branch and never merges or pushes. Reviewer's `diff.patch` is scoped
to `$(git merge-base main <branch>)...<branch-head>` — independent of whatever else lands on other
task branches concurrently. On a passing review, Reviewer squash-merges into `main` and pushes: one
task, one logical commit. Residual risk (two sibling branches touching the same file) surfaces as
an ordinary merge conflict to a human, same as in any parallel-development workflow.

## Classification (Analyst)

Minor-vs-major classification criteria live in `docs/dev-practices.md`'s "Task Classification
(Analyst)" section, not here — re-read that section live, it's the single source of truth and can
change independently of this file. The split gates the **review** checkpoint specifically: minor
gets an automated `/code-review` pass only (the existing `push-review-gate` mechanism, made
task-aware in a later phase — not a separate invocation), sufficient to merge; major additionally
requires an explicit human review checkpoint *after* the automated pass clears, before merge —
separate from, and in addition to, the plan-approval sign-off major already requires at the
Analyst stage. This human checkpoint is never itself configurable away, regardless of
`dev-practices.md` settings.

## Minor-task ceremony — what's required vs. collapsed

| File | Minor task | Major task |
|---|---|---|
| `ask.md`, `status.jsonl`, `.claim` | Required | Required |
| `plan.md` | Collapsed: classification + one-paragraph approach, no acceptance-criteria section; shown together with `ask.md` for one combined yes/no | Full: classification, scope, approach, files touched, acceptance criteria, recorded sign-off |
| `dev-notes.md` | Required, short | Required, full |
| `review/round-N.md` | Only if a round actually fails | Required per failed round |
| `review/diff.patch` | Always required | Always required |
| `review/review.md` | Collapsed: command + outcome, few lines | Full verdict + round count |
| `test/test-report.md` | Required (Tester always runs) | Required |
| `audit.md` | Required (Auditor always runs) | Required |
| Human checkpoint post-automated-review | Not required | Required, before merge |

`ask.md` stays immutable per its own definition regardless of classification — "collapsing" means
a genuinely short `plan.md`/`review.md`, never merging content into `ask.md`.

## Claim / lock mechanism

Resolves the concurrent-agent race that a pure append-only log can't prevent on its own (two
near-simultaneous appends can't see each other). A role must atomically create `<task-dir>/.claim`
before acting, and delete it when done, via `.claude/skills/_lib/claim_lock.py`:

- `claim_lock.py claim <task-dir> --agent-id <id> --role <role>` — atomic create (fails cleanly if
  a live claim already exists).
- `claim_lock.py release <task-dir> --agent-id <id>` — removes the claim; refuses to touch a claim
  owned by a different `agent_id` (use `reclaim` instead).
- `claim_lock.py check <task-dir> [--timeout-min <n>]` — reports the current claim's contents (if
  any), its age, and whether it's past the timeout (default 30). Read-only, never modifies
  anything.
- `claim_lock.py reclaim <task-dir> --agent-id <id> --role <role> [--timeout-min <n>]` — takes over
  a claim only if it's past the timeout, based on its `claimed_at` age alone (the script never
  inspects `status.jsonl`). The caller is responsible for appending the `status.jsonl`
  `event: claim-reclaimed` line afterward — `reclaim` itself doesn't write it, so the takeover is
  visible rather than silent only if that follow-up append actually happens.

Default timeout is 30 minutes. See `_lib/README.md` for the script's full behavior.
