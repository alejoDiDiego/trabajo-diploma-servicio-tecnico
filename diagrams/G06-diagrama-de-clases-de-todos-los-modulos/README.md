# G06 - Diagrama de clases de todos los modulos (estado actual del repositorio)

Indice del entregable **G06**: nueve diagramas de clases PlantUML y sus nueve descripciones en
Markdown, reconstruidos contra el codigo fuente actual de este repositorio. El contenido esta en
español academico y conserva los identificadores exactos del codigo.

## Indice de los nueve pares

| N.º | Modulo documentado | Diagrama | Descripcion |
| --- | --- | --- | --- |
| G06.1 | `DOMAIN` (49 tipos) | [G06.1-domain.puml](G06.1-domain.puml) | [G06.1-domain.md](G06.1-domain.md) |
| G06.2 | `ABSTRACTIONS` (11 tipos) | [G06.2-abstractions.puml](G06.2-abstractions.puml) | [G06.2-abstractions.md](G06.2-abstractions.md) |
| G06.3 | `APPLICATION` (19 tipos) | [G06.3-application.puml](G06.3-application.puml) | [G06.3-application.md](G06.3-application.md) |
| G06.4.1 | `REPOSITORY` - tecnicos (9 tipos) | [G06.4.1-repository-tecnicos.puml](G06.4.1-repository-tecnicos.puml) | [G06.4.1-repository-tecnicos.md](G06.4.1-repository-tecnicos.md) |
| G06.4.2 | `REPOSITORY` - maestros (5 tipos) | [G06.4.2-repository-maestros.puml](G06.4.2-repository-maestros.puml) | [G06.4.2-repository-maestros.md](G06.4.2-repository-maestros.md) |
| G06.4.3 | `REPOSITORY` - ordenes (5 tipos) | [G06.4.3-repository-ordenes.puml](G06.4.3-repository-ordenes.puml) | [G06.4.3-repository-ordenes.md](G06.4.3-repository-ordenes.md) |
| G06.4.4 | `REPOSITORY` - stock, compras y garantias (6 tipos) | [G06.4.4-repository-stock-compras-garantias.puml](G06.4.4-repository-stock-compras-garantias.puml) | [G06.4.4-repository-stock-compras-garantias.md](G06.4.4-repository-stock-compras-garantias.md) |
| G06.5 | `SERVICES` (4 tipos) | [G06.5-services.puml](G06.5-services.puml) | [G06.5-services.md](G06.5-services.md) |
| G06.6 | `UI` / `PRESENTATION` (61 tipos) | [G06.6-ui.puml](G06.6-ui.puml) | [G06.6-ui.md](G06.6-ui.md) |

La subdivision de `REPOSITORY` en cuatro diagramas (G06.4.1 a G06.4.4) es **documental**: no crea
namespaces, carpetas fisicas ni proyectos nuevos. Los cuatro diagramas describen el mismo assembly
`REPOSITORY` de la carpeta fisica `INFRASTRUCTURE`.

## Leyenda de los diagramas

| Elemento | Significado |
| --- | --- |
| `class` | Clase concreta del proyecto; los miembros muestran visibilidad `+` publica, `#` protegida y `-` privada. |
| `abstract class` | Clase abstracta (por ejemplo `PermisoComponent`). |
| `interface` | Contrato de `ABSTRACTIONS` (no tiene implementacion en ese proyecto). |
| `<<static>>` | Clase estatica de constantes o de metodos (por ejemplo `EstadoOrdenServicio`, `DigitoVerificadorHelper`). |
| `package` | Carpeta/namespace real del proyecto; en el diagrama de UI cada paquete agrupa los formularios de una funcionalidad. |
| Clase anidada dibujada dentro de otra | Ayudante `private` declarado dentro del formulario: pertenencia por anidamiento, no composicion de datos. En G06.6 los ayudantes se declaran al nivel del paquete con su nombre calificado (`FrmX.Ayudante`), su alias y el estereotipo `<<nested_private>>`; el nombre calificado conserva la relacion de anidamiento lexico y no implica composicion de datos. |
| `<|--` | Generalizacion (herencia de clase o de contrato). |
| `<|..` | Realizacion de interfaz. |
| `o--` / `*--` | Agregacion / composicion **de objetos** cuando existe un campo o una lista real en el codigo. |
| `-->` | Asociacion o dependencia con navegacion real (campo, parametro o retorno). |
| `..>` | Dependencia de uso: instanciacion con `new`, llamada o referencia a un tipo externo. |
| `..` con etiqueta **FK conceptual** | Clave foranea de tabla entre agregados: identidad por id, **sin** navegacion, coleccion ni composicion en C#. |
| `note` | Aclaracion de una regla vigente o de un limite del diagrama. |

