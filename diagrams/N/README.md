# Diagramas N — archivo por caso

Este directorio reúne los 129 diagramas PlantUML que ya se presentaron en el chat siguiendo el formato acordado para los casos de uso N. Es un archivo documental: los cuerpos se conservan sin regenerar, modernizar, corregir ni completar. Cada archivo procede de una respuesta pública de `assistant`, y `source-manifest.json` registra su evento, hora UTC, bloque, título y SHA-256.

## Alcance

- `diagrams/N/` es el único directorio general. Cada subcarpeta `Nxx-...` representa el caso de uso `CUxx`, no un capítulo de los documentos N01/N02.
- Los nombres fijos preservan la separación original: tres diagramas funcionales `N01-*` y cinco extendidos `N02-*` por caso disponible. La carpeta N04 conserva además un archivo extra.
- Dieciséis casos tienen ocho diagramas base; N04 conserva además un extra; en total hay 129 archivos `.puml`. Los casos N17 y N18 no tienen diagramas: solo un README pendiente, sin archivos ficticios ni vacíos.
- Cada README de caso enlaza sus diagramas y repite el título extraído literalmente, el seq del evento, la hora UTC y el bloque. Estado de los casos disponibles: **Archivado sin modificaciones; pendiente de mejoras**.
- Los extractos públicos del chat del 18 al 20/10 son evidencia histórica de origen, no una nueva prueba de la fuente actual ni una certificación de ejecución. Una mejora futura de los diagramas será una tarea separada y no forma parte de este archivo.
- No se ejecutó renderizado; las comprobaciones estructurales y de fidelidad no certifican la sintaxis completa ni la legibilidad.

## Índice de casos

| CU | Carpeta | Documento original | Diagramas | Estado |
| --- | --- | --- | --- | --- |
| CU01 | [N01-gestionar-clientes](N01-gestionar-clientes/README.md) | `N01.1` / `N02-01` | 8 | Archivado sin modificaciones; pendiente de mejoras |
| CU02 | [N02-gestionar-equipos](N02-gestionar-equipos/README.md) | `N01.2` / `N02-02` | 8 | Archivado sin modificaciones; pendiente de mejoras |
| CU03 | [N03-gestionar-tipos-de-equipo-y-marcas](N03-gestionar-tipos-de-equipo-y-marcas/README.md) | `N01.3` / `N02-03` | 8 | Archivado sin modificaciones; pendiente de mejoras |
| CU04 | [N04-gestionar-ordenes-de-servicio](N04-gestionar-ordenes-de-servicio/README.md) | `N01.4` / `N02-04` | 9 | Archivado sin modificaciones; pendiente de mejoras |
| CU05 | [N05-gestionar-diagnosticos](N05-gestionar-diagnosticos/README.md) | `N01.4.1` / `N02-05` | 8 | Archivado sin modificaciones; pendiente de mejoras |
| CU06 | [N06-gestionar-presupuestos](N06-gestionar-presupuestos/README.md) | `N01.4.2` / `N02-06` | 8 | Archivado sin modificaciones; pendiente de mejoras |
| CU07 | [N07-gestionar-reparaciones](N07-gestionar-reparaciones/README.md) | `N01.4.3` / `N02-07` | 8 | Archivado sin modificaciones; pendiente de mejoras |
| CU08 | [N08-gestionar-repuestos-utilizados](N08-gestionar-repuestos-utilizados/README.md) | `N01.4.4` / `N02-08` | 8 | Archivado sin modificaciones; pendiente de mejoras |
| CU09 | [N09-gestionar-pruebas](N09-gestionar-pruebas/README.md) | `N01.4.5` / `N02-09` | 8 | Archivado sin modificaciones; pendiente de mejoras |
| CU10 | [N10-gestionar-entrega](N10-gestionar-entrega/README.md) | `N01.4.6` / `N02-10` | 8 | Archivado sin modificaciones; pendiente de mejoras |
| CU11 | [N11-gestionar-garantias-y-reingresos](N11-gestionar-garantias-y-reingresos/README.md) | `N01.4.7` / `N02-11` | 8 | Archivado sin modificaciones; pendiente de mejoras |
| CU12 | [N12-gestionar-repuestos-e-inventario](N12-gestionar-repuestos-e-inventario/README.md) | `N01.5` / `N02-12` | 8 | Archivado sin modificaciones; pendiente de mejoras |
| CU13 | [N13-gestionar-proveedores](N13-gestionar-proveedores/README.md) | `N01.6` / `N02-13` | 8 | Archivado sin modificaciones; pendiente de mejoras |
| CU14 | [N14-gestionar-compras](N14-gestionar-compras/README.md) | `N01.7` / `N02-14` | 8 | Archivado sin modificaciones; pendiente de mejoras |
| CU15 | [N15-gestionar-bitacora-e-historial](N15-gestionar-bitacora-e-historial/README.md) | `N01.8` / `N02-15` | 8 | Archivado sin modificaciones; pendiente de mejoras |
| CU16 | [N16-consultar-dashboard-y-reportes](N16-consultar-dashboard-y-reportes/README.md) | `N01.9` / `N02-16` | 8 | Archivado sin modificaciones; pendiente de mejoras |
| CU17 | [N17-gestionar-acceso-usuarios-y-permisos](N17-gestionar-acceso-usuarios-y-permisos/README.md) | `N01.10` / `N02-17` | 0 | Pendiente; solo README |
| CU18 | [N18-gestionar-idiomas-y-traducciones](N18-gestionar-idiomas-y-traducciones/README.md) | `N01.11` / `N02-18` | 0 | Pendiente; solo README |

## Procedencia

El manifiesto [source-manifest.json](source-manifest.json) define la base de rutas (raíz del repositorio), la normalización aplicada, los seq permitidos, los 129 registros de diagramas y los dos casos pendientes. No contiene la ruta absoluta del archivo de sesión ni el chat completo. Los cuerpos archivados no se publicaron como certificación de fuente actual ni de comportamiento en ejecución.
