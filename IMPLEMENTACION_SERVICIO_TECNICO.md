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