## Tipos propios por proyecto y por diagrama

| Proyecto (assembly) | Carpeta fisica | Archivos `Compile` semanticos | Tipos propios | Diagramas |
| --- | --- | --- | --- | --- |
| `DOMAIN` | `DOMAIN` | 44 archivos (+ `AssemblyInfo`) | 49 | G06.1 |
| `ABSTRACTIONS` | `ABSTRACTIONS` | 11 archivos (+ `AssemblyInfo`) | 11 | G06.2 |
| `APPLICATION` | `APPLICATION` | 19 archivos (+ `AssemblyInfo`) | 19 | G06.3 |
| `REPOSITORY` | `INFRASTRUCTURE` | 25 archivos (+ `AssemblyInfo`) | 25 | G06.4.1 a G06.4.4 |
| `SERVICES` | `SERVICES` | 4 archivos (+ `AssemblyInfo`) | 4 | G06.5 |
| `UI` | `PRESENTATION` | 57 archivos (+ `AssemblyInfo`, `Resources`, `Settings`) | 61 | G06.6 |
| **Total** |  |  | **169** | 9 diagramas |

## Inventario archivo fuente a tipo y diagrama

### DOMAIN - 44 archivos, 49 tipos (G06.1)

| Archivo | Tipo(s) |
| --- | --- |
| `DOMAIN/Exceptions/ReglaNegocioException.cs` | `ReglaNegocioException` |
| `DOMAIN/Features/Idiomas/Idioma.cs` | `Idioma` |
| `DOMAIN/Features/Idiomas/Palabra.cs` | `Palabra` |
| `DOMAIN/Features/Idiomas/Traduccion.cs` | `Traduccion` |
| `DOMAIN/Features/Permisos/CodigosPermiso.cs` | `CodigosPermiso` |
| `DOMAIN/Features/Permisos/FamiliaPermiso.cs` | `FamiliaPermiso` |
| `DOMAIN/Features/Permisos/PermisoComponent.cs` | `PermisoComponent` |
| `DOMAIN/Features/Permisos/PermisoSimple.cs` | `PermisoSimple` |
| `DOMAIN/Features/Clientes/Cliente.cs` | `Cliente`, `ClienteDocumentoDuplicadoException` |
| `DOMAIN/Features/TiposEquipo/TipoEquipo.cs` | `TipoEquipo` |
| `DOMAIN/Features/Marcas/Marca.cs` | `Marca` |
| `DOMAIN/Features/Equipos/Equipo.cs` | `Equipo` |
| `DOMAIN/Features/Ordenes/EstadoOrdenServicio.cs` | `EstadoOrdenServicio` |
| `DOMAIN/Features/Ordenes/ResultadoOrdenServicio.cs` | `ResultadoOrdenServicio` |
| `DOMAIN/Features/Ordenes/TipoOrden.cs` | `TipoOrden` |
| `DOMAIN/Features/Ordenes/EstadoPresupuesto.cs` | `EstadoPresupuesto` |
| `DOMAIN/Features/Ordenes/TipoItemPresupuesto.cs` | `TipoItemPresupuesto` |
| `DOMAIN/Features/Ordenes/TipoPresupuesto.cs` | `TipoPresupuesto` |
| `DOMAIN/Features/Ordenes/OrdenServicio.cs` | `OrdenServicio`, `OrdenOriginalSoloConsultaException` |
| `DOMAIN/Features/Proveedores/Proveedor.cs` | `Proveedor` |
| `DOMAIN/Features/Compras/EstadoCompra.cs` | `EstadoCompra` |
| `DOMAIN/Features/Compras/Compra.cs` | `Compra` |
| `DOMAIN/Features/Compras/DetalleCompra.cs` | `DetalleCompra` |
| `DOMAIN/Features/Garantias/Garantia.cs` | `Garantia`, `ReingresoGarantiaAbiertoException`, `ReingresoGarantiaOrigenNoDisponibleException` |
| `DOMAIN/Features/Garantias/EstadoEvaluacionGarantia.cs` | `EstadoEvaluacionGarantia` |
| `DOMAIN/Features/Garantias/EvaluacionGarantia.cs` | `EvaluacionGarantia` |
| `DOMAIN/Features/Ordenes/HistorialEstadoOrden.cs` | `HistorialEstadoOrden` |
| `DOMAIN/Features/Ordenes/Diagnostico.cs` | `Diagnostico` |
| `DOMAIN/Features/Ordenes/Presupuesto.cs` | `Presupuesto` |
| `DOMAIN/Features/Ordenes/DetallePresupuesto.cs` | `DetallePresupuesto`, `DetallePresupuestoInvalidoException` |
| `DOMAIN/Features/Ordenes/Entrega.cs` | `Entrega` |
| `DOMAIN/Features/Repuestos/TipoMovimientoStock.cs` | `TipoMovimientoStock` |
| `DOMAIN/Features/Repuestos/Repuesto.cs` | `Repuesto` |
| `DOMAIN/Features/Repuestos/MovimientoStock.cs` | `MovimientoStock` |
| `DOMAIN/Features/Reparaciones/ResultadoPrueba.cs` | `ResultadoPrueba` |
| `DOMAIN/Features/Reparaciones/Reparacion.cs` | `Reparacion` |
| `DOMAIN/Features/Reparaciones/ReparacionRepuesto.cs` | `ReparacionRepuesto` |
| `DOMAIN/Features/Reparaciones/Prueba.cs` | `Prueba` |
| `DOMAIN/Features/Usuarios/Exceptions/UsuarioNoExisteException.cs` | `UsuarioNoExisteException` |
| `DOMAIN/Features/Usuarios/Exceptions/UsuarioYaExisteException.cs` | `UsuarioYaExisteException` |
| `DOMAIN/Features/Usuarios/Usuario.cs` | `Usuario` |
| `DOMAIN/Features/ControlCambios/ControlCambio.cs` | `ControlCambio` |
| `DOMAIN/Features/Usuarios/UserDVH.cs` | `UserDVH` |
| `DOMAIN/Features/Bitacora/Bitacora.cs` | `EntradaBitacora` |
| `DOMAIN/Properties/AssemblyInfo.cs` | *(excluido: metadatos de ensamblado)* |

