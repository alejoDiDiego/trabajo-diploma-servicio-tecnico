# G06 - Diagramas de clases y Diagrama Entidad-Relación

Entregable actualizado el **20/10/2026** conforme a los diagramas y tablas revisados con el propietario. Contiene **nueve diagramas de clases y un DER**, cada uno con su documento Markdown: **21 archivos** en total, incluido este índice.

Se conserva el formato resumido acordado: paquetes funcionales, miembros significativos, identificadores exactos y tablas con las columnas **Nombre**, **Características** y **Comportamientos**. No es una reproducción exhaustiva de cada miembro del código.

## Índice

| N.º | Contenido | Tipos propios o entidades representadas | Diagrama | Descripción y tablas |
| --- | --- | --- | --- | --- |
| G06.1 | DOMAIN | 49 tipos propios; contratos externos como contexto | [G06.1-domain.puml](G06.1-domain.puml) | [G06.1-domain.md](G06.1-domain.md) |
| G06.2 | ABSTRACTIONS | 10 interfaces y un DTO | [G06.2-abstractions.puml](G06.2-abstractions.puml) | [G06.2-abstractions.md](G06.2-abstractions.md) |
| G06.3 | APPLICATION | 19 tipos propios | [G06.3-application.puml](G06.3-application.puml) | [G06.3-application.md](G06.3-application.md) |
| G06.4.1 | REPOSITORY - Aspectos técnicos | SqlHelper y siete repositorios: 8 tipos | [G06.4.1-repository-tecnicos.puml](G06.4.1-repository-tecnicos.puml) | [G06.4.1-repository-tecnicos.md](G06.4.1-repository-tecnicos.md) |
| G06.4.2 | REPOSITORY - Maestros | 5 repositorios; SqlHelper como referencia | [G06.4.2-repository-maestros.puml](G06.4.2-repository-maestros.puml) | [G06.4.2-repository-maestros.md](G06.4.2-repository-maestros.md) |
| G06.4.3 | REPOSITORY - Órdenes | 6 repositorios, incluido ReparacionRepository | [G06.4.3-repository-ordenes.puml](G06.4.3-repository-ordenes.puml) | [G06.4.3-repository-ordenes.md](G06.4.3-repository-ordenes.md) |
| G06.4.4 | REPOSITORY - Stock, Compras y Garantías | 6 repositorios, incluido ReporteRepository | [G06.4.4-repository-stock-compras-garantias.puml](G06.4.4-repository-stock-compras-garantias.puml) | [G06.4.4-repository-stock-compras-garantias.md](G06.4.4-repository-stock-compras-garantias.md) |
| G06.5 | SERVICES | 4 clases; contratos externos como contexto | [G06.5-services.puml](G06.5-services.puml) | [G06.5-services.md](G06.5-services.md) |
| G06.6 | UI / PRESENTATION | Program y 28 formularios: 29 tipos | [G06.6-ui.puml](G06.6-ui.puml) | [G06.6-ui.md](G06.6-ui.md) |
| G06.7 | DER físico compacto | 30 tablas de la aplicación y 42 FK | [G06.7-der.puml](G06.7-der.puml) | [G06.7-der.md](G06.7-der.md) |

Los cuatro diagramas REPOSITORY forman una sola capa. SqlHelper aparece desarrollado una vez y como referencia vacía en los otros tres: no son cuatro implementaciones distintas.

## Organización y convenciones

| Elemento | Significado |
| --- | --- |
| Paquetes funcionales | Agrupaciones documentales para conservar el formato solicitado; no crean carpetas, namespaces ni proyectos nuevos. |
| `+`, `-`, `#` | Visibilidad pública, privada y protegida, respectivamente. Los manejadores de eventos de UI representados son privados. |
| `{static}` y `<<static>>` | Miembro o clase estática. Los estados, tipos y resultados del dominio son constantes string, no enumeraciones de C#. |
| `...` | Parámetros omitidos deliberadamente para reducir el ancho; no es una firma C# literal. Los parámetros relevantes para un cambio, como continuarPago o el resultado out, se hacen explícitos. |
| `int?` y `DateTime?` | Tipos de valor anulables. La notación string? del diagrama de dominio expresa ausencia posible del valor; no afirma que el proyecto habilite nullable reference types de C#. |
| Herencia y realización | Generalización mediante triángulo vacío y realización de interfaces con línea discontinua. En UI, los estereotipos Form e IObservador resumen la herencia y el contrato para no duplicar 28 aristas. |
| Asociaciones de negocio de DOMAIN | Vínculos conceptuales por identificadores; no implican propiedades de navegación, colecciones ni composición C#. El DER separado representa las FK físicas. |
| Asociación de permisos | Los componentes pueden compartirse entre familias; no hay propiedad exclusiva de los hijos. |
| `<<referencia>>` | Tipo existente mostrado como contexto; no se redefine su implementación ni se agrega otra fila de clase propia en la tabla. |
| DER | PK/FK, nulabilidad y cardinalidades físicas; sus líneas continuas identifican FK integrantes de PK compuesta y sus líneas discontinuas, FK no integrantes de PK. No confundir esta convención con las dependencias de los diagramas de clases. |

## Cobertura y omisiones explícitas

