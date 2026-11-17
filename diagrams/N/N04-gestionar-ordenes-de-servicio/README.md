# Caso CU04 — N04-gestionar-ordenes-de-servicio

- Carpeta de caso: `N04-gestionar-ordenes-de-servicio` (CASE CU04; no es un capítulo N01/N02).
- Documento original: `N01.4` / `N02-04`.
- Estado: **Secuencias N02 independientes (2026-11-10); otros 8 diagramas históricos sin modificaciones (incluido el extra).**
- Cobertura: **12 diagramas actuales** (8 históricos no secuenciales, incluido el extra, y 4 secuencias independientes).
- Secuencia: dividida el 2026-11-10 en 4 archivos independientes, uno por parte, conservando el cuerpo y los participantes usados de la revisión de fuente del 2026-11-09 (commit `fd9ac1ad83c6fc3799e3487a9d25988e557c7f12`); metadato en [revisiones](../sequence-revisions.json).

Los títulos de la lista se copiaron literalmente del cuerpo extraído; no se reconstruyeron. Las filas históricas 1-6 y 8 conservan su contenido sin cambios (en N04 también la fila 9 del extra). La fila 7 enlaza únicamente la PARTE 1; las demás partes se enumeran en la sección de secuencia independiente. Cada archivo es un diagrama PlantUML completo con su propio título, participantes usados y cuerpo.


## Diagramas

1. [N01-casos-de-uso-negocio.puml](N01-casos-de-uso-negocio.puml) — `N01.4 - Gestionar órdenes de servicio` — seq `1126` · `2026-10-18T09:18:54.808Z` · bloque `1`.
2. [N01-proceso.puml](N01-proceso.puml) — `N01.4 - Proceso de Gestión de órdenes` — seq `1126` · `2026-10-18T09:18:54.808Z` · bloque `2`.
3. [N01-modelo-conceptual.puml](N01-modelo-conceptual.puml) — `N01.4 - Modelo conceptual de Gestión de órdenes` — seq `1126` · `2026-10-18T09:18:54.808Z` · bloque `3`.
4. [N02-casos-de-uso.puml](N02-casos-de-uso.puml) — `N02-04 - Caso de uso Gestionar órdenes de servicio` — seq `1126` · `2026-10-18T09:18:54.808Z` · bloque `4`.
5. [N02-flujo.puml](N02-flujo.puml) — `N02-04 - Flujo de Gestión de órdenes de servicio` — seq `1126` · `2026-10-18T09:18:54.808Z` · bloque `5`.
6. [N02-clases.puml](N02-clases.puml) — `N02-04 - Clases de información afectadas` — seq `1126` · `2026-10-18T09:18:54.808Z` · bloque `6`.
7. [N02-secuencia.puml](N02-secuencia.puml) — `N02-04 - Secuencia de Gestión de órdenes` — seq `1126` · `2026-10-18T09:18:54.808Z` · bloque `7` — **PARTE 1 de 4** de la secuencia independiente; no es un resumen del caso. Las demás partes están abajo.
8. [N02-der.puml](N02-der.puml) — `N02-04 - Entidades afectadas por Gestión de órdenes` — seq `1126` · `2026-10-18T09:18:54.808Z` · bloque `8`.
9. [extra-relaciones-principales-gestion-ordenes.puml](extra-relaciones-principales-gestion-ordenes.puml) — `Relaciones principales de Gestión de órdenes` — seq `2366` · `2026-10-20T22:28:47.032Z` · bloque `1` — archivo extra; no sustituye ninguno de los ocho estándar.

## Secuencia independiente (4 partes)

Cada enlace abre un archivo PlantUML autónomo de una sola página. `N02-secuencia.puml` es solo la PARTE 1.

1. [N02-secuencia.puml](N02-secuencia.puml) — **PARTE 1** — `N02-04 - Secuencia de Gestión de órdenes`
2. [N02-secuencia-02-recepcion-registro.puml](N02-secuencia-02-recepcion-registro.puml) — **PARTE 2** — `N02-04 - Recepción nueva y registro de orden`
3. [N02-secuencia-03-carga-orden.puml](N02-secuencia-03-carga-orden.puml) — **PARTE 3** — `N02-04 - CargarOrden con servicios y repositorios`
4. [N02-secuencia-04-operacion-retorno.puml](N02-secuencia-04-operacion-retorno.puml) — **PARTE 4** — `N02-04 - Operaciones sobre la orden y retorno al listado`

[Volver al índice](../README.md) · [Manifiesto de procedencia](../source-manifest.json)