### ABSTRACTIONS - 11 archivos, 11 tipos (G06.2)

| Archivo | Tipo(s) |
| --- | --- |
| `ABSTRACTIONS/Entities/IEntity.cs` | `IEntity` |
| `ABSTRACTIONS/Features/Idiomas/IIdioma.cs` | `IIdioma` |
| `ABSTRACTIONS/Features/Idiomas/IObservado.cs` | `IObservado` |
| `ABSTRACTIONS/Features/Idiomas/IObservador.cs` | `IObservador` |
| `ABSTRACTIONS/Features/Idiomas/IPalabra.cs` | `IPalabra` |
| `ABSTRACTIONS/Features/Idiomas/ITraduccion.cs` | `ITraduccion` |
| `ABSTRACTIONS/Features/Idiomas/TraduccionEditable.cs` | `TraduccionEditable` |
| `ABSTRACTIONS/Features/Permisos/IPermisoComponent.cs` | `IPermisoComponent` |
| `ABSTRACTIONS/Features/Usuarios/IUsuario.cs` | `IUsuario` |
| `ABSTRACTIONS/Services/IEncryptionService.cs` | `IEncryptionService` |
| `ABSTRACTIONS/Services/IPasswordHasher.cs` | `IPasswordHasher` |
| `ABSTRACTIONS/Properties/AssemblyInfo.cs` | *(excluido)* |

