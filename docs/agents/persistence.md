# Understand persistence before changing SQL

**Verified implementation:** the table map below is inferred from repository DDL, not queried
from a live database. A legacy deployment may differ if initialization failed or has not run.
There are 20 business tables and 10 technical tables, excluding temporary seed tables.
No business-table CHECK constraint enforcing states, quantities, or monetary rules was found.
`WITH CHECK` on FK installation is not a business CHECK constraint.

## SQL execution and startup

[SqlHelper](../../INFRASTRUCTURE/SQLHelper.cs), `ExecuteTransaction`, opens a connection,
begins a SqlTransaction, executes a scalar batch, commits, then converts its return value.
Exceptions attempt rollback while preserving the original exception. `ExecuteQuery` loads a DataTable.
Repositories parameterize inputs and hydrate domain objects; there is no ORM/repository-interface layer.
Default repository constructors resolve connection configuration named `UrlDB`.

[Program](../../PRESENTATION/Program.cs), `Main`, runs initialization in this order:
integrity, languages, permissions, audit, translation change control, users/assignments,
equipment types, brands, customers, equipment, orders/history/diagnosis/budgets/delivery,
parts/movements, suppliers, purchases, repairs/tests, warranties/evaluations, default language,
then FrmPrincipal. Reports have no initializer and do not add tables.
The SQL Server **database must already exist**; startup creates/migrates its tables, not the database.
Initialization is not a read-only smoke test and not one global transaction.

DDL uses existence/column/index guards, but guards do not certify migration success.
[PresupuestoRepository](../../INFRASTRUCTURE/Features/Ordenes/PresupuestoRepository.cs),
`Inicializar`, separates column additions from the filtered index batch.
[ReparacionRepository](../../INFRASTRUCTURE/Features/Reparaciones/ReparacionRepository.cs),
`Inicializar`, adds `id_consumo` first, then migrates the legacy composite PK/index in a second batch.
Keep these phases: SQL Server can compile column references before an earlier ALTER executes
and raise SQL error 207 on legacy schemas. The phases commit separately.
[ClienteRepository](../../INFRASTRUCTURE/Features/Clientes/ClienteRepository.cs), `Inicializar`,
also uses a second batch after the defensive ALTERs: it installs `UX_Clientes_Documento` only when
the index is absent **and** no `GROUP BY documento HAVING COUNT(*) > 1` group exists, inactive rows
included. A database that already holds duplicate documents keeps starting normally: the index
stays absent there, nothing is deleted, merged or rewritten, and the transactional guard described
below remains the enforcement path.
The two old SQL files under [Scripts](../../INFRASTRUCTURE/Scripts) are project `None` items;
current initialization lives in repositories, not execution of those scripts.

## Business tables: keys and relationships

Every `id_*` PK listed here is an integer identity. FK columns use the referenced table's listed PK.
`?` indicates a nullable FK. Other columns are intentionally omitted: read each owner's
`Inicializar` before a schema change. Most owners match the table feature; exceptions are explicit.

| Table | PK | All current declared FKs | Meaningful UNIQUE constraint |
| --- | --- | --- | --- |
| TiposEquipo | id_tipo_equipo | None | nombre |
| Marcas | id_marca | None | nombre |
| Clientes | id_cliente | None | UX_Clientes_Documento on documento, unfiltered (active + inactive); installed only when no duplicate group exists |
| Equipos | id_equipo | id_cliente -> Clientes; id_tipo_equipo -> TiposEquipo; id_marca -> Marcas | None; no serial uniqueness claim |
| OrdenesServicio | id_orden | id_cliente -> Clientes; id_equipo -> Equipos; id_tecnico_asignado? -> Usuarios; id_orden_origen? -> same table; id_usuario_alta -> Usuarios | numero_orden |
| HistorialOrdenes | id_historial | id_orden -> OrdenesServicio; id_usuario -> Usuarios | None |
| Diagnosticos | id_diagnostico | id_orden -> OrdenesServicio; id_usuario_tecnico -> Usuarios | id_orden (0..1 per order) |
| Presupuestos | id_presupuesto | id_orden -> OrdenesServicio | id_orden filtered to tipo Original AND estado <> Anulado |
| PresupuestoDetalle | id_detalle | id_presupuesto -> Presupuestos; id_repuesto? -> Repuestos | None |
| Entregas | id_entrega | id_orden -> OrdenesServicio; id_usuario -> Usuarios | id_orden |
| Repuestos | id_repuesto | None | codigo |
| MovimientosStock | id_movimiento | id_repuesto -> Repuestos; id_usuario -> Usuarios; id_compra? -> Compras; id_reparacion? -> Reparaciones | None |
| Proveedores | id_proveedor | None | razon_social; not CUIT |
| Compras | id_compra | id_proveedor -> Proveedores; id_usuario -> Usuarios | None |
| CompraDetalle | id_detalle | id_compra -> Compras; id_repuesto -> Repuestos | None; repeated part rows possible |
| Reparaciones | id_reparacion | id_orden -> OrdenesServicio; id_usuario_tecnico -> Usuarios | (id_orden, numero_intervencion); not unique open intervention |
| ReparacionRepuesto | id_consumo | id_reparacion -> Reparaciones; id_repuesto -> Repuestos | None; ordinary index on (id_reparacion, id_repuesto) |
| Pruebas | id_prueba | id_reparacion -> Reparaciones; id_usuario_tecnico -> Usuarios; id_usuario_anulacion? -> Usuarios | None |
| Garantias | id_garantia | id_orden_original -> OrdenesServicio | id_orden_original, including annulled warranties |
| EvaluacionesGarantia | id_evaluacion | id_orden_reingreso -> OrdenesServicio; id_garantia -> Garantias; id_usuario -> Usuarios | id_orden_reingreso |

