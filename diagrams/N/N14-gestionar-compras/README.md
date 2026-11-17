# Caso CU14 — N14-gestionar-compras

- Carpeta de caso: `N14-gestionar-compras` (CASE CU14; no es un capítulo N01/N02).
- Documento original: `N01.7` / `N02-14`.
- Estado: **Secuencias N02 independientes (2026-11-10); otros 7 diagramas históricos sin modificaciones.**
- Cobertura: **12 diagramas actuales** (7 históricos no secuenciales y 5 secuencias independientes).
- Secuencia: dividida el 2026-11-10 en 5 archivos independientes, uno por parte, conservando el cuerpo y los participantes usados de la revisión de fuente del 2026-11-09 (commit `fd9ac1ad83c6fc3799e3487a9d25988e557c7f12`); metadato en [revisiones](../sequence-revisions.json).

Los títulos de la lista se copiaron literalmente del cuerpo extraído; no se reconstruyeron. Las filas históricas 1-6 y 8 conservan su contenido sin cambios (en N04 también la fila 9 del extra). La fila 7 enlaza únicamente la PARTE 1; las demás partes se enumeran en la sección de secuencia independiente. Cada archivo es un diagrama PlantUML completo con su propio título, participantes usados y cuerpo.

## Diagramas

1. [N01-casos-de-uso-negocio.puml](N01-casos-de-uso-negocio.puml) — `N01.7 - Gestionar compras` — seq `2062` · `2026-10-20T04:56:05.658Z` · bloque `1`.
2. [N01-proceso.puml](N01-proceso.puml) — `N01.7 - Proceso de Gestión de compras` — seq `2062` · `2026-10-20T04:56:05.658Z` · bloque `2`.
3. [N01-modelo-conceptual.puml](N01-modelo-conceptual.puml) — `N01.7 - Modelo conceptual de Gestión de compras` — seq `2062` · `2026-10-20T04:56:05.658Z` · bloque `3`.
4. [N02-casos-de-uso.puml](N02-casos-de-uso.puml) — `N02-14 - Caso de uso Gestionar compras` — seq `2062` · `2026-10-20T04:56:05.658Z` · bloque `4`.
5. [N02-flujo.puml](N02-flujo.puml) — `N02-14 - Flujo de Gestión de compras` — seq `2062` · `2026-10-20T04:56:05.658Z` · bloque `5`.
6. [N02-clases.puml](N02-clases.puml) — `N02-14 - Clases de información de Gestión de compras` — seq `2062` · `2026-10-20T04:56:05.658Z` · bloque `6`.
7. [N02-secuencia.puml](N02-secuencia.puml) — `N02-14 - Secuencia de Gestión de compras` — seq `2062` · `2026-10-20T04:56:05.658Z` · bloque `7` — **PARTE 1 de 5** de la secuencia independiente; no es un resumen del caso. Las demás partes están abajo.
8. [N02-der.puml](N02-der.puml) — `N02-14 - Entidades de Gestión de compras` — seq `2062` · `2026-10-20T04:56:05.658Z` · bloque `8`.

## Secuencia independiente (5 partes)

Cada enlace abre un archivo PlantUML autónomo de una sola página. `N02-secuencia.puml` es solo la PARTE 1.

1. [N02-secuencia.puml](N02-secuencia.puml) — **PARTE 1** — `N02-14 - Secuencia de Gestión de compras`
2. [N02-secuencia-02-borrador-renglones.puml](N02-secuencia-02-borrador-renglones.puml) — **PARTE 2** — `N02-14 - Borrador y renglones`
3. [N02-secuencia-03-confirmacion-stock.puml](N02-secuencia-03-confirmacion-stock.puml) — **PARTE 3** — `N02-14 - Confirmación con stock`
4. [N02-secuencia-04-cancelar-borrador.puml](N02-secuencia-04-cancelar-borrador.puml) — **PARTE 4** — `N02-14 - Cancelación de borrador`
5. [N02-secuencia-05-anular-confirmada.puml](N02-secuencia-05-anular-confirmada.puml) — **PARTE 5** — `N02-14 - Anulación de compra confirmada`

[Volver al índice](../README.md) · [Manifiesto de procedencia](../source-manifest.json)