### APPLICATION - 19 archivos, 19 tipos (G06.3)

| Archivo | Tipo(s) |
| --- | --- |
| `APPLICATION/Features/Bitacora/BitacoraService.cs` | `BitacoraService` |
| `APPLICATION/Features/Clientes/ClienteService.cs` | `ClienteService` |
| `APPLICATION/Features/Compras/CompraService.cs` | `CompraService` |
| `APPLICATION/Features/ControlCambios/ControlCambioService.cs` | `ControlCambioService` |
| `APPLICATION/Features/Equipos/EquipoService.cs` | `EquipoService` |
| `APPLICATION/Features/Garantias/GarantiaService.cs` | `GarantiaService` |
| `APPLICATION/Features/Idiomas/IdiomaService.cs` | `IdiomaService` |
| `APPLICATION/Features/Integridad/DigitoVerificadorHelper.cs` | `DigitoVerificadorHelper` |
| `APPLICATION/Features/Integridad/IntegridadService.cs` | `IntegridadService` |
| `APPLICATION/Features/Marcas/MarcaService.cs` | `MarcaService` |
| `APPLICATION/Features/Ordenes/OrdenServicioService.cs` | `OrdenServicioService` |
| `APPLICATION/Features/Permisos/PermisoService.cs` | `PermisoService` |
| `APPLICATION/Features/Proveedores/ProveedorService.cs` | `ProveedorService` |
| `APPLICATION/Features/Reportes/ReporteService.cs` | `ReporteService` |
| `APPLICATION/Features/Repuestos/RepuestoService.cs` | `RepuestoService` |
| `APPLICATION/Features/TiposEquipo/TipoEquipoService.cs` | `TipoEquipoService` |
| `APPLICATION/Features/Usuarios/Exceptions/DatosUsuarioIncorrectosException.cs` | `DatosUsuarioIncorrectosException` |
| `APPLICATION/Features/Usuarios/UsuarioPermisoService.cs` | `UsuarioPermisoService` |
| `APPLICATION/Features/Usuarios/UsuarioService.cs` | `UsuarioService` |
| `APPLICATION/Properties/AssemblyInfo.cs` | *(excluido)* |

### REPOSITORY - 25 archivos, 25 tipos (G06.4.1 a G06.4.4)

