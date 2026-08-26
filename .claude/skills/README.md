# Project Skills — Index

Ten skills every project bootstrapped from `_frw` carries, copied verbatim into
`<project>/.claude/skills/` by the bootstrap step. Each one does the mechanical, format-precise
part of a `CLAUDE.md` process; each re-reads its own source-of-truth rule/doc live rather than
freezing a copy of it — see each `SKILL.md` for the exact doc section it defers to.

| Skill | Use for |
|---|---|
| [`file-task`](file-task/SKILL.md) | File a new Task Record for any ask — intake for the Analyst/Developer/Reviewer/Tester/Auditor pipeline (`docs/project/tasks/README.md`) |
| [`analyst-plan-task`](analyst-plan-task/SKILL.md) | Analyst — classify a filed task minor/major and draft its `plan.md` |
| [`developer-work-task`](developer-work-task/SKILL.md) | Developer — implement an approved task's `plan.md` on its own git branch and write `dev-notes.md` |
| [`push-review-gate`](push-review-gate/SKILL.md) | Rule 15/16 pre-push review + `review_log.md` entry |
| [`log-change-request`](log-change-request/SKILL.md) | Rule 17 — append to `_frw`'s `change_requests.jsonl` (this project's only write into `_frw`) |
| [`sync-framework-updates`](sync-framework-updates/SKILL.md) | Pull latest `copy_me/*` from `_frw` and merge into this project's own files |
| [`discovery-iteration`](discovery-iteration/SKILL.md) | Track A — one bounded discovery-plan iteration |
| [`new-module`](new-module/SKILL.md) | Scaffold a new `docs/modules/<name>/` folder |
| [`new-adr`](new-adr/SKILL.md) | Create and register a new numbered ADR |
| [`new-api-operation`](new-api-operation/SKILL.md) | Document a new HTTP/MCP operation per ADR-0005 |

Two further skills — `bootstrap-project` and `propagate-framework-change` — exist only in `_frw`'s
own `.claude/skills/` at the bundle root; they are about creating projects and maintaining the
framework itself, not something a project needs of itself, so they are not copied here.

`_lib/` also ships alongside these ten — small scripts other skills invoke internally, not a
user-invocable skill itself. See [`_lib/README.md`](_lib/README.md).
