# Diagramas N — archivo por caso

Este directorio reúne 168 diagramas PlantUML actuales en 18 carpetas: 113 cuerpos históricos sin cambios y 55 archivos de secuencia independientes en los 16 casos desarrollados. La procedencia histórica del archivo (extractos públicos del chat del 18 al 20/10/2026) se conserva en [source-manifest.json](source-manifest.json) con 129 registros. El 2026-11-09, dentro de `feat-015`, se revisaron únicamente los 16 cuerpos de secuencia contra la fuente actual; el 2026-11-10, dentro de `feat-016`, cada uno se dividió en archivos autónomos, uno por parte. Los 55 archivos actuales ya no son páginas de un archivo multipágina: son diagramas separados y no se usa `newpage`.

## Alcance

- `diagrams/N/` es el único directorio general. Cada subcarpeta `Nxx-...` representa el caso de uso `CUxx`, no un capítulo de los documentos N01/N02.
- Los 16 casos desarrollados conservan sus diagramas históricos (7 por caso; N04 conserva 8, incluido el extra) y añaden entre 2 y 6 archivos de secuencia independientes: 55 en total. N04 tiene exactamente cuatro archivos de secuencia separados.
- Los 113 diagramas no secuenciales siguen siendo los cuerpos históricos archivados sin cambios de contenido. N17 y N18 no tienen diagramas: solo un README pendiente, sin archivos ficticios ni vacíos.
- [source-manifest.json](source-manifest.json) conserva exactamente 129 registros históricos (128 estándar + 1 extra) y su allowlist: 16 corresponden a los cuerpos de secuencia originales de una sola página extraídos del chat del 18 al 20/10/2026 y 113 a los demás cuerpos históricos, que permanecen byte a byte. Los 16 originales son la línea base `baselineSha256`; las versiones multipágina refinadas por `feat-015` el 2026-11-09 se registran por separado en `preSplit.sha256` de `sequence-revisions.json` (schemaVersion 2) y fueron divididas por `feat-016` el 2026-11-10 en los 55 archivos actuales registrados en `files`. El manifiesto no describe ni esas versiones refinadas ni los 55 archivos actuales; `baselineSha256` y `preSplit.sha256` no son intercambiables.
- [sequence-revisions.json](sequence-revisions.json) usa `schemaVersion` 2 y registra, por caso, el SHA-256 histórico de línea base, el SHA-256/tamaño/páginas del cuerpo completo previo a la división, y por archivo su parte, ruta, título, SHA-256, tamaño y participantes usados.
- La fuente declarada sigue siendo `main` en el commit completo `fd9ac1ad83c6fc3799e3487a9d25988e557c7f12`, con fecha de inspección 2026-11-09 y fecha de división 2026-11-10. No hay una nueva inspección de fuente, certificación de ejecución ni renderizado.
- No se ejecutó renderizado; las comprobaciones estructurales y de anclas no certifican la sintaxis completa, el layout ni la legibilidad.

## Estado de las secuencias (2026-11-10)

- Casos desarrollados: N01 a N16, con 55 archivos independientes. Cada archivo tiene un único `@startuml`/`@enduml`, un único título y una sola página; no contiene `newpage`.
- El primer archivo de cada caso, `N02-secuencia.puml`, es únicamente la PARTE 1 de su secuencia. No es un resumen ni un archivo agregado de las demás partes; las partes 02..N se enlazan por separado.
- Cada archivo declara solo los participantes usados por sus propios mensajes, con el texto, tipo y orden originales. El cuerpo conserva los mensajes, llamadas, ramas y guardas de la revisión de fuente del 2026-11-09; la división es mecánica y no reescribe contenido de negocio.
- N04 tiene exactamente cuatro archivos separados. N17 y N18 continúan pendientes y solo tienen README; no se renumeraron los casos.

## Índice de casos