DDL owner anchors (`Inicializar` unless otherwise named):
[TipoEquipoRepository](../../INFRASTRUCTURE/Features/TiposEquipo/TipoEquipoRepository.cs),
[MarcaRepository](../../INFRASTRUCTURE/Features/Marcas/MarcaRepository.cs),
[ClienteRepository](../../INFRASTRUCTURE/Features/Clientes/ClienteRepository.cs),
[EquipoRepository](../../INFRASTRUCTURE/Features/Equipos/EquipoRepository.cs),
[OrdenServicioRepository](../../INFRASTRUCTURE/Features/Ordenes/OrdenServicioRepository.cs),
[HistorialEstadoOrdenRepository](../../INFRASTRUCTURE/Features/Ordenes/HistorialEstadoOrdenRepository.cs),
[DiagnosticoRepository](../../INFRASTRUCTURE/Features/Ordenes/DiagnosticoRepository.cs),
[EntregaRepository](../../INFRASTRUCTURE/Features/Ordenes/EntregaRepository.cs),
[RepuestoRepository](../../INFRASTRUCTURE/Features/Repuestos/RepuestoRepository.cs),
[MovimientoStockRepository](../../INFRASTRUCTURE/Features/Repuestos/MovimientoStockRepository.cs),
[ProveedorRepository](../../INFRASTRUCTURE/Features/Proveedores/ProveedorRepository.cs),
[CompraRepository](../../INFRASTRUCTURE/Features/Compras/CompraRepository.cs),
[GarantiaRepository](../../INFRASTRUCTURE/Features/Garantias/GarantiaRepository.cs),
[EvaluacionGarantiaRepository](../../INFRASTRUCTURE/Features/Garantias/EvaluacionGarantiaRepository.cs).
PresupuestoRepository owns budget/detail DDL; RepuestoRepository later adds its part FK.

Budget detail carries the quoted catalogue part: `EmitirConDetalle` and `CrearBorrador` bind one
nullable `@IdRepuesto<i>` parameter per line and store `NULL` for labour/service lines; the same
single transaction still inserts the header, all detail rows and, on emission, the order-state
update plus its history row. Readback uses `ListarDetalle`, which already hydrates nullable
`id_repuesto`. No `Repuestos` or `MovimientosStock` statement, lock or movement belongs to that
batch: a budget line is a quotation that neither reserves nor deducts stock, and only
`ReparacionRepository.ConsumirRepuesto` deducts it with its own cost snapshot.
`OrdenServicioService.PublicarBorrador` revalidates the stored detail and rejects a stored
subtotal that does not match it instead of rewriting persisted economics. Runtime acceptance
pending (feat-007 `in-progress`).
ReparacionRepository owns all three repair tables and adds the movement repair FK;
CompraRepository adds the movement purchase FK.

## Technical tables

