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

## Addressed in feat-006 (verified at runtime 2026-10-13)

`feat-006` is `done`: its Checker verified all **6/6 acceptance criteria and 4/4 scenarios**
through the real WinForms UI with database readback on 2026-10-13 (see the dated feat-006 Checker
section in [progress.md](../../progress.md)). The rows below combine inspected source with that
runtime evidence.

| Item | Implementation inspected | Remaining boundary |
| --- | --- | --- |
| Unique documento | [ClienteRepository](../../INFRASTRUCTURE/Features/Clientes/ClienteRepository.cs), `Inicializar`, installs `UX_Clientes_Documento` only when the index is absent and no duplicate `documento` group exists (inactive rows included); `Agregar`/`Modificar` re-check ownership under `TABLOCKX,HOLDLOCK` in the write batch | A database that already holds duplicates has no index installed; the transactional guard still rejects new duplicates, but the legacy rows themselves stay duplicated and are never merged, renumbered or deleted |
| Inactive reservation | `Desactivar`/`Reactivar` only toggle `activo`; `ObtenerConflictoDocumento` and the write batches ignore the flag | No expiry or reuse rule exists: an inactive documento keeps reserving its document. Reactivating the record does not release it, and editing other fields of the row does not release it either (only changing the documento to a free value does); this was observed at runtime on the inactive cliente 3004 |
| Shareable contacts | `telefono` and `email` keep no constraint, index or service check; the new lookup reads only `documento` | Nothing prevents two customers from sharing a phone or an email, by design |
| Business message | [ClienteService](../../APPLICATION/Features/Clientes/ClienteService.cs), `Crear`/`Modificar`, rethrow the concrete exception; [FrmClientes](../../PRESENTATION/Forms/Clientes/FrmClientes.cs), `MostrarError`/`MostrarDocumentoDuplicado`, translate the typed conflict by exception type, never by SQL or message text | The message names the existing owner; the dialog closes before the service runs, so a rejected creation loses the typed fields exactly as other post-dialog errors do |
| Usuarios finding | Usuarios stores only username/password/activo/dvh, so it holds no customer-style personal identity data. [UsuarioService](../../APPLICATION/Features/Usuarios/UsuarioService.cs), `Modificar`, now pre-checks `ObtenerPorUsername` excluding the edited id, [UsuarioRepository](../../INFRASTRUCTURE/Features/Usuarios/UsuarioRepository.cs), `Modificar`, maps unique-index errors 2601/2627 to `UsuarioYaExisteException`, and [FrmAdministrarUsuarios](../../PRESENTATION/Forms/Auth/FrmAdministrarUsuarios.cs), `MostrarUsuarioDuplicado`, shows `Mensaje.UsuarioYaExiste` | `Crear` already rejected a username held by an inactive account; the edit path previously surfaced the raw index error. No new personal-data column was added |

Uniqueness is exact string equality after the factory `Trim()`, evaluated by the database
collation. No digit-only validation, punctuation stripping, numeric conversion or leading-zero
removal was added, so `12.345.678` and `12345678` remain different documents. Write batches on
`Clientes` now take a table lock; that is inspected SQL, not evidence of parallel correctness
(see the deferred parallel-execution section below).

## Addressed in feat-007 (verified at runtime 2026-10-13)

`feat-007` is `done`: its Checker verified all **5/5 acceptance criteria and 3/3 scenarios**
through the real WinForms UI with database readback on 2026-10-13 (see the dated feat-007 Checker
section in [progress.md](../../progress.md)). The rows below combine inspected source with that
runtime evidence.

