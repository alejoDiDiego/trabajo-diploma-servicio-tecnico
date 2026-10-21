# Navigate the actual architecture

**Verified implementation:** a layered, feature-organized Windows Forms application using
ADO.NET SQL Server repositories and explicit construction. It is not an EF/DI/CQRS application.

## Six-project map

All manifests target .NET Framework 4.7.2. Names below are assembly/root namespace names,
not guesses from directory names.

| Physical project | Assembly / root namespace | Project references | Responsibility |
| --- | --- | --- | --- |
| [PRESENTATION/UI.csproj](../../PRESENTATION/UI.csproj) | UI | APPLICATION, DOMAIN, SERVICES, ABSTRACTIONS | WinForms, MDI navigation, view models, input/permission/translation handling |
| [APPLICATION/APPLICATION.csproj](../../APPLICATION/APPLICATION.csproj) | APPLICATION | REPOSITORY, DOMAIN, SERVICES, ABSTRACTIONS | Use-case orchestration and reference/state validation |
| [INFRASTRUCTURE/REPOSITORY.csproj](../../INFRASTRUCTURE/REPOSITORY.csproj) | REPOSITORY | DOMAIN, ABSTRACTIONS | SQL queries, mapping, DDL/seeds, transactional batches |
| [DOMAIN/DOMAIN.csproj](../../DOMAIN/DOMAIN.csproj) | DOMAIN | ABSTRACTIONS | Entities, factories, state constants, business exceptions |
| [SERVICES/SERVICES.csproj](../../SERVICES/SERVICES.csproj) | SERVICES | ABSTRACTIONS | Session, language observer, password hashing and encryption helper |
| [ABSTRACTIONS/ABSTRACTIONS.csproj](../../ABSTRACTIONS/ABSTRACTIONS.csproj) | ABSTRACTIONS | None | Small entity/security/permission/language contracts and translation DTO |

Use [TP_INTEGRADOR_2022.sln](../../TP_INTEGRADOR_2022.sln).
[TP_INTEGRADOR.slnx](../../TP_INTEGRADOR.slnx) still names `CROSSCUTTING/SERVICES.csproj`,
which is not the physical SERVICES project. This documentation does not repair that file.

## Runtime path and ownership

`UI form handler -> APPLICATION service -> DOMAIN validation + concrete REPOSITORY -> SqlHelper -> SQL Server`.
UI source has no direct SQL/repository use in the inspected tree. Keep it that way.
Services typically have parameterless constructors that allocate repositories with `new`;
repositories have both a parameterless `UrlDB` constructor and a connection-string constructor.
There are no repository interfaces. ABSTRACTIONS provides other contracts, not repository ports.

| Feature directory under APPLICATION/Features | Owner and principal persistence collaborator |
| --- | --- |
| Clientes, Equipos | ClienteService -> ClienteRepository; EquipoService also validates Cliente/TipoEquipo/Marca repositories |
| Marcas, TiposEquipo | MarcaService, TipoEquipoService -> matching master repository |
| Ordenes | OrdenServicioService -> OrdenServicio, HistorialEstadoOrden, Diagnostico, Presupuesto, Entrega repositories; also Reparacion and reference repositories |
| Repuestos | RepuestoService -> RepuestoRepository + MovimientoStockRepository |
| Compras, Proveedores | CompraService -> Compra/Proveedor/Repuesto repositories; ProveedorService -> ProveedorRepository |
| Garantias | GarantiaService -> GarantiaRepository + EvaluacionGarantiaRepository; re-entry/evaluation orchestration remains in OrdenServicioService |
| Reportes | ReporteService -> ReporteRepository + RepuestoRepository; dashboard shares this service |
| Usuarios | UsuarioService and UsuarioPermisoService -> Usuario/UsuarioPermiso/Permiso repositories |
| Permisos | PermisoService -> PermisoRepository + UsuarioPermisoRepository |
| Idiomas, ControlCambios | IdiomaService -> IdiomaRepository; ControlCambioService -> ControlCambioRepository + IdiomaRepository |
| Integridad, Bitacora | IntegridadService -> Usuario/Integridad repositories; BitacoraService -> BitacoraRepository |