| CU | Carpeta | Documento original | Históricos | Secuencia | `.puml` actuales |
| --- | --- | --- | --- | --- | --- |
| CU01 | [N01-gestionar-clientes](N01-gestionar-clientes/README.md) | `N01.1` / `N02-01` | 7 | 3 partes | 10 |
| CU02 | [N02-gestionar-equipos](N02-gestionar-equipos/README.md) | `N01.2` / `N02-02` | 7 | 3 partes | 10 |
| CU03 | [N03-gestionar-tipos-de-equipo-y-marcas](N03-gestionar-tipos-de-equipo-y-marcas/README.md) | `N01.3` / `N02-03` | 7 | 2 partes | 9 |
| CU04 | [N04-gestionar-ordenes-de-servicio](N04-gestionar-ordenes-de-servicio/README.md) | `N01.4` / `N02-04` | 8 | 4 partes | 12 |
| CU05 | [N05-gestionar-diagnosticos](N05-gestionar-diagnosticos/README.md) | `N01.4.1` / `N02-05` | 7 | 3 partes | 10 |
| CU06 | [N06-gestionar-presupuestos](N06-gestionar-presupuestos/README.md) | `N01.4.2` / `N02-06` | 7 | 6 partes | 13 |
| CU07 | [N07-gestionar-reparaciones](N07-gestionar-reparaciones/README.md) | `N01.4.3` / `N02-07` | 7 | 3 partes | 10 |
| CU08 | [N08-gestionar-repuestos-utilizados](N08-gestionar-repuestos-utilizados/README.md) | `N01.4.4` / `N02-08` | 7 | 3 partes | 10 |
| CU09 | [N09-gestionar-pruebas](N09-gestionar-pruebas/README.md) | `N01.4.5` / `N02-09` | 7 | 4 partes | 11 |
| CU10 | [N10-gestionar-entrega](N10-gestionar-entrega/README.md) | `N01.4.6` / `N02-10` | 7 | 3 partes | 10 |
| CU11 | [N11-gestionar-garantias-y-reingresos](N11-gestionar-garantias-y-reingresos/README.md) | `N01.4.7` / `N02-11` | 7 | 4 partes | 11 |
| CU12 | [N12-gestionar-repuestos-e-inventario](N12-gestionar-repuestos-e-inventario/README.md) | `N01.5` / `N02-12` | 7 | 4 partes | 11 |
| CU13 | [N13-gestionar-proveedores](N13-gestionar-proveedores/README.md) | `N01.6` / `N02-13` | 7 | 2 partes | 9 |
| CU14 | [N14-gestionar-compras](N14-gestionar-compras/README.md) | `N01.7` / `N02-14` | 7 | 5 partes | 12 |
| CU15 | [N15-gestionar-bitacora-e-historial](N15-gestionar-bitacora-e-historial/README.md) | `N01.8` / `N02-15` | 7 | 2 partes | 9 |
| CU16 | [N16-consultar-dashboard-y-reportes](N16-consultar-dashboard-y-reportes/README.md) | `N01.9` / `N02-16` | 7 | 4 partes | 11 |
| CU17 | [N17-gestionar-acceso-usuarios-y-permisos](N17-gestionar-acceso-usuarios-y-permisos/README.md) | `N01.10` / `N02-17` | 0 | Pendiente | 0 |
| CU18 | [N18-gestionar-idiomas-y-traducciones](N18-gestionar-idiomas-y-traducciones/README.md) | `N01.11` / `N02-18` | 0 | Pendiente | 0 |

## Secuencias independientes (55 archivos)

Cada enlace abre directamente un archivo autónomo. `N02-secuencia.puml` es solo la PARTE 1 de su caso; las partes 02..N están en el mismo directorio del caso. Los títulos son los títulos originales de cada página, conservados literalmente.

### CU01 — N01-gestionar-clientes

[README del caso](N01-gestionar-clientes/README.md)

1. [N02-secuencia.puml](N01-gestionar-clientes/N02-secuencia.puml) — **PARTE 1** — `N02-01 - Secuencia de Gestión de clientes`
2. [N02-secuencia-02-alta-modificacion.puml](N01-gestionar-clientes/N02-secuencia-02-alta-modificacion.puml) — **PARTE 2** — `N02-01 - Alta y modificación de clientes`
3. [N02-secuencia-03-estado-recarga.puml](N01-gestionar-clientes/N02-secuencia-03-estado-recarga.puml) — **PARTE 3** — `N02-01 - Desactivación, reactivación y recarga`

### CU02 — N02-gestionar-equipos

[README del caso](N02-gestionar-equipos/README.md)

1. [N02-secuencia.puml](N02-gestionar-equipos/N02-secuencia.puml) — **PARTE 1** — `N02-02 - Secuencia de Gestión de equipos`
2. [N02-secuencia-02-alta-modificacion.puml](N02-gestionar-equipos/N02-secuencia-02-alta-modificacion.puml) — **PARTE 2** — `N02-02 - Alta y modificación de equipos`
3. [N02-secuencia-03-estado-recarga.puml](N02-gestionar-equipos/N02-secuencia-03-estado-recarga.puml) — **PARTE 3** — `N02-02 - Desactivación, reactivación y recarga`

