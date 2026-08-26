---
name: file-task
description: File a new Task Record for any user ask, however small — creates its folder under docs/project/tasks/, records the raw request verbatim, and adds it to the open-tasks index. Use at the start of any task, or when asked to "file a task", "start a task record", or "log this as a task". Filing is intake only — it never classifies, plans, or implements anything.
---

# File a Task Record

Creates a new Task Record — the durable, on-disk substrate the Analyst/Developer/Reviewer/
Tester/Auditor pipeline works against. **Re-read `docs/project/tasks/README.md` live before
filing** — it is the single source of truth for the file layout and schemas below; this skill
must never freeze a stale copy of it.

## Procedure

1. **Generate the id.** Run `.claude/skills/_lib/append_jsonl.py --gen-id TASK` — self-generated,
   no coordination needed, the same scheme `_data/change_requests.jsonl` uses.
2. **Pick a slug.** A short, human-readable, hyphenated summary of the ask (e.g.
   `fix-login-timeout`). The folder and branch both carry it, but the bare id (without the slug)
   is the canonical cross-reference everywhere else (`Follow-up of:`, `task_log.md`, commit
   messages).
3. **Create the folder** `docs/project/tasks/TASK-<id>-<slug>/`.
4. **Write `ask.md`** — the raw request, verbatim, exactly as given. Never edited after this
   step; a request that later changes becomes a new Task Record instead, cross-referenced back to
   this one.
5. **Append `status.jsonl`** — one line with the full schema (`ts`, `role: "intake"` — filing
   itself isn't one of the five pipeline roles — `event`, `status: "new"`, `notes`) per
   `docs/project/tasks/README.md`'s "Precise artifacts" section, via
   `.claude/skills/_lib/append_jsonl.py <task-dir>/status.jsonl --stdin` (pipe the JSON in — e.g.
   write it to a scratch file first and redirect that file's contents in, or heredoc it — never
   `--json '<...>'` with the object interpolated into a shell-quoted argument, since an apostrophe
   in the ask's own text, routine in prose, breaks single-quoting and can silently corrupt or
   truncate the entry).
6. **Add a `task_log.md` row** — id, filed date, classification (leave as "— pending Analyst"),
   status (`new`), owner-role (`—`, unclaimed).
7. **Report the id, and stop.** Filing is intake only — it does not classify, plan, or start
   implementation. State the id and that the task is ready for the Analyst role
   (`analyst-plan-task`).
