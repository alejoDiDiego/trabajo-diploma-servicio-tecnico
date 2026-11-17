# Caso CU06 — N06-gestionar-presupuestos

- Carpeta de caso: `N06-gestionar-presupuestos` (CASE CU06; no es un capítulo N01/N02).
- Documento original: `N01.4.2` / `N02-06`.
- Estado: **Secuencias N02 independientes (2026-11-10); otros 7 diagramas históricos sin modificaciones.**
- Cobertura: **13 diagramas actuales** (7 históricos no secuenciales y 6 secuencias independientes).
- Secuencia: dividida el 2026-11-10 en 6 archivos independientes, uno por parte, conservando el cuerpo y los participantes usados de la revisión de fuente del 2026-11-09 (commit `fd9ac1ad83c6fc3799e3487a9d25988e557c7f12`); metadato en [revisiones](../sequence-revisions.json).

Los títulos de la lista se copiaron literalmente del cuerpo extraído; no se reconstruyeron. Las filas históricas 1-6 y 8 conservan su contenido sin cambios (en N04 también la fila 9 del extra). La fila 7 enlaza únicamente la PARTE 1; las demás partes se enumeran en la sección de secuencia independiente. Cada archivo es un diagrama PlantUML completo con su propio título, participantes usados y cuerpo.

## Diagramas

1. [N01-casos-de-uso-negocio.puml](N01-casos-de-uso-negocio.puml) — `N01.4.2 - Gestionar presupuestos` — seq `1285` · `2026-10-19T18:23:39.546Z` · bloque `1`.
2. [N01-proceso.puml](N01-proceso.puml) — `N01.4.2 - Proceso de Gestión de presupuestos` — seq `1285` · `2026-10-19T18:23:39.546Z` · bloque `2`.
3. [N01-modelo-conceptual.puml](N01-modelo-conceptual.puml) — `N01.4.2 - Modelo conceptual de Gestión de presupuestos` — seq `1285` · `2026-10-19T18:23:39.546Z` · bloque `3`.
4. [N02-casos-de-uso.puml](N02-casos-de-uso.puml) — `N02-06 - Caso de uso Gestionar presupuestos` — seq `1285` · `2026-10-19T18:23:39.546Z` · bloque `4`.
5. [N02-flujo.puml](N02-flujo.puml) — `N02-06 - Flujo de Gestión de presupuestos` — seq `1285` · `2026-10-19T18:23:39.546Z` · bloque `5`.
6. [N02-clases.puml](N02-clases.puml) — `N02-06 - Clases de información afectadas` — seq `1285` · `2026-10-19T18:23:39.546Z` · bloque `6`.
7. [N02-secuencia.puml](N02-secuencia.puml) — `N02-06 - Secuencia de Gestión de presupuestos` — seq `1285` · `2026-10-19T18:23:39.546Z` · bloque `7` — **PARTE 1 de 6** de la secuencia independiente; no es un resumen del caso. Las demás partes están abajo.
8. [N02-der.puml](N02-der.puml) — `N02-06 - Entidades de Gestión de presupuestos` — seq `1285` · `2026-10-19T18:23:39.546Z` · bloque `8`.

## Secuencia independiente (6 partes)

Cada enlace abre un archivo PlantUML autónomo de una sola página. `N02-secuencia.puml` es solo la PARTE 1.

1. [N02-secuencia.puml](N02-secuencia.puml) — **PARTE 1** — `N02-06 - Secuencia de Gestión de presupuestos`
2. [N02-secuencia-02-publicar-eliminar.puml](N02-secuencia-02-publicar-eliminar.puml) — **PARTE 2** — `N02-06 - Borradores: publicación y eliminación`
3. [N02-secuencia-03-emision-directa.puml](N02-secuencia-03-emision-directa.puml) — **PARTE 3** — `N02-06 - Emisión directa de original y adicional`
4. [N02-secuencia-04-aprobacion-rechazo.puml](N02-secuencia-04-aprobacion-rechazo.puml) — **PARTE 4** — `N02-06 - Aprobación y rechazo`
5. [N02-secuencia-05-solicitud-cancelar.puml](N02-secuencia-05-solicitud-cancelar.puml) — **PARTE 5** — `N02-06 - Solicitud y cancelación de adicional`
6. [N02-secuencia-06-anulacion.puml](N02-secuencia-06-anulacion.puml) — **PARTE 6** — `N02-06 - Ventanas de anulación`

[Volver al índice](../README.md) · [Manifiesto de procedencia](../source-manifest.json)