### CU03 — N03-gestionar-tipos-de-equipo-y-marcas

[README del caso](N03-gestionar-tipos-de-equipo-y-marcas/README.md)

1. [N02-secuencia.puml](N03-gestionar-tipos-de-equipo-y-marcas/N02-secuencia.puml) — **PARTE 1** — `N02-03 - Secuencia de Gestión de tipos de equipo y marcas`
2. [N02-secuencia-02-catalogo-marcas.puml](N03-gestionar-tipos-de-equipo-y-marcas/N02-secuencia-02-catalogo-marcas.puml) — **PARTE 2** — `N02-03 - Catálogo de marcas y mantenimiento`

### CU04 — N04-gestionar-ordenes-de-servicio

[README del caso](N04-gestionar-ordenes-de-servicio/README.md)

1. [N02-secuencia.puml](N04-gestionar-ordenes-de-servicio/N02-secuencia.puml) — **PARTE 1** — `N02-04 - Secuencia de Gestión de órdenes`
2. [N02-secuencia-02-recepcion-registro.puml](N04-gestionar-ordenes-de-servicio/N02-secuencia-02-recepcion-registro.puml) — **PARTE 2** — `N02-04 - Recepción nueva y registro de orden`
3. [N02-secuencia-03-carga-orden.puml](N04-gestionar-ordenes-de-servicio/N02-secuencia-03-carga-orden.puml) — **PARTE 3** — `N02-04 - CargarOrden con servicios y repositorios`
4. [N02-secuencia-04-operacion-retorno.puml](N04-gestionar-ordenes-de-servicio/N02-secuencia-04-operacion-retorno.puml) — **PARTE 4** — `N02-04 - Operaciones sobre la orden y retorno al listado`

### CU05 — N05-gestionar-diagnosticos

[README del caso](N05-gestionar-diagnosticos/README.md)

1. [N02-secuencia.puml](N05-gestionar-diagnosticos/N02-secuencia.puml) — **PARTE 1** — `N02-05 - Secuencia de Gestión de diagnósticos`
2. [N02-secuencia-02-inicio-diagnostico.puml](N05-gestionar-diagnosticos/N02-secuencia-02-inicio-diagnostico.puml) — **PARTE 2** — `N02-05 - Inicio del diagnóstico`
3. [N02-secuencia-03-fin-eval-garantia.puml](N05-gestionar-diagnosticos/N02-secuencia-03-fin-eval-garantia.puml) — **PARTE 3** — `N02-05 - Finalización y evaluación pendiente de garantía`

### CU06 — N06-gestionar-presupuestos

[README del caso](N06-gestionar-presupuestos/README.md)

1. [N02-secuencia.puml](N06-gestionar-presupuestos/N02-secuencia.puml) — **PARTE 1** — `N02-06 - Secuencia de Gestión de presupuestos`
2. [N02-secuencia-02-publicar-eliminar.puml](N06-gestionar-presupuestos/N02-secuencia-02-publicar-eliminar.puml) — **PARTE 2** — `N02-06 - Borradores: publicación y eliminación`
3. [N02-secuencia-03-emision-directa.puml](N06-gestionar-presupuestos/N02-secuencia-03-emision-directa.puml) — **PARTE 3** — `N02-06 - Emisión directa de original y adicional`
4. [N02-secuencia-04-aprobacion-rechazo.puml](N06-gestionar-presupuestos/N02-secuencia-04-aprobacion-rechazo.puml) — **PARTE 4** — `N02-06 - Aprobación y rechazo`
5. [N02-secuencia-05-solicitud-cancelar.puml](N06-gestionar-presupuestos/N02-secuencia-05-solicitud-cancelar.puml) — **PARTE 5** — `N02-06 - Solicitud y cancelación de adicional`
6. [N02-secuencia-06-anulacion.puml](N06-gestionar-presupuestos/N02-secuencia-06-anulacion.puml) — **PARTE 6** — `N02-06 - Ventanas de anulación`

### CU07 — N07-gestionar-reparaciones

[README del caso](N07-gestionar-reparaciones/README.md)

