# Session Progress

## Current State

- Last Updated: 2026-10-05 (feat-005 fixes complete and verified)
- Current Objective: `feat-005` **done**: COM-07, GAR-08 and INF-11 fixed and re-verified through the real UI with database readback. `feat-004` sweep A-F remains complete (all 144 rows).
- Next objective: owner review of the changes; optional push/commit already prepared as one commit. REP-07 (partial return) remains blocked/not a defect.
- Checkout observed: `checkpoint-6-arreglos-y-mejoras` @ `b5ba8cc` plus local changes (3 source files + guides/tracker); one commit pending.
- Preexisting untracked entries to preserve: `.codegraph/`, `.playwright-mcp/`, `skills-lock.json`.
- Run artifacts (outside the repository): `%LOCALAPPDATA%\ServicioTecnicoDesktopTests\evidence\sweep-20261003B\` with `sweep-log.md` and `sweep-20261003B.md`; sweep scripts under `...\scripts\sweep\`.

## Sweep result (run 20261003B, completed)

All 144 catalogue rows were exercised through the real WinForms UI with read-only SQL readback.

- **PASS:** CLI-01..08, EQP-01..07, CAT-01..05, PRO-01..03, STK-01..07, COM-01..06/08..10, ORD-01..10, DIA-01..07, PRE-01..13, ADI-01..11, REP-01..06/08..10, PRU-01..08, ENT-01..08, GAR-01..07/09..11, DSH-01..06, INF-01..10, HIST-01, LIST-01, LANG-01, PERM-01, INT-01..05.
- **RESOLVED (feat-005, verified 2026-10-05):**
  - COM-07: `AnularConfirmadaConStock` now validates `SUM(cantidad)` per part. Verified: aggregate 2 > stock 1 -> blocked (stock 1, no negative); with coverage 2 -> annul succeeds (stock 2 -> 0, 2 movements); repeat annul rejected; single-row COM-06 still blocked.
  - GAR-08: `EvaluarReingreso` takes an explicit `continuarPago` flag from the UI destination dialog. Verified: withdrawal + reason containing `pago` -> ListoRetiro/GarantiaNoCubierta; paid -> PendientePresupuesto; GAR-05/07 unchanged.
  - INF-11: report queries use `< DATEADD(day, 1, @Hasta)`. Verified: Desde=Hasta=2026-10-04 report equals the DB for that day and excludes later records; INF-07/INF-09 range values match the DB.
- **BLOCKED:** REP-07 partial return: the UI "Quitar consumo" returns the whole (part, cost) group; partial quantities are not exposed. Not a confirmed defect.
- Fix locations for the three defects are in the "Pending fixes recorded" table below and in [known limitations](docs/agents/known-limitations.md).
- Fixtures created through the UI (kept for review): type/brand/customer/equipment/supplier/parts AGENT_* and orders 2002..2009, 3002..3010, purchases 1..6, plus warranty/evaluation/consumption/test rows.
- Operational notes for the next run: order ids jumped (2xxx then 3xxx); `open_order` searches by number and verifies the header. Form steps take ~40s each; use command timeouts >= 1200s and read `sweep-log.md` (written incrementally). Relaunch/login via `scripts\sweep\sweepkit.ensure_login()`.

## Handoff for the next session

1. Read [AGENTS.md](AGENTS.md); run `powershell -NoProfile -File ".\init.ps1"`.
2. Review the `feat-005` diff (3 source files + guides) and the fix evidence below.
3. Optional next work: push the commit, or investigate REP-07 partial-return UI separately.
4. If touching the defects again, re-run the affected catalogue rows plus neighbours.

## Reference: catalogue phases and driver

- Phases: A (CLI, EQP, CAT, PRO, STK, PERM-01), B (COM), C (ORD, DIA, PRE, ADI, REP, PRU, ENT), D (GAR), E (DSH, INF), F (INT + closure).
- Desktop driver: external environment at `%LOCALAPPDATA%\ServicioTecnicoDesktopTests` (pywinauto 0.6.9). See [Desktop testing (local only)](docs/agents/desktop-testing.md).
- Fixtures use the `AGENT_TEST_<run-id>` prefix (masters `AGENT_`); records are kept unless the owner asks for cleanup.

## Verification Evidence

| Check | Command or procedure | Result | Boundary |
| --- | --- | --- | --- |
| Build | MSBuild `TP_INTEGRADOR_2022.sln` Debug/Any CPU | PASS: six projects, UI.exe produced | Compilation only |
| Desktop environment | external `check-environment.ps1` | PASS: windows-mcp 0.8.7, pywinauto 0.6.9, input desktop readable | Tooling only |
| Sweep A-F (run 20261003B) | pywinauto form flows + read-only SQL readback; see external `sweep-20261003B.md` | Complete: all 144 rows executed or recorded; defects later fixed in feat-005 | Full sweep; defects were findings |
| Fix COM-07 | Aggregate 2 > stock 1 annul (blocked); coverage 2 annul (reverses exactly); repeat; single-row COM-06 | PASS: no negative stock; state/movements correct | UI + DB |
| Fix GAR-08 | Withdrawal + reason containing `pago`; paid button; withdrawal without `pago`; accept | PASS: GarantiaNoCubierta / PendientePresupuesto / GarantiaNoCubierta / Autorizado | UI + DB |
| Fix INF-11 | Report Desde=Hasta=2026-10-04 with filter on; INF-07/INF-09 ranges | PASS: matches DB for that day and excludes later days | UI + DB |
| Fixtures | Orders 2002-2009/3002-3014, purchases 1-6/1002-1003, parts AGENT_PART/AGENT_LOW, customer CLI_20261003B, supplier AGENT_PROV_20261003B | Created through the UI; intentionally kept for review | Non-production DB |
| Harness verification | `powershell -NoProfile -File ".\init.ps1"` | Re-run at end of session | Structural checks only |

Historical (2026-10-03, `feat-002`): the budget, repair/tests/reopen, delivery and warranty
re-entry cycles were validated through the real forms against the non-production database;
orders 1002-1006 and `AGENT_` fixtures were intentionally kept. Detailed log and screenshots:
`%LOCALAPPDATA%\ServicioTecnicoDesktopTests\evidence\visual-validation-20261003.md`.

## Blockers / Not Re-tested

- COM-07, GAR-08 and INF-11 are **fixed and re-verified** (feat-005); no longer open.
- REP-07 (partial return) is BLOCKED: the UI "Quitar consumo" returns the whole (part, cost) group and does not expose a partial quantity. Not a confirmed defect.
- Known tool limitation remains: Windows-MCP omits controls with an empty accessible name; pywinauto by AutomationId was used. The report DateTimePicker is exposed as a Pane; the filter was enabled with Space + arrow keys.
- No repository-owned automated suite exists; this is desktop evidence recorded as a run log, not a reproducible CI gate.

## Fixes applied (feat-005)

| Finding | Change | File |
| --- | --- | --- |
| COM-07 | `AnularConfirmadaConStock` validates `SUM(cantidad)` per `id_repuesto` before reversing | [CompraRepository](INFRASTRUCTURE/Features/Compras/CompraRepository.cs) |
| GAR-08 | `EvaluarReingreso` takes explicit `continuarPago`; UI passes the destination decision | [OrdenServicioService](APPLICATION/Features/Ordenes/OrdenServicioService.cs), [FrmOrdenServicioDetalle](PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.cs) |
| INF-11 | All report range queries use `< DATEADD(day, 1, @Hasta)` | [ReporteRepository](INFRASTRUCTURE/Features/Reportes/ReporteRepository.cs) |

These are also recorded in [known limitations](docs/agents/known-limitations.md).

## Next Session / Recommended Next Step

1. Re-read [AGENTS.md](AGENTS.md), resolve the checkout, and compare fresh Git state with this record.
2. Review the `feat-005` diff and the verification evidence above.
3. Optional: push the commit, or investigate REP-07 partial-return UI separately.
