# `_lib/` — shared scripts for other skills

Not a user-invocable skill itself — these are small, stdlib-only Python 3 helpers that other
skills in this directory call internally, the same "present for internal use, not a slash-command
target" role `docs/modules/_module-template/` plays for module scaffolding.

- `append_jsonl.py` — safe append-only JSONL writer + self-generated id (`<PREFIX>-<epoch-ms>-
  <hex4>`) helper. Used anywhere this project appends to a `.jsonl` log — today, a Task Record's
  `status.jsonl` (see `docs/project/tasks/README.md`).
- `claim_lock.py` — atomic claim/lock helper for Task Record folders (`docs/project/tasks/<id>/
  .claim`), used by the Analyst/Developer/Reviewer/Tester/Auditor pipeline skills so two roles can
  never act on the same task concurrently.

See `docs/project/tasks/README.md` for how the Task Record pipeline uses both — that file, and the
pipeline itself, ship in a later phase of the same rollout `_lib/` is the first phase of; this note
is a forward reference, not a claim the file exists yet.