- Los diagramas de clases representan **137 tipos propios**: 49 de DOMAIN, 11 de ABSTRACTIONS, 19 de APPLICATION, 25 de REPOSITORY, 4 de SERVICES y 29 de UI. Las interfaces y referencias de otras capas no se suman otra vez.
- La versión extensa anterior incluía además 32 clases auxiliares privadas anidadas de UI. Se omiten en esta vista resumida por acuerdo de formato; no fueron eliminadas del código ni convertidas en clases públicas.
- Los parciales Designer pertenecen al mismo formulario. Se omiten sus controles individuales y los tipos generados de recursos, configuración y metadatos de ensamblado, junto con las salidas bin y obj.
- Se muestran las propiedades públicas de salida de los editores y miembros seleccionados del detalle de orden, no todos los campos, constructores o eventos de cada formulario.
- Las clases de constantes del dominio se conservan sin desarrollar sus valores en el diagrama; sus tablas describen el vocabulario vigente.
- El DER omite atributos descriptivos, tipos repetidos y varios índices secundarios para conservar las 42 relaciones en una sola imagen. Excluye explícitamente sysdiagrams.

## Reglas y diferencias relevantes

| Aspecto | Representación vigente |
| --- | --- |
| Documento de cliente | Política de identidad para activos e inactivos, excepción con propietario y validación transaccional. No equivale a afirmar que el índice único esté instalado en toda base. |
| Presupuesto con repuesto | Referencia comercial opcional, validación antes de guardar y precio cotizado separado del costo de consumo; no reserva ni consume stock. |
| Presupuestos e intervenciones | Uno a muchos; el presupuesto Original no Anulado tiene unicidad filtrada, no unicidad global por orden. |
| Pruebas | Intervención actual y finalización explícita; el historial de intervenciones queda disponible en consulta. |
| Garantías | Cobertura 0..1 por orden original y visitas de reingreso 0..N; solo una visita abierta a la vez según la regla del programa. |
| Original con reingresos | Solo consulta por existencia de cualquier hijo de garantía, aunque después cierre; se mantienen inspección y creación de nuevos reingresos sujetos a validación. El bloqueo de persistencia es específico de las rutas documentadas, no universal a todos los repositorios. |
| Retornos de repositorios | Transiciones y cancelación de entrega devuelven códigos int; Registrar de MovimientoStockRepository devuelve MovimientoStock y ContarReingresosGarantia de ReporteRepository devuelve DataTable. |
| Sesión y cifrado | SessionManager administra una sesión única creada por Login; SesionIdioma es el Singleton observable de inicialización diferida. EncryptionService no tiene consumidores encontrados en el código actual; no se publican secretos criptográficos. |
| UI | Main y los eventos seleccionados son privados; Actualizar recibe idiomaObservado. Aperturas del menú como MDI y editores/detalles como diálogos modales. |
| Reglas e integridad | Estados como const string; DVH/DVV de Usuarios; los montos presupuestados no son cobros y la autorización mayormente en UI no se presenta como barrera backend universal. |

## Evidencia y límites de verificación

### Código

Las capas fueron contrastadas con el código y manifiestos actuales durante esta sesión. Snapshot de fuente: rama `checkpoint-6-arreglos-y-mejoras`, base **9417b47**, el **20/10/2026**. Las modificaciones de esta tarea son documentales: no cambian el comportamiento del programa ni actualizan automáticamente diagramas ajenos a esta carpeta.

Anclas de proyectos: [DOMAIN](../../DOMAIN/DOMAIN.csproj), [ABSTRACTIONS](../../ABSTRACTIONS/ABSTRACTIONS.csproj), [APPLICATION](../../APPLICATION/APPLICATION.csproj), [REPOSITORY](../../INFRASTRUCTURE/REPOSITORY.csproj), [SERVICES](../../SERVICES/SERVICES.csproj) y [UI](../../PRESENTATION/UI.csproj). Para contexto: [arquitectura](../../docs/agents/architecture.md) y [persistencia](../../docs/agents/persistence.md).

### Base de datos

El DER conserva la **consulta real de metadatos realizada el 20/10/2026 en esta misma sesión** sobre TPIntegrador, esquema dbo. Se observaron 30 tablas de aplicación, 42 FK habilitadas y confiables y una tabla sysdiagrams excluida. **No existe un índice único de documento de cliente en la base inspeccionada.** No se consultaron filas de negocio para explicar su ausencia.

Ese estado físico no se sustituye por lo que el DDL condicional del código intentaría instalar en otra base. Las tablas de clase describen responsabilidades y reglas del programa; el DER describe restricciones físicas observadas. Más detalle en [la descripción del DER](G06.7-der.md).

### Comprobaciones y exclusiones

La aceptación documental incluye revisión de fuente y contenido, correspondencia entre diagramas y tablas, enlaces relativos, estructura PlantUML, estado JSON, alcance de archivos y comprobaciones del repositorio. La evidencia final de ejecución de esos controles se registra en [progress.md](../../progress.md); no se toma un resultado histórico como aprobación automática de esta revisión.

No se encontró un renderizador PlantUML local. No se instalaron herramientas ni se enviaron diagramas a servicios externos. La estructura se revisa sin afirmar un render real: el límite 1920 por 1080 del DER no certifica su legibilidad visual. Tampoco se ejecutaron compilación, inicio de aplicación, migraciones, semillas, pruebas de formularios, concurrencia ni escrituras de base de datos para actualizar estos documentos.