| Item | Implementation inspected | Remaining boundary |
| --- | --- | --- |
| Spare-part item type | [TipoItemPresupuesto](../../DOMAIN/Features/Ordenes/TipoItemPresupuesto.cs), `Repuesto`; [FrmOrdenServicioDetalle](../../PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.cs), `CargarComboTiposItem`; [IdiomaRepository](../../INFRASTRUCTURE/Features/Idiomas/IdiomaRepository.cs) seeds `TipoItem.Repuesto` = "Repuesto"/"Spare part" | Label rendering was observed in ES/EN at 900x600 and 2000x1200; other DPIs and numeric cultures were not exercised |
| Explicit part choice | `CargarComboRepuestosPresupuesto` loads parts through `RepuestoService.Listar` and offers only `Activo` ones with `SelectedIndex = -1`; `BTN_AgregarItem_Click` requires the choice, snapshots `codigo - descripcion` and passes the part id to `DetallePresupuesto.CrearNuevo` | Covered by the runtime run (part choice, prefill, quantity, price and grid row; the picker rendered at 900x600 and 2000x1200). Still a boundary: an active part whose `PrecioReferencia` exceeds the price control maximum (10,000,000) cannot be prefilled: the picker warns and leaves the field at zero, and that price cannot be typed either |
| Blocking path | `DetallePresupuesto` factory plus `ValidarParaGuardar` raise the typed `DetallePresupuestoInvalidoException` with translation keys; `OrdenServicioService.ValidarItemsYCalcularSubtotal` validates every line and the active part before any write; the four budget methods rethrow with `throw;` and `MostrarError`/`BuscarDetalleInvalido` translate by exception type, never by message text | A price with more than two decimals, an amount outside `decimal(18,2)` or a stored subtotal that does not match `cantidad * precio` is now rejected at write/publish time; no such pre-existing row was reproduced |
| Stock/cost separation | Budget writes touch only `Presupuestos`/`PresupuestoDetalle` and, on emission, the order state plus history; `EmitirConDetalle`/`CrearBorrador` bind `@IdRepuesto<i>` inside their existing transaction. `ReparacionRepository.ConsumirRepuesto` remains the only stock deduction with its own cost row; no reservation, movement, auto-consumption or budget/consumption link was added | Covered by the runtime run: no movement or consumption at create, publish, approve, annul or re-emission; the active zero-stock part was quoted at 0.00; the quoted line stayed fixed after a master price/cost change; repair consumption snapshotted its own cost (333.00) |
| Draft publication | `OrdenServicioService.PublicarBorrador` loads `ListarDetalle` and rejects a stored subtotal that does not match the detail instead of repairing persisted economics | Publication of a draft whose stored price was silently rounded by SQL (only possible outside the current UI) would now be refused |

Documentation drift now aligned: [business-test-catalogue.md](business-test-catalogue.md) row
PRE-01 and the root `IMPLEMENTACION_SERVICIO_TECNICO.md` described the Repuesto budget item type as
reserved infrastructure that is not enabled for emission. Both statements were corrected by
`feat-010` (2026-10-13, documentation only, no runtime test repeated); the feat-007 Checker
observation that first reported them stays as dated historical evidence in
[progress.md](../../progress.md).

## Addressed in feat-008 (verified at runtime 2026-10-13)

`feat-008` is `done`: its Checker verified all **4/4 acceptance criteria and 3/3 scenarios**
through the real WinForms UI with database readback on 2026-10-13 (see the dated feat-008 Checker
section in [progress.md](../../progress.md)). The rows below combine inspected source with that
runtime evidence.

