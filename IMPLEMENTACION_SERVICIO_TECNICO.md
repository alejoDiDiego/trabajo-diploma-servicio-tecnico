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

## Checkpoint 2

### Objetivo

Incorporar el ciclo de vida de ordenes de servicio (recepcion, asignacion de
tecnico, diagnostico reparable/no reparable, presupuesto con detalle,
aprobacion/rechazo, cancelacion, entrega e historial de estados) sobre la base
del checkpoint 1 (clientes, equipos, catalogos), con bitacora tipo `ORDENES`,
6 permisos nuevos y menu `Gestion > Ordenes` visible segun permisos.

### Branch y origen

- Branch: `checkpoint-2-ordenes` (trabajo local, sin upstream).
- Origen: `checkpoint-1-servicio-tecnico` en `2039ad8`
  ("fix(checkpoint-1): telefono obligatorio, asteriscos de obligatorios y
  regla futura sin cascada").
- Commits del checkpoint (locales, sin push):
  - `feat(ordenes): backend de ordenes, diagnostico, presupuesto y entrega`
  - `feat(ui): gestion de ordenes con detalle de 5 pestanas`
  - `fix(ui): habilitar Emitir tras agregar o quitar item del presupuesto`
  - `docs(checkpoint-2): informe de implementacion de ordenes de servicio`
- Working tree: limpio tras el commit (ver seccion GIT).
- Estado local/remoto: `origin` no tiene la rama `checkpoint-2-ordenes`;
  la rama solo existe en local. NO se hizo push.

### Tablas nuevas

Nuevas (creacion idempotente con `IF OBJECT_ID` + `ALTER`/`FK` defensivos;
FKs sin accion en cascada para conservar historia):

- `OrdenesServicio` (`id_orden` PK identity, `numero_orden` int
  (= `id_orden` del mismo batch, ver Decisiones), `id_cliente` FK a Clientes,
  `id_equipo` FK a Equipos, `id_tecnico_asignado` FK nullable a Usuarios,
  `id_orden_origen` FK nullable autorreferenciada (reservada para garantia
  CP4), `tipo_orden` default `'Normal'`, `estado`, `resultado` nullable,
  `fecha_ingreso` default `GETDATE()`, `problema_informado`,
  `estado_fisico_ingreso`, `accesorios_ingreso`, `observaciones_ingreso`,
  `observacion_resultado` nullable, `id_usuario_alta` FK a Usuarios; indice
  sobre `numero_orden`).
- `HistorialOrdenes` (`id_historial` PK identity, `id_orden` FK a
  OrdenesServicio, `estado_anterior` nullable (null = alta), `estado_nuevo`,
  `fecha_hora` default `GETDATE()`, `id_usuario` FK a Usuarios,
  `observacion`).
- `Diagnosticos` (`id_diagnostico` PK identity, `id_orden` FK a
  OrdenesServicio, `id_usuario_tecnico` FK a Usuarios, `fecha` default
  `GETDATE()`, `descripcion`, `es_reparable` bit default 1,
  `tiempo_estimado_dias` nullable, `observaciones`).
- `Presupuestos` (`id_presupuesto` PK identity, `id_orden` FK a
  OrdenesServicio con indice unico `UX_Presupuesto_Orden` (un presupuesto por
  orden), `fecha_emision` default `GETDATE()`, `estado` default
  `'Pendiente'`, `subtotal`/`descuento`/`total` decimal(18,2),
  `dias_garantia`, `fecha_respuesta` nullable, `medio_respuesta` nullable,
  `motivo_rechazo` nullable, `observaciones`).
- `PresupuestoDetalle` (`id_detalle` PK identity, `id_presupuesto` FK a
  Presupuestos, `id_repuesto` nullable (reservado para stock CP3),
  `tipo_item` (`ManoObra`/`Servicio`), `descripcion`, `cantidad`,
  `precio_unitario` decimal(18,2), `subtotal`).
- `Entregas` (`id_entrega` PK identity, `id_orden` FK a OrdenesServicio,
  `fecha_entrega` default `GETDATE()`, `id_usuario` FK a Usuarios,
  `entregado_a`, `documento_receptor` nullable, `observaciones`).

Sin cambios en tablas existentes (Clientes/Equipos/Usuarios intactas).

### Esquema resumido

```text
Clientes 1 ----< * OrdenesServicio * >---- 1 Equipos
OrdenesServicio >---- 1 Usuarios (id_tecnico_asignado, nullable)
OrdenesServicio >---- 0..1 OrdenesServicio (id_orden_origen, garantia CP4)
OrdenesServicio 1 ----< * HistorialOrdenes >---- 1 Usuarios
OrdenesServicio 1 ---- 0..1 Diagnosticos
OrdenesServicio 1 ---- 0..1 Presupuestos 1 ----< * PresupuestoDetalle
OrdenesServicio 1 ---- 0..1 Entregas
Bitacora(tipo_actividad: ... + ORDENES)
```

Flujo de estados CP2:

```text
Recibido -> EnDiagnostico -> PendientePresupuesto -> EsperandoRespuesta
    |              |                   |                      |
    |              +-> ListoRetiro     |                      +-> AutorizadoReparacion
    |              (NoReparable)       |                      +-> ListoRetiro (PresupuestoRechazado)
    +-> ListoRetiro (Cancelado, desde Recibido/EnDiagnostico/Pendiente/
                     Esperando/Autorizado) -> Entregado
```

### Capas

- DOMAIN (`Features/Ordenes`, 11 archivos): 5 clases de constantes
  (`EstadoOrdenServicio` con 10 estados, `ResultadoOrdenServicio` con 5
  resultados, `TipoOrden` con Normal/Garantia, `EstadoPresupuesto` con
  Borrador/Pendiente/Aprobado/Rechazado, `TipoItemPresupuesto` con
  ManoObra/Servicio) + 6 entidades (`OrdenServicio` con transiciones
  `IniciarDiagnostico`/`MarcarPendientePresupuesto`/`MarcarEsperandoRespuesta`/
  `AutorizarReparacion`/`MarcarNoReparable`/`MarcarPresupuestoRechazado`/
  `Cancelar`/`Entregar` + `AsignarTecnico`/`ActualizarRecepcion` sin cambio de
  estado, `HistorialEstadoOrden`, `Diagnostico`, `Presupuesto` con
  `Aprobar`/`Rechazar` y validacion `total = subtotal - descuento`,
  `DetallePresupuesto` con `Subtotal = cantidad * precio`,
  `Entrega`). Constructores `CrearNuevo` con validacion via
  `ReglaNegocioException` y `CargarDesdeDB` sin validacion de negocio.
- APPLICATION: `OrdenServicioService.cs` (CrearOrden con validacion
  cliente/equipo activos y pertenencia equipo-cliente, AsignarTecnico,
  IniciarDiagnostico, FinalizarDiagnostico reparable/no, EmitirPresupuesto
  con `List<DetallePresupuesto>`, RegistrarAprobacion/Rechazo, CancelarOrden,
  EntregarOrden, ModificarRecepcion, Listar/ListarPorCliente, ObtenerPorId,
  ObtenerDiagnostico/Presupuesto/Entrega, ListarDetalle/ListarHistorial,
  ListarTecnicosElegibles; bitacora `ORDENES` por operacion);
  `BitacoraService` suma el tipo `ORDENES`.
- INFRASTRUCTURE: 5 repositories (`OrdenServicioRepository` con
  `CrearConHistorial` y `CambiarEstadoConHistorial` en el mismo batch,
  `DiagnosticoRepository` con `CrearConTransicion`,
  `PresupuestoRepository` con `EmitirConDetalle`/`AprobarConTransicion`/
  `RechazarConTransicion` + `ListarDetalle`, `HistorialEstadoOrdenRepository`,
  `EntregaRepository` con `CrearConTransicion`); `PermisoRepository` (seed de
  6 permisos, familia `Gestion ordenes`, composiciones Administrador/Lectura
  y traducciones `Bitacora.ORDENES` ES/EN); `IdiomaRepository` (212 seeds
  ES/EN de ordenes, ver Traducciones).
- UI: `FrmOrdenesServicio` (+ Designer; grilla con filtros por cliente,
  estado y texto numero/problema, `CHK_Entregadas`, botones Crear/Ver
  detalle/Cancelar/Entregar con confirmacion y control de permisos);
  `FrmOrdenServicioDetalle` (+ Designer; dialogo modal con modo nuevo
  —solo recepcion + Crear— y modo detalle con 5 tabs: Recepcion,
  Diagnostico, Presupuesto, Historial, Entrega; fila de item editable simple
  con controles inline + grilla, sin dialogo de item aparte);
  `FrmPrincipal` con `TSMI_Ordenes` en `Gestion` (visible/habilitado por
  `ORDENES_VER`, traduccion por observer); `Program.Main` inicializa
  `OrdenServicioService`.
- ABSTRACTIONS: sin cambios.
- SERVICES: sin cambios (se reutilizan `SessionManager` y `SesionIdioma`).

### Archivos existentes modificados

- `APPLICATION/APPLICATION.csproj`
- `APPLICATION/Features/Bitacora/BitacoraService.cs`
- `DOMAIN/DOMAIN.csproj`
- `DOMAIN/Features/Permisos/CodigosPermiso.cs`
- `INFRASTRUCTURE/REPOSITORY.csproj`
- `INFRASTRUCTURE/Features/Idiomas/IdiomaRepository.cs`
- `INFRASTRUCTURE/Features/Permisos/PermisoRepository.cs`
- `PRESENTATION/Program.cs`
- `PRESENTATION/UI.csproj`
- `PRESENTATION/Forms/FrmPrincipal.cs` + `FrmPrincipal.Designer.cs`

### Archivos nuevos

- `DOMAIN/Features/Ordenes/EstadoOrdenServicio.cs`
- `DOMAIN/Features/Ordenes/ResultadoOrdenServicio.cs`
- `DOMAIN/Features/Ordenes/TipoOrden.cs`
- `DOMAIN/Features/Ordenes/EstadoPresupuesto.cs`
- `DOMAIN/Features/Ordenes/TipoItemPresupuesto.cs`
- `DOMAIN/Features/Ordenes/OrdenServicio.cs`
- `DOMAIN/Features/Ordenes/HistorialEstadoOrden.cs`
- `DOMAIN/Features/Ordenes/Diagnostico.cs`
- `DOMAIN/Features/Ordenes/Presupuesto.cs`
- `DOMAIN/Features/Ordenes/DetallePresupuesto.cs`
- `DOMAIN/Features/Ordenes/Entrega.cs`
- `APPLICATION/Features/Ordenes/OrdenServicioService.cs`
- `INFRASTRUCTURE/Features/Ordenes/OrdenServicioRepository.cs`
- `INFRASTRUCTURE/Features/Ordenes/HistorialEstadoOrdenRepository.cs`
- `INFRASTRUCTURE/Features/Ordenes/DiagnosticoRepository.cs`
- `INFRASTRUCTURE/Features/Ordenes/PresupuestoRepository.cs`
- `INFRASTRUCTURE/Features/Ordenes/EntregaRepository.cs`
- `PRESENTATION/Forms/Ordenes/FrmOrdenesServicio.cs` (+ Designer)
- `PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.cs` (+ Designer)

### Permisos (6 nuevos)

- `ORDENES_VER/CREAR/EDITAR/CANCELAR/ENTREGAR`, `PRESUPUESTOS_DECIDIR`.
- Familia nueva `Gestion ordenes` (colgada de `Administrador`; `ORDENES_VER`
  tambien cuelga de `Lectura general`).

### Traducciones (212 seeds ES/EN en IdiomaRepository + 2 en PermisoRepository)

Menu (`Menu.Ordenes`), titulos y filtros de `FrmOrdenesServicio`
(`Ordenes.*`), detalle (`OrdenDetalle.*`: cabecera Numero/Estado/Resultado/
Tipo/Fecha/Cliente/Equipo/Tecnico, 5 tabs, recepcion, diagnostico,
presupuesto con items/descuento/garantia/medio/motivo/totales, entrega),
estados (`Estado.*`: 7 estados CP2), resultados (`Resultado.*`), tipos
(`Tipo.Normal`, `TipoItem.*`), estados de presupuesto (`PresupuestoEstado.*`),
columnas (`Columna.Numero/Equipo/Tecnico/Estado/Resultado/Fecha/Descripcion/
Tipo/Cantidad/Precio/Subtotal/Anterior/Nuevo/Usuario/Observacion`), mensajes
de confirmacion/error (`Mensaje.ConfirmarCancelar/ConfirmarEntregar/
OrdenNoCancelable/OrdenNoEntregable/OrdenCamposObligatorios/
ItemCamposObligatorios`) y titulos de confirmacion. Mas `Bitacora.ORDENES`
(ES: "Ordenes" / EN: "Orders") en `PermisoRepository`.

### Decisiones

- `numero_orden = id_orden` del mismo batch (`INSERT` + `SCOPE_IDENTITY()` +
  `UPDATE numero_orden = @Id` en `CrearConHistorial`); el formato `OS-`
  es solo visual en el detalle (`"Orden #{0}"` / `"Order #{0}"`), no se
  persiste.
- Estado + historial en un mismo batch SQL sin UoW formal
  (`CambiarEstadoConHistorial`, `CrearConTransicion`, `EmitirConDetalle`,
  `AprobarConTransicion`, `RechazarConTransicion`); atomicidad indirecta
  (ver Limitaciones).
- `EmitirPresupuesto` recibe `List<DetallePresupuesto>` (entidades de
  dominio) sin DTO intermedio; el service valida tipos
  (`ManoObra`/`Servicio`, sin `Repuesto` hasta CP3) y calcula el subtotal.
- `Borrador` de presupuesto reservado: CP2 emite directo en `Pendiente`.
- `EnReparacion`/`EnPruebas`/`PendienteEvaluacionGarantia` (estados) y
  `Reparado`/`GarantiaNoCubierta` (resultados) existen como constantes pero
  sin transiciones en CP2 (reservados para reparacion/garantia futuros);
  `Garantia` (`TipoOrden`) y `id_orden_origen`/`id_repuesto` quedan
  reservados para CP4/CP3.
- Tecnico elegible = usuario activo + `ORDENES_EDITAR` efectivo
  (`ListarTecnicosElegibles`/`ValidarTecnicoElegible` por permisos efectivos,
  no por rol).
- `AsignarTecnico`/`ModificarRecepcion` no escriben historial (solo bitacora);
  el historial registra altas y cambios de estado.
- Fila de item editable simple (controles inline tipo/descripcion/cantidad/
  precio + grilla + Agregar/Quitar) sin dialogo `FrmPresupuestoItemEditar`.
- Crear dentro del detalle en modo nuevo (id 0, solo recepcion + Crear orden);
  sin dialogo de alta separado.
- Paquetes: ninguno nuevo.

### Delegaciones

- 4 exploradores (repo, esquema, permisos/idiomas, UI base): base del disenio
  (OK).
- Backend (entidades, repositories, service, permisos, bitacora): OK,
  verificado funcional.
- Dump de esquema (tablas/columnas/FKs desde SQL): OK, consistente con los
  repositories.
- UI (lista + detalle 5 tabs + menu + 212 seeds + init): OK, verificado
  visual.
- Revision (cruce backend/UI/DB): 1 BLOCKER encontrado y corregido
  (`BTN_Emitir` no se habilitaba tras agregar/quitar item), 3 pendientes
  info/minor documentados abajo (OK).
- Testing (funcional 72/72 + visual 22/22 + regresion CP1): PASS.

### Revisiones cruzadas

- BLOCKER corregido: `BTN_Emitir` quedaba deshabilitado tras
  agregar/quitar item (el `Enabled` dependia de `_itemsNuevo.Count` pero no
  se reevaluaba). Fix: 2 lineas `AplicarPermisosDetalle()` al final de
  `BTN_AgregarItem_Click` y `BTN_QuitarItem_Click` en
  `FrmOrdenServicioDetalle.cs`. Re-test: PASS.
- Pendientes info/minor (no bloquean, quedan para proximo checkpoint):
  1. `PNL_Filtros` 27px overflow bajo resize en `FrmOrdenesServicio`
     (cosmetico menor).
  2. `LBL_Cliente` overlap en EN (`Customer:` mas largo) en
     `FrmOrdenesServicio` (cosmetico menor).
  3. Cobertura ES/EN por service + observer en pantallas de ordenes sin
     recorrido exhaustivo control por control (info).

### Pruebas funcionales (harness temporal fuera del repo): 72/72 PASS

- Crear orden (cliente/equipo validos, activos, pertenencia equipo-cliente;
  rechazos con inactivo/inexistente/ajeno, problema vacio): PASS.
- Asignar tecnico (elegible activo con `ORDENES_EDITAR`; rechazos inactivo,
  sin permiso, orden entregada): PASS.
- Diagnostico reparable (-> `PendientePresupuesto`) y no reparable
  (-> `ListoRetiro` + `NoReparable`; motivo obligatorio; duplicado
  rechazado): PASS.
- Presupuesto emitir (items `ManoObra`/`Servicio`, subtotal/total,
  descuento > subtotal rechazado, unico por orden, exige diagnostico
  reparable): PASS.
- Aprobar (-> `AutorizadoReparacion`) y rechazar (-> `ListoRetiro` +
  `PresupuestoRechazado`, motivo obligatorio): PASS.
- Cancelar (motivo obligatorio; estados validos/invalidos): PASS.
- Entregar (solo `ListoRetiro`, receptor obligatorio; `Entregas` 1 fila):
  PASS.
- Invalidas (transiciones fuera de orden, ids inexistentes): PASS con
  `ReglaNegocioException`.
- Historial (alta + cada transicion con usuario/observacion): PASS.
- Bitacora tipo `ORDENES` en cada operacion: PASS.

### Pruebas visuales (harness STA + UI real): 22/22 PASS

- `FrmOrdenesServicio` abre (vacio + con datos), filtros cliente/estado/
  texto, `CHK_Entregadas`, `DrawToBitmap` OK, resize OK.
- `FrmOrdenServicioDetalle` abre en modo nuevo y en detalle por cada tab
  (Recepcion/Diagnostico/Presupuesto/Historial/Entrega), `DrawToBitmap` OK.
- ES->EN->ES en ambas pantallas sin excepcion.
- `TSMI_Ordenes` visible/habilitado por `ORDENES_VER`; acceso denegado sin
  permiso.
- 2 cosmeticos menores pendientes (ver Problemas): no bloquean.

### Pruebas de regresion (CP1): PASS

- Login valido/invalido/logout, ES<->EN por service y observer,
  `VerificarIntegridadUsuarios`, CRUD clientes/equipos/catalogos,
  baja/reactivacion de usuarios: PASS sin cambios.

### Herramienta visual

Harness STA temporal fuera del repo: instancia los forms reales de `UI.exe`
con sesion admin, vuelca arbol de controles, `DrawToBitmap` a PNG, prueba de
resize y `PerformClick` en Crear/Detalle/Cancelar/Entregar/Emitir/Aprobar/
Rechazar contra UI real. Mas smoke con `UI.exe` real (login admin/123, menu
`Gestion > Ordenes`).

### Problemas encontrados y corregidos

1. BLOCKER Emitir: `BTN_Emitir.Enabled = editaItems && _itemsNuevo.Count > 0`
   nunca se reevaluaba tras agregar/quitar item porque esos handlers no
   llamaban a `AplicarPermisosDetalle()`. Fix: 2 lineas (una llamada al
   final de cada handler). Re-test: PASS.
2. Cosmeticos menores pendientes (NO corregidos en este checkpoint):
   - `PNL_Filtros` 27px overflow bajo resize en `FrmOrdenesServicio`.
   - `LBL_Cliente` overlap en EN en `FrmOrdenesServicio`.

### Limitaciones

- Atomicidad indirecta: estado + historial van en el mismo batch SQL del
  repository, sin UoW/transaccion formal a nivel service; un fallo entre
  batches (p. ej. bitacora) no revierte la transicion.
- Sin tipeo SO/repuestos: items solo `ManoObra`/`Servicio`; `id_repuesto`
  nullable reservado para stock CP3.
- MDI no end-to-end en harness: el smoke verifica `TSMI_Ordenes` por
  permisos efectivos + handlers; la navegacion MDI completa queda a prueba
  humana.
- LocalDB vs Docker: el testing corrio contra LocalDB; en Docker pueden
  diferir collation/rutas de conexion.

### Datos de prueba

Limpieza posterior al testing: `DELETE` en una transaccion de todas las
filas de prueba (Entregas -> PresupuestoDetalle -> Presupuestos ->
Diagnosticos -> HistorialOrdenes -> OrdenesServicio). Tablas de ordenes
quedaron en 0 filas. Bitacora conserva el historial (no se purga por
trazabilidad).

### Pruebas humanas pendientes

1. Iniciar `UI.exe`, login admin/123, confirmar que `Gestion > Ordenes`
   abre `FrmOrdenesServicio`.
2. Crear una orden desde el detalle en modo nuevo (cliente + equipo +
   problema) y verificar numero `OS-` y estado `Recibido`.
3. Asignar un tecnico elegible y verificar que un usuario sin
   `ORDENES_EDITAR` no aparece como elegible.
4. Iniciar y finalizar un diagnostico reparable; verificar
   `PendientePresupuesto`.
5. Emitir un presupuesto con 2 items (mano de obra + servicio), descuento
   y garantia; verificar totales.
6. Aprobar el presupuesto (medio + observaciones) y verificar
   `AutorizadoReparacion`.
7. Crear otra orden, emitir presupuesto y rechazarlo con motivo; verificar
   `ListoRetiro` + `PresupuestoRechazado`.
8. Crear otra orden, finalizar diagnostico no reparable; verificar
   `ListoRetiro` + `NoReparable`.
9. Cancelar una orden en `Recibido` con motivo; intentar cancelar una
   entregada (debe fallar).
10. Entregar una orden en `ListoRetiro` (receptor obligatorio) y verificar
    estado `Entregado` y fila en `Entregas`.
11. Probar operaciones invalidas (emitir sin diagnostico, aprobar sin
    presupuesto, entregar sin `ListoRetiro`) y confirmar mensajes.
12. Abrir el tab Historial y verificar alta + transiciones con usuario y
    fecha.
13. Cambiar idioma ES<->EN con ambas pantallas abiertas y verificar
     traduccion de filtros, tabs, botones y columnas.
14. Regresion CP1: clientes/equipos/catalogos, baja/reactivacion de
     usuarios, bitacora e integridad (`Recalcular DV`).

### Usuarios modelo en seed

- Usuarios agregados al seed (password "123", activo=1, idempotentes): recepcionista, tecnico, encargado. Archivos: APPLICATION/Features/Usuarios/UsuarioService.cs (+3 CrearUsuarioBase), INFRASTRUCTURE/Features/Usuarios/UsuarioPermisoRepository.cs (familias para encargado + simples directos para recepcionista/tecnico/BITACORA_VER).
- Matriz: recepcionista = CLIENTES_VER/CREAR/EDITAR, EQUIPOS_VER/CREAR/EDITAR, TIPOS_EQUIPO_VER, MARCAS_VER, ORDENES_VER/CREAR/EDITAR/ENTREGAR (sin DESACTIVAR/CANCELAR/DECIDIR); tecnico = CLIENTES_VER, EQUIPOS_VER, TIPOS_EQUIPO_VER, MARCAS_VER, ORDENES_VER/EDITAR; encargado = operativo completo (VER/CREAR/EDITAR/DESACTIVAR en clientes/equipos/tipos/marcas, ORDENES_VER/CREAR/EDITAR/CANCELAR/ENTREGAR, PRESUPUESTOS_DECIDIR) + BITACORA_VER, sin gestion de usuarios/permisos.
- Verificacion: build 0 errores; matriz efectiva comprobada por SQL con expansion de familias (encargado 23, recepcionista 12, tecnico 6); login 123 OK x3; idempotencia (doble Inicializar sin duplicados); tecnico en ListarTecnicosElegibles; VerificarIntegridadUsuarios()=true. Sin commit (lo hace el orquestador).

Familias de rol base 'Rol recepcionista' (12 simples), 'Rol tecnico' (6 simples) y 'Rol encargado' (anidada: Gestion clientes/equipos/catalogos/ordenes + BITACORA_VER), colgadas de Raiz, expandibles en futuros checkpoints; usuarios migrados a su rol con limpieza idempotente de asignaciones directas redundantes; matriz efectiva sin cambios (23/12/6); verificado build 0 errores, login x3, doble init sin duplicados, integridad true.

## Checkpoint 3

### Objetivo

Incorporar stock de repuestos con movimientos auditados e intervenciones de
reparacion con consumo de repuestos y pruebas de funcionamiento sobre ordenes
autorizadas, con bitacora tipo `REPUESTOS`, 4 permisos nuevos y submenu
`Inventario` visible segun permisos.

### Branch y origen

- Branch: `checkpoint-3-reparaciones` (trabajo local, sin upstream).
- Origen: `checkpoint-2-ordenes` en `a7d0755`
  ("feat(checkpoint-2): familias de rol recepcionista, tecnico y encargado en
  seed").
- Commits del checkpoint (locales, sin push):
  - `16c208c feat(reparaciones): backend de repuestos, stock, intervenciones
    y pruebas`
  - `bdad0a4 feat(ui): gestion de repuestos y pestanas de reparaciones y
    pruebas`
  - `592723c fix(i18n): semillas ES/EN de repuestos, reparaciones y estados
    EnReparacion/EnPruebas`
  - `docs(checkpoint-3): informe de implementacion de repuestos y
    reparaciones` (este commit).
- Working tree: limpio tras el commit (ver seccion GIT).
- Estado local/remoto: `origin` no tiene la rama
  `checkpoint-3-reparaciones`; la rama solo existe en local. NO se hizo push
  por decision del usuario.

### Tablas nuevas

Nuevas (creacion idempotente con `IF OBJECT_ID` + `ALTER`/`UX` defensivos;
FKs sin accion en cascada para conservar historia):

- `Repuestos` (`id_repuesto` PK identity, `codigo` unico
  `UX_Repuestos_Codigo`, `descripcion`, `stock_actual` int,
  `stock_minimo` int, `costo_actual` decimal(18,2),
  `precio_referencia` decimal(18,2), `activo` bit default 1).
- `MovimientosStock` (`id_movimiento` PK identity, `id_repuesto` FK a
  Repuestos, `fecha` default `GETDATE()`, `tipo`
  (`Compra`/`ConsumoReparacion`/`AjustePositivo`/`AjusteNegativo`),
  `cantidad`, `stock_anterior`, `stock_posterior`, `id_usuario` FK a
  Usuarios, `id_compra` nullable, `id_reparacion` FK nullable a
  Reparaciones, `observacion`).
- `Reparaciones` (`id_reparacion` PK identity, `id_orden` FK a
  OrdenesServicio, `numero_intervencion` int (secuencial por orden, ver
  Reglas), `id_usuario_tecnico` FK a Usuarios, `fecha_inicio` default
  `GETDATE()`, `fecha_fin` nullable, `trabajo_realizado` nullable,
  `observaciones` nullable; indice unico
  `UX_Reparaciones_Orden_Nro` (`id_orden`, `numero_intervencion`)).
- `ReparacionRepuesto` (PK compuesta (`id_reparacion`, `id_repuesto`),
  `cantidad`, `costo_unitario` decimal(18,2); costo historico por consumo,
  ver Reglas).
- `Pruebas` (`id_prueba` PK identity, `id_reparacion` FK a Reparaciones,
  `id_usuario_tecnico` FK a Usuarios, `fecha` default `GETDATE()`,
  `descripcion`, `resultado` (`Aprobada`/`RequiereRevision`),
  `observaciones` nullable).

Sin cambios en tablas existentes (Clientes/Equipos/Ordenes intactas;
presupuesto intacto, sin `TipoItem` Repuesto).

### Esquema resumido

```text
Repuestos 1 ----< * MovimientosStock >---- 0..1 Reparaciones
Repuestos * ----< * ReparacionRepuesto * >---- 1 Reparaciones
OrdenesServicio 1 ----< * Reparaciones >---- 1 Usuarios (tecnico)
Reparaciones 1 ----< * Pruebas
Bitacora(tipo_actividad: ... + REPUESTOS)
```

Flujo de estados CP3 (sobre CP2):

```text
AutorizadoReparacion -> EnReparacion -> EnPruebas -> ListoRetiro (Reparado)
EnPruebas -falla-> EnReparacion (nueva intervencion N+1)
EnReparacion/EnPruebas -cancelar-> ListoRetiro (Cancelado, cierra abierta)
```

### Capas

- DOMAIN (`Features/Repuestos` 3 archivos + `Features/Reparaciones`
  4 archivos): `Repuesto` (`CrearNuevo` con validacion de
  codigo/descripcion/stocks/costos, `CargarDesdeDB`, `Activo` con
  `Desactivar`/`Reactivar`), `MovimientoStock`, `TipoMovimientoStock`
  (`Compra`/`ConsumoReparacion`/`AjustePositivo`/`AjusteNegativo`);
  `Reparacion` (`Iniciar`/`Finalizar` con trabajo obligatorio),
  `ReparacionRepuesto` (cantidad + `costo_unitario` historico), `Prueba`
  (`CrearNuevo`), `ResultadoPrueba`
  (`Aprobada`/`RequiereRevision`). `OrdenServicio` suma
  `IniciarReparacion`/`FinalizarReparacion`/`RegistrarPruebaAprobada`
  (-> `ListoRetiro` + `Reparado`)/`RegistrarPruebaFallida`
  (-> `EnReparacion`); `Cancelar` extendida a `EnReparacion`/`EnPruebas`.
  `CodigosPermiso` suma 4 `REPUESTOS_*`.
- APPLICATION: `RepuestoService.cs` (Crear/Modificar/Desactivar/Reactivar/
  `AjustarStock` con motivo obligatorio/Listar/`ObtenerPorCodigo`/
  `ListarMovimientos(Filtros)`; bitacora `REPUESTOS` por operacion);
  `OrdenServicioService` extendido (`IniciarReparacion`/`ConsumirRepuesto`/
  `FinalizarReparacion`/`RegistrarPrueba` + `ListarReparaciones`/
  `ObtenerReparacion`/`ListarConsumidos`/`ListarPruebas`; `Cancelar`
  extendida con `CerrarAbiertaYCancelar`; bitacora `ORDENES` en
  reparaciones/pruebas); `BitacoraService` suma el tipo `REPUESTOS`.
- INFRASTRUCTURE: `RepuestoRepository` (Crear atomico con movimiento
  `AjustePositivo` de stock inicial, Modificar solo maestros sin stock,
  `AjustarStock` con motivo y sin negativo con `THROW`,
  Desactivar/Reactivar, `ObtenerPorCodigo`),
  `MovimientoStockRepository` (Inicializar + Listar con filtros);
  `ReparacionRepository` con 6 batches atomicos (`IniciarReparacion`,
  `IniciarIntervencionAdicional`, `ConsumirRepuesto` con `THROW` por
  stock, `FinalizarReparacion`, `RegistrarPrueba` con transicion de
  estado + historial, `CerrarAbiertaYCancelar`) + lecturas
  (`ObtenerPorId`/`ObtenerAbierta`/`ListarPorOrden`/`ListarConsumidos`/
  `ListarPruebas`/`ObtenerPruebaPorId`/`EsUltimaFinalizada`);
  `PermisoRepository` (seed de 4 permisos, familia `Gestion repuestos`,
  composiciones Administrador/Lectura, grants de Rol tecnico/encargado,
  traducciones `Bitacora.REPUESTOS`); `IdiomaRepository` (152 seeds
  ES/EN, ver Traducciones).
- UI: `Forms/Repuestos` (8 archivos: `FrmRepuestos` +
  `FrmRepuestoEditar` + `FrmAjusteStock` + `FrmMovimientosStock`, `.cs` +
  Designer); `FrmOrdenServicioDetalle` suma 2 tabs (`Reparaciones`:
  iniciar/consumir/finalizar con grillas; `Pruebas`: combo de
  intervencion + registrar aprobada/fallida); `FrmPrincipal` con submenu
  `Inventario > Repuestos` (visible/habilitado por `REPUESTOS_VER`,
  traduccion por observer); `Program.Main` inicializa `RepuestoService`
  + `ordenServicioService.InicializarReparaciones`.
- ABSTRACTIONS: sin cambios.
- SERVICES: sin cambios (se reutilizan `SessionManager` y `SesionIdioma`).

### Archivos existentes modificados

- `APPLICATION/APPLICATION.csproj`
- `APPLICATION/Features/Bitacora/BitacoraService.cs`
- `APPLICATION/Features/Ordenes/OrdenServicioService.cs`
- `DOMAIN/DOMAIN.csproj`
- `DOMAIN/Features/Ordenes/OrdenServicio.cs`
- `DOMAIN/Features/Permisos/CodigosPermiso.cs`
- `INFRASTRUCTURE/REPOSITORY.csproj`
- `INFRASTRUCTURE/Features/Idiomas/IdiomaRepository.cs`
- `INFRASTRUCTURE/Features/Permisos/PermisoRepository.cs`
- `PRESENTATION/Program.cs`
- `PRESENTATION/UI.csproj`
- `PRESENTATION/Forms/FrmPrincipal.cs` + `FrmPrincipal.Designer.cs`
- `PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.cs` +
  `FrmOrdenServicioDetalle.Designer.cs`
- `IMPLEMENTACION_SERVICIO_TECNICO.md` (este informe)

### Archivos nuevos

- `DOMAIN/Features/Repuestos/Repuesto.cs`
- `DOMAIN/Features/Repuestos/MovimientoStock.cs`
- `DOMAIN/Features/Repuestos/TipoMovimientoStock.cs`
- `DOMAIN/Features/Reparaciones/Reparacion.cs`
- `DOMAIN/Features/Reparaciones/ReparacionRepuesto.cs`
- `DOMAIN/Features/Reparaciones/Prueba.cs`
- `DOMAIN/Features/Reparaciones/ResultadoPrueba.cs`
- `APPLICATION/Features/Repuestos/RepuestoService.cs`
- `INFRASTRUCTURE/Features/Repuestos/RepuestoRepository.cs`
- `INFRASTRUCTURE/Features/Repuestos/MovimientoStockRepository.cs`
- `INFRASTRUCTURE/Features/Reparaciones/ReparacionRepository.cs`
- `PRESENTATION/Forms/Repuestos/FrmRepuestos.cs` (+ Designer)
- `PRESENTATION/Forms/Repuestos/FrmRepuestoEditar.cs` (+ Designer)
- `PRESENTATION/Forms/Repuestos/FrmAjusteStock.cs` (+ Designer)
- `PRESENTATION/Forms/Repuestos/FrmMovimientosStock.cs` (+ Designer)

### Permisos (4 nuevos)

- `REPUESTOS_VER/CREAR/EDITAR/DESACTIVAR`.
- Familia nueva `Gestion repuestos` (colgada de `Raiz` y de
  `Administrador`; `REPUESTOS_VER` tambien cuelga de
  `Lectura general`).
- Rol tecnico suma `REPUESTOS_VER`.
- Rol encargado suma `Gestion repuestos` anidada.
- Recepcionista sin cambios.
- Tipo de bitacora `REPUESTOS` (+ traducciones ES/EN en
  `PermisoRepository`).

### Traducciones (152 seeds ES/EN en IdiomaRepository + 2 en PermisoRepository)

Menu (`Menu.Inventario`/`Menu.Repuestos`), `FrmRepuestos` y filtros/botones
(`Repuestos.*`: codigo, descripcion, stocks, costos, movimientos, alertas),
tabs del detalle (`OrdenDetalle.TabReparaciones`/`TabPruebas`), estados
(`Estado.EnReparacion`/`Estado.EnPruebas`), resultado
(`Resultado.Reparado`), columnas y mensajes de reparaciones/pruebas/
movimientos. Mas `Bitacora.REPUESTOS` (ES: "Repuestos" / EN: "Spare
parts") en `PermisoRepository`.

### Reglas

- Intervenciones N° secuencial `MAX+1` por orden con lock
  (`UPDLOCK, HOLDLOCK`); indice unico (`id_orden`,
  `numero_intervencion`).
- Repeat-consumo acumula cantidad (`cantidad = cantidad + @Cantidad`);
  `costo_unitario` conserva el del primer consumo (costo historico).
- Ajustes con motivo obligatorio y sin dejar stock negativo (`THROW`).
- Consumo atomico con `THROW` (repuesto inexistente / stock
  insuficiente).
- Crear con stock inicial > 0 genera movimiento `AjustePositivo`
  ("Stock inicial"); `Modificar` no toca el stock.
- Prueba solo sobre la ultima intervencion finalizada y sin intervencion
  abierta en la orden.
- Tecnico de la intervencion = tecnico asignado de la orden (validado
  como elegible).
- Solo repuestos activos para consumir.
- Presupuesto intacto: sin `TipoItem` Repuesto.

### Decisiones

- Extender `OrdenServicioService` en lugar de crear un service nuevo: las
  intervenciones viven dentro del ciclo de la orden y reutilizan sus
  validaciones, sesion y bitacora `ORDENES`.
- Sin `TipoItem` Repuesto: el costo de repuestos no entra al presupuesto;
  el presupuesto queda intacto.
- Cancelar con intervencion abierta: `CerrarAbiertaYCancelar` en el mismo
  batch (cierra la abierta y cancela la orden) en lugar de exigir cierre
  manual previo.
- Intervenciones secuenciales N+1 con lock en lugar de identity global
  (numeracion legible por orden).
- Repeat-consumo acumula con el costo del primero (costo historico, no
  promedio).
- Submenu `Inventario` top-level en `Gestion` (no dentro de Ordenes) para
  futuro crecimiento de stock/compras.
- Bitacora con 1 solo tipo nuevo (`REPUESTOS`); reparaciones y pruebas
  bitacoran como `ORDENES`.
- Paquetes: ninguno nuevo.

### Delegaciones

- 4 exploradores (repo, esquema, permisos/idiomas, UI base): base del
  disenio (OK).
- Backend (entidades, repositories, services, permisos, bitacora): OK,
  verificado funcional.
- Dump de esquema (tablas/columnas/FKs desde SQL): OK, consistente con
  los repositories.
- UI (4 forms de repuestos + 2 tabs del detalle + submenu + seeds +
  init): OK con incidente — rate limit al devolver el trabajo; se cerro
  con auditoria de cierre (verificacion de archivos y build por el
  orquestador).
- Revision cruzada (backend/UI/DB): 0 blockers, 7 hallazgos menores/info
  (ver Revisiones cruzadas).
- Testing (funcional 76/76 + regresion 17 + visual 30/30): PASS.
- Micro-fix seeds (6 seeds `Estado.EnReparacion`/`EnPruebas` +
  `Resultado.Reparado`): OK, verificado en `fix(i18n)`.

### Revisiones cruzadas

- 0 blockers.
- 7 hallazgos menores/info (no bloquean, quedan documentados):
  1. Repeat-consumo conserva el costo del primero (documentado como
     regla; pendiente decidir si actualizar al ultimo).
  2. Cancelar con abierta sobrescribe la observacion de cierre.
  3. Race entre la lectura de estado en el service y el batch del
     repository (validacion de estado fuera del batch).
  4. Batches sin `WHERE` de estado (el estado lo valida el service, no
     el SQL).
  5. `EsUltimaFinalizada` sin usos (la ultima se calcula en el service).
  6. Seed `Columna.Numero` duplicado (clave repetida).
  7. Ajuste de stock permitido sobre repuesto inactivo.

### Pruebas funcionales (harness temporal fuera del repo): 76/76 PASS

- Repuestos crear/modificar/desactivar/reactivar, codigo unico,
  `ObtenerPorCodigo`: PASS.
- Ajustes positivo/negativo con motivo; motivo vacio y negativo bajo
  cero rechazados: PASS.
- Stock inicial genera `AjustePositivo`; `Modificar` no toca stock:
  PASS.
- Iniciar reparacion (autorizada -> `EnReparacion` N°1; abierta
  duplicada rechazada; sin tecnico rechazada): PASS.
- Consumir (descuenta stock, acumula repeat, inactivo/inexistente/
  insuficiente rechazados): PASS.
- Finalizar (trabajo obligatorio; exige `EnReparacion`; -> `EnPruebas`):
  PASS.
- Pruebas (solo ultima finalizada sin abierta; aprobada -> `ListoRetiro`
  + `Reparado`; fallida -> `EnReparacion`): PASS.
- Segunda intervencion N°2 tras falla; prueba sobre intervencion vieja
  rechazada: PASS.
- Cancelar en `EnReparacion`/`EnPruebas` cierra la abierta; motivo
  obligatorio: PASS.
- Invalidas (transiciones fuera de orden, ids inexistentes): PASS con
  `ReglaNegocioException`.
- Bitacora tipos `REPUESTOS`/`ORDENES` en cada operacion: PASS.

### Pruebas visuales (harness STA + UI real): 30/30 PASS (42 PNG)

- `FrmRepuestos`/`FrmRepuestoEditar`/`FrmAjusteStock`/
  `FrmMovimientosStock` abren (nuevo + editar con datos reales),
  `DrawToBitmap` OK, resize OK.
- `FrmOrdenServicioDetalle` tabs Reparaciones/Pruebas por estado
  (Autorizado/EnReparacion/EnPruebas/ListoRetiro), `DrawToBitmap` OK.
- ES->EN->ES sin excepcion; submenu `Inventario` visible por
  `REPUESTOS_VER`.
- 1 bug visual corregido en este checkpoint (ver Problemas).

### Pruebas de regresion (CP1+CP2): 17 checks PASS

- Login valido/invalido/logout, ES<->EN por service y observer,
  `VerificarIntegridadUsuarios`, CRUD clientes/equipos/catalogos,
  baja/reactivacion de usuarios, ciclo
  ordenes/diagnostico/presupuesto/entrega: PASS sin cambios.

### Herramienta visual

Harness STA temporal fuera del repo: instancia los forms reales de
`UI.exe` con sesion admin, vuelca arbol de controles, `DrawToBitmap` a
PNG (42), prueba de resize y `PerformClick` en Crear/Ajustar/Iniciar/
Consumir/Finalizar/RegistrarPrueba contra UI real. Mas smoke con `UI.exe`
real (login admin/123, menu `Gestion > Inventario > Repuestos` y detalle
de orden).

### Problemas encontrados y corregidos

1. Bug visual post-testing: faltaban 6 seeds (`Estado.EnReparacion`/
   `Estado.EnPruebas` ES/EN + `Resultado.Reparado` ES/EN) y los
   tabs/grillas mostraban la clave en lugar del texto. Fix: 6
   `AgregarSeed` en `IdiomaRepository` (commit `fix(i18n)`). Re-test
   visual: PASS.

### Limitaciones

- Atomicidad indirecta parcial: estado + historial + reparacion van en el
  mismo batch SQL del repository, sin UoW/transaccion formal a nivel
  service; un fallo entre batches (p. ej. bitacora) no revierte la
  transicion (heredado de CP2).
- Dropdowns de consumo/prueba no capturados control por control en el
  harness (cubiertos por service + smoke de tabs).
- Estado `EnPruebas` verificado por service y smoke de tab, sin PNG
  dedicado en la corrida inicial (cubierto en re-test post-fix).
- `RecalcularDV` por menu no fue clickeado en el harness (requiere
  confirmacion modal); se cubrio via `IntegridadService` directo: PASS
  (heredado CP1/CP2).
- ES/EN parcial: recorrido por service + observer en pantallas nuevas,
  sin control-por-control exhaustivo.
- MDI a prueba humana: el smoke verifica visibilidad por permisos +
  handlers; la navegacion MDI completa queda a prueba humana.
- Orden de limpieza FK en testing (prefijo CP3T): el borrado debe
  respetar `Pruebas` -> `ReparacionRepuesto` -> `Reparaciones` ->
  `MovimientosStock` -> `Repuestos` -> resto del ciclo.

### Datos de prueba

Limpieza posterior al testing: `DELETE` en una transaccion de todas las
filas con prefijo CP3T (`Pruebas` -> `ReparacionRepuesto` ->
`Reparaciones` -> `MovimientosStock` -> `Repuestos`, luego resto del
ciclo). Tablas CP3 quedaron en 0 filas (0 restos). Bitacora conserva el
historial (no se purga por trazabilidad).

### Pruebas humanas pendientes

1. Crear un repuesto con stock inicial y verificar el movimiento
   `AjustePositivo` en Movimientos.
2. Ajustar stock positivo/negativo con motivo; intentar un ajuste que
   deje negativo (debe fallar).
3. Intentar consumir con stock insuficiente (debe fallar con mensaje).
4. Iniciar reparacion en una orden autorizada y verificar N°1 y estado
   `EnReparacion`.
5. Consumir un repuesto activo y verificar descuento de stock y
   movimiento `ConsumoReparacion`.
6. Repetir el consumo del mismo repuesto y verificar que acumula
   cantidad con costo historico.
7. Finalizar la reparacion con trabajo realizado y verificar
   `EnPruebas`.
8. Registrar una prueba fallida y verificar el retorno a
   `EnReparacion`.
9. Iniciar la segunda intervencion y verificar N°2.
10. Registrar una prueba aprobada y verificar `ListoRetiro` +
    `Reparado`.
11. Entregar la orden reparada y verificar la fila en `Entregas`.
12. Probar operaciones invalidas (iniciar sin tecnico, consumir
    inactivo, prueba en intervencion vieja) y confirmar mensajes.
13. Cambiar idioma ES<->EN con repuestos y detalle abiertos y verificar
    traduccion completa.
14. Regresion CP1+CP2: clientes/equipos/catalogos,
    ordenes/diagnostico/presupuesto/entrega, bitacora e integridad
    (`Recalcular DV`).

### Ajuste: quitar consumo con devolucion de stock

- Service `OrdenServicioService.QuitarConsumo(idReparacion, idRepuesto,
  cantidad)`: solo sobre reparacion abierta (`FechaFin` nula) y orden en
  `EnReparacion`; valida cantidad > 0, fila existente en
  `ReparacionRepuesto` y cantidad <= consumida; delega el batch y
  bitacora `ORDENES` (`CONSUMO_DEVUELTO`).
- Batch `ReparacionRepository.DevolverConsumo`: valida fila/cantidad con
  `THROW` (50003 sin consumo, 50004 exceso); si cantidad = consumida hace
  `DELETE` total, si es parcial hace `UPDATE cantidad - @Cantidad`;
  devuelve stock (`stock_actual + @Cantidad` con `UPDLOCK, HOLDLOCK`) y
  registra movimiento `DevolucionConsumo` con `stock_anterior/posterior`.
- Constante `TipoMovimientoStock.DevolucionConsumo` + validacion en
  `MovimientoStock`; filtro `DevolucionConsumo` en
  `FrmMovimientosStock` + seeds ES/EN (`MovimientoTipo.DevolucionConsumo`,
  `OrdenDetalle.QuitarConsumo`, `OrdenDetalle.CantidadDevolver`,
  `Mensaje/Titulo.ConfirmarDevolucion`).
- UI: `BTN_QuitarConsumo` en el tab Reparaciones (cantidad a devolver
  <= consumida + confirmacion); visible/habilitado solo con reparacion
  abierta y orden en `EnReparacion`.

### Ajuste: presupuestos 1:N (Original + Adicionales)

- Nuevo `DOMAIN/Features/Ordenes/TipoPresupuesto.cs`
  (`Original`/`Adicional`); `Presupuesto` suma `Tipo` (validado en
  `CrearNuevo`/`CargarDesdeDB`); `ObtenerPorOrden` queda como compatibilidad
  (devuelve el Original); nuevos `ObtenerOriginal`, `ListarAdicionales`,
  `ExisteAdicionalPendiente`, `CalcularMontoAutorizado` (SUM de `total`
  con `estado = 'Aprobado'`, calculado, no persistido).
- Migracion idempotente en `PresupuestoRepository.Inicializar`: columna
  `tipo` (`DEFAULT 'Original'`), `DROP` de `UX_Presupuesto_Orden` y nuevo
  indice filtrado `UX_Presupuesto_Original` (`id_orden WHERE tipo =
  'Original'`); ver Fix bloqueante.
- Flujo: `SolicitarAdicional` (`EnReparacion`/`EnPruebas` ->
  `PendientePresupuesto`, exige Original aprobado y sin adicional
  pendiente); `EmitirAdicional` atomico directo a `Pendiente` + orden a
  `EsperandoRespuesta` (reutiliza `EmitirConDetalle`); `AprobarAdicional`
  -> orden a `EnReparacion` (NO `Autorizado`); `RechazoAdicional` ->
  orden a `ListoRetiro` + `PresupuestoRechazado`, cerrando la reparacion
  abierta (`fecha_fin = GETDATE()`) y conservando consumos, pruebas e
  historial.
- Guards Original-vs-Adicional: aprobar/rechazar/emision original usan el
  medio correspondiente (`ReglaNegocioException` cruzada); items validados
  por helper comun (`ManoObra`/`Servicio`, subtotal calculado).
- UI: selector `CBO_SelectorPresupuesto` (Original / Adicionales / nuevo),
  `LBL_MontoAutorizado` (`OrdenDetalle.MontoAutorizado`), boton
  `BTN_SolicitarAdicional` + emitir/aprobar/rechazar segun tipo; seeds ES/EN
  (`OrdenDetalle.SelectorPresupuesto/MontoAutorizado/NuevoAdicional/
  PresupuestoNuevo/SolicitarAdicional`, `Tipo.Original/Adicional`).

### Fix bloqueante: `Inicializar()` en 2 batches (Error 207)

- Causa: SQL Server compila el batch completo antes de ejecutarlo; el batch
  unico con `CREATE UNIQUE INDEX ... WHERE tipo` fallaba con Error 207
  (`Invalid column name 'tipo'`) en DBs viejas sin la columna.
- Fix: `Inicializar()` dividido en 2 `ExecuteTransaction` (2 compilaciones):
  fase 1 crea/migra todo lo que NO referencia `tipo` (incluye `ALTER ADD
  tipo` + `DROP` del UX viejo); fase 2 crea `UX_Presupuesto_Original`
  cuando la columna ya existe. Todo idempotente.
- Verificado: reproduce 207 en scratch legacy, fix 13/13, real idempotente,
  flujo re-test identico al baseline.

### Decisiones del ajuste

- `EmitirAdicional` directo a `Pendiente` sin flujo Borrador en dos pasos:
  se reutiliza el camino atomico del Original (INSERT + detalle + orden a
  `EsperandoRespuesta` + historial) para no duplicar estados intermedios.
- Pausa impuesta por la maquina de estados: solicitar el adicional lleva la
  orden a `PendientePresupuesto` y emitirlo a `EsperandoRespuesta`; la
  reparacion queda pausada hasta aprobar (retoma `EnReparacion`) o rechazar
  (cierra a `ListoRetiro`).
- Rechazo del adicional cierra la abierta y conserva todo (consumos,
  pruebas, historial); no se borra trabajo realizado.
- Repeat-consumo sin cambios (acumula cantidad, costo del primero).
- Precio vs costo: `PresupuestoDetalle.PrecioUnitario` = comercial autorizado
  (lo que paga el cliente); `ReparacionRepuesto.CostoUnitario` = interno
  (costo historico del consumo); el consumo no aumenta el presupuesto.

### Pruebas del ajuste (harness temporal fuera del repo)

- Quitar consumo 13/13 PASS (total/parcial, validaciones fila/cantidad,
  stock y movimiento `DevolucionConsumo` con anterior/posterior).
- Escenarios: C 17/17 + D 9/9 + E 4/4 + F 12/13 (1 aclarado) + G 8/10
  (2 aclarados, artefactos del harness) + visual 11/11 + regresion PASS.
- Bug visual previo: n/a en este ajuste. 3 FAIL pre-existentes son deuda del
  harness (asserts), no del producto.
- Datos: filas ADIC de prueba limpiadas; bitacora conservada (no se purga
  por trazabilidad).
- Delegaciones: backend, dump de esquema, UI, revision cruzada (1 fix:
  filtro `DevolucionConsumo` en `FrmMovimientosStock`), testing, fix de
  migracion.

### Limitaciones del ajuste

- Atomicidad indirecta (heredada CP2/CP3): estado + historial + presupuesto /
  reparacion van en el mismo batch del repository, sin UoW formal; un fallo
  entre batches (p. ej. bitacora) no revierte la transicion.
- Un solo adicional pendiente por orden (`ExisteAdicionalPendiente`
  bloquea el siguiente hasta responder el actual).
- MDI y `RecalcularDV` por menu a prueba humana (harness cubre service +
  smoke por permisos/handlers).
- ES/EN por service + observer en pantallas tocadas, sin recorrido exhaustivo
  control por control.

### Checklist humano del ajuste

1. Solicitar adicional en `EnReparacion` -> verificar
   `PendientePresupuesto`; emitir -> `EsperandoRespuesta`; aprobar ->
   retoma `EnReparacion` y el monto autorizado suma el adicional.
2. Rechazar un adicional -> verificar `ListoRetiro` +
   `PresupuestoRechazado`, abierta cerrada y consumos/pruebas conservados.
3. Quitar consumo parcial y total -> verificar stock devuelto y movimiento
   `DevolucionConsumo` en `FrmMovimientosStock`.
4. Probar invalidas: quitar sin fila o con cantidad mayor, solicitar sin
   Original aprobado o con pendiente existente, emitir/aprobar por el medio
   del tipo contrario (deben fallar con mensaje).

### Correccion: eliminar vs anular + cancelar solicitud

- Dominio: `EstadoPresupuesto.Anulado` (terminal, conserva historial) +
  `Presupuesto.MotivoAnulacion` + `Presupuesto.Anular(motivo)` (solo desde
  `Pendiente`/`Aprobado`/`Rechazado`, motivo obligatorio; `Borrador` nunca
  se anula, se elimina).
- Borrador como via alternativa (la emision atomica `Emitir*` queda intacta
  como via rapida): `Presupuesto.CrearBorrador` (sin transicion de orden) +
  `PresupuestoRepository.CrearBorrador` (INSERT borrador + detalle, sin tocar
  la orden) + `PublicarBorrador` (borrador -> `Pendiente` + orden a
  `EsperandoRespuesta` + historial) + `EliminarBorrador` (fisico
  detalle + cabecera, solo `Borrador`, con `THROW` 50010/50012).
- `AnularPresupuesto` con consecuencias por tipo/estado (batch atomico
  `AnularConTransicion`: presupuesto -> `Anulado` + motivo; orden/historial/
  cierre segun caso; `MontoAutorizado` = SUM de `total` con
  `estado = 'Aprobado'`, excluye solo anulados):
  - Original `Aprobado` sin reparaciones -> orden a `PendientePresupuesto`;
    con reparaciones -> `throw` (no se puede anular trabajo iniciado).
  - Original `Pendiente` -> orden a `PendientePresupuesto`.
  - Adicional `Pendiente` -> orden a `PendientePresupuesto`.
  - Adicional `Aprobado` sin actividad posterior
    (`HasActividadPosterior` por `FechaRespuesta`: reparaciones/movimientos/
    pruebas posteriores) -> orden a `PendientePresupuesto`; con actividad
    -> `throw`.
  - `Rechazado` (Original o Adicional) -> sin cambio de orden (la orden ya
    esta en `ListoRetiro`), solo historial de anulacion.
- Indice filtrado: `UX_Presupuesto_Original` ahora
  `WHERE tipo = 'Original' AND estado <> 'Anulado'` (varios Originales en
  historial si los previos estan anulados, uno solo activo;
  `ObtenerPorOrden`/`ObtenerOriginal`/`ExisteOriginalActivo` filtran igual).
- `CancelarSolicitudAdicional`: resuelve el origen recorriendo el historial
  hacia atras (ultimo estado distinto de `PendientePresupuesto`,
  `EnReparacion`/`EnPruebas`); bloquea si hay adicionales
  `Pendiente`/`Aprobado`/`Rechazado` no anulados (`THROW` 50014); borra
  adicionales `Borrador` fisicos; restaura la orden al origen +
  observacion en historial + bitacora; `PuedeCancelarSolicitud` para gating
  de UI.
- UI (`FrmOrdenServicioDetalle`): selector incluye anulados con motivo
  visible; botones guardar borrador / publicar borrador / eliminar borrador
  (con confirmacion), anular con motivo obligatorio + confirmacion, cancelar
  solicitud con confirmacion; gating: anular exige `PRESUPUESTOS_DECIDIR`,
  resto exige `ORDENES_EDITAR`. Seeds ES/EN: `GuardarBorrador`,
  `EliminarBorrador`, `Anular`, `CancelarSolicitud`, `MotivoAnulacion`,
  `PresupuestoEstado.Borrador/Anulado`, `ConfirmarEliminarBorrador`,
  `ConfirmarAnular`, `ConfirmarCancelarSolicitud` (+ titulos).
- Migracion: columna `motivo_anulacion` NULL + recreacion del indice con
  estado, en las 2 fases idempotentes existentes (`Inicializar`); registros
  viejos intactos (motivo NULL).
- Fix testing: `HasActividadPosterior` reutilizaba el mismo array de
  `SqlParameter` en 3 `ExecuteTransaction` (error "already contained" de
  `SqlHelper.AddRange`); fix de 4 lineas (fabrica `P1()` con un array nuevo
  por consulta); re-test 35/35.
- Bug #3063 (reporte de usuario, orden #3063): tras anular el Original,
  todos los botones quedaban deshabilitados y era imposible crear un Original
  nuevo. Causa: `editaItems` exigia `_presupuesto == null` sin modo
  nuevo-Original. Fix solo UI + 2 seeds: placeholder `(nuevo original)`
  (Id -2, `OrdenDetalle.NuevoOriginal` ES/EN) ofrecido solo si no hay
  Original activo + orden en `PendientePresupuesto` + sin pausa por solicitud
  (`PuedeCancelarSolicitud`); backend intacto (ya lo soportaba) y gating de
  `SolicitarAdicional` ya correcto. Testing 40/40 (orden #3063 preservada de
  solo lectura, end-to-end con clics reales, regresion ES/EN, limpieza FIX3).

### Decisiones de la correccion

- Sin flujo Borrador en dos pasos para emitir: la emision atomica directa a
  `Pendiente` queda intacta como via rapida; el borrador es solo alternativa
  de carga progresiva.
- Pausa por estados: publicar lleva a `EsperandoRespuesta`; anular/aprobar
  retoman segun el caso; cancelar solicitud restaura el origen
  (`EnReparacion`/`EnPruebas`).
- Rechazo de adicional cierra la abierta preexistente (regla ya
  documentada); la anulacion no borra trabajo: si hay actividad posterior,
  bloquea con `throw`.
- Precio vs costo sin cambios (comercial autorizado vs costo historico del
  consumo; el consumo no aumenta el presupuesto).

### Pruebas de la correccion (harness temporal fuera del repo)

- B 23/23 (borrador crear/publicar/eliminar + validaciones) + C 6/6
  (anulacion Original/Adicional por estado + `throw` con trabajo/actividad)
  + D 6/6 (cancelar solicitud: origen, bloqueos, borrado de borradores,
  restauracion) + visual PASS (4 PNG) + re-test 35/35 post-fix
  `HasActividadPosterior`.
- Bug + fix: reuse de `SqlParameter` en `HasActividadPosterior` (ver arriba).
- Datos: filas ANUL de prueba limpiadas (0 restos); bitacora conservada (no
  se purga por trazabilidad).
- Delegaciones: backend, UI, revision (confirma CancelarSolicitud + seed),
  testing.

### Limitaciones de la correccion

- Atomicidad indirecta (heredada CP2/CP3): presupuesto + orden + historial
  van en el mismo batch del repository, sin UoW formal; un fallo entre
  batches (p. ej. bitacora) no revierte la transicion.
- Un solo adicional pendiente por orden (se mantiene).
- MDI y `RecalcularDV` por menu a prueba humana.
- ES/EN por service + observer en pantallas tocadas, sin recorrido
  exhaustivo control por control.

### Checklist humano de la correccion

1. Crear un borrador, eliminarlo (verificar borrado fisico) y crear otro y
   publicarlo (verificar `Pendiente` + `EsperandoRespuesta`).
2. Anular un presupuesto `Pendiente` y uno `Aprobado` sin reparaciones
   (verificar motivo obligatorio y retorno a `PendientePresupuesto`).
3. Anular un Original erroneo y emitir un Original nuevo (verificar que el
   anterior queda como historial anulado y solo hay un activo).
4. Cancelar una solicitud de adicional desde `EnReparacion` y desde
   `EnPruebas` (verificar restauracion del origen).
5. Anular un adicional `Aprobado` sin actividad (verificar retorno) y con
   actividad posterior (debe fallar con mensaje).
6. Verificar que el monto autorizado excluye anulados.

### Correccion: adicional desde Autorizado + fix cancelar 3067 + gating Iniciar + anular/registrar pruebas

1. Adicional desde `AutorizadoReparacion`: solicitar/emitir/aprobar/cancelar/
   rechazar con retorno al origen (`Autorizado` -> `Autorizado`, resto ->
   `EnReparacion`); el monto autorizado suma el adicional; orden 3067 NO
   mutada (solo lectura de verificacion).
   - Dominio (`OrdenServicio`): `SolicitarAdicional` y
     `CancelarSolicitudAdicional` aceptan `AutorizadoReparacion` como origen
     valido ademas de `EnReparacion`/`EnPruebas`; nuevos `ReabrirPruebas`
     (`ListoRetiro`/`Reparado` -> `EnPruebas`) y `ReabrirReparacion`
     (`ListoRetiro`/`Reparado` -> `EnReparacion`, retrabajo por prueba
     fallida).
   - Service (`OrdenServicioService`): `TryObtenerOrigenSolicitud` incluye
     `AutorizadoReparacion` al recorrer el historial hacia atras;
     `AprobarAdicional` calcula `estadoDestino` (origen `Autorizado` ->
     `AutorizarReparacion()`, resto -> `EnReparacion`) y lo pasa al
     repository; `RechazarAdicional`/`CancelarSolicitudAdicional` ya
     restauraban el origen recorrido y ahora lo cubren tambien desde
     `Autorizado`.
   - Repository (`PresupuestoRepository.AprobarAdicionalConTransicion`):
     parametro nuevo `@EstadoDestino` (batch: adicional -> `Aprobado` +
     orden `EsperandoRespuesta` -> destino + historial con el destino).
   - UI: `puedeSolicitar` incluye `AutorizadoReparacion` (con Original
     aprobado y sin adicional pendiente, como antes).
2. Bug 3067 diagnosticado: NO era el origen por historial (ya tomaba la
   ultima transicion a `PendientePresupuesto`) sino el bloqueo por adicional
   `Aprobado`/`Rechazado` viejo; fix = cancelar solo bloquea `Pendiente`.
   Verificado read-only `PuedeCancelar(3067)=true`.
   - Service + repository (`CancelarSolicitudAdicional` /
     `PuedeCancelarSolicitud` + batch SQL `THROW` 50014): el bloqueo pasa de
     `IN ('Pendiente','Aprobado','Rechazado')` a `= 'Pendiente'`.
   - Fundamento: aprobar/rechazar saca a la orden de `PendientePresupuesto`,
     por lo que un `Aprobado`/`Rechazado` pertenece siempre a un ciclo
     anterior y no debe impedir cancelar la solicitud vigente.
   - Orden 3067 intacta: solo se ejecuto `PuedeCancelar` de lectura; ningun
     INSERT/UPDATE/DELETE sobre sus datos.
3. Iniciar Reparacion deshabilitado salvo `Autorizado` o
   `EnReparacion`-sin-abierta (el service ya lo exigia; era solo UI).
   - UI (`FrmOrdenServicioDetalle`): `puedeIniciarReparacion` pasa de
     `Autorizado || EnReparacion` a `Autorizado || (EnReparacion &&
     !hayAbierta)`; backend sin cambios (ya rechazaba reparacion abierta
     duplicada).
4. `AnularPrueba` (ultima no-anulada, motivo persistido en
   `motivo_anulacion`, `Entregado` bloquea; aprobada en
   `ListoRetiro`/`Reparado` -> `EnPruebas`) + registrar pruebas en
   `ListoRetiro`/`Reparado` (aprueba se queda, falla -> `EnReparacion`).
   Columna Motivo + `Resultado.Anulada` + seeds.
   - Dominio: `ResultadoPrueba.Anulada` + `Prueba.MotivoAnulacion` +
     `Prueba.Anular(motivo)` (motivo obligatorio, ya-anulada rechaza) +
     `CargarDesdeDB` con motivo; `OrdenServicio.ReabrirPruebas` y
     `ReabrirReparacion` (ver punto 1).
   - Service: `AnularPrueba(idPrueba, motivo)` (motivo obligatorio;
     inexistente/ya-anulada/`Entregado` rechazan; exige ser la ultima no
     anulada de la orden via `ListarPruebasPorOrden`; si era aprobada en
     `ListoRetiro`/`Reparado` reabre a `EnPruebas`; bitacora
     `PRUEBA_ANULADA`); `RegistrarPrueba` acepta `EnPruebas` (flujo
     existente con transicion) o `ListoRetiro`/`Reparado` (aprobada sin
     transicion, fallida con retrabajo a `EnReparacion`).
   - Repository (`ReparacionRepository`): columna `motivo_anulacion`
     NULL en `CREATE Pruebas` + `ALTER` defensivo; nuevos
     `RegistrarPruebaSinTransicion` (solo INSERT), `RegistrarPruebaConRetrabajo`
     (INSERT + `ListoRetiro` -> `EnReparacion` + historial) y `AnularPrueba`
     (UPDATE a `Anulada` + motivo, con reapertura a `EnPruebas` + historial
     si corresponde; `THROW` 50020/50021); nuevo `ListarPruebasPorOrden` +
     `MapearPrueba` centralizado (SELECTs incluyen `motivo_anulacion`).
   - UI: `BTN_AnularPrueba` (visible solo con permiso `ORDENES_EDITAR`,
     orden no nueva y ultima no-anulada en la vista actual; habilitado solo
     si la fila seleccionada es esa ultima y la orden no esta `Entregado`;
     confirmacion + motivo obligatorio); grilla de pruebas con columna
     `Motivo` (motivo solo en filas anuladas) y resultado traducido
     (`Anulada`/`Aprobada`/`Fallida`); `habilitaPrueba` incluye
     `ListoRetiro`/`Reparado`; precarga de pruebas por reparacion con
     fallback defensivo.
   - Seeds ES/EN (`IdiomaRepository`): `OrdenDetalle.AnularPrueba`,
     `Resultado.Anulada`, `Columna.Motivo`,
     `Mensaje.ConfirmarAnularPrueba`.

### Decisiones de la correccion (4 fixes)

- Retorno al origen: aprobar/cancelar/rechazar un adicional restauran el
  estado previo a la solicitud (`Autorizado` -> `Autorizado`,
  `EnReparacion`/`EnPruebas` -> `EnReparacion`); el rechazo de adicional
  mantiene la regla previa (cierra la abierta preexistente) y el monto
  autorizado suma el adicional aprobado.
- Solo-`Pendiente` bloquea cancelar: los `Aprobado`/`Rechazado` no anulados
  son historia de ciclos cerrados, no emision vigente; bloquear por ellos
  dejaba solicitudes vigentes incancelables (caso 3067).
- Reapertura conserva `Reparado` (observacion, sin cambio): `ReabrirPruebas`
  y `ReabrirReparacion` cambian solo el estado, no el resultado; la orden
  reabierta sigue marcada `Reparado` hasta la proxima prueba decisiva.
  Se documenta como LOW residual, no como defecto a corregir en este pase.
- Sin `Borrador` nuevo para pruebas: a diferencia de presupuestos (borrador
  como via alternativa), las pruebas se anulan directo con motivo
  obligatorio; no hay flujo publicar/eliminar para pruebas.

### Pruebas de la correccion (harness temporal fuera del repo)

- Servicios 86/86 PASS + visual 8/8 PASS + regresion PASS (CP1+CP2+CP3
  previos intactos).
- Datos P4 de prueba limpiados (0 restos); orden 3067 intacta (solo lectura
  `PuedeCancelar=true`); bitacora conservada (no se purga por trazabilidad).
- Delegaciones: backend, UI, revision (0 blockers, 5 LOW documentados abajo),
  testing.

### Limitaciones de la correccion

- Atomicidad indirecta (heredada CP2/CP3): orden + historial + prueba van en
  el mismo batch del repository, sin UoW formal; un fallo entre batches
  (p. ej. bitacora) no revierte la transicion.
- Residuales LOW de revision (0 blockers, documentados sin corregir en este
  pase): `Reparado` residual al reabrir (ver Decisiones); `Autorizado` no
  chequea abierta antes de solicitar (el service la exige al iniciar, no al
  solicitar); `idUsuario` sin usar en `RegistrarPruebaSinTransicion`;
  `catch` silencioso en la precarga de pruebas por reparacion (fallback a
  lista vacia); `motivo_anulacion` > 500 sin validacion previa (corte a nivel
  de columna).
- MDI y `RecalcularDV` por menu a prueba humana.
- ES/EN por service + observer en pantallas tocadas, sin recorrido
  exhaustivo control por control.

### Checklist humano de la correccion

1. Adicional desde `Autorizado`: solicitar -> emitir -> aprobar (verificar
   retorno a `Autorizado` y monto sumado) y luego rechazar/cancelar otro
   (verificar retorno al origen en cada caso).
2. Cancelar en 3067: verificar `PuedeCancelar=true` (boton habilitado) y
   cancelar una solicitud vigente (verificar restauracion del origen).
3. Iniciar deshabilitado: en `EnReparacion` con reparacion abierta verificar
   boton Iniciar deshabilitado; cerrarla y verificar que se habilita.
4. Anular prueba: anular la ultima (verificar motivo obligatorio, columna
   Motivo y reapertura a `EnPruebas` si era aprobada en `ListoRetiro`);
   intentar anular una anterior o en `Entregado` (debe fallar con mensaje).
5. Registrar en `ListoRetiro`/`Reparado`: aprobada se queda, fallida vuelve
   a `EnReparacion` (verificar historial de retrabajo).

### Cambio: pruebas por lote con finalizacion explicita

1. `RegistrarPrueba` solo persiste en `EnPruebas`: valida orden en
   `EnPruebas`, sin reparacion abierta y ultima intervencion finalizada;
   crea la prueba (`Aprobada` o `RequiereRevision`) sobre la ultima
   intervencion SIN mover el estado de la orden. Bitacora
   `PRUEBA_REGISTRADA` por cada registro.
   - Service (`OrdenServicioService.RegistrarPrueba`): exige
     `Estado == EnPruebas`; resuelve la ultima intervencion con
     `ObtenerUltimaIntervencion`; INSERT via
     `ReparacionRepository.RegistrarPrueba` (sin transicion de orden).
   - Repository (`ReparacionRepository.RegistrarPrueba`): solo INSERT en
     `Pruebas` + SELECT de retorno; sin cambio de estado ni historial.
2. `FinalizarPruebas` explicito decide por TODAS las no-anuladas de la
   ultima intervencion: exige orden en `EnPruebas`, sin reparacion abierta
   y ultima intervencion finalizada; filtra `Resultado != Anulada`; sin
   validas -> `throw`; todas `Aprobada` -> `ListoRetiro`/`Reparado`
   (`FinalizarPruebasAprobadas`); alguna `RequiereRevision` ->
   `EnReparacion` + `Resultado = NULL`
   (`FinalizarPruebasRequiereRevision`). Bitacora `PRUEBAS_FINALIZADAS`
   con conteo y resultado (`TodasAprobadas`/`RequiereRevision`).
   - Repository (`ReparacionRepository.FinalizarPruebas`): la orden
     `EnPruebas` -> destino + historial en el mismo batch.
3. `AnularPrueba` acotado: solo en `EnPruebas`, solo prueba no-anulada de
   la ultima intervencion, con motivo obligatorio + usuario/fecha; SIN
   reapertura (no mueve el estado de la orden). Bitacora `PRUEBA_ANULADA`.
   - Dominio (`Prueba.Anular(motivo, idUsuario)`): motivo y usuario
     obligatorios; ya-anulada rechaza; fija `Resultado = Anulada`,
     `MotivoAnulacion`, `FechaAnulacion = Now`, `IdUsuarioAnulacion`.
   - Columnas nuevas en `Pruebas`: `fecha_anulacion datetime NULL` +
     `id_usuario_anulacion int NULL` + `FK_Pruebas_UsuarioAnulacion`
     (`CREATE` + `ALTER` defensivo); `CargarDesdeDB`/`MapearPrueba` las
     propagan.
4. Limpieza de codigo muerto del modelo anterior:
   - Dominio (`OrdenServicio`): eliminados `RegistrarPruebaAprobada`,
     `RegistrarPruebaFallida`, `ReabrirPruebas`, `ReabrirReparacion`;
     agregados `FinalizarPruebasAprobadas` y
     `FinalizarPruebasRequiereRevision` (ambos exigen `EnPruebas`).
   - Repository (`ReparacionRepository`): eliminados
     `RegistrarPruebaSinTransicion`, `RegistrarPruebaConRetrabajo`,
     `ListarPruebasPorOrden`, `EsUltimaFinalizada`; `AnularPrueba` pierde
     el parametro `conReapertura` (UPDATE a `Anulada` + motivo +
     `fecha_anulacion`/`id_usuario_anulacion`, sin transicion de orden).
   - Bitacora: `PRUEBA_REGISTRADA` / `PRUEBA_ANULADA` /
     `PRUEBAS_FINALIZADAS` en vez de `Prueba aprobada` / `Prueba fallida`
     por registro.
5. UI (`FrmOrdenServicioDetalle` + Designer): boton `BTN_FinalizarPruebas`
   (visible con permiso `ORDENES_EDITAR`, orden no nueva; habilitado solo
   en `EnPruebas` editable con pruebas registradas; confirmacion +
   mensaje de destino tras finalizar); `habilitaPrueba` restringido a
   `EnPruebas`; gating de `AnularPrueba` mantenido (ultima no-anulada de
   la vista, orden no `Entregado`); grilla conserva columna `Motivo`;
   micro-fix de gating P1-P3 aplicado.
   - Seeds ES/EN (`IdiomaRepository`): `Boton.FinalizarPruebas`,
     `Mensaje.ConfirmarFinalizarPruebas`,
     `Titulo.ConfirmarFinalizarPruebas` y mensajes de destino.

### Decisiones del cambio (pruebas por lote)

- La ultima prueba NO decide: registrar N pruebas no mueve el estado; la
  decision es colectiva y explicita en `FinalizarPruebas`.
- Las anuladas NO participan: `FinalizarPruebas` filtra
  `Resultado != Anulada`; anular todo deja cero validas y el finalizar
  rechaza con `throw` (no hay destino por defecto).
- Sin rollback: una vez finalizado (`ListoRetiro` o `EnReparacion`), no se
  reabre el lote; el retrabajo requiere cerrar la reparacion e iniciar una
  intervencion nueva (`NumeroIntervencion + 1`) con sus propias pruebas.
- Retrabajo con intervencion nueva: `FinalizarPruebasRequiereRevision`
  vuelve a `EnReparacion` con `Resultado = NULL` (limpia el residual
  `Reparado` del modelo anterior); la proxima decision la toma el
  siguiente lote de la nueva intervencion.
- SUPERSEDED explicito: reemplaza y deja obsoleto el modelo anterior
  documentado en la correccion previa (registro en
  `ListoRetiro`/`Reparado` con decision por prueba + reapertura al anular
  con `ReabrirPruebas`/`ReabrirReparacion` y `conReapertura`). Ese
  comportamiento ya no existe en codigo; la presente subseccion es la
  referencia vigente.
- Precio vs costo: sin cambio; rige lo ya documentado (precio de venta en
  presupuesto vs costo de insumos en consumo, sin margen en este modulo).

### Pruebas del cambio (harness temporal fuera del repo)

- Servicios 38/38 PASS + invalidas 4/4 PASS + visual PASS + regresion PASS
  (CP1+CP2+CP3 previos intactos); 0 bugs.
- Datos TST de prueba limpiados (0 restos); bitacora conservada (no se
  purga por trazabilidad).
- Delegaciones: backend, UI, revision (0 blockers, 5 LOW), micro-fix
  (gating P1-P3), testing.

### Limitaciones del cambio

- Atomicidad indirecta (heredada CP2/CP3): orden + historial + pruebas van
  en el batch del repository, sin UoW formal; un fallo entre batches
  (p. ej. bitacora) no revierte la transicion.
- Sin finalizar parcial: `FinalizarPruebas` siempre evalua TODAS las
  no-anuladas de la ultima intervencion; no hay cierre por prueba
  individual ni por subconjunto.
- MDI y `RecalcularDV` por menu a prueba humana.
- ES/EN por service + observer en pantallas tocadas, sin recorrido
  exhaustivo control por control.

### Checklist humano del cambio

1. Registrar N pruebas en `EnPruebas` (verificar que el estado NO se
   mueve y que cada registro suma una fila con bitacora
   `PRUEBA_REGISTRADA`).
2. Anular una prueba (verificar motivo obligatorio, columnas
   usuario/fecha y que el estado NO se mueve ni reabre nada).
3. Finalizar con alguna `RequiereRevision` no-anulada (verificar destino
   `EnReparacion` con `Resultado NULL` e historial).
4. Iniciar intervencion nueva y registrar sus pruebas (verificar que el
   lote anterior no influye en la decision del lote nuevo).
5. Finalizar con todas `Aprobada` (verificar destino
   `ListoRetiro`/`Reparado`); anular todas y finalizar (verificar rechazo
   con mensaje por falta de pruebas validas).

### Cancelar entrega

- Contenido: `CancelarEntrega(idOrden, motivo)` en service + dominio +
  repository + UI. Solo sobre orden en `Entregado` con entrega registrada;
  motivo obligatorio. Destinos: `Entregado`/`Reparado` -> `EnPruebas` con
  `Resultado = NULL` (seguir probando); `Entregado` con otro resultado ->
  `ListoRetiro` con resultado intacto. Batch atomico
  `CancelarEntregaConTransicion`: `DELETE` fisico de `Entregas` + `UPDATE`
  de orden + `INSERT` en historial, con bitacora `ENTREGA_CANCELADA` en
  tipo `ORDENES`. UI: `BTN_CancelarEntrega` en el tab Entrega, visible y
  habilitado solo con permiso `ORDENES_ENTREGAR`, orden `Entregado` y
  entrega cargada; confirmacion + motivo obligatorio; al cancelar sin
  entrega, los campos del tab se limpian. La orden cancelada admite
  re-entrega normal. Seeds ES/EN: `OrdenDetalle.CancelarEntrega`,
  `Mensaje/Titulo.ConfirmarCancelarEntrega`.
- Decisiones: sin estado intermedio `ListoRetiro` para `Reparado`, porque
  la orden quedaria varada (nada la devolveria a `EnPruebas`); el destino
  directo `EnPruebas` conserva la capacidad de seguir probando.
  `DELETE` fisico de la entrega porque la `UNIQUE` por orden impediria la
  re-entrega; la trazabilidad queda en historial + bitacora
  (`ENTREGA_CANCELADA` con motivo). Gating con `ORDENES_ENTREGAR`
  (mismo permiso que entregar).
- Pruebas: servicios 67/67 + regresion 14/14 + visual 8/8, 0 bugs. Datos
  CENT de prueba limpiados; bitacora conservada (no se purga por
  trazabilidad). Delegaciones: backend, UI, revision (0 blockers),
  testing.
- Limitaciones: atomicidad indirecta (heredada CP2/CP3): orden + historial
  van en el mismo batch del repository, sin UoW formal; un fallo entre
  batches (p. ej. bitacora) no revierte la transicion. MDI y
  `RecalcularDV` por menu a prueba humana. ES/EN por service + observer
  en pantallas tocadas, sin recorrido exhaustivo control por control.

### Checklist humano de cancelar entrega

1. Cancelar en `Entregado`/`Reparado` -> verificar `EnPruebas` con
   resultado vacio, seguir probando y re-entregar.
2. Cancelar en `Entregado` no reparable -> verificar `ListoRetiro` con
   resultado intacto y re-entregar.

### Reabrir pruebas

- Contenido: `ReabrirPruebas(idOrden, motivo)` en service + dominio +
  repository + UI. Solo sobre orden en `ListoRetiro`/`Reparado` con motivo
  obligatorio; otros casos `throw`. Transicion `ListoRetiro`/`Reparado` ->
  `EnPruebas` con `Resultado = NULL` + `INSERT` en historial, con bitacora
  `PRUEBAS_REABIERTAS` en tipo `ORDENES`. Batch atomico
  `ReabrirPruebasConTransicion`: `UPDATE` de orden + `INSERT` en historial.
  UI: `BTN_ReabrirPruebas` ("Volver a pruebas") en el tab Entrega, visible
  y habilitado solo con permiso `ORDENES_EDITAR` y orden
  `ListoRetiro`/`Reparado`; confirmacion + motivo obligatorio; coexiste con
  `Entregar`/`CancelarEntrega` sin alterarlos. Sin cambios de esquema.
  Seeds ES/EN: `OrdenDetalle.ReabrirPruebas`,
  `Mensaje/Titulo.ConfirmarReabrirPruebas`.
- Decisiones: sin `ListoRetiro` intermedio, porque la orden ya esta alli y
  el objetivo es volver a probar; el destino directo `EnPruebas` conserva
  la capacidad de seguir probando. `NULL` explicito en `Resultado` porque
  `CambiarEstadoConHistorial` usa `COALESCE` y no puede escribir `NULL`;
  por eso el metodo propio `ReabrirPruebasConTransicion`. Gating con
  `ORDENES_EDITAR` (correccion tecnica, no entrega). Convive con
  `Entregar`/`CancelarEntrega`: cada boton mantiene su gating y la orden
  reabierta admite re-finalizar y entregar normal.
- Pruebas: servicios 21/21 + regresion 14/14 + visual 10/10, 0 bugs. Datos
  REAB de prueba limpiados; bitacora conservada (no se purga por
  trazabilidad). Delegaciones: backend, UI, revision (0 blockers),
  testing.
- Limitaciones: atomicidad indirecta (heredada CP2/CP3): orden + historial
  van en el mismo batch del repository, sin UoW formal; un fallo entre
  batches (p. ej. bitacora) no revierte la transicion. MDI y
  `RecalcularDV` por menu a prueba humana. ES/EN por service + observer
  en pantallas tocadas, sin recorrido exhaustivo control por control.

### Checklist humano de reabrir pruebas

1. Reabrir en `ListoRetiro`/`Reparado` -> verificar `EnPruebas` con
   resultado vacio, seguir probando y re-finalizar.
2. Re-finalizar y entregar normal (verificar historial de reapertura y
   entrega posterior).
