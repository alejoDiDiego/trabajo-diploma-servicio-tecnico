# Find the use case before changing the implementation

This application follows an electronics repair job from reception to delivery, with stock,
purchasing, warranty follow-up, and operational reporting. This is a source navigation map,
not an operator manual or a newly executed desktop test. Read [root rules](../../AGENTS.md)
before running anything; startup writes to the configured database before login.

## First reading path

1. Locate the user's action in the table below.
2. Follow its form handler into the application service, domain guard, and repository batch.
3. Read [business flows](business-flows.md) for transitions and rollback windows, then
   [persistence](persistence.md) for constraints and transaction boundaries.
4. Check [known limitations](known-limitations.md) before describing an outcome as guaranteed.

## Product journey and entry points

[FrmPrincipal](../../PRESENTATION/Forms/FrmPrincipal.cs) opens feature forms through its
`TSMI_*_Click` handlers. Menus depend on effective permissions, not only seeded role names.
The form names below are source names; displayed labels depend on the current language.

| User objective | Start at the UI | Application owner / next guide |
| --- | --- | --- |
| Maintain customer contact data and equipment ownership | [FrmClientes](../../PRESENTATION/Forms/Clientes/FrmClientes.cs), [FrmEquipos](../../PRESENTATION/Forms/Equipos/FrmEquipos.cs) | ClienteService / EquipoService; business flows, masters and reception |
| Maintain equipment types and brands | [FrmTiposEquipo](../../PRESENTATION/Forms/Catalogos/FrmTiposEquipo.cs), [FrmMarcas](../../PRESENTATION/Forms/Catalogos/FrmMarcas.cs) | TipoEquipoService / MarcaService; architecture feature ownership |
| Receive a job and inspect its history | [FrmOrdenesServicio](../../PRESENTATION/Forms/Ordenes/FrmOrdenesServicio.cs), [FrmOrdenServicioDetalle](../../PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.cs) | OrdenServicioService; business flows, reception and diagnosis |
| Assign a technician, diagnose, and obtain budget approval | FrmOrdenServicioDetalle | OrdenServicioService; business flows, normal path and Original/Adicional documents |
| Repair, consume/return parts, record tests, and finalize testing | FrmOrdenServicioDetalle | OrdenServicioService + ReparacionRepository; business flows, repairs and explicit testing |
| Deliver or reverse a permitted delivery/testing action | FrmOrdenServicioDetalle | OrdenServicioService + EntregaRepository; business flows, rollback windows and warranty coexistence |
| Maintain parts and inspect/adjust stock | [FrmRepuestos](../../PRESENTATION/Forms/Repuestos/FrmRepuestos.cs), [FrmMovimientosStock](../../PRESENTATION/Forms/Repuestos/FrmMovimientosStock.cs), [FrmAjusteStock](../../PRESENTATION/Forms/Repuestos/FrmAjusteStock.cs) | RepuestoService; persistence, cost snapshots and movement history |
| Maintain suppliers and receive purchases into stock | [FrmProveedores](../../PRESENTATION/Forms/Proveedores/FrmProveedores.cs), [FrmCompras](../../PRESENTATION/Forms/Compras/FrmCompras.cs), [FrmCompraDetalle](../../PRESENTATION/Forms/Compras/FrmCompraDetalle.cs) | ProveedorService / CompraService; business flows, purchases and annulment limits |
| Inspect coverage, create a separate re-entry, and evaluate it | [FrmGarantias](../../PRESENTATION/Forms/Garantias/FrmGarantias.cs), FrmOrdenServicioDetalle | GarantiaService for coverage reads; OrdenServicioService for re-entry/evaluation; business flows, warranty |
| Monitor current work and query operational summaries | [FrmDashboard](../../PRESENTATION/Forms/Dashboard/FrmDashboard.cs), [FrmReportes](../../PRESENTATION/Forms/Reportes/FrmReportes.cs) | ReporteService; business flows, actual query definitions |
| Manage access, translations, audit, and integrity | FrmPrincipal and its administration forms | [Security and localization](security-and-localization.md); these are shared foundations, not new business checkpoints |

The normal reparable journey is reception -> diagnosis -> budget decision -> repair -> tests
-> explicit test finalization -> ready for pickup -> delivery. Non-reparable, rejected,
cancelled, and warranty decisions branch away from that path. A result and an order state
are different fields: cancelled work can still await pickup, and delivered work keeps its result.
Do not replace the guarded actions with a generic state dropdown.

## Three distinctions that prevent incorrect changes

- **Identity versus eligibility:** seeded families are convenience groupings. Technician
  eligibility is active user + effective `ORDENES_EDITAR`; acting-user authorization is
  largely enforced in forms and must be inspected separately.
- **Commercial amount versus internal cost:** approved budgets are commitments, not payments.
  Purchase/master cost and per-consumption snapshots serve stock/repair costing, not billing.
- **History versus a fresh job:** warranty re-entry creates another order for the same equipment.
  It does not reset the delivered original or renew its warranty. Cancellation and annulment
  are specific guarded actions, not generic undo or deletion of all dependent history.

## Boundaries and verification

The [index](README.md) maps the five implemented checkpoints. No invoicing, collections,
cash/AFIP, mobile client, AI diagnosis, or export feature should be inferred from this journey.
Six dashboard KPIs and eleven report choices do not imply eleven independent business modules.
Read the actual query definitions, especially date boundaries and warranty-rate denominators.

For an authorized behavior change, continue with [development and testing](development-and-testing.md).
Source coverage is not runtime certification: live schema, numeric cultures, layout/DPI,
physical interaction, and parallel execution remain separate verification tasks.