1. [N02-secuencia.puml](N07-gestionar-reparaciones/N02-secuencia.puml) — **PARTE 1** — `N02-07 - Secuencia de Gestión de reparaciones`
2. [N02-secuencia-02-inicio-adicional.puml](N07-gestionar-reparaciones/N02-secuencia-02-inicio-adicional.puml) — **PARTE 2** — `N02-07 - Inicio de reparación e intervención adicional`
3. [N02-secuencia-03-fin-reparacion.puml](N07-gestionar-reparaciones/N02-secuencia-03-fin-reparacion.puml) — **PARTE 3** — `N02-07 - Finalización de reparación`

### CU08 — N08-gestionar-repuestos-utilizados

[README del caso](N08-gestionar-repuestos-utilizados/README.md)

1. [N02-secuencia.puml](N08-gestionar-repuestos-utilizados/N02-secuencia.puml) — **PARTE 1** — `N02-08 - Secuencia de Gestión de repuestos utilizados`
2. [N02-secuencia-02-consumo-repuesto.puml](N08-gestionar-repuestos-utilizados/N02-secuencia-02-consumo-repuesto.puml) — **PARTE 2** — `N02-08 - Consumo de repuesto`
3. [N02-secuencia-03-devolucion-consumo.puml](N08-gestionar-repuestos-utilizados/N02-secuencia-03-devolucion-consumo.puml) — **PARTE 3** — `N02-08 - Devolución de consumo`

### CU09 — N09-gestionar-pruebas

[README del caso](N09-gestionar-pruebas/README.md)

1. [N02-secuencia.puml](N09-gestionar-pruebas/N02-secuencia.puml) — **PARTE 1** — `N02-09 - Secuencia de Gestión de pruebas`
2. [N02-secuencia-02-registro-anulacion.puml](N09-gestionar-pruebas/N02-secuencia-02-registro-anulacion.puml) — **PARTE 2** — `N02-09 - Registro y anulación de pruebas`
3. [N02-secuencia-03-fin-pruebas.puml](N09-gestionar-pruebas/N02-secuencia-03-fin-pruebas.puml) — **PARTE 3** — `N02-09 - Finalización de pruebas`
4. [N02-secuencia-04-reapertura-pruebas.puml](N09-gestionar-pruebas/N02-secuencia-04-reapertura-pruebas.puml) — **PARTE 4** — `N02-09 - Reapertura de pruebas`

### CU10 — N10-gestionar-entrega

[README del caso](N10-gestionar-entrega/README.md)

1. [N02-secuencia.puml](N10-gestionar-entrega/N02-secuencia.puml) — **PARTE 1** — `N02-10 - Secuencia de Gestión de entrega`
2. [N02-secuencia-02-registro-entrega.puml](N10-gestionar-entrega/N02-secuencia-02-registro-entrega.puml) — **PARTE 2** — `N02-10 - Registro de entrega`
3. [N02-secuencia-03-cancelar-entrega.puml](N10-gestionar-entrega/N02-secuencia-03-cancelar-entrega.puml) — **PARTE 3** — `N02-10 - Cancelación de entrega`

### CU11 — N11-gestionar-garantias-y-reingresos

[README del caso](N11-gestionar-garantias-y-reingresos/README.md)

1. [N02-secuencia.puml](N11-gestionar-garantias-y-reingresos/N02-secuencia.puml) — **PARTE 1** — `N02-11 - Secuencia de Gestión de garantías y reingresos`
2. [N02-secuencia-02-crear-reingreso.puml](N11-gestionar-garantias-y-reingresos/N02-secuencia-02-crear-reingreso.puml) — **PARTE 2** — `N02-11 - Creación de reingreso`
3. [N02-secuencia-03-eval-pendiente.puml](N11-gestionar-garantias-y-reingresos/N02-secuencia-03-eval-pendiente.puml) — **PARTE 3** — `N02-11 - Evaluación pendiente del reingreso`
4. [N02-secuencia-04-decision-garantia.puml](N11-gestionar-garantias-y-reingresos/N02-secuencia-04-decision-garantia.puml) — **PARTE 4** — `N02-11 - Decisión de garantía`

### CU12 — N12-gestionar-repuestos-e-inventario

[README del caso](N12-gestionar-repuestos-e-inventario/README.md)

