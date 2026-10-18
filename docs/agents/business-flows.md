# Preserve workflow transitions and rollback windows

**Verified implementation:** this guide traces source guards and SQL batches, not fresh runtime
executions. All order use cases below are owned by
[OrdenServicioService](../../APPLICATION/Features/Ordenes/OrdenServicioService.cs);
order transition guards are in [OrdenServicio](../../DOMAIN/Features/Ordenes/OrdenServicio.cs).
Method names in the tables are source anchors. There is no free-form state editing workflow.

## Masters, reception, and diagnosis

- [Cliente](../../DOMAIN/Features/Clientes/Cliente.cs), `CrearNuevo`, requires name, surname,
  document, and phone. Email/address/observations are optional.
- **Implemented (feat-006, source verified; runtime acceptance pending):** documento is a unique
  customer identity across active **and** inactive rows. `CrearNuevo` still only validates and
  trims; [ClienteService](../../APPLICATION/Features/Clientes/ClienteService.cs), `Crear`/`Modificar`,
  pre-check with `ObtenerConflictoDocumento` and reject a duplicate with
  `ClienteDocumentoDuplicadoException`, and
  [ClienteRepository](../../INFRASTRUCTURE/Features/Clientes/ClienteRepository.cs),
  `Agregar`/`Modificar`, repeat the ownership check under `TABLOCKX,HOLDLOCK` inside the write
  batch, so the guard is not only a UI/service check. Editing a row that keeps its own unchanged
  documento still succeeds, which grandfathers pre-existing duplicate groups instead of rewriting
  them. Telefono and email remain non-unique and freely shareable.
- [ClienteService](../../APPLICATION/Features/Clientes/ClienteService.cs), `Desactivar`, only
  deactivates the customer: the inactive row keeps reserving its documento, and reactivating it is
  the way to reuse that identity. Customer and equipment active flags are independent, not cascading.
- [EquipoService](../../APPLICATION/Features/Equipos/EquipoService.cs),
  `ValidarReferenciasExistentesYActivas`, validates customer/type/brand for creation/editing.
- `CrearOrden` requires an existing active customer and active equipment belonging to it.
  The order stores both FKs: its historical customer is not recalculated when equipment ownership changes.
- `ListarTecnicosElegibles`/`ValidarTecnicoElegible` require an active user with effective
  `ORDENES_EDITAR`. Receptionists/administrators may qualify; `Rol tecnico` is not an exclusive filter.
- `AsignarTecnico` rejects delivered orders. `ModificarRecepcion` rejects delivered orders,
  but does not itself invoke the standard session guard used by most mutations.
- `IniciarDiagnostico`: Recibido + assigned technician -> EnDiagnostico.
- `FinalizarDiagnostico`: one diagnosis at most (0..1 per order). Reparable Normal ->
  PendientePresupuesto; reparable Garantia -> PendienteEvaluacionGarantia;
  non-reparable -> ListoRetiro + NoReparable.

## Normal order happy path

| Service method | Preconditions and outcome |
| --- | --- |
| `CrearOrden` | Creates Recibido + reception data + initial HistorialOrdenes |
| `IniciarDiagnostico`, `FinalizarDiagnostico` | Assigned technician; one diagnosis; reparable -> PendientePresupuesto |
| `EmitirPresupuesto` / `PublicarBorrador` | Reparable diagnosis + eligible Original; -> EsperandoRespuesta |
| `RegistrarAprobacionPresupuesto` | Pending Original; records optional response medium; -> AutorizadoReparacion |
| `IniciarReparacion` | Eligible assigned technician; -> EnReparacion + intervention |
| `FinalizarReparacion` | Open intervention + work performed; -> EnPruebas |
| `RegistrarPrueba` | Current completed intervention, no open intervention; persists only |
| `FinalizarPruebas` | At least one valid current test; all approved -> ListoRetiro + Reparado |
| `EntregarOrden` | ListoRetiro; records receiver/user/time, -> Entregado |

