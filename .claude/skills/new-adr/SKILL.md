---
name: new-adr
description: Create a new globally-numbered Architecture Decision Record from docs/decisions/TEMPLATE.md and register it in docs/decisions/README.md. Use for "new ADR", "record this decision", "write an ADR", or "document why we chose X".
---

# New ADR

Creates a new Architecture Decision Record per `CLAUDE.md`'s "ID namespacing" rule: ADRs are
numbered globally (not per module) since one decision often touches several modules.

## Procedure

1. **Confirm this needs an ADR before writing one.** Per Workflow Rules 2–4: a new significant
   decision (architecture, security, or business-rule impact; ambiguous, controversial, or hard to
   reverse) should be confirmed with the user first unless it's small/reversible enough for a
   Rule 4 judgment call — in which case record the reasoning inline in the ADR itself rather than
   skipping it. Don't create the file until that's settled.
2. **Get the next number.** Read `docs/decisions/README.md`'s index table for the highest existing
   ADR number; the new one is the next sequential integer, global across all modules.
3. **Copy the template**: `docs/decisions/TEMPLATE.md` → `docs/decisions/NNNN-<short-title>.md`.
4. **Fill it in**: Date, Status (`Proposed` or `Accepted`), Modules affected, Context, Options
   considered (with real tradeoffs, not just the chosen one), Decision, Consequences (what this
   makes easier or harder later, what it precludes).
5. **Add the index row** to `docs/decisions/README.md` (ID, Title, Status, Modules affected, Date).
6. **Never edit or delete a past Accepted ADR's Decision/Consequences.** If circumstances change,
   write a new ADR and set the old one's Status to `Superseded by ADR-XXXX` instead.