| Table | PK | FKs | Other UNIQUE / constraint |
| --- | --- | --- | --- |
| Usuarios | id_usuario identity | None | username; active flag and dvh |
| Permisos | id_permiso identity | None | nombre; codigo filtered IS NOT NULL |
| PermisoComposicion | (id_permiso_padre, id_permiso_hijo) | Both -> Permisos | PK prevents duplicate edge, not shared children or SQL cycles |
| UsuarioPermisos | (id_usuario, id_permiso) | -> Usuarios, Permisos, both ON DELETE CASCADE | Assigned components; soft user deletion retains them |
| Idiomas | id_idioma identity | None | nombre; technical CHECK rejects blank trimmed name |
| Palabras | id_palabra identity | None | texto (translation key) |
| Traducciones | (id_idioma, id_palabra) | -> Idiomas, Palabras, ON DELETE CASCADE | One translation per key/language pair |
| Bitacora | id_bitacora identity | None | Actor username text, not Usuario FK |
| ControlCambios | id_cambio identity | None | id_idioma/id_palabra are historical values, not declared FKs |
| DigitosVerticales | id_dvv identity | None | nombre_tabla; currently populated/verified for Usuarios |

Owners: [UsuarioRepository](../../INFRASTRUCTURE/Features/Usuarios/UsuarioRepository.cs),
`Inicializar`; [UsuarioPermisoRepository](../../INFRASTRUCTURE/Features/Usuarios/UsuarioPermisoRepository.cs),
`CrearTablaUsuarioPermisos`; [PermisoRepository](../../INFRASTRUCTURE/Features/Permisos/PermisoRepository.cs),
`CrearTablaPermisos`/`CrearTablaComposicion`; [IdiomaRepository](../../INFRASTRUCTURE/Features/Idiomas/IdiomaRepository.cs),
`CrearTablas`; [BitacoraRepository](../../INFRASTRUCTURE/Features/Bitacora/BitacoraRepository.cs),
[ControlCambioRepository](../../INFRASTRUCTURE/Features/ControlCambios/ControlCambioRepository.cs),
[IntegridadRepository](../../INFRASTRUCTURE/Features/Integridad/IntegridadRepository.cs), `Inicializar`.
[UsuarioRepository](../../INFRASTRUCTURE/Features/Usuarios/UsuarioRepository.cs),
`Agregar`/`Modificar`, map unique-index errors 2601/2627 on `UX_Usuarios_Username` to
`UsuarioYaExisteException`; [UsuarioService](../../APPLICATION/Features/Usuarios/UsuarioService.cs),
`Crear`/`Modificar`, pre-check `ObtenerPorUsername` (which also returns inactive accounts) and
exclude the edited id. Usuarios stores only username/password/activo/dvh: unlike Clientes it holds
no personal identity data that could be duplicated.

## Customer document uniqueness (feat-006)

**Verified implementation (source inspection, not re-executed against a database in this scope).**
`Clientes.documento` is a customer identity that active and inactive rows reserve equally.
[ClienteRepository](../../INFRASTRUCTURE/Features/Clientes/ClienteRepository.cs), `Agregar` and
`Modificar`, run the ownership check and the DML in the same `SqlHelper.ExecuteTransaction` batch:
`SELECT ... FROM Clientes WITH (TABLOCKX, HOLDLOCK)` holds the table lock for that batch,
`Agregar` refuses any row with the proposed documento, and `Modificar` refuses only when the
proposed documento differs (SQL equality, database collation) from the edited row's current value
and belongs to another `id_cliente`. A conflict executes no DML; the batch returns the conflicting
id negated and the repository converts it into `ClienteDocumentoDuplicadoException`
([Cliente](../../DOMAIN/Features/Clientes/Cliente.cs)). Unique-index errors 2601/2627 on
`UX_Clientes_Documento` map to the same exception without exposing SQL text.
`ObtenerConflictoDocumento(documento, idExcluir)` is the read-side lookup over all rows; it returns
no conflict when `idExcluir` already owns the proposed documento, which grandfathers legacy
duplicate groups so those rows stay visible and editable with unchanged identity.
[ClienteService](../../APPLICATION/Features/Clientes/ClienteService.cs), `Crear`/`Modificar`,
pre-check before any write or audit and rethrow the concrete exception type.
`Desactivar`/`Reactivar` only toggle `activo`: an inactive row keeps reserving its documento, and
`Listar` still includes inactive rows only when `incluirInactivos` is requested. `telefono` and
`email` carry no constraint and stay freely shareable. Uniqueness compares the factory-trimmed
string exactly as the database collation compares it; there is no digit-only, punctuation,
numeric or leading-zero normalisation.

## Warranty coverage versus re-entry visits (feat-009)

**Verified implementation (source inspection, not re-executed against a database in this scope).
No DDL changed: no new table, column, constraint or index was added or altered.**