`RegistrarRechazoPresupuesto`/`RegistrarRechazoAdicional` produce ListoRetiro +
PresupuestoRechazado, with required reason and optional response medium.
The response medium is stored but not required by `Presupuesto.Aprobar`/`Rechazar` in
[Presupuesto](../../DOMAIN/Features/Ordenes/Presupuesto.cs).
`CancelarOrden` accepts only Recibido, EnDiagnostico, PendientePresupuesto,
EsperandoRespuesta, or AutorizadoReparacion; -> ListoRetiro + Cancelado.
It cannot cancel EnReparacion/EnPruebas/ListoRetiro/Entregado or pending warranty evaluation.
The repository's `CerrarAbiertaYCancelar` remains present but is not the current service flow.

## Budgets: separate documents, not generic versions

One order has many budgets. A filtered unique index permits only one **non-Anulado Original**;
Adicional documents coexist. Approved economics are not edited in place.
`CalcularMontoAutorizado` sums all Aprobado totals, Original and Adicional; the order has no
separate persisted authorized-total column. Individual budget totals are persisted snapshots.
See [PresupuestoRepository](../../INFRASTRUCTURE/Features/Ordenes/PresupuestoRepository.cs),
`Inicializar`, `CalcularMontoAutorizado`, `HasActividadPosterior`.

| Action | Window and effect |
| --- | --- |
| `CrearBorrador` | PendientePresupuesto; validate Original uniqueness or approved Original for an Adicional |
| `PublicarBorrador` | Borrador -> Pendiente; order -> EsperandoRespuesta; Original needs reparable diagnosis; the stored detail is revalidated (active part reference, quantity/price/subtotal) and an inconsistent stored subtotal is rejected instead of rewritten |
| `EliminarBorrador` | Physical deletion of draft and detail only; not deletion of emitted history |
| `AnularPresupuesto` pending | Required reason; EsperandoRespuesta -> PendientePresupuesto |
| Annul approved Original | Only AutorizadoReparacion, with no repair interventions started |
| Annul approved Adicional | Reject if repairs, repair movements, or tests occur after `fecha_respuesta`; eligible order state must also permit reverting to PendientePresupuesto |
| Annul rejected Original | ListoRetiro + PresupuestoRechazado, no Entrega; no other non-annulled Original and no later non-annulled Adicional |
| Annul rejected Adicional | ListoRetiro + PresupuestoRechazado, no Entrega; no later non-annulled Adicional, including drafts |

Rejected annulment -> PendientePresupuesto + explicit NULL result; previous result observation
is combined with the annul reason (`CombineObservacion`), not silently lost.
Inspect both `ValidarAnulacionRechazado*` and `AnularRechazadoConTransicion`.
The Additional validator does not explicitly re-check an approved parent; do not turn its
comment into a stronger guarantee than the code.

### Additional request pause/resume

`SolicitarAdicional` needs approved Original, required reason, and no pending additional.
From AutorizadoReparacion/EnReparacion/EnPruebas it pauses to PendientePresupuesto.
`EmitirAdicional` or publishing its draft moves to EsperandoRespuesta.
Approval resumes AutorizadoReparacion if that was the origin; otherwise EnReparacion
(including an EnPruebas origin, which must undergo repair/retest rather than instantly pass).
`CancelarSolicitudAdicional` restores the origin, physically removes additional drafts,
and blocks cancellation if any Pendiente additional exists; previously approved/rejected
additions do not block it. It does not convert a pending budget to Rechazado.

**Confirmed caveat:** `TryObtenerOrigenSolicitud` searches history backwards and excludes
observations beginning `Presupuesto anulado`. The origin is inferred from state/history text,
not a dedicated request ID. Changing those history strings can change workflow behavior.

### Price is not cost

Budget rows contain selling price/subtotal; consumption rows snapshot part cost.
`ValidarItemsYCalcularSubtotal` accepts ManoObra, Servicio and Repuesto and validates **every**
line through [DetallePresupuesto](../../DOMAIN/Features/Ordenes/DetallePresupuesto.cs),
`ValidarParaGuardar` (positive `cantidad`, non-negative `precio` with at most two decimals and a
`decimal(18,2)`-representable amount, `subtotal = cantidad * precio`) before any budget write.
A Repuesto item also needs a `PresupuestoDetalle.id_repuesto` that
[RepuestoRepository](../../INFRASTRUCTURE/Features/Repuestos/RepuestoRepository.cs),
`ObtenerPorId`, resolves to an **active** catalogue part; available stock is not required, so an
active part with zero stock stays quotable. Selecting a part in
[FrmOrdenServicioDetalle](../../PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.cs),
`CBO_PRepuesto_SelectedIndexChanged`, prefills the editable unit price from `PrecioReferencia` and
snapshots `codigo - descripcion` into the line; the operator's value is the commercial snapshot
and the master cost is neither substituted nor re-read later.

