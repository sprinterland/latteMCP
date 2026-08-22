# latteMCPclient — Domain Model

Technology-agnostic. Concepts this module deals with, independent of how they are currently
stored or transmitted. Shared cross-module terms belong in `../../glossary.md` instead.

This module owns no entities. It holds exactly one piece of state for the duration of a run —
the bearer token obtained at login (`CLIENT-REQ-001`) — and otherwise only displays data it
receives from `latteMCP`, which itself owns no data (see `../latteMCP/domain-model.md`). The
underlying business entities (`Order`, `MenuItem`, `Waitress`, ...) are defined in
`../latteAPI/domain-model.md`.
