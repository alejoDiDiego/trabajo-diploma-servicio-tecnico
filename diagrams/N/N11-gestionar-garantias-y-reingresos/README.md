# Caso CU11 — N11-gestionar-garantias-y-reingresos

- Carpeta de caso: `N11-gestionar-garantias-y-reingresos` (CASE CU11; no es un capítulo N01/N02).
- Documento original: `N01.4.7` / `N02-11`.
- Estado: **Secuencias N02 independientes (2026-11-10); otros 7 diagramas históricos sin modificaciones.**
- Cobertura: **11 diagramas actuales** (7 históricos no secuenciales y 4 secuencias independientes).
- Secuencia: dividida el 2026-11-10 en 4 archivos independientes, uno por parte, conservando el cuerpo y los participantes usados de la revisión de fuente del 2026-11-09 (commit `fd9ac1ad83c6fc3799e3487a9d25988e557c7f12`); metadato en [revisiones](../sequence-revisions.json).

Los títulos de la lista se copiaron literalmente del cuerpo extraído; no se reconstruyeron. Las filas históricas 1-6 y 8 conservan su contenido sin cambios (en N04 también la fila 9 del extra). La fila 7 enlaza únicamente la PARTE 1; las demás partes se enumeran en la sección de secuencia independiente. Cada archivo es un diagrama PlantUML completo con su propio título, participantes usados y cuerpo.

## Diagramas

1. [N01-casos-de-uso-negocio.puml](N01-casos-de-uso-negocio.puml) — `N01.4.7 - Gestionar garantías y reingresos` — seq `1809` · `2026-10-20T02:31:32.198Z` · bloque `1`.
2. [N01-proceso.puml](N01-proceso.puml) — `N01.4.7 - Proceso de Gestión de garantías y reingresos` — seq `1809` · `2026-10-20T02:31:32.198Z` · bloque `2`.
3. [N01-modelo-conceptual.puml](N01-modelo-conceptual.puml) — `N01.4.7 - Modelo conceptual de garantías y reingresos` — seq `1809` · `2026-10-20T02:31:32.198Z` · bloque `3`.
4. [N02-casos-de-uso.puml](N02-casos-de-uso.puml) — `N02-11 - Caso de uso Gestionar garantías y reingresos` — seq `1809` · `2026-10-20T02:31:32.198Z` · bloque `4`.
5. [N02-flujo.puml](N02-flujo.puml) — `N02-11 - Flujo de Gestión de garantías y reingresos` — seq `1809` · `2026-10-20T02:31:32.198Z` · bloque `5`.
6. [N02-clases.puml](N02-clases.puml) — `N02-11 - Clases de información afectadas` — seq `1809` · `2026-10-20T02:31:32.198Z` · bloque `6`.
7. [N02-secuencia.puml](N02-secuencia.puml) — `N02-11 - Secuencia de Gestión de garantías y reingresos` — seq `1809` · `2026-10-20T02:31:32.198Z` · bloque `7` — **PARTE 1 de 4** de la secuencia independiente; no es un resumen del caso. Las demás partes están abajo.
8. [N02-der.puml](N02-der.puml) — `N02-11 - Entidades afectadas por garantías y reingresos` — seq `1809` · `2026-10-20T02:31:32.198Z` · bloque `8`.

## Secuencia independiente (4 partes)

Cada enlace abre un archivo PlantUML autónomo de una sola página. `N02-secuencia.puml` es solo la PARTE 1.

1. [N02-secuencia.puml](N02-secuencia.puml) — **PARTE 1** — `N02-11 - Secuencia de Gestión de garantías y reingresos`
2. [N02-secuencia-02-crear-reingreso.puml](N02-secuencia-02-crear-reingreso.puml) — **PARTE 2** — `N02-11 - Creación de reingreso`
3. [N02-secuencia-03-eval-pendiente.puml](N02-secuencia-03-eval-pendiente.puml) — **PARTE 3** — `N02-11 - Evaluación pendiente del reingreso`
4. [N02-secuencia-04-decision-garantia.puml](N02-secuencia-04-decision-garantia.puml) — **PARTE 4** — `N02-11 - Decisión de garantía`

[Volver al índice](../README.md) · [Manifiesto de procedencia](../source-manifest.json)