**Implemented rule, runtime acceptance pending (feat-007 in-progress):** saving, emitting,
publishing, approving or annulling a budget line neither reserves nor moves stock. Stock changes
only through explicit repair consumption (`ReparacionRepository.ConsumirRepuesto`, with its own
`id_consumo` and cost snapshot) or a purchase/stock-adjustment path. `PresupuestoDetalle` and
`ReparacionRepuesto` remain separate: there is no automatic budget-detail-to-consumption link,
reservation, reconciliation or auto-consumption. Annulment keeps the previously quoted part,
quantity and price with no stock reversal and re-emission stores newly entered snapshots; the
rollback windows in the table above are unchanged. Zero-price lines are allowed
(`precio >= 0`).
There is no general budget revision/version graph.

## Repairs, consumption, and explicit testing

An order has multiple numbered interventions. `IniciarReparacion` creates the first from
AutorizadoReparacion or a new one from EnReparacion with no open intervention.
`ConsumirRepuesto`/`QuitarConsumo` require an open intervention and EnReparacion.
Each consumption creates a separate `id_consumo` row with the then-current master cost.
Returns consume oldest IDs first, deleting fully returned rows or reducing their quantities;
remaining rows keep their own costs. UI grouping by part/cost does not change persistence.
Stock mutation + consumption/return + movement are transactional with stock locks.
Anchor: [ReparacionRepository](../../INFRASTRUCTURE/Features/Reparaciones/ReparacionRepository.cs),
`ConsumirRepuesto`, `DevolverConsumo`; this is not proof of parallel correctness.

- `RegistrarPrueba`: only EnPruebas and latest finished intervention; does not change order state.
- `AnularPrueba`: any non-Anulada test of the current intervention while EnPruebas,
  with reason, timestamp, and acting user; not only the most recent test.
- `FinalizarPruebas`: ignores Anulada; no valid test is an error; all valid Aprobada ->
  ListoRetiro/Reparado; any RequiereRevision -> EnReparacion/NULL result.
- `ReabrirPruebas`: only ListoRetiro + Reparado and required reason -> EnPruebas/NULL result.
- `CancelarEntrega`: required reason; physically deletes Entrega. Repaired delivery ->
  EnPruebas/NULL; other results -> ListoRetiro retaining their result.

**Implemented (feat-008, source verified; runtime acceptance pending):** the Pruebas tab is a
selector over the order's interventions and only the **current** intervention - the latest by
number, `ObtenerIdUltimaIntervencion` - admits mutations. In
[FrmOrdenServicioDetalle](../../PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.cs),
`ActualizarAccionesPruebas` resolves the combo through `IntervencionPruebaSeleccionada`, requires
`EsIntervencionActual` plus `GrillaPruebasCoherente` (the intervention whose tests really loaded),
and only then enables `BTN_RegistrarPrueba`, the description and observations boxes, the result
radios and `BTN_FinalizarPruebas`. An earlier (non-current) intervention leaves those controls
disabled, keeps `BTN_AnularPrueba` hidden through the existing `ActualizarBotonAnularPrueba`
restriction (EnPruebas, edit permission, selected non-Anulada row of the latest intervention) and
shows the `OrdenDetalle.PruebasIntervencionAnterior` reason, so the misleading
`Mensaje.SeleccioneRegistro` answer is not reachable from that view. The intervention combo itself
is gated only by the view permission, so a historical selection never disables navigation;
returning to the current intervention restores the eligible controls and keeps unsaved
registration text, which is a draft for the current intervention, never historical test data.
`CargarPruebasDeCombo` clears `_pruebasActuales`, the previous binding, `CurrentCell` and the row
selection before replacing the data and again after binding, records `_idIntervencionPruebaCargada`
and recomputes the actions, so the grid, its selection and the enabled actions describe one
intervention after every switch; while loading or after a failed readback the testing mutations
stay disabled. `CargarTabPruebas` no longer caches a failed read as an empty list, so the selected
intervention retries through the service and a failure leaves that grid empty with the existing
error message. The handlers refuse a missing or historical selection with
`Mensaje.PruebaIntervencionObligatoria` and `Mensaje.PruebasSoloIntervencionActual`, and
`BTN_FinalizarPruebas_Click` re-checks current-view eligibility before its confirmation dialog.

