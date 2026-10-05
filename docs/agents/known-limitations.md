# Known limitations and honest verification boundaries

This is a risk register, not authorization to implement fixes. **Confirmed** means a source
path was inspected; none of these risks was re-executed against a database in this docs task.
Keep recommendations separate from implementation claims.

## Highest-priority confirmed risks

| Risk | Evidence anchor | Consequence / intended future policy |
| --- | --- | --- |
| Startup writes and integrity normalization | [UsuarioService](../../APPLICATION/Features/Usuarios/UsuarioService.cs), `Inicializar`; [IntegridadService](../../APPLICATION/Features/Integridad/IntegridadService.cs), `RecalcularTodosDV` | Seeds/migrations alter data; unconditional recalculation can mask prior tampering. Separate verification from explicit authorized repair. |
| UI-centered authorization | [FrmOrdenServicioDetalle](../../PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.cs), `AplicarPermisosDetalle`; [OrdenServicioService](../../APPLICATION/Features/Ordenes/OrdenServicioService.cs), `ObtenerIdUsuarioSesion` | A session/state guard is not an action permission check. Do not expose services as a fully secured backend. |
| Warranty partial success | OrdenServicioService, `FinalizarDiagnostico`, `RegistrarEvaluacionPendienteReingreso`, `EvaluarReingreso` | Diagnosis/evaluation insertion and evaluation/order transition cross transaction boundaries. Errors can leave partially completed workflow. |
| Post-commit audit/readback failure | [SqlHelper](../../INFRASTRUCTURE/SQLHelper.cs), `ExecuteTransaction`; service audit calls after repository writes | Error does not guarantee rollback of the business write. Inspect persisted outcome before retrying. |

## Resolved in feat-005 (2026-10-04)

The three defects confirmed by the business-flow sweep (run 20261003B) were fixed and re-verified
through the real UI with database readback. They are no longer open risks:

| Finding | Fix applied | Verification |
| --- | --- | --- |
| COM-07 over-reversal | [CompraRepository](../../INFRASTRUCTURE/Features/Compras/CompraRepository.cs), `AnularConfirmadaConStock`, now validates `SUM(cantidad)` per `id_repuesto` before reversing | Aggregate 2 > stock 1 -> annul blocked, stock stays 1 (no negative); with coverage 2 the annulment succeeds and reverses exactly (2 movements); repeat annul rejected |
| GAR-08 `pago` heuristic | [OrdenServicioService](../../APPLICATION/Features/Ordenes/OrdenServicioService.cs), `EvaluarReingreso`, takes an explicit `continuarPago` flag from the UI destination dialog instead of `Contains("pago")` | Withdrawal button + reason containing `pago` -> ListoRetiro/GarantiaNoCubierta; paid button -> PendientePresupuesto; accept unchanged |
| INF-11 report boundary | [ReporteRepository](../../INFRASTRUCTURE/Features/Reportes/ReporteRepository.cs), all range queries now use `< DATEADD(day, 1, @Hasta)` | Report with Desde=Hasta=2026-10-04 includes that day's records and excludes later ones; INF-07/INF-09 range values match the database |

The UI keeps sending calendar dates; the `desde > hasta` validation is unchanged. The
`[pago: ...]` marker on the reason remains only as audit text and no longer decides the branch.

## Confirmed semantic/report inconsistencies

- `ContarOrdenesPorResultado` includes ListoRetiro only, not delivered history. A broad
  final-result report would need a separately approved definition change.
- UI `TasaGarantia` calls `TasaAceptacionGarantia`, not warranty re-entry/delivered-order rate.
  Approval rate excludes pending budgets from the denominator by design of current code.
- [GarantiaRepository](../../INFRASTRUCTURE/Features/Garantias/GarantiaRepository.cs),
  `ObtenerVigente`, compares current timestamp with end timestamp and ignores start date.
  Calendar-day inclusive coverage is intended policy, not the implemented date predicate.
- [EntregaRepository](../../INFRASTRUCTURE/Features/Ordenes/EntregaRepository.cs),
  `CancelarEntregaConTransicion`, leaves existing warranty/re-entries in place;
  `CrearConTransicion` does not regenerate an existing warranty on re-delivery.
- [FrmOrdenServicioDetalle](../../PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.cs),
  `BTN_CrearReingreso_Click`, submits the original order's historical customer. The service
  validates current equipment ownership, so changing the owner can block re-entry from this UI.
  This is a source-path finding, not a newly reproduced desktop failure.
- OrdenServicioService, `TryObtenerOrigenSolicitud`, infers request origin from history and an
  observation prefix. It is a text heuristic, not explicit request-cycle identity.
- `HasActividadPosterior` checks timestamps in separate reads. This is not a comprehensive,
  atomic activity ledger or generic budget-versioning mechanism.

## Validation, preservation, and UI caveats

**Confirmed source facts:** SQL lengths/uniqueness can reject values without equivalent
friendly field validation everywhere; exception messages are shown/wrapped by forms/services,
not uniformly mapped to localized per-field diagnostics. Soft-delete/reactivation and historic
reference display require checking both active-only and include-inactive paths.
Latest source preserves prior result observation during rejected-budget annulment through
`CombineObservacion` and preserves existing text in the unused `CerrarAbiertaYCancelar` path.
Do not repeat an old claim that these fixes are absent, or turn that into universal preservation.

**Not re-tested:** long input SQL-error presentation, numeric comma/dot editing, pending
NumericUpDown edit coercion/zero values, translations at small sizes/large DPI, and physical
button hit-testing. No custom TryParse path proving a universal comma-parser bug was found.
Use the [desktop matrix](development-and-testing.md), not historical screenshots/counts, for
current evidence. Missing/invalid input should be tested through both UI and service paths.

## Parallel execution is explicitly deferred

F6 parallel/concurrent scenarios remain deferred; historical sequential exercises do not prove
them. UPDLOCK/HOLDLOCK and transactional stock batches protect inspected operations, but
application read-then-write state checks and multi-transaction use cases still need deliberate
parallel tests. `Reparaciones` has intervention-number uniqueness, not a unique open-row index.
Do not claim concurrent correctness based on source locks or a sequential harness.

## Environment and scope limitations

- The `.slnx` SERVICES path is stale; use the valid `.sln` without claiming it was fixed here.
- No installed Krypton dependency was found; no repository-owned formal automated suite was found.
- CodeGraph was malformed/auto-sync disabled during inspection; docs do not require or repair it.
- Windows/.NET Framework 4.7.2 and SQL Server are current implementation choices, not a
  cross-platform browser application. Database contents/schema were not queried in this task.
- Weak demo passwords and source-defined encryption material require production review;
  this guide deliberately contains no credentials or derivation values.
- No implemented invoicing/payment/cash/AFIP/mobile/AI-diagnosis/export scope should be inferred.

Future notes are planning inputs only. Do not implement them during onboarding/documentation
work, declare a checkpoint complete because these guides exist, or treat them as permission to
change frameworks, schema, data, Git history, or published branches.
