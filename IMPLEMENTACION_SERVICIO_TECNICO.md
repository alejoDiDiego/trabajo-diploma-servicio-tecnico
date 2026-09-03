# Implementacion Servicio Tecnico - Checkpoint 1

## Checkpoint 1

### Objetivo

Incorporar el modulo de servicio tecnico (gestion de clientes, equipos y catalogos
de tipos de equipo y marcas) sobre la base existente de usuarios, permisos,
idiomas, bitacora, control de cambios e integridad DVH/DVV, con baja logica en
todas las entidades nuevas y menu de gestion visible segun permisos.

### Branch y origen

- Branch: `checkpoint-1-servicio-tecnico` (trabajo local, sin upstream).
- Origen: `main` en `6f4e299` ("merge: feat/mdi-observer-composite-v2 + docs
  rescatada a main").
- Commits del checkpoint (locales, sin push):
  - `feat(usuarios): baja logica con activo, reactivacion y DVH canonico`
  - `feat(servicio-tecnico): backend de clientes, equipos, tipos y marcas`
  - `feat(ui): gestion de servicio tecnico y modernizacion de pantallas`
  - `fix(ui): ocultar DVH y tolerar idioma nulo en grillas nuevas`
  - `docs(checkpoint-1): informe de implementacion del servicio tecnico`
- Working tree: limpio tras el commit (ver seccion GIT).
- Estado local/remoto: `origin/main` permanece en `6f4e299`;
  `checkpoint-1-servicio-tecnico` solo existe en local. NO se hizo push.

### Tablas nuevas y cambio en Usuarios

Nuevas (creacion idempotente con `IF OBJECT_ID` + `ALTER` defensivos):

- `Clientes` (`id_cliente` PK identity, `nombre`, `apellido`, `documento`,
  `telefono`, `email`, `direccion`, `observaciones`, `activo` bit default 1,
  `fecha_alta` datetime default `GETDATE()`).
- `TiposEquipo` (`id_tipo_equipo` PK identity, `nombre` unico
  `UX_TiposEquipo_Nombre`, `activo` bit default 1).
- `Marcas` (`id_marca` PK identity, `nombre` unico `UX_Marcas_Nombre`,
  `activo` bit default 1).
- `Equipos` (`id_equipo` PK identity, `id_cliente` FK a Clientes,
  `id_tipo_equipo` FK a TiposEquipo, `id_marca` FK a Marcas, `modelo`,
  `numero_serie`, `imei`, `color`, `observaciones`, `activo` bit default 1;
  FKs `NO ACTION` para conservar historia).

Existente modificada:

- `Usuarios.activo` (bit, default 1; `ALTER` defensivo si la columna no existe).
  La baja de usuarios paso de `DELETE` fisico a baja logica.

### Esquema resumido

```text
Clientes 1 ----< * Equipos >---- 1 TiposEquipo
                        \------ 1 Marcas
Usuarios (activo, dvh) ----< * UsuarioPermisos *>---- Permisos (Composite)
DigitosVerticales(nombre_tabla, dvv) // DVV vertical de Usuarios
Bitacora(tipo_actividad: CLIENTES/EQUIPOS/TIPOS_EQUIPO/MARCAS/USUARIOS/...)
```

### Capas

- DOMAIN: entidades nuevas `Cliente`, `Equipo`, `TipoEquipo`, `Marca`
  (constructores `CrearNuevo` con validacion, `CargarDesdeDB`, `Activo`
  conmutado solo por repository/service); `Usuario` suma `Activo`,
  `Desactivar()`/`Reactivar()`, `CargarDesdeDB(..., activo)`;
  `CodigosPermiso` suma 16 codigos de servicio tecnico.
- APPLICATION: `ClienteService`, `EquipoService`, `TipoEquipoService`,
  `MarcaService` (CRUD + `Desactivar`/`Reactivar` + bitacora por operacion;
  `EquipoService` valida FKs existentes y activas en el service, no en la
  entidad); `UsuarioService` (login bloquea inactivos, `Eliminar` = baja
  logica con recalculos DVH/DVV, nuevo `Reactivar`, `Inicializar` recalcula
  todos los DV tras el cambio de formato DVH); `UsuarioPermisoService`
  (no asigna ni quita permisos a usuarios inactivos); `BitacoraService`
  (nuevos tipos `CLIENTES`, `EQUIPOS`, `TIPOS_EQUIPO`, `MARCAS`);
  `DigitoVerificadorHelper` (DVH canonico con `Activo` como `"1"`/`"0"`).
- INFRASTRUCTURE: `ClienteRepository`, `EquipoRepository`,
  `TipoEquipoRepository`, `MarcaRepository` (idempotentes, baja logica por
  `UPDATE activo`, mapeo `DBNull`-seguro); `UsuarioRepository`
  (`INSERT`/`UPDATE` con `activo`, `Eliminar` delega en `CambiarEstado`,
  `LeerActivo` defensivo); `PermisoRepository` (seed de 16 permisos simples,
  3 familias nuevas y composiciones Administrador/Lectura); `IdiomaRepository`
  (seeds ES/EN de gestion, formularios, campos, columnas y acciones).
- UI: `FrmClientes` + `FrmClienteEditar`, `FrmEquipos` + `FrmEquipoEditar`,
  `FrmTiposEquipo`, `FrmMarcas`, `FrmCatalogoEditar` (dialogo compartido
  nombre); `FrmPrincipal` con menu top-level `Gestion` (Clientes, Equipos,
  Catalogos > Tipos de equipo, Marcas) visible/habilitado por permiso;
  `FrmAdministrarUsuarios` con boton Reactivar creado por codigo y columna
  `Activo`; `FrmAsignarPermisosUsuario` oculta `Password` y `DVH`;
  `Program.Main` inicializa los 4 catalogos/servicios nuevos y fija idioma
  por defecto.
- ABSTRACTIONS: `IUsuario` suma `Activo`.
- SERVICES: sin cambios (se reutilizan `SessionManager` y `SesionIdioma`).

### Archivos existentes modificados

- `ABSTRACTIONS/Features/Usuarios/IUsuario.cs`
- `APPLICATION/APPLICATION.csproj`
- `APPLICATION/Features/Bitacora/BitacoraService.cs`
- `APPLICATION/Features/Integridad/DigitoVerificadorHelper.cs`
- `APPLICATION/Features/Usuarios/UsuarioPermisoService.cs`
- `APPLICATION/Features/Usuarios/UsuarioService.cs`
- `DOMAIN/DOMAIN.csproj`
- `DOMAIN/Features/Permisos/CodigosPermiso.cs`
- `DOMAIN/Features/Usuarios/Usuario.cs`
- `INFRASTRUCTURE/REPOSITORY.csproj`
- `INFRASTRUCTURE/Features/Idiomas/IdiomaRepository.cs`
- `INFRASTRUCTURE/Features/Permisos/PermisoRepository.cs`
- `INFRASTRUCTURE/Features/Usuarios/UsuarioRepository.cs`
- `PRESENTATION/Program.cs`
- `PRESENTATION/UI.csproj`
- `PRESENTATION/Forms/FrmPrincipal.cs` + `FrmPrincipal.Designer.cs`
- `PRESENTATION/Forms/Auth/FrmAdministrarUsuarios.cs` +
  `FrmAdministrarUsuarios.Designer.cs` (solo retoque visual menor)
- `PRESENTATION/Forms/Auth/FrmAsignarPermisosUsuario.cs`
- `PRESENTATION/Forms/Bitacora/FrmBitacora.Designer.cs` (solo retoque visual)

### Archivos nuevos

- `DOMAIN/Features/Clientes/Cliente.cs`
- `DOMAIN/Features/Equipos/Equipo.cs`
- `DOMAIN/Features/TiposEquipo/TipoEquipo.cs`
- `DOMAIN/Features/Marcas/Marca.cs`
- `APPLICATION/Features/Clientes/ClienteService.cs`
- `APPLICATION/Features/Equipos/EquipoService.cs`
- `APPLICATION/Features/TiposEquipo/TipoEquipoService.cs`
- `APPLICATION/Features/Marcas/MarcaService.cs`
- `INFRASTRUCTURE/Features/Clientes/ClienteRepository.cs`
- `INFRASTRUCTURE/Features/Equipos/EquipoRepository.cs`
- `INFRASTRUCTURE/Features/TiposEquipo/TipoEquipoRepository.cs`
- `INFRASTRUCTURE/Features/Marcas/MarcaRepository.cs`
- `PRESENTATION/Forms/Clientes/FrmClientes.cs` (+ Designer)
- `PRESENTATION/Forms/Clientes/FrmClienteEditar.cs` (+ Designer)
- `PRESENTATION/Forms/Equipos/FrmEquipos.cs` (+ Designer)
- `PRESENTATION/Forms/Equipos/FrmEquipoEditar.cs` (+ Designer)
- `PRESENTATION/Forms/Catalogos/FrmTiposEquipo.cs` (+ Designer)
- `PRESENTATION/Forms/Catalogos/FrmMarcas.cs` (+ Designer)
- `PRESENTATION/Forms/Catalogos/FrmCatalogoEditar.cs` (+ Designer)

### Permisos (16 nuevos)

- `CLIENTES_VER/CREAR/EDITAR/DESACTIVAR`, `EQUIPOS_VER/CREAR/EDITAR/DESACTIVAR`,
  `TIPOS_EQUIPO_VER/CREAR/EDITAR/DESACTIVAR`, `MARCAS_VER/CREAR/EDITAR/DESACTIVAR`.
- Familias nuevas: `Gestion clientes`, `Gestion equipos`, `Gestion catalogos`
  (colgadas de `Administrador`; los `*_VER` tambien cuelgan de
  `Lectura general`).

### Traducciones (seeds ES/EN)

Menu (`Menu.Gestion/Clientes/Equipos/Catalogos/TiposEquipo/Marcas`), titulos de
los 4 formularios, filtros, botones Crear/Editar/Desactivar/Reactivar, campos
(`Campo.*`), columnas (`Columna.*`: Nombre, Apellido, Documento, Telefono,
Email, Activo, Cliente, Tipo, Marca, Modelo, NumeroSerie, Imei, Color),
acciones Aceptar/Cancelar y tipos de bitacora
(`Bitacora.CLIENTES/EQUIPOS/TIPOS_EQUIPO/MARCAS`).

### Decisiones

- Krypton diferido a futuro: no se agrego dependencia nueva; la UI sigue en
  WinForms estandar.
- Baja logica en Clientes/TiposEquipo/Marcas/Equipos/Usuarios (columna
  `activo`; `Reactivar` reutiliza el permiso `DESACTIVAR`, sin codigo nuevo).
- DVH canonico: `Activo` serializado como `"1"`/`"0"` (nunca `True/False`)
  para estabilidad entre SQL y C#.
- Dialogos de edicion no llaman al service: exponen propiedades y el form
  llamador (`FrmClientes`/`FrmEquipos`/catalogos) invoca Crear/Modificar.
- Menu `Gestion` top-level (no colgado de `Usuario`) para separar dominio de
  negocio de administracion.
- Paquetes: ninguno nuevo.

### Ajuste: baja logica independiente + validaciones

- Baja logica independiente (verificado, sin cambios): `Cliente.Activo` y
  `Equipo.Activo` son independientes, sin cascada. `ClienteService.Desactivar` /
  `Reactivar` solo tocan `Clientes`; los repositories solo hacen
  `UPDATE Clientes`; las FKs de `Equipos` son `NO ACTION`. Las grillas muestran
  historia (no se filtra) y los combos ya excluyen inactivos. No se agrego
  badge/columna nueva ni se cambio `Modificar`/`Reactivar` de Equipo.
- Telefono obligatorio: `Cliente.CrearNuevo` valida
  `string.IsNullOrWhiteSpace(telefono)` con `ReglaNegocioException`
  ("El telefono es obligatorio.") y persiste `telefono.Trim()`; `Email` /
  `Direccion` / `Observaciones` siguen opcionales (`""`). `FrmClienteEditar`
  valida `TXT_Telefono` en `OnFormClosing` con
  `Mensaje.ClienteCamposObligatorios` (ES: "Nombre, apellido, documento y
  telefono son obligatorios." / EN: "First name, last name, document and phone
  are required."; `UPDATE` idempotente en `IdiomaRepository` porque
  `AgregarSeed` es `IF NOT EXISTS`).
- Asteriscos de obligatorios (solo en `Actualizar()`, concatenando `" *"` al
  texto traducido; sin tocar `Designer.Text` ni seeds; guard null-idioma
  existente respetado): `FrmClienteEditar` (Nombre/Apellido/Documento/
  Telefono), `FrmEquipoEditar` (Cliente/Tipo/Marca), `FrmCatalogoEditar`
  (Nombre). Opcionales sin asterisco. Sin claves nuevas (fallback a clave si
  falta traduccion).
- REGLA FUTURA (no implementar ahora, sin codigo especulativo): cuando exista
  `OrdenServicio`, no se podra desactivar un `Cliente` con ordenes abiertas
  (`Estado != Entregado`).

### Delegaciones

- A-E exploracion del repo y esquema: base para el disenio (OK).
- T1 usuarios-activo (baja logica, reactivar, DVH canonico): OK, verificado.
- T2-T3 clientes + tipos/marcas (entidades, repositories, services, seeds):
  OK, verificado.
- Auditoria intermedia (permisos efectivos, DVH/DVV, bitacora): OK.
- T4 equipos (entidad con 3 FKs, validacion en service, `ListarPorCliente`):
  OK, verificado.
- T5 UI clientes/equipos/catalogos + menu Gestion: OK, verificado visual.
- T6 traducciones ES/EN + tipos de bitacora: OK (116 filas ST presentes).
- T7 integridad/DVV y regresion (tamper detectado, recalculo restaura): OK.
- Fixes post-T7 (5 cambios de 1-3 lineas: ocultar DVH en asignaciones;
  guard null-idioma en `ConfigurarColumna` de Clientes/Equipos/Tipos/Marcas):
  OK, re-validados en este checkpoint.
- Revisiones cruzadas: harness funcional (47 checks PASS), harness visual
  STA `DrawToBitmap` (todos los forms abren, sin solapes ni truncados),
  regresion sesion/idioma/DV (PASS).

### Pruebas funcionales (harness temporal fuera del repo, prefijo CP1TEST)

Fuente previa: `cp1test.log` (15:14). Re-ejecutadas las criticas en este
checkpoint con prefijo CP1RT donde aplicaba:

- A-tablas/DVV/usuarios/permisos16: PASS (14 tablas, DVV con 1 fila,
  5 usuarios base, 16 permisos ST).
- B1 cliente crear/editar/desactivar/reactivar con `activo` en SQL: PASS.
- B2 tipo/marca/equipo crear/editar, `ListarPorCliente`, bloqueo con cliente
  inactivo y con tipo/marca inexistentes: PASS.
- B3 usuario crear, asignar `CLIENTES_VER`, login OK, baja, login bloqueado,
  reactivar, login OK, `UsuarioPermisos` conservados (1=1): PASS.
- B4 integridad `true` -> tamper `false` -> `RecalcularTodosDV` -> `true`:
  PASS.
- B5 bitacora con tipos CLIENTES/EQUIPOS/MARCAS/TIPOS_EQUIPO/USUARIOS: PASS.

### Pruebas visuales (harness STA + UI real)

- Todos los forms nuevos y de edicion abren (nuevo + editar con datos
  reales), `DrawToBitmap` OK, resize OK, sin WARN de layout fuera de cliente
  ni truncados.
- Forms historicos (Login, Principal, Usuarios, Permisos, Asignar, Idiomas,
  Bitacora, ControlCambios) abren sin regresion.
- `FrmAdministrarUsuarios`: columnas visibles `[Id,Username,Activo]`,
  ocultas `[Password,DVH]`; boton Reactivar visible.
- ES->EN->ES en `FrmMarcas` sin excepcion (`Marcas`/`Brands`/`Marcas`).

### Pruebas de regresion

- Login valido/invalido/logout: PASS.
- ES<->EN por service y por UI observer: PASS.
- `VerificarIntegridadUsuarios` + `RecalcularTodosDV`: PASS.

### Re-test post-fix de este checkpoint (harness CP1RT fuera del repo)

- `RT-login` admin/123: PASS.
- `RT-a-*` (`FrmAsignarPermisosUsuario` con sesion admin): grilla 5 cols,
  6 filas; visibles `[Id,Username,Activo]`; ocultas `[Password,DVH]`: PASS.
- `RT-b-*` (Clientes/Equipos/Tipos/Marcas con idioma null, sin
  `CambiarIdioma`): ningun form lanzo `NullReferenceException`
  (Clientes 10 cols/1 fila, Equipos 13/1, Tipos 3/1, Marcas 3/1): PASS.
- `RT-c-*` (smoke `FrmPrincipal` real, login admin/123): Principal abre,
  MenuStrip OK, `Gestion` visible; permisos efectivos
  `CLIENTES/EQUIPOS/TIPOS/MARCAS_VER=true`; subitems Clientes/Equipos y
  anidados Tipos/Marcas presentes con handlers Click: PASS.
- `RT-d-integridad` (`VerificarIntegridadUsuarios()=true`): PASS.

### Herramienta visual

Harness STA temporal fuera del repo (`CP1Visual*`, `CP1RT`): instancia los
forms reales de `UI.exe` con sesion admin, vuelca arbol de controles,
columnas visibles/ocultas de cada grilla, `DrawToBitmap` a PNG y prueba de
resize. Mas smoke con `UI.exe` real (login admin/123, menu Gestion).

### Problemas encontrados y corregidos (2 post-T7)

1. `FrmAsignarPermisosUsuario` mostraba la columna interna `DVH` (ademas del
   hash `Password`). Fix: ocultar `DVH` en `ConfigurarColumnasUsuarios`
   (3 lineas). Re-test `RT-a-dvh-oculto`: PASS.
2. `FrmClientes`/`FrmEquipos`/`FrmTiposEquipo`/`FrmMarcas` podian lanzar
   `NullReferenceException` en `ConfigurarColumna` si el idioma aun era null
   (acceso a `_sesionIdioma.idioma.BuscarTraduccion`). Fix: guard
   `_sesionIdioma.idioma == null ? claveTraduccion : ...` en cada
   `ConfigurarColumna` (1 linea por form). Re-test `RT-b-*`: PASS.

### Limitaciones

- `MenuStrip` es invisible a UIA: el smoke automatizado verifica Gestion por
  permisos efectivos + handlers; la apertura visual del menu queda a prueba
  humana.
- `RecalcularDV` por menu no fue clickeado en el harness (requiere
  confirmacion modal); se cubrio via `IntegridadService.RecalcularTodosDV`
  directo: PASS.
- ES/EN de pantallas nuevas se verifico via `CambiarIdioma` (service +
  observer en `FrmMarcas`); no se recorrieron los 7 forms en ambos idiomas.

### Datos de prueba

Limpieza posterior al re-test: `DELETE` en una transaccion de todas las filas
con prefijo `CP1TEST`/`CP1RT`/`cp1test_u_` (Equipos -> Clientes -> TiposEquipo
-> Marcas -> `UsuarioPermisos` -> Usuarios). Filas eliminadas: 1 equipo,
1 cliente, 1 tipo, 1 marca, 1 usuario + su asignacion. Tablas ST quedaron en 0
filas; Usuarios volvio a los 5 base; `UsuarioPermisos` a 6 filas. Bitacora
conserva el historial (34 filas, no se purga por trazabilidad). Tras el
`DELETE`, el DVV vertical quedo `false` (esperable: el DVV firma las filas);
se ejecuto `RecalcularTodosDV` via service real y `Verificar` volvio a
`true`.

### Pruebas humanas pendientes

1. Iniciar `UI.exe`, login admin/123, confirmar que el menu `Gestion` muestra
   Clientes, Equipos y Catalogos > Tipos de equipo/Marcas.
2. Crear un cliente desde `FrmClientes` y verificar que aparece en la grilla
   y en bitacora (`CLIENTES`).
3. Editar ese cliente y verificar persistencia tras recargar el form.
4. Desactivar/reactivar el cliente con `CHK_Inactivos` marcado y desmarcado.
5. Crear tipo y marca desde catalogos; verificar unicidad (duplicado
   rechazado).
6. Crear un equipo ligado al cliente/tipo/marca; verificar `ListarPorCliente`
   filtrando por combo.
7. Intentar crear un equipo con cliente inactivo: debe fallar con mensaje.
8. Desactivar/reactivar el equipo y verificar el filtro de inactivos.
9. Dar de baja un usuario desde `FrmAdministrarUsuarios`, intentar login
   (debe fallar) y reactivarlo (login OK, permisos conservados).
10. Cambiar idioma ES<->EN con `FrmClientes` abierto y verificar traduccion
    de titulos, filtros, botones y columnas.
11. Abrir `FrmAsignarPermisosUsuario` y confirmar que la grilla no muestra
    `Password` ni `DVH`.
12. Ejecutar `Recalcular DV` desde el menu y confirmar mensaje de exito y
    `VerificarIntegridadUsuarios()=true`.
13. Forzar un `UPDATE` manual en `Usuarios` y confirmar que el proximo login
    avisa "integridad comprometida".
14. Revisar bitacora: filtros por usuario/fecha/tipo con los tipos nuevos.
15. Probar `FrmEquipoEditar`/`FrmClienteEditar` con campos obligatorios
    vacios (deben advertir y no cerrar con OK).
16. Cerrar sesion y confirmar que `Gestion` se oculta sin sesion.