**Service guards unchanged by feat-008:**
[OrdenServicioService](../../APPLICATION/Features/Ordenes/OrdenServicioService.cs),
`RegistrarPrueba` still requires EnPruebas, no open intervention, a finished latest intervention
and that intervention as the target (`Solo se puede registrar la prueba de la ultima intervencion
finalizada.`); `AnularPrueba` still requires a non-Anulada test of the latest intervention;
`FinalizarPruebas` still ignores Anulada rows, rejects a finalization with no valid test and
returns ListoRetiro/Reparado or EnReparacion/NULL. `ReparacionRepository` and
[Prueba](../../DOMAIN/Features/Reparaciones/Prueba.cs) were intentionally left untouched. The
historical-intervention refusals are UI-only text; `Mensaje.SeleccioneRegistro` keeps its other
uses, and registering a test still does not finish testing.

**Confirmed caveat:** delivery cancellation neither annuls/deletes the generated warranty nor
blocks existing re-entries. Re-delivery skips warranty insertion if that order already has one,
so it does not refresh the original warranty dates. **Updated by feat-009 (source verified,
runtime acceptance pending):** cancelling the delivery of a warranty re-entry is now refused
while a sibling re-entry of the same original order is still open, and coverage rows are still
neither annulled nor deleted by that cancellation. Read
[EntregaRepository](../../INFRASTRUCTURE/Features/Ordenes/EntregaRepository.cs),
`CrearConTransicion`, `CancelarEntregaConTransicion` before extending this coexistence.

## Purchases and suppliers

[CompraService](../../APPLICATION/Features/Compras/CompraService.cs), `CrearBorrador`,
`AgregarItem`, `QuitarItem`, `Confirmar`, `Cancelar`, `Anular`:
drafts persist detail but change no stock; confirmation adds stock, records Compra movements,
and sets each part's latest master cost. Cancellation retains header/detail as Cancelada.
Confirmed annulment needs a reason and stock coverage; -> Cancelada + linked
AjusteNegativo movements (`id_compra` and purchase ID in observation).
It does **not** restore the previous master cost.

**Fixed (feat-005):** [CompraRepository](../../INFRASTRUCTURE/Features/Compras/CompraRepository.cs),
`AnularConfirmadaConStock`, validates the summed quantity per part (`SUM(cantidad)` grouped by
`id_repuesto`) before reversing, so repeated-part rows cannot over-reverse stock. The reversal
cursor still writes one movement per detail row.
[ProveedorService](../../APPLICATION/Features/Proveedores/ProveedorService.cs), `Desactivar`,
is soft deletion; purchase history remains.

## Warranty and separate re-entry

Automatic creation occurs during delivery only for Normal + Reparado with an approved Original
whose `dias_garantia > 0`; no warranty for other results, zero days, or warranty-order delivery.
Dates are delivery timestamp and `DATEADD(day, days, delivery timestamp)`.
The delivery batch also requires the Garantias table to exist and skips insertion if any
warranty row already exists for that original, including an annulled row. Normal startup
initializes the table before opening FrmPrincipal; a direct service/harness call that bypasses
startup must not assume automatic coverage was created.
**Intended policy:** a calendar-date coverage range. **Current implementation:**
[GarantiaRepository](../../INFRASTRUCTURE/Features/Garantias/GarantiaRepository.cs),
`ObtenerVigente`, checks `anulada = 0 AND GETDATE() <= fecha_fin`, without a start-date check.
UI also compares DateTime.Now, so full-calendar-day inclusion is not guaranteed.