| Archivo | Tipo(s) | Diagrama |
| --- | --- | --- |
| `INFRASTRUCTURE/SQLHelper.cs` | `SqlHelper` | G06.4.1 |
| `INFRASTRUCTURE/Features/Usuarios/UsuarioRepository.cs` | `UsuarioRepository` | G06.4.1 |
| `INFRASTRUCTURE/Features/Usuarios/UsuarioPermisoRepository.cs` | `UsuarioPermisoRepository` | G06.4.1 |
| `INFRASTRUCTURE/Features/Permisos/PermisoRepository.cs` | `PermisoRepository` | G06.4.1 |
| `INFRASTRUCTURE/Features/Idiomas/IdiomaRepository.cs` | `IdiomaRepository` | G06.4.1 |
| `INFRASTRUCTURE/Features/ControlCambios/ControlCambioRepository.cs` | `ControlCambioRepository` | G06.4.1 |
| `INFRASTRUCTURE/Features/Integridad/IntegridadRepository.cs` | `IntegridadRepository` | G06.4.1 |
| `INFRASTRUCTURE/Features/Bitacora/BitacoraRepository.cs` | `BitacoraRepository` | G06.4.1 |
| `INFRASTRUCTURE/Features/Reportes/ReporteRepository.cs` | `ReporteRepository` | G06.4.1 |
| `INFRASTRUCTURE/Features/Clientes/ClienteRepository.cs` | `ClienteRepository` | G06.4.2 |
| `INFRASTRUCTURE/Features/Equipos/EquipoRepository.cs` | `EquipoRepository` | G06.4.2 |
| `INFRASTRUCTURE/Features/TiposEquipo/TipoEquipoRepository.cs` | `TipoEquipoRepository` | G06.4.2 |
| `INFRASTRUCTURE/Features/Marcas/MarcaRepository.cs` | `MarcaRepository` | G06.4.2 |
| `INFRASTRUCTURE/Features/Proveedores/ProveedorRepository.cs` | `ProveedorRepository` | G06.4.2 |
| `INFRASTRUCTURE/Features/Ordenes/OrdenServicioRepository.cs` | `OrdenServicioRepository` | G06.4.3 |
| `INFRASTRUCTURE/Features/Ordenes/HistorialEstadoOrdenRepository.cs` | `HistorialEstadoOrdenRepository` | G06.4.3 |
| `INFRASTRUCTURE/Features/Ordenes/DiagnosticoRepository.cs` | `DiagnosticoRepository` | G06.4.3 |
| `INFRASTRUCTURE/Features/Ordenes/PresupuestoRepository.cs` | `PresupuestoRepository` | G06.4.3 |
| `INFRASTRUCTURE/Features/Ordenes/EntregaRepository.cs` | `EntregaRepository` | G06.4.3 |
| `INFRASTRUCTURE/Features/Repuestos/RepuestoRepository.cs` | `RepuestoRepository` | G06.4.4 |
| `INFRASTRUCTURE/Features/Repuestos/MovimientoStockRepository.cs` | `MovimientoStockRepository` | G06.4.4 |
| `INFRASTRUCTURE/Features/Compras/CompraRepository.cs` | `CompraRepository` | G06.4.4 |
| `INFRASTRUCTURE/Features/Reparaciones/ReparacionRepository.cs` | `ReparacionRepository` | G06.4.4 |
| `INFRASTRUCTURE/Features/Garantias/GarantiaRepository.cs` | `GarantiaRepository` | G06.4.4 |
| `INFRASTRUCTURE/Features/Garantias/EvaluacionGarantiaRepository.cs` | `EvaluacionGarantiaRepository` | G06.4.4 |
| `INFRASTRUCTURE/Properties/AssemblyInfo.cs` | *(excluido)* |  |

### SERVICES - 4 archivos, 4 tipos (G06.5)

| Archivo | Tipo(s) |
| --- | --- |
| `SERVICES/Auth/SessionManager.cs` | `SessionManager` |
| `SERVICES/Idiomas/SesionIdioma.cs` | `SesionIdioma` |
| `SERVICES/Security/EncryptionService.cs` | `EncryptionService` |
| `SERVICES/Security/PasswordHasher.cs` | `PasswordHasher` |
| `SERVICES/Properties/AssemblyInfo.cs` | *(excluido)* |

### UI - 57 archivos, 61 tipos (G06.6)

