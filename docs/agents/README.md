# Portable project guide

Start with [root agent rules](../../AGENTS.md), then read only the detailed guide your task needs.
This documentation is self-contained: no prior transcript, local absolute path, Engram,
Gentle AI installation, or special agent framework is needed.

## Session state and verification

- [Feature tracker](../../feature_list.json): authorized tasks, dependencies, acceptance criteria, and visual scenarios; one active task.
- [Session progress](../../progress.md): current objective, checkout observation, evidence, blockers, and next step; also serves as the session handoff.
- [PowerShell verifier](../../init.ps1): run `powershell -NoProfile -File ".\init.ps1"` from the repository root. It reads state and checks relative file links and `git diff --check`; it does not build, launch, or connect to a database.
- For behavior changes, [visual state-machine acceptance](development-and-testing.md#primary-acceptance-visual-state-machine-flows) is the primary gate. Compilation and structural harness checks are supporting evidence.

## Quick reading path

For a first visit, read [use-case navigation](use-case-navigation.md) for the product journey
and screen-to-service entry points. Then select the relevant technical guide below.

1. [Architecture](architecture.md): six projects, ownership, construction, UI entry points.
2. [Business flows](business-flows.md): legal transitions and rollback windows.
3. [Persistence](persistence.md): inferred schema, constraints, transactions, startup.
4. [Security and localization](security-and-localization.md): identities, permissions, ES/EN.
5. [Development and testing](development-and-testing.md): safe setup and verification.
6. [Known limitations](known-limitations.md): confirmed risks and unverified behavior.
7. [Optional desktop-testing environment](desktop-testing.md): local-only agent tooling (not part of the app).
8. [Business test catalogue](business-test-catalogue.md): on-demand flow, reversal and cross-module sweeps by phase.

## Evidence and scope

**Verified implementation** means current source was inspected on
`checkpoint-5-dashboard-reportes`, commit `47a0bf8`, on 2026-09-30.
All six project manifests, startup, service/repository method maps, relevant SQL batches,
domain transitions, and main order/purchase/warranty/report UI paths were surveyed.
CodeGraph reported a malformed index and disabled auto-sync; direct source inspection was used.
No build, application launch, live-schema query, or database mutation was performed.
The guides were subsequently committed in `297b993`. Their original inspection date and
source snapshot remain historical evidence; current session results are recorded separately
in [progress.md](../../progress.md).

**Intended policy** describes a requirement or recommendation, not an implemented guarantee.
**Not re-tested** identifies a behavior needing a fresh desktop/database exercise.
Historical checkpoint reports and diagrams are useful context but not authoritative inventories
or current gates. If they disagree with the current source, document the discrepancy rather than
changing code to fit the older description.
Relative source links point to files; the named method is the stable search anchor within each file.

## Five-checkpoint product map

| Checkpoint | Implemented scope in the current tree |
| --- | --- |
| 1 | Customers, equipment, brands/types, active flags, existing identity/permission/localization foundation |
| 2 | Service reception, assignment, diagnosis, Original/Adicional budgets, order history and delivery |
| 3 | Parts, stock movements, repair interventions, consumption/returns, tests and controlled rollback actions |
| 4 | Suppliers, draft/confirmed/cancelled purchases, automatic warranties and separate warranty re-entry/evaluation |
| 5 | Six dashboard KPIs and eleven report choices, read-only report backend |

This is electronics repair management, not invoicing, payments, cash, AFIP, mobile,
AI-assisted diagnosis, or exports. A budget commitment is not revenue.
Implemented scope does not establish final completion or production hardening.

## How to update

Follow the form handler through the application service, domain method, and repository SQL.
Update facts and limitations together; do not copy old counts or diagrams without checking them.
Use [the verification checklist](development-and-testing.md) and preserve the authorization boundary.