`CrearReingresoGarantia` creates a new Recibido order of Tipo Garantia with origin FK,
same equipment, active current owner customer, and existing valid warranty; original untouched.
Since feat-009 it also refuses creation while a sibling re-entry of the same original order is
open (see the single-open-re-entry section below).
UI limits creation to a Normal original; service does not explicitly check original TipoOrden
or equipment Activo. Do not claim the normal-reception guard is identical here.
`BTN_CrearReingreso_Click` passes the original order's historical `IdCliente`, not a selectable
current owner. If equipment ownership changed, that UI path can fail the service's current-owner
guard; the service accepting the current owner is not proof that the form supplies it.
Reparable diagnosis registers Pendiente evaluation, then `EvaluarReingreso` (feat-005 takes an
explicit `continuarPago` flag supplied by the UI destination dialog):

| Decision | Destination |
| --- | --- |
| Accepted | AutorizadoReparacion without commercial budget |
| Rejected, `continuarPago = true` (UI "Reparacion paga") | PendientePresupuesto, ordinary paid-budget flow |
| Rejected, `continuarPago = false` (UI "No continua / retiro") | ListoRetiro + GarantiaNoCubierta |

The reason text no longer decides the branch; the UI asks with two buttons and passes the
decision. Acceptance needs ORDENES_EDITAR in UI; rejection needs
PRESUPUESTOS_DECIDIR, despite the broader service comment.
Accepted warranties have no approved Original in the normal path, so commercial additions
are blocked by the approved-Original prerequisite, not a universal TipoGarantia ban.
Diagnosis/state transition and pending evaluation creation are separate transactions;
evaluation decision and order transition are also separate, with partial-success risk.

### One open warranty re-entry at a time, plus its list (feat-009)

**Implemented policy, source verified; runtime acceptance pending.** A warranty re-entry is a
visit, not a second coverage document: the coverage row in `Garantias` stays 0..1 per original
order and every re-entry is a new `OrdenesServicio` row of type `Garantia` whose
`id_orden_origen` points back at the original. Several re-entries per warranty are acceptable,
but only one may be **open** at a time.

The boundary lives in one shared predicate,
[OrdenServicioRepository](../../INFRASTRUCTURE/Features/Ordenes/OrdenServicioRepository.cs),
`PredicadoReingresoAbierto`: a re-entry is **closed** when its order state is `Entregado`, or
when its evaluation is `Rechazada` **and** the order has already committed to a
post-evaluation destination (`PendientePresupuesto`, `EsperandoRespuesta`,
`AutorizadoReparacion`, `EnReparacion`, `EnPruebas`, `ListoRetiro`). Everything else is
**open**, including an accepted `ListoRetiro`/`Reparado` re-entry, which stays open until it
is delivered, and any unknown or inconsistent state, which is treated conservatively as open.
`EstadoEvaluacionGarantia.Aceptada` therefore closes the evaluation, not the visit.

- `CrearReingresoGarantia` pre-checks that predicate through
  `ObtenerReingresoGarantiaAbierto` for a friendly refusal and then relies on the guarded
  write. `OrdenServicioRepository.CrearConHistorial` repeats the same predicate under
  `TABLOCKX,HOLDLOCK` **before any INSERT in the same batch**; on conflict it inserts nothing,
  leaves the original order untouched and returns `-1`, and the service converts that sentinel
  into `ReingresoGarantiaAbiertoException` (`Mensaje.GarantiaReingresoAbierto`), which the
  form translates by exception type. The sentinel is not a SQL error and no partial order is
  written.
- `BTN_CrearReingreso_Click` runs the same pre-check and shows the translated business warning
  **before** prompting for the problem text; the repository stays authoritative for a stale or
  racing attempt, and after a conflict the form refreshes the list.
- **Reverse edge:** cancelling the delivery of a re-entry would return it to a non-terminal
  state. [EntregaRepository](../../INFRASTRUCTURE/Features/Ordenes/EntregaRepository.cs),
  `CancelarEntregaConTransicion`, therefore repeats the same guard (the shared predicate text)
  inside its existing delete/update/history transaction and returns `-1` without DML while a
  sibling is open; the service shows `Mensaje.GarantiaReaperturaBloqueada`.
