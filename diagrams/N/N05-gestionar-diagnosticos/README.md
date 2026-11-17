# Caso CU05 — N05-gestionar-diagnosticos

- Carpeta de caso: `N05-gestionar-diagnosticos` (CASE CU05; no es un capítulo N01/N02).
- Documento original: `N01.4.1` / `N02-05`.
- Estado: **Secuencias N02 independientes (2026-11-10); otros 7 diagramas históricos sin modificaciones.**
- Cobertura: **10 diagramas actuales** (7 históricos no secuenciales y 3 secuencias independientes).
- Secuencia: dividida el 2026-11-10 en 3 archivos independientes, uno por parte, conservando el cuerpo y los participantes usados de la revisión de fuente del 2026-11-09 (commit `fd9ac1ad83c6fc3799e3487a9d25988e557c7f12`); metadato en [revisiones](../sequence-revisions.json).

Los títulos de la lista se copiaron literalmente del cuerpo extraído; no se reconstruyeron. Las filas históricas 1-6 y 8 conservan su contenido sin cambios (en N04 también la fila 9 del extra). La fila 7 enlaza únicamente la PARTE 1; las demás partes se enumeran en la sección de secuencia independiente. Cada archivo es un diagrama PlantUML completo con su propio título, participantes usados y cuerpo.

## Diagramas

1. [N01-casos-de-uso-negocio.puml](N01-casos-de-uso-negocio.puml) — `N01.4.1 - Gestionar diagnósticos` — seq `1197` · `2026-10-19T17:47:20.445Z` · bloque `1`.
2. [N01-proceso.puml](N01-proceso.puml) — `N01.4.1 - Proceso de Gestión de diagnósticos` — seq `1197` · `2026-10-19T17:47:20.445Z` · bloque `2`.
3. [N01-modelo-conceptual.puml](N01-modelo-conceptual.puml) — `N01.4.1 - Modelo conceptual de Gestión de diagnósticos` — seq `1197` · `2026-10-19T17:47:20.445Z` · bloque `3`.
4. [N02-casos-de-uso.puml](N02-casos-de-uso.puml) — `N02-05 - Caso de uso Gestionar diagnósticos` — seq `1197` · `2026-10-19T17:47:20.445Z` · bloque `4`.
5. [N02-flujo.puml](N02-flujo.puml) — `N02-05 - Flujo de Gestión de diagnósticos` — seq `1197` · `2026-10-19T17:47:20.445Z` · bloque `5`.
6. [N02-clases.puml](N02-clases.puml) — `N02-05 - Clases de información afectadas` — seq `1197` · `2026-10-19T17:47:20.445Z` · bloque `6`.
7. [N02-secuencia.puml](N02-secuencia.puml) — `N02-05 - Secuencia de Gestión de diagnósticos` — seq `1197` · `2026-10-19T17:47:20.445Z` · bloque `7` — **PARTE 1 de 3** de la secuencia independiente; no es un resumen del caso. Las demás partes están abajo.
8. [N02-der.puml](N02-der.puml) — `N02-05 - Entidades afectadas por Gestión de diagnósticos` — seq `1197` · `2026-10-19T17:47:20.445Z` · bloque `8`.

## Secuencia independiente (3 partes)

Cada enlace abre un archivo PlantUML autónomo de una sola página. `N02-secuencia.puml` es solo la PARTE 1.

1. [N02-secuencia.puml](N02-secuencia.puml) — **PARTE 1** — `N02-05 - Secuencia de Gestión de diagnósticos`
2. [N02-secuencia-02-inicio-diagnostico.puml](N02-secuencia-02-inicio-diagnostico.puml) — **PARTE 2** — `N02-05 - Inicio del diagnóstico`
3. [N02-secuencia-03-fin-eval-garantia.puml](N02-secuencia-03-fin-eval-garantia.puml) — **PARTE 3** — `N02-05 - Finalización y evaluación pendiente de garantía`

[Volver al índice](../README.md) · [Manifiesto de procedencia](../source-manifest.json)
