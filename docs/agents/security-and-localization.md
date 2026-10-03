# Extend identity, permissions, and translations safely

**Verified implementation:** this is in-process desktop session management, not an API security
boundary. Permission checks are largely in forms; business state validation is not authorization.
Read [known limitations](known-limitations.md) before claiming production-grade enforcement.

## Authentication and secret handling

[UsuarioService](../../APPLICATION/Features/Usuarios/UsuarioService.cs), `Login`, retrieves the
user, rejects inactive accounts, verifies the password, loads effective permissions,
creates SessionManager, then checks integrity and audits. Integrity failure sets
`IntegridadComprometida`; it is not equivalent to rejecting the login.
`Eliminar` is soft deletion, with permission assignments retained and integrity recalculation.

[PasswordHasher](../../SERVICES/Security/PasswordHasher.cs), `HashPassword`/
`VerifyHashedPassword`, uses PBKDF2 SHA-256, 10,000 iterations, random 16-byte salt,
32-byte derived key, Base64 salt+key payload. It compares via SequenceEqual, not an explicitly
constant-time comparison. Do not describe the encryption helper as the login password hash.
[EncryptionService](../../SERVICES/Security/EncryptionService.cs), `Encrypt`/`Decrypt`, is a
separate AES helper with source-defined derivation material; do not reproduce it in docs/logs.

`UsuarioService.Inicializar` seeds demo accounts with weak default passwords. They are only
demo fixtures, not production-ready access. Do not copy passwords, hashes, SQL login credentials,
or encryption derivation values into guides. Ask for authorized configuration privately.
Connection configuration is `UrlDB`; setup examples use placeholders only.

## Permission Composite and seeded roles

Source path:
[CodigosPermiso](../../DOMAIN/Features/Permisos/CodigosPermiso.cs) ->
[PermisoRepository](../../INFRASTRUCTURE/Features/Permisos/PermisoRepository.cs),
`AgregarPermisosSimplesBase`, `AgregarFamiliasBase`, `AgregarComposicionesFamiliasBase` ->
[UsuarioPermisoRepository](../../INFRASTRUCTURE/Features/Usuarios/UsuarioPermisoRepository.cs),
`AgregarAsignacionesBase` ->
[UsuarioPermisoService](../../APPLICATION/Features/Usuarios/UsuarioPermisoService.cs),
`ListarPermisosEfectivos` ->
[SessionManager](../../SERVICES/Auth/SessionManager.cs), `TienePermiso`.

| Exact family name | Intended seeded grouping, verified from seed |
| --- | --- |
| Raiz | System root grouping; not an ordinary acting-user role |
| Administrador | Management families, REPORTES_VER, INTEGRIDAD_RECALCULAR, BITACORA_VER |
| Gestion usuarios / Gestion permisos | User maintenance; permission maintenance/composition/assignment respectively |
| Gestion idiomas / Gestion traducciones | Language management/change; translations and their change control respectively |
| Gestion clientes / Gestion equipos / Gestion catalogos | Customer/equipment/type/brand CRUD and deactivation codes |
| Gestion ordenes | View/create/edit/cancel/deliver and PRESUPUESTOS_DECIDIR |
| Gestion repuestos / Gestion proveedores / Gestion compras | Corresponding business management codes |
| Lectura general | Read codes including REPORTES_VER; also IDIOMAS_CAMBIAR, so not strictly read-only |
| Rol recepcionista | Customer/equipment view/create/edit, catalogue read, order view/create/edit/deliver |
| Rol tecnico | Customer/equipment/catalogue read, order view/edit, parts read |
| Rol encargado | Business management families, BITACORA_VER and REPORTES_VER |

Demo usernames map to matching role families for recepcionista/tecnico/encargado;
admin -> Administrador, usuarios -> Gestion usuarios, permisos -> Gestion permisos,
idiomas -> Gestion idiomas + Gestion traducciones, lector -> Lectura general.
Seed inserts use NOT EXISTS; the role assignment migration also removes redundant direct
assignments for those three demo role users. Re-running startup can restore seeded grants.
Do not assume changes to demo grants survive initialization unchanged.