- **Re-entry list:** `ListarReingresosGarantia` returns every `Garantia` order whose
  `id_orden_origen` is the shown order, ordered by reception timestamp then id, including
  delivered re-entries and re-entries without evaluation. The Garantia tab renders it in the
  permanent read-only `DGV_ReingresosGarantia` (number, **reception** date, translated state
  and translated result) through
  [FrmOrdenServicioDetalle](../../PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.cs),
  `CargarReingresosGarantia`, `RefrescarGrillaReingresos` and
  `ConfigurarColumnasReingresos`, with the `OrdenDetalle.ReingresosGarantia` caption and the
  shared `Columna.Numero`/`Columna.Fecha`/`Columna.Estado`/`Columna.Resultado` headers.
  An order without re-entries shows an empty list and no error; a failed read shows the error
  instead of a fabricated empty result.

**Not verified (runtime acceptance pending):** parallel creation, legacy databases that already
hold two open siblings (they stay visible and untouched; further creation is blocked until they
close), the reception-timestamp meaning of the date column, and the ES/EN rendering at other
window sizes.

### Original order read-only once it has warranty re-entries (feat-011)

**Implemented policy, source verified; runtime acceptance pending.** Two different warranty rules
coexist and must never be conflated:

| Rule | Predicate | Effect |
| --- | --- | --- |
| Only one **open** re-entry (feat-009) | `PredicadoReingresoAbierto`, state and evaluation aware | Refuses creating another re-entry while a sibling is open, and refuses cancelling the delivery of a re-entry while a sibling is open |
| Original order **read-only** (feat-011) | `TieneReingresosGarantia`, the pure **existence** of a child order with `id_orden_origen` = the original and `tipo_orden = 'Garantia'` | Refuses every mutation of that order - delivery cancellation included - regardless of the child's state, of the coverage validity or of the child being closed |

Once the first re-entry exists the original simply stays as it ended; the freeze is not a state and
never rewrites the order. **Still allowed:** inspection (header, history, budgets, repairs, tests,
delivery, warranty and the re-entry grid), `Ver original`, and creating another re-entry, which
keeps the feat-009 single-open precondition and its own pre-check. **Orders without any warranty
re-entry keep the previous behaviour**, including ENT-03 delivery cancellation and the delivered
read-only lock.

Guard inventory in [OrdenServicioService](../../APPLICATION/Features/Ordenes/OrdenServicioService.cs):
every existing-order mutator loads its order through the new `ObtenerOrdenParaModificar`
(existence plus `EsSoloConsultaPorReingresos`, the repository read) and refuses with
`OrdenOriginalSoloConsultaException` before any domain mutation, repository write or audit:
`AsignarTecnico`, `IniciarDiagnostico`, `FinalizarDiagnostico`, `EmitirPresupuesto`,
`RegistrarAprobacionPresupuesto`, `RegistrarRechazoPresupuesto`, `SolicitarAdicional`,
`EmitirAdicional`, `RegistrarAprobacionAdicional`, `RegistrarRechazoAdicional`, `CrearBorrador`,
`PublicarBorrador`, `EliminarBorrador` (which previously did not load its owning order at all),
`AnularPresupuesto` (before the document or the order is touched), `CancelarSolicitudAdicional`,
`QuitarConsumo`, `CancelarOrden`, `EntregarOrden`, `CancelarEntrega`, `ModificarRecepcion`,
`IniciarReparacion`, `ConsumirRepuesto`, `FinalizarReparacion`, `RegistrarPrueba`,
`FinalizarPruebas`, `ReabrirPruebas` and `AnularPrueba`. `EvaluarReingreso` guards the **target**
re-entry only; reading its frozen original for coherence stays allowed. `CrearOrden`,
`CrearReingresoGarantia` (new child, never a mutation of the original) and every read
(`ObtenerPorId`, `ObtenerDiagnostico`, `ObtenerPresupuesto*`, `Listar*`, `ObtenerEntrega`,
`PuedeCancelarSolicitud`, `ListarHistorial`, the re-entry reads) keep using
`ObtenerOrdenExistente`. `GarantiaService.Crear` and `GarantiaService.Anular` validate their target
original before writing coverage. Guarded methods rethrow `ReglaNegocioException` with `throw;`
so the concrete type (and its `ClaveMensaje`) survives to the form.