| Item | Implementation inspected | Remaining boundary |
| --- | --- | --- |
| Historical view | [FrmOrdenServicioDetalle](../../PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.cs), `ActualizarAccionesPruebas`, `IntervencionPruebaSeleccionada`, `EsIntervencionActual`, `GrillaPruebasCoherente`, `HayIntervencionAbierta` | The disabled inputs/button, the hidden annulment button, the visible translated reason and the ES/EN texts were exercised at runtime on order 4011: physical clicks, typing and keyboard produced no dialog and no write, and the actions were restored on returning to the current intervention |
| Layout | `CrearAvisoPruebasIntervencion` creates `LBL_PruebaIntervencionAviso` after `InitializeComponent` and takes about 34 logical px from `DGV_Pruebas` (derived from the grid's runtime height, never from fixed coordinates), keeping the grid's bottom edge and the input positions | Rendered at 900x600, at the default size and at 2000x1200 with the grid bottom edge and input positions unchanged; other DPIs and a longer translation were not exercised, and the label wraps inside one reserved band, so a very narrow window could clip it |
| Load coherence | `CargarPruebasDeCombo` clears `_pruebasActuales`, the binding, `CurrentCell` and the row selection before replacing the data and again after binding; `_idIntervencionPruebaCargada` plus `_cargandoPruebas` gate the actions; `CargarTabPruebas` no longer caches a failed read as an empty list | A failed read shows the existing error and leaves that intervention's grid empty with the testing mutations disabled; there is no retry control, so the operator must switch intervention or reload the order |
| Unchanged guards | [OrdenServicioService](../../APPLICATION/Features/Ordenes/OrdenServicioService.cs), `RegistrarPrueba`/`AnularPrueba`/`FinalizarPruebas`, and [ReparacionRepository](../../INFRASTRUCTURE/Features/Reparaciones/ReparacionRepository.cs) / [Prueba](../../DOMAIN/Features/Reparaciones/Prueba.cs) were deliberately not modified in this scope | The service refusals, the Anulada filter and the no-valid-test error keep their meaning, all three observed at runtime (PRU-03 refusal with the state unchanged, the annulled row excluded from finalization, reopen keeping the intervention tests and refinalization); the new historical refusals are UI-only text and were exercised at runtime |

The `Pruebas` tab also keeps the dead helper `ObtenerUltimaPruebaNoAnulada`, which was already
unused at `HEAD`; it was left in place instead of removed inside this scope.

## Addressed in feat-009 (verified at runtime 2026-10-13)

`feat-009` is `done`: its Checker verified all **5/5 acceptance criteria and 4/4 scenarios**
through the real WinForms UI with database readback on 2026-10-13 (see the dated feat-009 Checker
section in [progress.md](../../progress.md)). The rows below combine inspected source with that
runtime evidence.

| Item | Implementation inspected | Remaining boundary |
| --- | --- | --- |
| Single open re-entry | [OrdenServicioRepository](../../INFRASTRUCTURE/Features/Ordenes/OrdenServicioRepository.cs), `PredicadoReingresoAbierto` + `CrearConHistorial` (guard under `TABLOCKX, HOLDLOCK` before any INSERT, sentinel `-1`, repository returns `null`) and `ObtenerReingresoGarantiaAbierto`; [OrdenServicioService](../../APPLICATION/Features/Ordenes/OrdenServicioService.cs), `CrearReingresoGarantia`, pre-checks and converts the sentinel into the typed exception | Every observed refusal came from the service pre-check (six attempts on original 4012 created no order). Two simultaneous creations were **not** run, so the `TABLOCKX, HOLDLOCK` guard and the repository sentinel path isolated from the pre-check remain inspected SQL plus a clean build: the table lock is what the inspected code takes, not a measured parallel-behaviour result |
| Legacy parallel siblings | Nothing migrates, cancels, merges, deletes or resets a pre-existing second open re-entry; the guard only refuses **creation** while a sibling is open | A database that already holds two open siblings keeps them visible and untouched, and re-entry creation stays blocked until they close. No repair of the pre-existing condition was attempted |
| Delivery-cancellation edge | [EntregaRepository](../../INFRASTRUCTURE/Features/Ordenes/EntregaRepository.cs), `CancelarEntregaConTransicion`, repeats the shared predicate inside its existing transaction and returns `-1` without DML; the service shows `Mensaje.GarantiaReaperturaBloqueada` | Exercised at runtime: the cancellation of a warranty re-entry delivery is refused with a business message while another sibling of its original order is open and writes nothing (delivery, state/result and history unchanged), and it succeeds once no sibling is open, preserving the coverage row and the unrelated re-entries and regenerating no warranty on re-delivery |
| Re-entry list | `ListarReingresosGarantia` (no state filter, no evaluation join) plus [FrmOrdenServicioDetalle](../../PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.cs), `CargarReingresosGarantia`/`RefrescarGrillaReingresos`/`ConfigurarColumnasReingresos` and the designer's `DGV_ReingresosGarantia` | ES/EN headers and the empty-list path were observed at runtime (every row matched `fecha_ingreso` and the translated state/result); a failed read and large-DPI rendering were not exercised, and the 900x600 window clips the grid body (see the small-window clipping section below); the date column shows the re-entry's reception timestamp (`fecha_ingreso`), not the evaluation date |
| Conservative boundary | [EstadoEvaluacionGarantia](../../DOMAIN/Features/Garantias/EstadoEvaluacionGarantia.cs) keeps its three states; `Aceptada` closes the evaluation but not the visit, and any unknown or inconsistent order state counts as open | The predicate's behaviour on abnormal data (for example a re-entry cancelled before evaluation, which stays open) is inspected only |

## Small-window clipping (observed, NOT fixed, 2026-10-13)

These are **observed legacy layout limitations, explicitly not fixed** and not described anywhere
as resolved. They were seen through the real UI during the dated feat-006/feat-008/feat-009 Checker
runs recorded in [progress.md](../../progress.md); no designer file or layout was changed.

| Window | Observed clipping | Consequence | Evidence |
| --- | --- | --- | --- |
| Clientes list, MDI child at 900x600 | `PNL_Filtros` keeps its design width, so the `Ver inactivos` checkbox sits outside the visible form | The business message that a duplicate `documento` belongs to an inactive customer tells the operator to tick `Ver inactivos`, but that control is unreachable at this window size; `FrmClientes.Designer.cs` was not modified | feat-006 Checker section in [progress.md](../../progress.md), observation 2 |
| Garantia tab of the order detail, 900x600 at 150% DPI | Scrolling to its maximum reaches the re-entry caption only; the `DGV_ReingresosGarantia` body stays clipped. It renders fully at the default size and at 1400x1000 | The re-entry list cannot be read at that window size even though its data is correct | feat-009 Checker section in [progress.md](../../progress.md), `vis-gar-reentry-list` |
| Pruebas tab of the order detail, 900x600 | The controls below the test grid (description, result radios, observations, register/finalize buttons) are cut off by the form edge | Test registration and finalization are not operable at that size; the historical-selection reason label and the grid itself stay visible | feat-008 Checker section in [progress.md](../../progress.md), observations 1 and 2 |

The shared pattern is a fixed-size MDI child layout, not a data defect of these features. A layout
fix would need its own authorized scope; nothing here claims one.

## Confirmed semantic/report inconsistencies

- `ContarOrdenesPorResultado` includes ListoRetiro only, not delivered history. A broad
  final-result report would need a separately approved definition change.
- UI `TasaGarantia` calls `TasaAceptacionGarantia`, not warranty re-entry/delivered-order rate.
  Approval rate excludes pending budgets from the denominator by design of current code.
- [GarantiaRepository](../../INFRASTRUCTURE/Features/Garantias/GarantiaRepository.cs),
  `ObtenerVigente`, compares current timestamp with end timestamp and ignores start date.
  Calendar-day inclusive coverage is intended policy, not the implemented date predicate.
- [EntregaRepository](../../INFRASTRUCTURE/Features/Ordenes/EntregaRepository.cs),
  `CancelarEntregaConTransicion`, still leaves the coverage row and unrelated re-entries in
  place, and since feat-009 it is refused while another re-entry of the same original order is
  open (see the feat-009 section above); `CrearConTransicion` does not regenerate an existing
  warranty on re-delivery.
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