Families and simple permissions share Permisos; edges are many-to-many.
[FamiliaPermiso](../../DOMAIN/Features/Permisos/FamiliaPermiso.cs), `AgregarHijo`, and
[PermisoService](../../APPLICATION/Features/Permisos/PermisoService.cs), `AgregarComponente`,
prevent cycles through application logic. SQL PK/FKs alone do not enforce acyclicity.
Shared descendants are valid; never depict them as exclusively owned children.
Session permission snapshots are loaded at login; do not assume automatic refresh after seed/edit.

### Extend a permission end to end

**Intended policy for an authorized code task:** add the constant, idempotent simple seed,
correct family edges, optional demo assignment, bilingual permission-label seeds,
menu/form/handler checks, and targeted verification together. Never grant by a guessed role name.
Inspect seed NOT EXISTS conditions and both fresh/legacy setup paths; do not run initialization
on a real database merely to verify a new code.

## Acting-user permissions versus technician eligibility

[FrmPrincipal](../../PRESENTATION/Forms/FrmPrincipal.cs), `ActualizarMenuUsuario`, controls
menus. [FrmOrdenServicioDetalle](../../PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.cs),
`AplicarPermisosDetalle` and handlers, checks ORDENES_EDITAR, ORDENES_ENTREGAR,
PRESUPUESTOS_DECIDIR, etc. Delivery UI requires both edit eligibility and delivery permission.
`AplicarPermisosGarantia`: acceptance uses ORDENES_EDITAR; rejection uses PRESUPUESTOS_DECIDIR.
Dashboard/reports use REPORTES_VER in forms; ReporteService does not enforce that code.

[OrdenServicioService](../../APPLICATION/Features/Ordenes/OrdenServicioService.cs),
`ObtenerIdUsuarioSesion`, checks a session for most writes, but not the action-specific
permission. `ValidarTecnicoElegible` explicitly validates the **assigned technician's**
active status and effective ORDENES_EDITAR; that is not blanket authorization of the caller.
Backend services must not be advertised as fully authorized for untrusted callers.

## Integrity: actual coverage and startup hazard

[DigitoVerificadorHelper](../../APPLICATION/Features/Integridad/DigitoVerificadorHelper.cs),
`CalcularDVH`, hashes username/password payload/ID/active flag; `CalcularDVV` hashes ordered
row digests. [IntegridadService](../../APPLICATION/Features/Integridad/IntegridadService.cs),
`VerificarIntegridadUsuarios`, checks Usuarios only; exceptions become false.
`UsuarioService.Inicializar` unconditionally invokes `RecalcularTodosDV` after seeds.
That can normalize tampered values before later login verification sees them.
**Intended policy:** distinguish explicit repair/migration from verification; do not silently
claim tamper detection across restarts or coverage of orders/stock/other business tables.

## Observer translation flow

[SesionIdioma](../../SERVICES/Idiomas/SesionIdioma.cs), `CambiarIdioma`, notifies registered
IObservador forms. Controls/form/grid columns carry translation keys in `Tag`;
`Actualizar`/`ActualizarTextos` resolves current language and rebuilds translated labels/options.
Forms unregister in OnFormClosed to avoid stale observers.
[IdiomaRepository](../../INFRASTRUCTURE/Features/Idiomas/IdiomaRepository.cs),
`SembrarDatosIniciales`/`AgregarSeed`, and PermisoRepository permission-label seeds hold ES/EN.
[IdiomaService](../../APPLICATION/Features/Idiomas/IdiomaService.cs), `GuardarTraduccion`,
and [ControlCambioService](../../APPLICATION/Features/ControlCambios/ControlCambioService.cs),
`Restaurar`, support translation editing/history/restoration, not generic undo.

Use stable keys, preserve user-edited translations according to existing seed semantics,
and inspect any `ActualizarTraduccion` overwrites before changing them.
For UI changes test ES and EN at small/large sizes, long values, error dialogs, grid headers,
and current-culture numeric editing. Translation does not automatically switch numeric culture.