Cardinality, already expressed by the existing schema and now documented explicitly:

- An original order has **0..1** warranty **coverage** rows: `Garantias.id_orden_original` is
  unique through `UX_Garantias_OrdenOriginal`, annulled rows included.
- An original order has **0..N** warranty **visits** (re-entries): each one is a new
  `OrdenesServicio` row of type `Garantia` whose `id_orden_origen` references the original,
  and each re-entry has 0..1 `EvaluacionesGarantia` row (`id_orden_reingreso` unique).
- Creating a re-entry writes no `Garantias` row and never updates, resets or re-delivers the
  original order. The delivery batch keeps skipping warranty insertion when a coverage row
  already exists.

**Single shared predicate.** [OrdenServicioRepository](../../INFRASTRUCTURE/Features/Ordenes/OrdenServicioRepository.cs),
`PredicadoReingresoAbierto`, is a public `const` SQL fragment (table alias `o`, original
order in `@IdOrdenOrigen`) that defines a re-entry as **closed** only when `o.estado =
'Entregado'` or when `o.estado` is one of `PendientePresupuesto`, `EsperandoRespuesta`,
`AutorizadoReparacion`, `EnReparacion`, `EnPruebas` or `ListoRetiro` **and** the re-entry
has a `Rechazada` evaluation; every other state counts as open. It is used verbatim by the read
pre-check (`ObtenerReingresoGarantiaAbierto`), by the transactional guard of
`CrearConHistorial` and, through the same constant, by
[EntregaRepository](../../INFRASTRUCTURE/Features/Ordenes/EntregaRepository.cs),
`CancelarEntregaConTransicion`, so the three paths cannot drift apart. The fragment reads the
`EvaluacionesGarantia` table, which startup creates through
[EvaluacionGarantiaRepository](../../INFRASTRUCTURE/Features/Garantias/EvaluacionGarantiaRepository.cs),
`Inicializar`.

**Locked guard placement.** In `CrearConHistorial` the guard is the first statement of the
batch, runs only for `@TipoOrden = 'Garantia' AND @IdOrdenOrigen IS NOT NULL`, takes
`TABLOCKX, HOLDLOCK` on `OrdenesServicio`, and the single `INSERT` (plus `numero_orden`
update and history insert) happens inside the `ELSE` branch, so a conflict executes no DML at
all; the batch returns `-1` and the repository method returns `null` for the service to
convert into the typed business exception. Normal order creation is untouched: the guard branch
is skipped, and the success path still returns the new identity. In
`CancelarEntregaConTransicion` the same predicate is evaluated inside the existing
delete/update/history transaction, after that transaction has already taken
`TABLOCKX, HOLDLOCK` and read the order's `tipo_orden`/`id_orden_origen`; a conflict
returns `-1` and the delivery row, the order state and the history stay unchanged. The
original order is only ever read.

`ListarReingresosGarantia` reads the visits with `id_orden_origen = @IdOrdenOrigen AND
tipo_orden = 'Garantia'` ordered by `fecha_ingreso, id_orden`: it deliberately filters no
state and joins no evaluation, so delivered re-entries and re-entries without evaluations are
listed. Both reads reuse the repository's existing `Mapear` mapper.

## Snapshots, history, and atomicity

Monetary SQL columns use decimal(18,2): budget price/subtotal/discount/total,
purchase cost/subtotal/total, master cost, and per-consumption cost. Preserve snapshots;
changing master cost must not rewrite old consumption costs or approved budgets.
Return operations reduce/delete consumption quantities; they do not erase movement history.
HistorialOrdenes records order transitions in principal transactional writes;
Bitacora is a separate activity log, not a replacement for domain history.
ControlCambios concerns languages/translations, not generic business versioning.

**Confirmed boundaries:** services commonly audit after the principal repository commit.
An audit/readback failure can therefore return an error after successful business persistence;
do not blindly retry and create duplicates. Warranty diagnosis/evaluation has additional
multi-transaction boundaries described in [business flows](business-flows.md).
Stock batches use UPDLOCK/HOLDLOCK, but other state validations occur before writes.
Customer document writes additionally hold TABLOCKX/HOLDLOCK for their whole batch
(see the document-uniqueness section above). The warranty re-entry guard holds
TABLOCKX/HOLDLOCK on `OrdenesServicio` for its whole batch as well (see the
coverage-versus-visit section above); that is inspected SQL, not proof of parallel correctness.
No universal optimistic-concurrency guard or proven parallel safety should be inferred.