1. [N02-secuencia.puml](N12-gestionar-repuestos-e-inventario/N02-secuencia.puml) — **PARTE 1** — `N02-12 - Secuencia de Gestión de repuestos e inventario`
2. [N02-secuencia-02-escrituras-maestro.puml](N12-gestionar-repuestos-e-inventario/N02-secuencia-02-escrituras-maestro.puml) — **PARTE 2** — `N02-12 - Escrituras del maestro`
3. [N02-secuencia-03-estado-ajuste.puml](N12-gestionar-repuestos-e-inventario/N02-secuencia-03-estado-ajuste.puml) — **PARTE 3** — `N02-12 - Desactivación, reactivación y ajuste`
4. [N02-secuencia-04-movimientos.puml](N12-gestionar-repuestos-e-inventario/N02-secuencia-04-movimientos.puml) — **PARTE 4** — `N02-12 - Consulta de movimientos`

### CU13 — N13-gestionar-proveedores

[README del caso](N13-gestionar-proveedores/README.md)

1. [N02-secuencia.puml](N13-gestionar-proveedores/N02-secuencia.puml) — **PARTE 1** — `N02-13 - Secuencia de Gestión de proveedores`
2. [N02-secuencia-02-escrituras-recarga.puml](N13-gestionar-proveedores/N02-secuencia-02-escrituras-recarga.puml) — **PARTE 2** — `N02-13 - Escrituras, cambios de estado y recarga`

### CU14 — N14-gestionar-compras

[README del caso](N14-gestionar-compras/README.md)

1. [N02-secuencia.puml](N14-gestionar-compras/N02-secuencia.puml) — **PARTE 1** — `N02-14 - Secuencia de Gestión de compras`
2. [N02-secuencia-02-borrador-renglones.puml](N14-gestionar-compras/N02-secuencia-02-borrador-renglones.puml) — **PARTE 2** — `N02-14 - Borrador y renglones`
3. [N02-secuencia-03-confirmacion-stock.puml](N14-gestionar-compras/N02-secuencia-03-confirmacion-stock.puml) — **PARTE 3** — `N02-14 - Confirmación con stock`
4. [N02-secuencia-04-cancelar-borrador.puml](N14-gestionar-compras/N02-secuencia-04-cancelar-borrador.puml) — **PARTE 4** — `N02-14 - Cancelación de borrador`
5. [N02-secuencia-05-anular-confirmada.puml](N14-gestionar-compras/N02-secuencia-05-anular-confirmada.puml) — **PARTE 5** — `N02-14 - Anulación de compra confirmada`

### CU15 — N15-gestionar-bitacora-e-historial

[README del caso](N15-gestionar-bitacora-e-historial/README.md)

1. [N02-secuencia.puml](N15-gestionar-bitacora-e-historial/N02-secuencia.puml) — **PARTE 1** — `N02-15 - Secuencia de Gestión de bitácora e historial`
2. [N02-secuencia-02-historial-orden.puml](N15-gestionar-bitacora-e-historial/N02-secuencia-02-historial-orden.puml) — **PARTE 2** — `N02-15 - Historial de la orden`

### CU16 — N16-consultar-dashboard-y-reportes

[README del caso](N16-consultar-dashboard-y-reportes/README.md)

1. [N02-secuencia.puml](N16-consultar-dashboard-y-reportes/N02-secuencia.puml) — **PARTE 1** — `N02-16 - Secuencia de Consulta de dashboard y reportes`
2. [N02-secuencia-02-reportes-tabulares.puml](N16-consultar-dashboard-y-reportes/N02-secuencia-02-reportes-tabulares.puml) — **PARTE 2** — `N02-16 - Reportes tabulares`
3. [N02-secuencia-03-tasas-aprob-acept.puml](N16-consultar-dashboard-y-reportes/N02-secuencia-03-tasas-aprob-acept.puml) — **PARTE 3** — `N02-16 - Tasas de aprobación y aceptación`
4. [N02-secuencia-04-escalares.puml](N16-consultar-dashboard-y-reportes/N02-secuencia-04-escalares.puml) — **PARTE 4** — `N02-16 - Escalares`

## Procedencia

El manifiesto [source-manifest.json](source-manifest.json) define la base de rutas (raíz del repositorio), la normalización aplicada, los seq permitidos y los 129 registros históricos. No contiene la ruta absoluta del archivo de sesión ni el chat completo. Las revisiones de secuencia y sus anclas de fuente están en [sequence-revisions.json](sequence-revisions.json). Los cuerpos históricos no se publicaron como certificación de fuente actual ni de comportamiento en ejecución; la revisión de las 16 secuencias es inspección de fuente del 2026-11-09, y la división en archivos independientes del 2026-11-10 es solo de formato. No se ejecutó un renderizador PlantUML, por lo que la sintaxis completa, el layout y la legibilidad visual no están certificados.