**Persistence edge:** the guards above are the friendly, application-level refusal.
[OrdenServicioRepository](../../INFRASTRUCTURE/Features/Ordenes/OrdenServicioRepository.cs)
and [EntregaRepository](../../INFRASTRUCTURE/Features/Ordenes/EntregaRepository.cs) repeat the
existence test inside the locked batches that can bypass a service call
(`CancelarEntregaConTransicion`, `CambiarEstadoConHistorial`, `ReabrirPruebasConTransicion`), and
`CrearConHistorial` re-reads the origin eligibility before inserting a re-entry. See
[persistence](persistence.md) for the sentinels and the lock invariant.

**UI affordance (feat-011):** `FrmOrdenServicioDetalle` reads the predicate together with the order
in `CargarOrden` (a failed read shows the error and closes the detail instead of defaulting to
editable), resets it in `CargarModoNuevo`, shows the wrapping `LBL_OrdenSoloConsulta` banner above
the tabs and intersects every mutation in `AplicarPermisosDetalle`,
`ActualizarAccionesPruebas`, `ActualizarBotonAnularPrueba` and `AplicarPermisosGarantia` with the
freeze. Tabs, read-only grids, history, `Ver original`, `Cerrar` and the existing-budget selector
stay usable; `CargarSelectorPresupuesto` omits the "new document" pseudo entries while frozen.
`BTN_CrearReingreso` deliberately keeps its permission, coverage and single-open rules.
`FrmOrdenesServicio` revalidates the selected row before enabling `Cancelar`/`Entregar` and shows
`Mensaje.OrdenOriginalSoloConsulta` from a fresh check before its prompts. There is still no
cancel-delivery action in the orders list; cancellation of a delivery is reached through the order
detail.

## Dashboard and report definitions

Source: [ReporteRepository](../../INFRASTRUCTURE/Features/Reportes/ReporteRepository.cs), named
queries, [ReporteService](../../APPLICATION/Features/Reportes/ReporteService.cs), rate methods,
and [FrmReportes](../../PRESENTATION/Forms/Reportes/FrmReportes.cs), `EjecutarReporte`.

The eleven selector IDs are `OrdenesEstado`, `OrdenesResultado`, `ReparacionesTecnico`,
`TiempoPromedio`, `TasaAprobacion`, `RepuestosMasUtilizados`, `ComprasProveedor`, `Reingresos`,
`EvaluacionesEstado`, `TasaGarantia`, and `MontoAprobados` (`TiposReporte` in FrmReportes).
Low-stock data is an additional service query/dashboard KPI, not a twelfth selector choice.

| Choice / KPI | Actual definition |
| --- | --- |
| Dashboard open | All non-Entregado, including ListoRetiro and cancelled/rejected outcomes awaiting delivery |
| Dashboard repair | EnReparacion + EnPruebas, not merely AutorizadoReparacion |
| Other dashboard KPIs | EsperandoRespuesta; ListoRetiro; non-delivered Tipo Garantia; active stock <= minimum |
| Orders by state | Non-delivered, filtered by reception timestamp |
| Orders by result | Only ListoRetiro with nonempty result; delivered outcomes are excluded |
| Repairs by technician | Intervention count by start date, not distinct repaired orders |
| Average reception/delivery | Delivered orders; AVG of SQL DATEDIFF(day), cast to float; reception-date filter |
| Budget state counts / approval rate | Pending/approved/rejected by emission; rate approved/(approved+rejected), zero if no decisions |
| Most-used parts | Sum remaining consumption quantities by intervention start, after returns |
| Purchases by supplier | Confirmada count and total, purchase date filter |
| Warranty re-entry count / TasaGarantia | Tipo Garantia reception count; rate is accepted/(accepted+rejected) evaluations, not re-entry/delivery rate |
| Approved budget amount | Sum approved totals across both types, emission filter; not billing or collection |
| Low stock | Active parts with stock <= minimum; no date-range history |

**Fixed (feat-005):** report queries use an exclusive upper bound
(`< DATEADD(day, 1, @Hasta)`), so the whole selected final day is included. The UI keeps
sending calendar dates and the desde > hasta validation is unchanged.