These are source ownership maps, not a claim that each feature has its own independent service.
In particular there is no separate `PresupuestoService`, `ReparacionService`, or `DashboardService`.

## Construction and data shapes

- [OrdenServicioService](../../APPLICATION/Features/Ordenes/OrdenServicioService.cs), constructor,
  owns concrete collaborators; some warranty and audit collaborators are allocated inside methods.
- [OrdenServicio](../../DOMAIN/Features/Ordenes/OrdenServicio.cs), `CrearNuevo`,
  `CrearNuevoGarantia`, `CargarDesdeDB`: private constructor, validated creation vs DB hydration.
  Follow each entity's existing factory pattern rather than exposing setters indiscriminately.
- [EstadoOrdenServicio](../../DOMAIN/Features/Ordenes/EstadoOrdenServicio.cs) and related
  state/type/result classes contain `const string` values; they are not language enums.
- [SessionManager](../../SERVICES/Auth/SessionManager.cs), `Login`/`GetInstance`/`Logout`:
  static access around a private session instance; `GetInstance` throws before login.
- [SesionIdioma](../../SERVICES/Idiomas/SesionIdioma.cs), `GetInstance`: lazy singleton
  with Observer registration and synchronous notifications through `CambiarIdioma`.
  SessionManager is a single active-session holder, not a DI lifetime manager; unlike its
  session lock, the language singleton is not a general concurrency mechanism.
- [TraduccionEditable](../../ABSTRACTIONS/Features/Idiomas/TraduccionEditable.cs) is an actual
  mutable DTO (`IdPalabra`, `Clave`, `Texto`). Forms also define private `Fila*`/`Item*` view shapes.
  Reports return `DataTable`; do not invent a universal DTO layer or an absent DTO catalogue.
- [FamiliaPermiso](../../DOMAIN/Features/Permisos/FamiliaPermiso.cs), `AgregarHijo`, allows
  descendants in multiple branches while preventing cycles/duplicate membership at one level.
  This Composite is not exclusive ownership of child objects.

These are the actual reusable patterns: explicit service/repository construction, static
entity creation/hydration factories, single session/language holders, language Observer,
and permission Composite. Static entity factories are not evidence of a polymorphic GoF
Factory Method hierarchy. Do not infer additional patterns merely from a diagram label.

## UI source anchors

| Path | Methods to inspect |
| --- | --- |
| [Program](../../PRESENTATION/Program.cs) | `Main`: STA setup, initialization order, language, FrmPrincipal |
| [FrmPrincipal](../../PRESENTATION/Forms/FrmPrincipal.cs) | `ActualizarMenuUsuario`, `TSMI_Ordenes_Click`, `TSMI_Dashboard_Click`, `TSMI_Reportes_Click`, `CerrarFormulariosHijos` |
| [FrmOrdenServicioDetalle](../../PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.cs) | `CargarOrden`, `AplicarPermisosDetalle`, `BTN_Emitir_Click`, `BTN_FinalizarPruebas_Click`, `BTN_RechazarGarantia_Click` |
| [FrmCompraDetalle](../../PRESENTATION/Forms/Compras/FrmCompraDetalle.cs) | `CargarCompra`, `BTN_GuardarBorrador_Click`, `BTN_Confirmar_Click`, `BTN_AnularCompra_Click`, `AplicarPermisos` |
| [FrmGarantias](../../PRESENTATION/Forms/Garantias/FrmGarantias.cs) | `CargarGarantias`, `AplicarFiltro` |
| [FrmDashboard](../../PRESENTATION/Forms/Dashboard/FrmDashboard.cs) | `CargarResumen`, `BTN_Actualizar_Click` |
| [FrmReportes](../../PRESENTATION/Forms/Reportes/FrmReportes.cs) | `EjecutarReporte`, `ConfigurarColumnas` |

## Change policy, not a hidden refactor mandate

**Intended policy:** keep feature ownership and explicit dependencies, and use the simplest
targeted fix. Adding EF, a container, repository interfaces, new architecture layers, Mediator,
or a new .NET target is a separate design decision requiring approval.
Current project references contain no installed Krypton dependency; do not claim one exists.
Continue with [business flows](business-flows.md) before touching transition code.
