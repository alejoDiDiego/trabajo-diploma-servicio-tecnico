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
| Clientes | id_cliente | None | None; documento is not unique |
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
No universal optimistic-concurrency guard or proven parallel safety should be inferred.