| Archivo | Tipo(s) |
| --- | --- |
| `PRESENTATION/Program.cs` | `Program` |
| `PRESENTATION/Forms/FrmPrincipal.cs` | `FrmPrincipal` |
| `PRESENTATION/Forms/Auth/FrmLogin.cs` | `FrmLogin` |
| `PRESENTATION/Forms/Auth/FrmAdministrarUsuarios.cs` | `FrmAdministrarUsuarios` |
| `PRESENTATION/Forms/Auth/FrmAdministrarPermisos.cs` | `FrmAdministrarPermisos`, `NodoPermiso` (anidado) |
| `PRESENTATION/Forms/Auth/FrmAsignarPermisosUsuario.cs` | `FrmAsignarPermisosUsuario` |
| `PRESENTATION/Forms/Ordenes/FrmOrdenesServicio.cs` | `FrmOrdenesServicio`, `FilaOrden`, `ItemCliente`, `ItemEstado` (anidados) |
| `PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.cs` | `FrmOrdenServicioDetalle` y sus 12 ayudantes anidados |
| `PRESENTATION/Forms/Clientes/FrmClientes.cs` | `FrmClientes` |
| `PRESENTATION/Forms/Clientes/FrmClienteEditar.cs` | `FrmClienteEditar` |
| `PRESENTATION/Forms/Equipos/FrmEquipos.cs` | `FrmEquipos`, `FilaEquipo`, `ItemCliente` (anidados) |
| `PRESENTATION/Forms/Equipos/FrmEquipoEditar.cs` | `FrmEquipoEditar`, `ItemCliente` (anidado) |
| `PRESENTATION/Forms/Catalogos/FrmTiposEquipo.cs` | `FrmTiposEquipo` |
| `PRESENTATION/Forms/Catalogos/FrmMarcas.cs` | `FrmMarcas` |
| `PRESENTATION/Forms/Catalogos/FrmCatalogoEditar.cs` | `FrmCatalogoEditar` |
| `PRESENTATION/Forms/Repuestos/FrmRepuestos.cs` | `FrmRepuestos`, `FilaRepuesto` (anidado) |
| `PRESENTATION/Forms/Repuestos/FrmRepuestoEditar.cs` | `FrmRepuestoEditar` |
| `PRESENTATION/Forms/Repuestos/FrmAjusteStock.cs` | `FrmAjusteStock` |
| `PRESENTATION/Forms/Repuestos/FrmMovimientosStock.cs` | `FrmMovimientosStock`, `ItemRepuesto`, `ItemTipo`, `FilaMovimiento` (anidados) |
| `PRESENTATION/Forms/Proveedores/FrmProveedores.cs` | `FrmProveedores` |
| `PRESENTATION/Forms/Proveedores/FrmProveedorEditar.cs` | `FrmProveedorEditar` |
| `PRESENTATION/Forms/Compras/FrmCompras.cs` | `FrmCompras`, `FilaCompra`, `ItemProveedor`, `ItemEstado` (anidados) |
| `PRESENTATION/Forms/Compras/FrmCompraDetalle.cs` | `FrmCompraDetalle`, `ItemProveedor`, `ItemRepuesto`, `FilaDetalle` (anidados) |
| `PRESENTATION/Forms/Garantias/FrmGarantias.cs` | `FrmGarantias`, `FilaGarantia`, `ItemVigencia` (anidados) |
| `PRESENTATION/Forms/Dashboard/FrmDashboard.cs` | `FrmDashboard` |
| `PRESENTATION/Forms/Reportes/FrmReportes.cs` | `FrmReportes`, `ItemReporte` (anidado) |
| `PRESENTATION/Forms/Idiomas/FrmAdministrarTraducciones.cs` | `FrmAdministrarTraducciones` |
| `PRESENTATION/Forms/Bitacora/FrmBitacora.cs` | `FrmBitacora` |
| `PRESENTATION/Forms/ControlCambios/FrmControlCambios.cs` | `FrmControlCambios` |
| `PRESENTATION/Forms/*/*.Designer.cs` (28 archivos) | *(partial de la misma clase: no es un tipo propio)* |
| `PRESENTATION/Properties/AssemblyInfo.cs`, `Resources.Designer.cs`, `Settings.Designer.cs` | *(excluidos: metadatos y codigo generado)* |

## Criterio de abstraccion de miembros

1. Cada clase muestra sus **propiedades de estado**, sus **constructores de fabrica** (`CrearNuevo`,
   `CargarDesdeDB`, `Crear`, etc.) y los **metodos que expresan reglas o casos de uso**.
