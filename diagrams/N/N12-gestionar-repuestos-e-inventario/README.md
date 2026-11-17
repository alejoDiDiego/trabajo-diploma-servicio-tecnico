# Caso CU12 — N12-gestionar-repuestos-e-inventario

- Carpeta de caso: `N12-gestionar-repuestos-e-inventario` (CASE CU12; no es un capítulo N01/N02).
- Documento original: `N01.5` / `N02-12`.
- Estado: **Secuencias N02 independientes (2026-11-10); otros 7 diagramas históricos sin modificaciones.**
- Cobertura: **11 diagramas actuales** (7 históricos no secuenciales y 4 secuencias independientes).
- Secuencia: dividida el 2026-11-10 en 4 archivos independientes, uno por parte, conservando el cuerpo y los participantes usados de la revisión de fuente del 2026-11-09 (commit `fd9ac1ad83c6fc3799e3487a9d25988e557c7f12`); metadato en [revisiones](../sequence-revisions.json).

Los títulos de la lista se copiaron literalmente del cuerpo extraído; no se reconstruyeron. Las filas históricas 1-6 y 8 conservan su contenido sin cambios (en N04 también la fila 9 del extra). La fila 7 enlaza únicamente la PARTE 1; las demás partes se enumeran en la sección de secuencia independiente. Cada archivo es un diagrama PlantUML completo con su propio título, participantes usados y cuerpo.

## Diagramas

1. [N01-casos-de-uso-negocio.puml](N01-casos-de-uso-negocio.puml) — `N01.5 - Gestionar repuestos e inventario` — seq `1905` · `2026-10-20T03:28:05.702Z` · bloque `1`.
2. [N01-proceso.puml](N01-proceso.puml) — `N01.5 - Proceso de Gestión de repuestos e inventario` — seq `1905` · `2026-10-20T03:28:05.702Z` · bloque `2`.
3. [N01-modelo-conceptual.puml](N01-modelo-conceptual.puml) — `N01.5 - Modelo conceptual de repuestos e inventario` — seq `1905` · `2026-10-20T03:28:05.702Z` · bloque `3`.
4. [N02-casos-de-uso.puml](N02-casos-de-uso.puml) — `N02-12 - Caso de uso Gestionar repuestos e inventario` — seq `1905` · `2026-10-20T03:28:05.702Z` · bloque `4`.
5. [N02-flujo.puml](N02-flujo.puml) — `N02-12 - Flujo de Gestión de repuestos e inventario` — seq `1905` · `2026-10-20T03:28:05.702Z` · bloque `5`.
6. [N02-clases.puml](N02-clases.puml) — `N02-12 - Clases de información de repuestos e inventario` — seq `1905` · `2026-10-20T03:28:05.702Z` · bloque `6`.
7. [N02-secuencia.puml](N02-secuencia.puml) — `N02-12 - Secuencia de Gestión de repuestos e inventario` — seq `1905` · `2026-10-20T03:28:05.702Z` · bloque `7` — **PARTE 1 de 4** de la secuencia independiente; no es un resumen del caso. Las demás partes están abajo.
8. [N02-der.puml](N02-der.puml) — `N02-12 - Entidades de repuestos e inventario` — seq `1905` · `2026-10-20T03:28:05.702Z` · bloque `8`.

## Secuencia independiente (4 partes)

Cada enlace abre un archivo PlantUML autónomo de una sola página. `N02-secuencia.puml` es solo la PARTE 1.

1. [N02-secuencia.puml](N02-secuencia.puml) — **PARTE 1** — `N02-12 - Secuencia de Gestión de repuestos e inventario`
2. [N02-secuencia-02-escrituras-maestro.puml](N02-secuencia-02-escrituras-maestro.puml) — **PARTE 2** — `N02-12 - Escrituras del maestro`
3. [N02-secuencia-03-estado-ajuste.puml](N02-secuencia-03-estado-ajuste.puml) — **PARTE 3** — `N02-12 - Desactivación, reactivación y ajuste`
4. [N02-secuencia-04-movimientos.puml](N02-secuencia-04-movimientos.puml) — **PARTE 4** — `N02-12 - Consulta de movimientos`

[Volver al índice](../README.md) · [Manifiesto de procedencia](../source-manifest.json)
