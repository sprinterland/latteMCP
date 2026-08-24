---
name: sync-framework-updates
description: Pull the latest `_frw` `copy_me/` bundle and merge structural/process changes into this project's own CLAUDE.md, docs/, and .claude/skills/, preserving project-specific content. Use for "sync the framework", "pull framework updates", "check for _frw updates", "update from _frw", or "merge latest copy_me". The only inbound path from `_frw` into this project (CLAUDE.md's "Reusable Framework Template" section, FRW-ADR-0011) — any merge ambiguity gets logged to change_requests.jsonl and the user is asked before it's resolved, never guessed.
---

# Sync Framework Updates

Pulls whatever changed in `_frw`'s `copy_me/*` since this project last synced, and merges it into
this project's own already-customized `CLAUDE.md`/`docs/`/`.claude/skills/`. **Do not treat the
steps below as the rule itself** — `docs/framework-maintenance.md`'s "Maintenance rule" (the
"Inbound sync" flow specifically) is the source of truth and has changed shape before; re-read it
live every time this skill runs rather than trusting this summary.

This project's session has exactly one *write* path into the shared `_frw` repo —
`_data/change_requests.jsonl` (see `log-change-request`) — so this skill never writes anything
back into `_frw` except that one file, and only when it hits an ambiguous merge (step 4). It never
commits or pushes anything into the shared `_frw` repo itself; committing this project's own
merged files afterward is an ordinary local commit, same as any other doc change.

## Procedure

Mirrors `docs/framework-maintenance.md`'s "Inbound sync" flow step for step — re-read that flow
live before acting, this is a summary of it, not a replacement.

1. **Pull and compare.** Read `docs/framework-maintenance.md`'s "Bootstrapped from / last synced
   at ... commit `<sha>`, version `<...>`" line — this project's own record of where it last
   caught up to. `git pull` in the local `_frw` clone, then compare its current `VERSION`/commit
   against that recorded line. If they match, say so and stop — nothing to sync.
2. **Diff.** `git diff <last-synced-sha>..HEAD -- copy_me/` in the local `_frw` clone to get the
   exact list of changed files and their diffs since this project's last sync.
3. **Merge file by file.** For each changed `copy_me/` file, find its counterpart in this project
   (`copy_me/CLAUDE.md.template` → `CLAUDE.md`, `copy_me/docs/X` → `docs/X`,
   `copy_me/.claude/skills/Y/` → `.claude/skills/Y/`, a wholly new file → copy it in directly).
   Apply the upstream edit to this project's copy, preserving every project-specific fact already
   there (real business content, filled-in placeholders, this project's own ADRs, modules, glossary
   entries, `dev-practices.md` selections, etc.) — most upstream changes are process/structure
   edits that merge cleanly alongside project content that was never part of the template to begin
   with. Apply directly when the merge is unambiguous.
4. **On ambiguity, stop — don't guess.** If an upstream change collides with something this project
   customized, or it's unclear how the two should combine, do both of the following before touching
   that file further: log a `_data/change_requests.jsonl` entry describing the ambiguity (same
   mechanics as `log-change-request` — self-generated `CR-` id, `activity: "sync"`), and ask the
   user (e.g. via AskUserQuestion) how to resolve it. Every other unambiguous file from step 3 may
   still be applied while this one waits.
5. **Update the sync record.** Once every file from step 2 is either merged or deliberately
   deferred with its ambiguity logged, update `docs/framework-maintenance.md`'s "Bootstrapped
   from / last synced at" line to the new commit/version/date, and add a dated entry to
   `docs/project/CHANGELOG.md` summarizing what was pulled in.