2. No se listan propiedades triviales repetidas ni los controles de los `*.Designer.cs`.
3. En los contratos de `ABSTRACTIONS` solo figuran las operaciones que las implementaciones reales
   ofrecen; no se inventan constantes ni acciones.
4. En los ayudantes anidados de UI se listan sus **propiedades de proyeccion**; su comportamiento
   vive en el formulario contenedor.
5. Los tipos de otro proyecto que aparecen como contexto (por ejemplo `SqlHelper` en los diagramas de
   repositorio, o `IObservador` en el de UI) se referencian como **externos** y no se redefinen.

## Que NO se incluye (exclusiones explicitas)

- `Properties/AssemblyInfo.cs` de los seis proyectos: metadatos de ensamblado.
- `Properties/Resources.Designer.cs` y `Properties/Settings.Designer.cs` de `UI`: codigo generado.
- Los 28 `*.Designer.cs` de formularios: parcial de la misma clase, solo construccion de controles.
- Carpetas `bin/` y `obj/`: salidas de compilacion, no fuente versionada.
- Diagramas de secuencia/actividad historicos en `diagrams/` (por ejemplo `01-PermisosFormLoad.puml`):
  pertenecen a entregables anteriores y **no** se modificaron ni se reescribieron aqui.

## Reglas vigentes que el conjunto documenta

| Regla | Donde se ve |
| --- | --- |
| Ensamblados y carpetas: `PRESENTATION` -> `UI`, `INFRASTRUCTURE` -> `REPOSITORY` | G06.6, G06.4.x |
| .NET Framework 4.7.2 y WinForms en los seis proyectos | G06.6 |
| Repositorios concretos construidos con `new`; sin interfaces de repositorio ni contenedor de DI | G06.3, G06.4.x |
| Estados, tipos y resultados son `const string`, no enumeraciones | G06.1 |
| Documento de cliente unico, activo o inactivo | G06.1, G06.3, G06.4.2 |
| Presupuesto con item de repuesto: referencia, validacion y economia solo de cotizacion | G06.1, G06.3, G06.4.3 |
| Pruebas sobre la intervencion actual y finalizacion explicita | G06.3, G06.6 |
| Cobertura de garantia 0..1 y reingresos 0..N con un unico reingreso abierto | G06.1, G06.4.3, G06.4.4 |
| Orden original de solo consulta por existencia de reingresos, con excepciones tipadas y codigos de resultado | G06.1, G06.3, G06.4.3 |
| Permisos efectivos compartidos; la composicion no implica propiedad exclusiva | G06.1, G06.2, G06.5 |

## Snapshot y limites de verificacion

- **Snapshot**: rama `checkpoint-6-arreglos-y-mejoras` con los cambios locales de `feat-006` a
  `feat-011` sin commitear (incluye el congelamiento por reingresos de garantia y sus excepciones y
  codigos de resultado). Los diagramas describen **ese** estado del codigo fuente.
- **Verificacion hecha**: lectura del codigo fuente y de los manifiestos `Compile` de los seis
  proyectos (conteo de tipos y de miembros); revision estructural de los `.puml` (un solo
  `@startuml`/`@enduml`, llaves y comillas balanceadas, alias unicos, sin `!include` remoto) y
  revision de los enlaces relativos de estos documentos.
- **No se ejecuto**: compilacion, lanzamiento de la aplicacion, acceso a la base de datos ni
  representacion grafica de los diagramas. El unico renderizador PlantUML local no fue identificado
  en el entorno, de modo que la sintaxis se valido de forma **estructural** y no con un render real.
- Los diagramas describen estructura y reglas del codigo; no son evidencia de comportamiento en
  runtime ni del contenido de una base de datos concreta.

## Archivos de este entregable

Los 19 archivos de esta carpeta son: este indice, los nueve `.puml` y los nueve `.md` enlazados en
la tabla del indice. Ningun otro archivo del repositorio forma parte del entregable G06.
