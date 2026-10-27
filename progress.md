# Session Progress
## Publication authorization (feat-014, 2026-10-27)

- The owner explicitly authorized both a commit and a push to origin/main for the 149 diagrams/N archive files and the two tracking files; preexisting local tooling is excluded.
- Preflight observed main at aff17b4 with origin/main equal to that commit; the archive remained byte-identical to its 129 public chat sources (268,095 bytes), and the structural gate passed. Publication identity and remote verification are reported in the conversation and Git history.
- Sequence-diagram refinement is authorized only as an N04 proposal in chat. Do not overwrite the archived sequence or propagate changes to other cases until the owner approves the proposal.

## feat-014 Checker verification (2026-10-27) - PASS

- Independent verdict: PASS. `feat-014` is `done`; all five acceptance criteria are verified. This dated Checker record was prepended and does not rewrite the Builder record farther down.
- Fidelity: a fresh read-only decode of the public session archive (concatenated ZSTD frames; the live file grows, 1347 frames in the last pass) used only assistant `text` content for the allowlist seq 913, 987, 1048, 1126, 1197, 1285, 1359, 1451, 1587, 1729, 1809, 1905, 1977, 2062, 2140, 2211 and 2366, with CRLF-to-LF canonicalization only. All 129 bodies matched byte-for-byte against the files: 268,095 UTF-8 bytes. The N04 extra keeps sha256 `c74f23c887b05ec33d96c0a78fc9695bd7244186e35469047fe44efa650a16a3` and does not replace `N02-clases`. Old rejected seqs 601/637/687/773/863/879/888 are excluded.
- Manifest/layout: 129 unique records (128 standard + 1 extra) match path/case/cuOrder/original N01/N02/folder/file/seq/UTC ISO/block/title/bytes/sha; the allowlist is exact and the totals 129/128/1/2/18/18/268,095 match. The delivery is 149 new files: 129 `.puml`, 19 README (root + 18 cases) and `source-manifest.json`, in 18 case folders. N17/N18 are README-only with zero placeholder `.puml`.
- READMEs: 184/184 relative Markdown links resolve; the CU01-18 mapping and every listed title/seq/UTC/block match the extraction; the root wording was checked for the agreed-format presentation (not individual approval), 16 x 8 + 1 = 129, rendering-not-executed, and the absence of a current-source or runtime certification claim.
- Hygiene: all 149 files are UTF-8 without BOM, LF, final LF and no trailing whitespace; each `.puml` has exactly one `@startuml` and one `@enduml`, an exact title, and no include or URL. `source-manifest.json` parses.
- Preservation/scope: 280/280 protected-baseline records (275 tracked plus 5 preexisting untracked files under `.codegraph/`, `.playwright-mcp/` and `skills-lock.json`; 13,930,341 bytes) rehashed identically, including 21 old G06 files and 40 tracked `.puml`. Exactly 149 untracked files under `diagrams/N` plus these two state files changed; the 13 prior feature entries are semantically identical to HEAD `aff17b4`; branch `main`, HEAD unchanged, nothing staged.
- Gates after the state update: `git diff --check` clean; `init.ps1` PASS with 14 tasks and 296 relative links.
- Limits: no PlantUML renderer was executed; syntax, layout and visual legibility are not certified. No build, application startup, database, concurrency or runtime test was performed. The 18-20 Oct chat extracts are provenance evidence, not a new current-source proof. Temporary Checker artifacts remain outside the repository under `%TEMP%\feat014-checker`.
## Current documentation task (feat-013, 2026-10-20) - completed

- Objective achieved: applied the owner's reviewed compact G06 PlantUML diagrams and Spanish Nombre/Características/Comportamientos tables; added the compact physical DER and its table descriptions. This dated section supersedes older Current State objectives for this documentation task only.
- Checkout: checkpoint-6-arreglos-y-mejoras @ 9417b47. feat-013 is done; all five acceptance criteria are verified and no task remains in progress. Earlier feat-001..feat-012 evidence is historical and was not rewritten.
- Deliverables: [G06 index](diagrams/G06-diagrama-de-clases-de-todos-los-modulos/README.md), the nine updated layer pairs and the new [compact DER](diagrams/G06-diagrama-de-clases-de-todos-los-modulos/G06.7-der.puml) with its companion description. Exactly 21 documentation files belong to this delivery (19 existing files updated, two DER files added), plus the two task-state files. The four repository groups now contain 8/5/6/6 own types, with ReparacionRepository in orders and ReporteRepository in stock/purchases/warranties.
- Scope preserved: no application source, configuration, SQL, database data, older diagrams or local tooling changed. The baseline had no tracked changes. The preexisting .codegraph/, .playwright-mcp/ and skills-lock.json remain untouched; all 259 protected-file fingerprints match the pre-edit manifest. Nothing was staged, committed, pushed or switched.
- Evidence boundary: class diagrams are source/documentation verified, not runtime accepted. DER evidence is the earlier read-only SQL metadata inspection in this same session on 2026-10-20, TPIntegrador dbo: 30 application tables, 42 enabled/trusted FKs, six nullable FK columns, four unique FK columns, three composite primary keys and one excluded SSMS sysdiagrams table. UX_Clientes_Documento is not installed in that inspected database. No business rows were read to infer why. No new database access was used for this documentation update.

### Verification Evidence - feat-013

| Check | Result |
| --- | --- |
| Baseline harness | PASS before edits: 12 tasks, 292 relative links and git diff --check; source base 9417b47, only the three preexisting local-tool entries untracked. |
| Integrated diagram/table coverage | PASS: 10 pairs, 21 documentation files, 137 represented own class/contract types plus 30 DER tables, each with exactly one matching descriptive row. UI deliberately omits the 32 private nested helpers rather than claiming they were deleted. |
| Source anchors and APIs | PASS: 955 represented member names found in their owner source; APP/REPO comparator also confirmed 382 public method name/return pairs and all 55 public operations of OrdenServicioService. |
| DER structure | PASS: 30 entities, 42 FK edges, six identifying FK edges, six nullable FK fields and four unique FK fields; no invented relationships for Bitacora, ControlCambios or DigitosVerticales. |
| Markdown, PlantUML and encoding | PASS: 151 relative links, table column counts, one start/end per diagram, balanced declaration/note/legend blocks and quotes, unique aliases, no remote includes; all 21 documents UTF-8 without BOM, LF, final newline and no trailing whitespace. |
| Secret-pattern scan | PASS for added/updated documentation: no embedded connection credentials, password/hash values or cryptographic secret assignments. Private field names appear without values. |
| Protected scope | PASS: 259 pre-edit hashes unchanged, including all protected tracked source/configuration/SQL/old diagrams and the preexisting local-tool files. Only the 23 authorized paths are changed or added. |
| Independent documentation review | PASS, no blocking findings: one child reviewed the parent's five UI/DER/index documents; the other independently reviewed the six DOMAIN/ABSTRACTIONS/SERVICES documents. The parent reviewed APP/REPO content and source anchors independently of that writer and ran the integrated validator. |
| Final structural gate | init.ps1 and git diff --check passed during verification; the closure re-run is recorded in the next line. Git emits only core.autocrlf warnings about a future conversion; byte checks confirm the documented LF files and no Git configuration was changed. |

- Closure check: PASS after the final state update: init.ps1 (13 tasks, 294 relative links and git diff --check) and the integrated validator (10 pairs, 21 documents, 151 G06 links, 955 member-name anchors and 259 protected-file fingerprints), with no errors. One whitespace-only line introduced while indenting the new tracker entry was detected by the closure diff check and removed; the complete closure re-run then passed.
- Temporary-validator corrections: a reused typed PowerShell variable and parsing of linked first-column Markdown labels initially caused validator failures; both were investigated and corrected only in the temporary validator, then the integrated run passed. They were not defects in application files or permission to change source.
- Review artifacts are outside the repository under %TEMP%: g06-feat013-protected-manifest.json, g06-feat013-validate-4df874.ps1 and g06-feat013-validation.json. They are not application dependencies or deliverables.
- Not run / limits: no build, application launch, migrations, seeds, behavior/concurrency/visual tests, database writes, tool installation or remote rendering. No local PlantUML renderer was found in this session. Structural syntax checks do not certify rendering; the DER 1920x1080 limit does not certify visual legibility. No runtime or cryptographic security claim is renewed.
- Blockers: none for the authorized documentation acceptance; unavailable rendering is an explicitly disclosed verification limit, not a passing render.
- Next Session: no remaining implementation action for feat-013. An owner-requested rendered-image/layout review or Git publication would be a separate authorization.

## Publication handoff (2026-10-18)

- Owner explicitly authorised commit and push on the current branch, excluding the three local-tool entries. This supersedes historical no-Git-authorization statements below for this publication only; no branch switch, merge, rebase or force push was authorised or performed.
- Published project commit: c2adddeba934398c9878c50cc0f56b3e153bb527, branch checkpoint-6-arreglos-y-mejoras, 36 project files (17 previously tracked modifications and 19 G06 documentation files). Remote branch and local HEAD were independently confirmed equal with git ls-remote after push.
- Completed scope remains feat-001 through feat-012, all done; no passing feature was reopened. The current N01 request is a chat-only academic draft, not a new repository implementation task or backlog.
- Checks before publication: init.ps1 PASS (12 tasks, 292 links, git diff --check); staged diff --check PASS; exact 36-file publication scope; high-signal credential-pattern scan of additions clean. No new build, application launch, database access or runtime certification.
- Preserved locally and excluded from both commits: .codegraph/, .playwright-mcp/, skills-lock.json. No content in those entries was modified.
- This handoff-only follow-up commit records the publication; no application or G06 artifact is changed by this note. Future source work remains subject to the repository harness and a newly authorised feature.


## G06 documentation round (feat-012, 2026-10-17)

- Current objective: owner-authorized feat-012 is **done** (fresh independent Checker PASS on retry round 1, 2026-10-17; all six criteria verified); no feature remains in progress. It produced nine G06 PlantUML diagrams and companion Spanish class-description tables in the new diagrams/G06-diagrama-de-clases-de-todos-los-modulos folder. Exact scope and acceptance criteria are in feature_list.json (the sole backlog). Earlier Current State objectives below are historical.
- Checkout: checkpoint-6-arreglos-y-mejoras; all preexisting tracked source/guide changes and .codegraph/, .playwright-mcp/, skills-lock.json must be preserved. No Git writes are authorized.
- Baseline: powershell -NoProfile -File .\init.ps1 PASS (11 tasks, 292 relative links, diff hygiene). This proves structure only.
- Verification boundary: documentation/source conformance, new links and syntax/content checks only; no build, application startup or database access. Fresh Planner -> Builder -> Checker, sequential; no second Builder in this round.
- File-write workaround: Google Drive lacks the temp-file/hard-link support used by DSH file writers; authorized repository writes use PowerShell/.NET shell writes instead.
- Stage: retry round 1 complete with an independent Checker **PASS** (2026-10-17): `feat-012` is `done`, all six acceptance criteria are verified (criteria 2 and 3 moved to true), and no feature remains in progress. The previous Checker FAIL evidence and the retry Builder report remain below as historical. The Checker wrote only `feature_list.json` and this file over the nine G06 paths repaired by the Builder plus the two state files (21-path authorization, no new path); no source/config/SQL/build/app/DB/install/download/remote-render/Git write. Recommended next step: none for `feat-012`; the pending checkpoint-6 work may be committed when the owner authorises a Git write.

## Current State

- Last Updated: 2026-10-17 (`feat-011` Checker: **all six criteria and all four `vis-freeze-*` scenarios PASS** through the real WinForms UI with SQL readback, a real two-process concurrency proof and the regressions re-run; the feature is `done`). Previous update: 2026-10-17 (`feat-011` Builder: the read-only-original rule is implemented in source and the six-project Debug / Any CPU build passes; **no application launch, no database access and no runtime test were performed**, so its six criteria and four `vis-freeze-*` scenarios remain unverified and the feature stays `in-progress`, awaiting its Checker). Previous update: 2026-10-17 (owner-authorized `feat-011` added: after a delivered order enters warranty the original order must stay read-only - no delivery cancellation, no edits, no transitions - leaving only inspection and opening warranty re-entries). Previous update: 2026-10-13 (feat-010 Checker: **documentation-only verification PASS**; the catalogue, the root implementation notes and the limitations register were judged against the dated feat-006..feat-009 runtime evidence, and links, harness and protected scope were checked; no runtime test repeated)
- Current Objective: `feat-011` is **`done`**: independently verified on 2026-10-17 through the real WinForms UI against the authorized non-production database, with SQL readback, an external two-process concurrency proof and the feat-009/GAR/ENT regressions re-run (see the feat-011 Checker section below); all six criteria and all four `vis-freeze-*` scenarios passed and no task remains `in-progress`. Previous objective (historical): `feat-011` was **`in-progress`** while the Builder implemented the immutable-original rule in source - typed domain exceptions, the existence predicate `TieneReingresosGarantia`, its scoped repository guards, the guard on every existing-order service mutator, the three paired ES/EN keys and the UI freeze with the consultation banner - and verified it by source inspection plus the compiled solution **only**. That Builder state is historical: nothing was runtime-verified then, and the feat-011 Checker section below closes every open item. Previous objective (historical): `feat-010` is **`done`** (documentation only, owner-authorized): its Builder aligned [business-test-catalogue.md](docs/agents/business-test-catalogue.md) (CLI-03, CLI-04, PRE-01, PRU-01, GAR-02, GAR-09, ENT-03), the root `IMPLEMENTACION_SERVICIO_TECNICO.md` and [known-limitations.md](docs/agents/known-limitations.md) with the rules enforced by feat-006..feat-009, and record the clipped 900x600 controls as a known limitation; no source, configuration, SQL or database change was made and **no runtime test was repeated**; its Checker verified all five criteria with documentation, link, tracker and diff checks only (see the feat-010 Checker section below). `feat-009` is **done**: only one warranty re-entry of an original order may be open at a time (refused with a business message while a sibling is open, allowed again once the sibling is delivered or its rejection reaches a committed destination) and the Garantia tab lists the re-entries with number, reception date and translated state/result - independently verified through the real UI with database readback in its Checker section. No task remains `in-progress`: the next session can start the following authorized work.
- Previous objective (historical): `feat-008` **done** (current-intervention-only test registration, verified through the real UI with database readback, 4/4 criteria and 3/3 scenarios). The `feat-009` Builder report below is historical: it certifies source plus compilation only, and its open items were closed by the Checker section above. `feat-010` (documentation only) is `done` (5/5 criteria, documentation-only checks).
- Previous objectives (historical): `feat-007` **done** (spare-part budget item type, verified through the real UI with database readback, 5/5 criteria and 3/3 scenarios). `feat-006` **done** (unique customer documento across active and inactive clientes, verified through the real UI with database readback, 6/6 criteria and 4/4 scenarios). `feat-005` **done**: COM-07, GAR-08 and INF-11 fixed and re-verified through the real UI. `feat-004` sweep A-F remains complete (all 144 rows).
- Checkout observed: `checkpoint-6-arreglos-y-mejoras` @ `5114582` plus local changes (the uncommitted feat-006 work preserved + the feat-007 diff + the feat-008 diff + the feat-009 diff + the feat-010 diff + the feat-011 source/documentation diff and the feat-011 Checker tracker diff); nothing staged, committed or pushed.
- Preexisting untracked entries to preserve: `.codegraph/`, `.playwright-mcp/`, `skills-lock.json`.
- Environment note: the leftover `UI.exe` (PID 84192) was closed **gracefully through its own window** by the Checker on 2026-10-13 (pywinauto WM_CLOSE on `FrmPrincipal`, no dialog, process exited; no taskkill/Stop-Process and no user or IDE process touched). The documented Debug / Any CPU solution build then passed with exit 0 and refreshed `PRESENTATION\bin\Debug\UI.exe`. The feat-009 Checker run's own `UI.exe` (PID 73756) was also closed gracefully through its own window (WM_CLOSE on `FrmPrincipal`, no dialog) at the end of the session and `tasklist` reports no remaining instance; ``PRESENTATION\bin\Debug`` is free for the next build. The feat-011 Checker run own ``UI.exe`` (PID 31404, launched 18:57:43 from the freshly rebuilt binary, SHA256 781BF1B8...) was closed gracefully through its own window (WM_CLOSE on ``FrmPrincipal``, no dialog) at 21:18:43 and ``tasklist`` reports no remaining instance; no taskkill/Stop-Process was used.
- Run artifacts (outside the repository): `%LOCALAPPDATA%\ServicioTecnicoDesktopTests\evidence\sweep-20261003B\` with `sweep-log.md` and `sweep-20261003B.md`, and `...\evidence\feat006-20261013\` with `feat006-log.md` and `shots\`; driver scripts under `...\scripts\sweep\` (`feat006_s0_login.py` .. `feat006_s7_regression.py`). feat-007 kept `...\evidence\feat007-20261013\`; this Checker run kept `...\evidence\feat008-20261013\` with `feat008-log.md`, `state.json` and 17 screenshots in `shots\`, plus the driver scripts `...\scripts\sweep\feat008kit.py`, `feat008_s0_setup.py`, `feat008_s1_previous.py`, `feat008_s1b_disabled.py`, `feat008_s2_switchback.py`, `feat008_s2b_switchback.py`, `feat008_s3_current.py`, `feat008_s3b_current.py`, `feat008_s3c_current.py`, `feat008_s3d_current.py`, `feat008_s3e_current.py`, `feat008_s4_texts_layout.py` and `feat008_s5_en_selection.py`. feat-009 kept `...\evidence\feat009-20261013\` with `feat009-log.md`, `state.json`, the `s0-*`..`s4d-*` screenshots and the `feat009*.py` drivers listed in its Checker section.

## Builder report (feat-011, 2026-10-17)

Objective: once an original order has **any** warranty re-entry created, that original is read-only -
cancelling its delivery is refused and every other mutation of it is refused, with a translated
business message and no write - while inspection stays available and creating further warranty
re-entries keeps the feat-009 single-open rule. Orders without any warranty re-entry keep today's
behaviour, ENT-03 delivery cancellation included. Scope: only the 17 authorized files; no new file,
no project/`.csproj` edit, no DDL, no migration, no data change.

Rule decided by the owner and implemented as stated: the freeze is **existence-based**, so it does
not expire when the re-entries close and never consults coverage validity, expiry or evaluation
state. It is not a new state and never rewrites the order.

| File | Method / member | Change |
| --- | --- | --- |
| [OrdenServicio](DOMAIN/Features/Ordenes/OrdenServicio.cs) | `OrdenOriginalSoloConsultaException` (new, same file) | `ReglaNegocioException` subclass carrying `ClaveMensaje` so the UI translates by type; no new file and no project edit |
| [Garantia](DOMAIN/Features/Garantias/Garantia.cs) | `ReingresoGarantiaOrigenNoDisponibleException` (new, same file) | Typed origin-ineligible refusal for the locked re-entry creation |
| [OrdenServicioRepository](INFRASTRUCTURE/Features/Ordenes/OrdenServicioRepository.cs) | result codes (new) | `ResultadoOk 0`, `ResultadoReingresoAbierto -1`, `ResultadoOrdenSoloConsulta -2`, `ResultadoOrigenNoDisponible -3`; the SQL literals in each batch match them |
| same | `TieneReingresosGarantia` (new) | Single existence read over `id_orden_origen` + `tipo_orden = 'Garantia'`: no state filter, no evaluation join, no coverage/expiry check, no parent restriction; deliberately not built from `PredicadoReingresoAbierto` |
| same | `CambiarEstadoConHistorial` | Now returns `int`; `TABLOCKX,HOLDLOCK` existence guard is the first statement, before the first `UPDATE`/history `INSERT`; `-2` writes nothing |
| same | `ReabrirPruebasConTransicion` | Same guard and `int` return; `resultado = NULL` path and history insert unchanged on success |
| same | `CrearConHistorial` | Gained `out int codigoResultado`; in the warranty branch the origin eligibility (`Entregado` + `Reparado` + delivery row, same lock) is re-read first and returns `-3` before any `INSERT`, then the **unchanged** single-open predicate returns `-1`; normal creation unchanged |
| [EntregaRepository](INFRASTRUCTURE/Features/Ordenes/EntregaRepository.cs) | `CancelarEntregaConTransicion` | After the existing locked parent read and before the sibling test and the `DELETE`: any warranty child returns `-2` with no DML; the `-1` sibling branch, the delete/update/history transaction and `CrearConTransicion` are untouched |
| [OrdenServicioService](APPLICATION/Features/Ordenes/OrdenServicioService.cs) | `EsSoloConsultaPorReingresos` (new, public) | Read wrapper over the repository predicate, used by both forms |
| same | `ObtenerOrdenParaModificar`, `ValidarBloqueoSoloConsulta` (new, private) | Existence + predicate + typed refusal; sentinel decoder for the `-2` batches |
| same | 28 existing-order mutators | Owner load replaced by `ObtenerOrdenParaModificar` **before** any domain mutation, repository write or audit: `AsignarTecnico`, `IniciarDiagnostico`, `FinalizarDiagnostico`, `EmitirPresupuesto`, `RegistrarAprobacionPresupuesto`, `RegistrarRechazoPresupuesto`, `SolicitarAdicional`, `EmitirAdicional`, `RegistrarAprobacionAdicional`, `RegistrarRechazoAdicional`, `CrearBorrador`, `PublicarBorrador`, `EliminarBorrador` (previously loaded no owning order at all), `AnularPresupuesto` (before `presupuestoDb.Anular` and before the order is touched), `CancelarSolicitudAdicional`, `QuitarConsumo`, `CancelarOrden`, `EntregarOrden`, `CancelarEntrega`, `ModificarRecepcion`, `IniciarReparacion`, `ConsumirRepuesto`, `FinalizarReparacion`, `RegistrarPrueba`, `FinalizarPruebas`, `ReabrirPruebas`, `AnularPrueba` and `EvaluarReingreso` (target re-entry only) |
| same | guarded catches, batch results | Every guarded method now rethrows with `throw;` (the concrete type and its `ClaveMensaje` survive); the four `CambiarEstadoConHistorial` call sites and `ReabrirPruebasConTransicion` decode `-2`; `CancelarEntrega` maps `-2` and `-1` exactly instead of "any negative"; `CrearReingresoGarantia` decodes `-3` before the existing `-1` sentinel |
| same | reads and creation kept | `EvaluarReingreso` guards only its target re-entry and still reads its frozen original; `CrearOrden`, `CrearReingresoGarantia` and all reads keep `ObtenerOrdenExistente` |
| [GarantiaService](APPLICATION/Features/Garantias/GarantiaService.cs) | `Crear`, `Anular`, `ValidarOrdenOriginalModificable` (new) | The target original is validated before coverage is written; concrete type preserved with `throw;` |
| [IdiomaRepository](INFRASTRUCTURE/Features/Idiomas/IdiomaRepository.cs) | `SembrarDatosIniciales` | Three new paired ES/EN keys: `Mensaje.OrdenOriginalSoloConsulta`, `OrdenDetalle.OrdenOriginalSoloConsulta`, `Mensaje.GarantiaOrigenNoDisponible`; idempotent `AgregarSeed`, no existing translation overwritten |
| [FrmOrdenServicioDetalle.Designer.cs](PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.Designer.cs) | designer | New wrapping `LBL_OrdenSoloConsulta` (12,106, 896x32, `Tag OrdenDetalle.OrdenOriginalSoloConsulta`, hidden by default) between the header panel and the tabs; tabs moved from y=112 to y=142 keeping 480 px, `BTN_Cerrar` from y=610 to y=640, client area 920x654 -> 920x684, so no tab content, header data or bottom edge moved |
| [FrmOrdenServicioDetalle](PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.cs) | `CargarOrden`, `CargarModoNuevo`, `ActualizarTextos`, `ActualizarAvisoSoloConsulta` (new) | Predicate read together with the order and before any tab load (a failed read shows the error and closes the detail instead of defaulting to editable), reset in new mode, banner text translated by `Tag` |
| same | `AplicarPermisosDetalle`, `ActualizarAccionesPruebas`, `ActualizarBotonAnularPrueba`, `AplicarPermisosGarantia` | Every mutation eligibility intersected with the freeze, including the direct `puedeDecidir` paths, delivery/cancellation/reopen, diagnosis, budget/draft/additional, repair/consumption/test/evaluation inputs and buttons; tabs, read-only grids, history, `Ver original`, `Cerrar` and the existing-budget selector stay usable (`CBO_SelectorPresupuesto` enabled for a frozen order) |
| same | `CargarSelectorPresupuesto` | Pseudo `(nuevo ...)` entries omitted while frozen, so only existing documents can be selected |
| same | `BTN_CancelarEntrega_Click`, `BTN_CrearReingreso_Click`, `MostrarError` | Fresh fail-closed freeze check before the confirmation and the reason prompt; re-entry refresh after a typed refusal; typed mapping by `InnerException` walk for both new exceptions, never by message text |
| [FrmOrdenesServicio](PRESENTATION/Forms/Ordenes/FrmOrdenesServicio.cs) | `AplicarPermisos`, `BTN_Cancelar_Click`, `BTN_Entregar_Click`, `MostrarError`, `EsSoloConsulta`/`BuscarSoloConsulta` (new) | Selected row revalidated for legacy frozen rows and again fail-closed before prompts or navigation; freeze type mapped by exception type; no new cancel-delivery action and no designer change |
| guides | business-flows.md, persistence.md, business-test-catalogue.md, known-limitations.md | Frozen-original rule versus open-sibling rule, guard inventory, scoped enforcement, result codes and the cancellation-versus-creation lock invariant; catalogue rows ENT-03, ENT-07, GAR-02, GAR-09 and GAR-11 distinguish frozen-original from no-child behaviour; new feat-011 limitations section plus the updated original-cancellation caveat and banner layout note, preserving the coverage-date, permission, small-window and partial-success limits |

Intentionally untouched (authorized but no change needed):
[GarantiaRepository](INFRASTRUCTURE/Features/Garantias/GarantiaRepository.cs) (the coverage/visits
comment from feat-009 already describes the cardinality; no query change was required). Not
touched at all because they are outside the authorized list: `PresupuestoRepository`,
`DiagnosticoRepository`, `ReparacionRepository`, `EvaluacionGarantiaRepository`,
`HistorialEstadoOrdenRepository`, [SQLHelper](INFRASTRUCTURE/SQLHelper.cs), every `.csproj`,
`IMPLEMENTACION_SERVICIO_TECNICO.md` and the desktop tooling.

Checks actually run:

| Check | Command | Result |
| --- | --- | --- |
| Baseline harness | `powershell -NoProfile -File ".\init.ps1"` and `git diff --check` before any edit | PASS: 11 tasks, state/completion gates, 274 relative links, `git diff --check` exit 0 |
| Build (documented command) | vswhere + `MSBuild .\TP_INTEGRADOR_2022.sln /t:Build /p:Configuration=Debug "/p:Platform=Any CPU" /m` | **PASS: exit 0**; six projects, `UI -> PRESENTATION\bin\Debug\UI.exe` refreshed and no `UI.exe` was running (`tasklist` reports no match), so no output was locked and no process was killed |
| Build (clean rebuild) | same command with `/t:Rebuild` | **PASS: exit 0**, 0 warnings, 0 errors, all six assemblies produced |
| Intermediate build after the domain/repository/service edits | same command | exit 1 only because the new repository constants had not been written yet by a failed script step; the constants were applied and the build then passed (recorded here for honesty, not as a feature failure) |
| Guard-before-write matrix | scripted scan of `OrdenServicioService.cs`: every 8-space method declaration, its `ObtenerOrdenParaModificar` line, its first repository-write or `new BitacoraService()` line and its catch shape | 29 declarations carry a guard (28 mutators plus the helper itself); **0 methods reach a write before their guard**; **0 methods with a plain `catch (ReglaNegocioException)` lack `throw;`** |
| Remaining unguarded loads | same script, listing every remaining `ObtenerOrdenExistente` use | 11 read methods (`PuedeCancelarSolicitud`, `ObtenerDiagnostico`, `ObtenerPresupuesto`, `ObtenerPresupuestoOriginal`, `ListarAdicionales`, `ListarTodosPresupuestos`, `ExisteAdicionalPendiente`, `CalcularMontoAutorizado`, `ObtenerEntrega`, `ListarReparaciones`), the re-entry creation path (`CrearReingresoGarantia`), the frozen-original read inside `EvaluarReingreso`, `RegistrarEvaluacionPendienteReingreso` reading its original, and the helper itself; every mutating path is guarded |
| Form/SQL check | grep for SQL keywords and repository types in both order forms | 0 SQL statements in `FrmOrdenServicioDetalle.cs`, `FrmOrdenesServicio.cs` and the designer; both call the service only |
| Seed pairing | text count per key in `IdiomaRepository` | `Mensaje.OrdenOriginalSoloConsulta`, `OrdenDetalle.OrdenOriginalSoloConsulta` and `Mensaje.GarantiaOrigenNoDisponible` each ES=1 / EN=1 |
| Tracker JSON | `ConvertFrom-Json` over `feature_list.json` | Valid; `feat-011` `in-progress` and the only non-`done` feature, 17 authorized files, 6/6 criteria `verified: false`, 4/4 scenarios `planned` with empty evidence |
| Diff hygiene | `git status --short`, `git diff --check`, per-file BOM/ending readback | Only authorized files changed (16 of 17; no new file); `git diff --check` exit 0; BOM-less files keep their original endings (CRLF for source, LF for the guides, the tracker and this file) |
| Harness (final) | `powershell -NoProfile -File ".\init.ps1"` | PASS (see the re-run recorded below) |

Deviations from the planner result (all deliberate, none silently dropped):

1. `ReingresoGarantiaOrigenNoDisponibleException` lives in
   [Garantia](DOMAIN/Features/Garantias/Garantia.cs) as instructed, but the origin re-read also
   had to check that an `Entregas` row exists for the origin: `Entregado` + `Reparado` alone is
   not the same condition the service validates before creating a re-entry, and the criterion is
   that the order cannot keep a warranty re-entry after its delivery was cancelled.
2. `CancelarEntregaConTransicion` returns `-2` for the frozen original and keeps `-1` for the open
   sibling, so `CancelarEntrega` now maps both codes explicitly instead of treating every negative
   value as the sibling conflict. This is what the planner asked for, but it means a hypothetical
   future negative code is no longer silently reported as a sibling conflict.
3. `CambiarEstadoConHistorial` and `ReabrirPruebasConTransicion` changed their return type from
   `void` to `int` (like `CancelarEntregaConTransicion` in feat-009) instead of adding a new method
   pair: their callers are all inside the guarded service, which keeps the sentinel decoding in one
   place.
4. `CrearConHistorial` gained an `out int codigoResultado` parameter rather than a result object or
   a second method, because its `OrdenServicio` return cannot express both `-1` and `-3`; both call
   sites (`CrearOrden`, `CrearReingresoGarantia`) were updated.
5. The order detail grew by 30 design px in height (client 920x654 -> 920x684) and moved the tab
   control and `BTN_Cerrar` down by the same amount, instead of shrinking the tab pages: shrinking
   them by the full banner height would have clipped the bottom of the Presupuesto page (its
   lowest control ends at y=438 in a 454 px page), and the header panel's last label box ends at
   y=98 in a 104 px panel, so neither the header nor the tabs had spare room.
6. `FrmOrdenesServicio` gained a small read helper that treats a failed predicate read as "not
   frozen" for the *button state*, while the click-time revalidation is fail-closed (error shown,
   no prompt, no write). The detail form stays strictly fail-closed. The authoritative refusal is
   always the service/repository guard.
7. The catalogue row `ENT-07` was updated in addition to the rows the planner listed (ENT-03, GAR-02,
   GAR-09, GAR-11): it is the row that describes the delivered lock the freeze extends, and leaving
   it unchanged would have contradicted the new behaviour.

**Not verified (builder scope):** no application launch, no startup migration or seed run, no
database read or write, no fixture, no concurrency exercise and none of the four `vis-freeze-*`
scenarios. The refusals, the banner and its ES/EN texts, the enabled/disabled matrix at runtime,
the banner layout at 900x600 and at large DPI, and the `-2`/`-3` branches isolated from the service
pre-check are proven by source inspection plus compilation only. This Builder report certifies
nothing.

**Next action for the Checker:** run `vis-freeze-cancel-delivery`, `vis-freeze-mutations`,
`vis-freeze-readonly-and-reentry` and `vis-freeze-regression-no-reentry` through the real WinForms
UI against the authorized non-production database: deliver a repaired order, create one warranty
re-entry, then attempt to cancel the original delivery and every other mutating action (business
message, no write, `OrdenesServicio`/`Entregas`/`Garantias`/`HistorialOrdenes`/`Bitacora`
unchanged), confirm the frozen order is still inspectable and that another re-entry can be opened
once the previous open one closes, and confirm an order **without** re-entries still cancels its
delivery. Read back the three new translation keys rather than trusting the seeded text.

## Checker verification (feat-011, 2026-10-17) - independent, runtime

**Verdict: PASS. All six acceptance criteria and all four `vis-freeze-*` scenarios verified.** Independent verification only:
no implementation, designer, SQL or configuration file was modified by this Checker; only `feature_list.json` and this file
were updated. Scope, source inventory and every refusal were re-derived from the repository, not taken from the Builder report.

### What was checked

| Check | Command / method | Result |
| --- | --- | --- |
| Harness | `powershell -NoProfile -File .\init.ps1` | PASS: 11 tasks, state/completion gates, 292 relative links, `git diff --check` |
| Whitespace / staging | `git diff --check`, `git diff --cached --stat`, `git status --short` | exit 0, nothing staged; exactly the 16 authorized files changed plus the preexisting `FrmRepuestos.cs`; `GarantiaRepository.cs` untouched; `.codegraph/`, `.playwright-mcp/`, `skills-lock.json` preserved |
| Build (documented) | vswhere + `MSBuild .\TP_INTEGRADOR_2022.sln /t:Rebuild /p:Configuration=Debug "/p:Platform=Any CPU" /m` | exit 0, six projects, 0 warnings / 0 errors; `UI.exe` rebuilt 18:50:17, SHA256 `781BF1B844FCF9E131A46758159723D6A2C547F6B947FC99B432F923EE8424AF`; the runtime process (PID 31404, launched 18:57:43) loaded that path, so the exercised binary is the fresh build |
| Source re-derivation | read the repository | the existence predicate `TieneReingresosGarantia` has no child-state, coverage, evaluation or parent-state filter; 28 existing-order service mutators load through `ObtenerOrdenParaModificar` before their first write and rethrow with `throw;`; the guards sit inside the same `ExecuteTransaction` batches under `TABLOCKX, HOLDLOCK`; result codes `-2`/`-3` are decoded by `ValidarBloqueoSoloConsulta`, `CancelarEntrega` and `CrearReingresoGarantia`; no form contains SQL; the three ES/EN keys were seeded and `AgregarSeed` never overwrites; `BTN_CrearReingreso` is not intersected with the freeze |
| Fixtures (real UI) | pywinauto 0.6.9 drivers under `scripts\sweep\feat011_*` | customer 4002, equipment 4002..4009, orders FX-A 5002, FX-B 5003, FX-C 5004, FX-D 5005, FX-E 5006, FX-F 5007, FX-G 5008, FX-H 5009; children 5010, 5011, 5012, 5013, 5014, 5015, 5016 |
| `vis-freeze-cancel-delivery` | UI + forced real mouse click + application/repository calls + SQL readback | PASS (see the log; also the stale-view path on FX-F) |
| `vis-freeze-mutations` | UI control matrix, 59 real mouse clicks, 28 external service calls, 3 locked batches, SQL readback | PASS; 0 allowed writes, snapshot and all counters unchanged |
| `vis-freeze-readonly-and-reentry` | ES/EN banner + matrix + re-entry grid, feat-009 single-open, creation after closing, Ver original, child editability | PASS |
| `vis-freeze-regression-no-reentry` | ENT-03/ENT-04/ENT-05/ENT-06 on FX-B and FX-C, then the first child | PASS |
| Criterion 6 (concurrency) | two independent `agent011harness.exe` processes loading the built assemblies, released by one shared go-file barrier, with a controlled post-barrier stagger and two unstaggered rounds | PASS: creation-first gives cancellation `-2`; cancellation-first gives creation `-3`; a stale creation after a committed cancellation gives `-3`; two simultaneous rounds committed on exactly one side, never both |
| Regressions | feat-009 single-open and reverse edge, GAR-05/06/07/08/10, ENT-04/05 | PASS (the reverse edge refused with the translated message and batch `-1`; the sibling-free cancellation succeeded) |
| Layout / sizes / DPI | banner and tab rectangles at the default size, 900x600, 1400x900 and on the main window; ES and EN | PASS; system DPI 144 (150 percent) was in force; the banner stays visible and the preexisting 900x600 clipping of the tab body is unchanged, not fixed |

### Observed facts worth keeping

- Freeze predicate: FX-C 5004 (delivered **NoReparable**, **no coverage**) froze as soon as a child row existed, and FX-A stayed frozen while all of its children were closed - the rule is existence-based, as required.
- Refusal mechanism on a frozen original: the banner is shown and `BTN_CancelarEntrega` leaves the UIA tree; a forced real mouse click and a click on the still-enabled stale control wrote nothing, and the click-time recheck produced `Mensaje.OrdenOriginalSoloConsulta`.
- External calls: 20 of 28 service mutators returned the typed `FREEZE|Mensaje.OrdenOriginalSoloConsulta`; the other 8 were refused by their own earlier state rule (finished intervention, non-adicional or non-borrador document, re-entry without a pending evaluation) and also wrote nothing. All three bypass batches (`CambiarEstadoConHistorial`, `ReabrirPruebasConTransicion`, `CancelarEntregaConTransicion`) returned `-2`.
- Lock-order proof: the harness runs two processes with one barrier; `race-fixd-create-first`, `race-fixe-cancel-first`, `race-fixe-stale-create`, `race-fixg-simultaneous` and `race-fixh-simultaneous` under `evidence\feat011-20261017\` hold the raw outputs.
- Test artifact to disclose: in the stale round the second (redundant) raw cancellation call committed one extra history row on FX-E (`Entregado -> EnPruebas` when it was already `EnPruebas`); that is the raw repository batch being called twice without the service pre-check, not a product defect, and the creation in the same round still returned `-3` with no child.
- Instrumentation note: one derived `CHECKSUM` digit in the `Diagnosticos` block of `full_snapshot(4012)` differed between my first reading and the final reading; the underlying row (`id_diagnostico` 4011, `es_reparable` 1, `tiempo_estimado_dias` 30, `descripcion` length 31) is stable and consistent, `DiagnosticoRepository.Inicializar` performs DDL only, and no write path was ever invoked against 4012. All of 4012 order/coverage/delivery/history values match the feat-009 record. Reported as an unexplained instrumentation difference, not as a verified data change.

### Not re-tested / boundaries

- Only the full-permission `admin` session was exercised; a restricted-permission variant was not reachable without credentials.
- Only the machine's DPI 144 (150 percent) and the default culture were exercised.
- The preexisting 900x600 tab-body clipping and the large-DPI rendering limits recorded in `known-limitations.md` were observed and left as they are; the feat-011 banner does not fix them.
- The feat-011 guides (`business-flows.md`, `persistence.md`, `business-test-catalogue.md`, `known-limitations.md`) were read and matched the observed behaviour, but their "runtime acceptance pending" wording was **not** updated because this Checker may only change the trackers.
- The fixtures were kept for review (no cleanup), as authorized; the historical fixtures and their related rows were not altered.
## Builder report (feat-010, 2026-10-13) - documentation only

Objective: align the maintained business test catalogue, the root implementation notes and the
limitations register with the rules that `feat-006`..`feat-009` enforce, and record the small-window
clipping as a known limitation. Scope: only the five authorized files
(`docs/agents/business-test-catalogue.md`, `IMPLEMENTACION_SERVICIO_TECNICO.md`,
`docs/agents/known-limitations.md`, `feature_list.json`, `progress.md`). **No source, configuration,
SQL or database change; no build, no application launch, no database access and no runtime test
repeated.**

| Target | Statement corrected | New wording (summary) |
| --- | --- | --- |
| catalogue CLI-03 | "Allowed; document is not unique" | Blocked with a business message naming the owner and no record created; an inactive owner blocks the creation too and its message directs the operator to tick `Ver inactivos`, because an inactive customer keeps reserving its documento; legacy duplicate groups stay untouched |
| catalogue CLI-04 | allowed edits only | Adds that the record's own unchanged documento still saves, that a documento owned by another active or inactive customer is blocked with the owner message and the record unchanged, and that telefono/email stay freely shareable |
| catalogue PRE-01 | "Repuesto item type is reserved infrastructure, not emit-enabled" | The draft adds labour/service/spare-part items; a spare part requires an existing active part and the draft persists its reference, quantity, unit price and subtotal; the reference price prefills an editable price; zero prices and active zero-stock parts are allowed; no stock reservation, movement or automatic consumption |
| catalogue PRU-01 | registration steps only | Adds the feat-008 consultation-only historical selection with a visible translated reason, unavailable registration/annulment/finalization, historical tests still displayed and eligible actions restored on returning to the current intervention, with the unchanged service guards |
| catalogue GAR-02 | steps without the open-re-entry precondition | Steps now require no open warranty re-entry of the original order |
| catalogue GAR-09 | "Confirm actual behavior in source" | Blocked while a sibling is open (business message, no new order) and allowed after it closes by delivery or by a rejected evaluation with a committed destination, original untouched; coverage 0..1, visits 0..N through `id_orden_origen`, one open visit, `Aceptada` and an accepted ListoRetiro do not close it, and the original Garantia tab lists the re-entries read-only |
| catalogue ENT-03 | normal cancellation only | Adds that a delivered warranty re-entry with another open sibling is refused with a business message, leaving delivery, state/result and history unchanged |
| root notes | `PresupuestoDetalle.id_repuesto` "solo infraestructura" and `Emitir` validating only `ManoObra`/`Servicio`; `TipoItemPresupuesto` "no habilitado en emision"; the N4 "reingreso multiple permitido" decision; the historical test-registration UI gating; `garantias_abiertas` equated with open re-entries | Corrected to the feat-007 quotation rule, the feat-009 cardinality/close rule and the feat-008 consultation-only policy, and preceded by a new historical-scope block plus a "Reglas vigentes alineadas por feat-010" summary after the title |
| known limitations | the feat-006..feat-009 headings/preamble labelled runtime acceptance pending or scenarios unverified, their "were not exercised" boundaries, the "reserved until the row is edited or reactivated" statement, the documentation-drift paragraph and the unqualified delivery-cancellation limitation | Replaced with the recorded runtime verdicts (feat-006 6/6 and 4/4, feat-007 5/5 and 3/3, feat-008 4/4 and 3/3, feat-009 5/5 and 4/4, 2026-10-13, real UI with database readback), a new small-window clipping section marked NOT fixed and pointers to the Checker sections of this file; genuinely untested boundaries (concurrency, other DPIs/cultures, long inputs, universal hit-testing) kept |
| tracker | `feat-010` | `status` set to `in-progress` (the only one), its five criteria still `verified: false`, `visual_scenarios: []` and a dated Builder note appended to its evidence |

Checks actually run:

| Check | Command | Result |
| --- | --- | --- |
| Harness | `powershell -NoProfile -File ".\init.ps1"` | PASS: 10 tasks, state/completion gates, 274 relative links, `git diff --check` |
| Whitespace / diff | `git diff --check`; per-file BOM and line-ending readback | Exit 0; the edited files keep their original BOM-less encoding (LF for the Markdown docs and the tracker, CRLF for the root implementation notes) |
| Relative links | `init.ps1` link pass plus a manual check of the root implementation notes | Every touched link resolves against its own document directory; the root `IMPLEMENTACION_SERVICIO_TECNICO.md` is outside `init.ps1`'s link check and was validated manually |
| Protected files | SHA-256 manifest of every tracked file except the five authorized ones (251 files) plus the untracked `.codegraph/`, `.playwright-mcp/` and `skills-lock.json`, before and after the edits | PASS: byte-identical; only the five authorized files differ |
| Catalogue structure | row re-read after the edits | Six columns in every corrected row; row ids and all untouched rows unchanged |

**Not applicable / not run:** no build, no application launch, no database query, no sweep and no
fixture command. The wording records the Checkers' dated runtime evidence; it does not renew it.

**Next action for the Checker:** re-read the seven corrected catalogue rows and the corrected
sections, confirm the relative links (including in the root notes), re-run `init.ps1` and
`git diff --check`, re-check the protected-file manifest, and verify that the tracker keeps
`feat-010` as the only `in-progress` feature with `visual_scenarios: []` and all five criteria
`verified: false`.

## Checker verification (feat-010, 2026-10-13) - independent, documentation only

Objective: independently verify that the five authorized files align the business test catalogue, the
root implementation notes and the limitations register with the rules enforced by
`feat-006`..`feat-009`, and that the 900x600 clipping is recorded as a known limitation.
**Verdict: 5/5 acceptance criteria PASS; `feat-010` is `done`.** No build, application launch,
database access, sweep or fixture command was run ([AGENTS.md](AGENTS.md): for documentation-only
changes, link/content/status checks suffice).

| # | Criterion | Verdict | Exact wording relied on |
| --- | --- | --- | --- |
| 1 | CLI-03 states the enforced uniqueness rule, its blocked path and the inactive-owner case | PASS | `docs/agents/business-test-catalogue.md` CLI-03: "Create second customer with same document (active or inactive owner)" / "Blocked with a business message naming the owner; no record is created. An inactive owner blocks the creation too and its message directs the operator to tick Ver inactivos, because an inactive customer keeps reserving its document" / note "Enforced by the guarded write in `ClienteRepository.Agregar` (feat-006); legacy duplicate groups are left untouched and `UX_Clientes_Documento` is installed only in a database with no pre-existing duplicate group" - all three match the dated feat-006 Checker observations (owner message, inactive-owner message, index absent because of three legacy duplicate groups) |
| 2 | Other feat-006-invalidated catalogue statements corrected; unaffected rows keep wording and status | PASS | CLI-04 adds "Its own unchanged document saves; a document owned by another active or inactive customer is blocked with the owner message and the record stays unchanged" and "telefono and email remain freely shareable". `Compare-Object` of `HEAD` against the worktree: exactly 7 rows changed (CLI-03, CLI-04, PRE-01, PRU-01, ENT-03, GAR-02, GAR-09), 7 removed plus 7 added lines, 144 row ids in both versions with an identical id sequence, no row added or removed; PRE-01 is the catalogue's only `Repuesto` mention |
| 3 | The "reserved/not emit-enabled" spare-part statements are corrected in PRE-01 and the root notes, matching feat-007 | PASS | PRE-01: "A Repuesto item requires an existing active part and the draft stores its part reference, quantity, unit price and subtotal" / "the reference price prefills an editable unit price; zero prices and active zero-stock parts are allowed; the quoted line reserves no stock and causes no reservation, movement or automatic consumption". Root notes: the `PresupuestoDetalle.id_repuesto` bullet now says "Corregido por feat-007 (2026-10-13) ... `Emitir` valida `ManoObra`, `Servicio` y `Repuesto`, exige un repuesto existente y activo y no reserva ni mueve stock"; the `TipoItemPresupuesto` bullet says "(por entonces solo infraestructura; feat-007 la habilito en la emision como linea cotizada)"; a historical-scope banner plus a Spanish current-rules section precede the per-checkpoint prose |
| 4 | The clipped `Ver inactivos` control is recorded as a known limitation with its consequence and not described as fixed | PASS | `docs/agents/known-limitations.md` heading "Small-window clipping (observed, NOT fixed, 2026-10-13)": "These are **observed legacy layout limitations, explicitly not fixed** and not described anywhere as resolved"; the Clientes row states the consequence ("The business message that a duplicate `documento` belongs to an inactive customer tells the operator to tick `Ver inactivos`, but that control is unreachable at this window size") and "A layout fix would need its own authorized scope; nothing here claims one" |
| 5 | Relative links remain valid, `init.ps1` passes, no source/SQL/database change | PASS | `init.ps1` PASS (10 tasks, 274 relative links, `git diff --check` exit 0); manual link sweep of the five files: root notes 1/1, catalogue 4/4, known-limitations 40/40, progress 57/57, feature_list 0 broken; `git diff --check` exit 0; no `.sql`/`.config`/`.csproj`/`.sln` in the diff; protected scope unchanged (below) |

Checks actually run:

| Check | Procedure | Result |
| --- | --- | --- |
| Harness | `powershell -NoProfile -File ".\init.ps1"` | PASS: 10 tasks, state/completion gates, 274 relative links, `git diff --check`; exit 0 |
| Whitespace and diff | `git diff --check`; `git status --short`; `git diff --stat`; `git diff --cached --name-only`; `git ls-files --others --exclude-standard` | Exit 0; the same 27 modified files and the three pre-existing untracked entries (`.codegraph/`, `.playwright-mcp/`, `skills-lock.json`), nothing staged |
| Relative links | per-file regex over the five authorized files resolved against each containing directory | 0 broken; the root `IMPLEMENTACION_SERVICIO_TECNICO.md` is outside `init.ps1`'s link pass and was checked in the same sweep |
| Catalogue structure | row-id regex, `Compare-Object` against `HEAD`, per-row pipe count | 144 rows, identical id sequence, exactly the 7 claimed rows changed, 6 columns in every data row (the three unchanged cross-cutting HIST/LIST/LANG rows keep their 5-column shape) |
| Tracker | `ConvertFrom-Json` over `feature_list.json`, literal counts, per-line location of the edit targets | Valid; before the update `feat-010` was the only `in-progress` feature with 5/5 criteria `verified: false` and `visual_scenarios: []`; the five `"verified": false` and the one `"status": "in-progress"` literals were all inside `feat-010` |
| Encoding | per-file BOM and newline scan | BOM-less; catalogue, limitations, tracker and progress LF, root notes CRLF; no mojibake markers |
| Protected scope | SHA-256 of all 251 tracked files except the five authorized ones plus the untracked inventory, and modification times | Aggregate `979316b88ac6b6d5a2050e09549835bd8bdd80184799f834a1233230e14f4cc9`; every protected tracked file was last written at or before 08:15:41 while the five authorized files were written 10:19:03-10:23:25; `feat-010` is mentioned only inside the five authorized files; the three untracked entries keep their 2026-09-06/2026-10-03 timestamps |

Findings recorded, not fixed (documentation observations; none blocks a criterion):

1. **Historical detail dropped by the CP3 replacement (deliberate, disclosed by the Builder):** the
   root notes' testing-UI bullet was replaced wholesale, so still-true historical details it carried
   (the `Motivo` column, the preload fallback, `habilitaPrueba` including `ListoRetiro`/`Reparado`)
   are no longer stated there. The replacement is explicitly labelled "regla vigente desde feat-008"
   and the file's top banner marks the per-checkpoint reports as historical.
2. **Loose attribution (minor):** the known-limitations "Inactive reservation" row says the
   non-release by reactivation or other edits "was observed at runtime on the inactive cliente 3004";
   the runtime run observed the telefono edit keeping D1 and the reactivation keeping the owner
   message, while the clause "(only changing the documento to a free value does)" is source-derived.
3. **Superseded historical lines remain by design:** the CP1-CP3 root-note lines still state
   `id_repuesto` "reservado para stock CP3" and "sin `Repuesto` hasta CP3"; the new historical-scope
   banner names exactly those statements as superseded by feat-007/feat-008/feat-009.
4. **Manifest-comparison limitation:** the Builder's pre-edit manifest was not persisted anywhere
   readable (no file in the repository or under the external evidence directory), so the before/after
   comparison rests on content scope, the two-hour modification-time gap and the absence of any
   `feat-010` reference outside the five authorized files, not on a stored hash pair.

Residual uncertainty: the corrected wording certifies the dated feat-006..feat-009 Checker evidence
and does not renew it; concurrency, other DPIs and cultures, long inputs, universal hit-testing and
the failed-read path stay untested, as the register states. No runtime test, build, application
launch or database access was performed by this Checker.
## Builder report (feat-009, 2026-10-13)

Objective: while an original order already has an open warranty re-entry, another re-entry of
that order must be refused with a business message and create no order; once the open re-entry
closes and that warranty visit ends, a new one is allowed. The Garantia section of the original
order must also list its warranty re-entries (number, reception date, translated state and
result) in a permanent grid, with ES/EN labels, empty and error-free for an order without
re-entries. Scope: only the authorized `feat-009` files; no data migration and no DDL change.

Implemented policy (from the owner's rule, decided by the orchestrator): a re-entry is **closed**
when its order state is `Entregado`, or its evaluation is `Rechazada` **and** the order has
committed to a post-evaluation destination (`PendientePresupuesto`, `EsperandoRespuesta`,
`AutorizadoReparacion`, `EnReparacion`, `EnPruebas`, `ListoRetiro`). Everything else is
**open**, including an accepted `ListoRetiro`/`Reparado` re-entry (open until delivered) and
any unknown or inconsistent state (conservatively open).

| File | Method | Change |
| --- | --- | --- |
| [Garantia](DOMAIN/Features/Garantias/Garantia.cs) | class comment | Documents the cardinality: coverage 0..1 per original order (UX_Garantias_OrdenOriginal, annulled included) vs 0..N sequential re-entry visits |
| same | `ReingresoGarantiaAbiertoException` (new, same file) | `ReglaNegocioException` subclass carrying `ClaveMensaje` so the UI translates by type; no new file and no project edit |
| [EstadoEvaluacionGarantia](DOMAIN/Features/Garantias/EstadoEvaluacionGarantia.cs) | comment only | `Aceptada` closes the evaluation, not the visit; the three states are unchanged |
| [OrdenServicioRepository](INFRASTRUCTURE/Features/Ordenes/OrdenServicioRepository.cs) | `PredicadoReingresoAbierto` (new public const) | Single shared SQL fragment (alias `o`, original in `@IdOrdenOrigen`) used by the two reads and both guards |
| same | `CrearConHistorial` | Guard is the first statement of the batch, only for `@TipoOrden = 'Garantia' AND @IdOrdenOrigen IS NOT NULL`, under `TABLOCKX,HOLDLOCK`; on conflict it inserts nothing, returns `-1` and the method returns `null`. Success path unchanged |
| same | `ObtenerReingresoGarantiaAbierto` (new) | Read pre-check with the same predicate over all siblings (`TOP 1`), reusing `Mapear` |
| same | `ListarReingresosGarantia` (new) | `id_orden_origen` + `tipo_orden = 'Garantia'`, ordered by `fecha_ingreso, id_orden`; no state filter and no evaluation join, so delivered and evaluation-less re-entries are listed |
| [EntregaRepository](INFRASTRUCTURE/Features/Ordenes/EntregaRepository.cs) | `CancelarEntregaConTransicion` | Now returns `int`; the same predicate is evaluated inside the existing delete/update/history transaction (after its own `TABLOCKX,HOLDLOCK` read) and a conflict returns `-1` without DML |
| [OrdenServicioService](APPLICATION/Features/Ordenes/OrdenServicioService.cs) | `CrearReingresoGarantia` | Repository pre-check before the write, sentinel converted into `ReingresoGarantiaAbiertoException` (`Mensaje.GarantiaReingresoAbierto`) and `catch (ReglaNegocioException) { throw; }` so the concrete type survives; every existing validation keeps its order |
| same | `CancelarEntrega` | Friendly sibling pre-check (`Mensaje.GarantiaReaperturaBloqueada`), sentinel check on the repository result and the same `throw;` |
| same | `ObtenerReingresoGarantiaAbierto`, `ListarReingresosGarantia` (new) | Thin wrappers exposing the two repository reads to the forms, following the existing concrete-construction and error conventions |
| [GarantiaRepository](INFRASTRUCTURE/Features/Garantias/GarantiaRepository.cs) | `Inicializar` comment | Separates coverage (0..1) from visits (0..N); no index, DDL or behavioural change |
| [IdiomaRepository](INFRASTRUCTURE/Features/Idiomas/IdiomaRepository.cs) | `SembrarDatosIniciales` | Paired ES/EN seeds `Mensaje.GarantiaReingresoAbierto`, `Mensaje.GarantiaReaperturaBloqueada` and `OrdenDetalle.ReingresosGarantia`; reuses `Columna.Numero`/`Columna.Fecha`/`Columna.Estado`/`Columna.Resultado` |
| [FrmOrdenServicioDetalle](PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.cs) | `FilaReingreso`, `_reingresos` | View row (hidden id, number, reception date, translated state and result) and cached list |
| same | `CargarReingresosGarantia`, `RefrescarGrillaReingresos`, `ConfigurarColumnasReingresos` (new) | Read, bind and header configuration; an empty list is a success and a failed read shows the error instead of a fabricated empty result |
| same | `CargarTabGarantia`, `CargarModoNuevo`, `CargarOrden` | Clear and rebind the list independently of the coverage lookup; column configuration added to the load path |
| same | `ActualizarTextos` | Translates the new caption and re-renders the cached rows on a language change without another database read |
| same | `BTN_CrearReingreso_Click` | Pre-check with the translated warning **before** `PedirTexto`; the list is refreshed after a successful creation (`CargarOrden`) and after a conflict |
| same | `MostrarError`, `BuscarReingresoAbierto` (new) | Recognizes the typed exception by walking `InnerException` and translates `ClaveMensaje`; never matches SQL or message text |
| [FrmOrdenServicioDetalle.Designer.cs](PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.Designer.cs) | designer | Permanent read-only `DGV_ReingresosGarantia` (12,305, 864x137, anchored on all four sides) plus its `LBL_ReingresosGarantia` caption at (12,280) inside `TAB_Garantia`, with `TAB_Garantia.AutoScroll = true`; no budget or testing control touched |
| guides | business-flows.md, persistence.md, known-limitations.md | Policy, cardinality, shared predicate and guard placement, explicit "no DDL changed", and the boundaries that remain unproven |

Checks actually run:

| Check | Command | Result |
| --- | --- | --- |
| Build (documented command) | vswhere + `MSBuild .\TP_INTEGRADOR_2022.sln /t:Build /p:Configuration=Debug "/p:Platform=Any CPU" /m /v:m` | **PASS: exit 0**; six projects and `UI -> PRESENTATION\bin\Debug\UI.exe`; no `UI.exe` was running, so no output was locked |
| Build (clean rebuild) | same command with `/t:Rebuild` | **PASS: exit 0**, 0 warnings, 0 errors, all six assemblies produced |
| Source inspection | predicate usages, batch statement order, form grep | `PredicadoReingresoAbierto` is referenced by `OrdenServicioRepository` (guard + read) and by `EntregaRepository` (reverse edge); in `CrearConHistorial` the locked guard precedes the only `INSERT INTO OrdenesServicio`, which sits in the `ELSE` branch, and a conflict selects `-1`; in `CancelarEntregaConTransicion` the guard precedes `DELETE FROM Entregas`; 0 matches for SQL or repository types in the order detail form |
| Seed pairing | regex count per key in `IdiomaRepository` | `Mensaje.GarantiaReingresoAbierto`, `Mensaje.GarantiaReaperturaBloqueada` and `OrdenDetalle.ReingresosGarantia` each ES=1 / EN=1 |
| Tracker JSON | `ConvertFrom-Json` over `feature_list.json` | Valid; `feat-009` `in-progress` and the only one, 5/5 criteria `verified: false`, 4/4 scenarios `planned` |
| Diff hygiene | `git status --short`, `git diff --stat`, `git diff --check` | Only authorized files changed (no new file); `git diff --check` exit 0; BOM-less files keep their original endings (`CRLF` for source, `LF` for the guides, the tracker and this file) |
| Harness | `powershell -NoProfile -File ".\init.ps1"` | PASS: 10 tasks, state/completion gates, 238 relative links, `git diff --check` |

Deviations from the planner result (all deliberate, none silently dropped):

1. The shared predicate is a `public const` on `OrdenServicioRepository` referenced by
   `EntregaRepository`, so the forward guard, the read pre-check and the reverse-edge guard
   cannot drift apart. The planner asked for one shared fragment for the read and the
   transactional check; sharing it across the two repositories is the same idea one step wider.
2. `CancelarEntregaConTransicion` changed its return type from `void` to `int` to carry the
   conflict sentinel (`-1`) to the service; its transaction and rollback window are unchanged.
3. `CrearConHistorial` signals the conflict by returning `null` (the batch's `-1`
   converted), the only sentinel its `OrdenServicio` return type can express; the service
   converts it into the typed exception.
4. The repository pre-check in `CrearReingresoGarantia` sits after the existing validations and
   immediately before the write, so every pre-existing refusal keeps its current precedence.

Intentionally untouched (no change needed, reported as such):
[FrmGarantias](PRESENTATION/Forms/Garantias/FrmGarantias.cs) (list-to-original navigation was
not requested by the owner), [EvaluacionGarantia](DOMAIN/Features/Garantias/EvaluacionGarantia.cs),
[EvaluacionGarantiaRepository](INFRASTRUCTURE/Features/Garantias/EvaluacionGarantiaRepository.cs)
and [GarantiaService](APPLICATION/Features/Garantias/GarantiaService.cs): the factories, the
evaluation destinations (accept, paid continuation, withdrawal) and the correspondence checks
are preserved. No data migration, no DDL, no index change; pre-existing parallel open siblings
are neither cancelled, merged, deleted nor reset.

**Not verified (builder scope):** no application launch, no startup migration or seed run, no
database read or write, no concurrency exercise, no inspection of live legacy siblings, and none
of the four `vis-gar-*` scenarios. The guard's parallel behaviour, the ES/EN rendering, the
empty-list and failed-read paths and the grid layout at 900x600 or large DPI are proven by source
inspection plus compilation only. This Builder report certifies nothing.

**Next action for the Checker:** run `vis-gar-block-parallel-reentry`, `vis-gar-allow-after-close`,
`vis-gar-reentry-list` and `vis-gar-regression` through the real WinForms UI against the
authorized non-production database: deliver a repaired order, create one re-entry, attempt a
second one (business message, no new order, grid listing the open re-entry), close the first one
(evaluation rejected with a committed destination, or deliver it) and create a new one, and
read back `OrdenesServicio` rows by `id_orden_origen` plus the translated grid headers in ES
and EN.

## Checker verification (feat-009, 2026-10-13) - independent, runtime

Objective: independently verify the single-open warranty re-entry guard plus the re-entry list of
`feat-009` through the real WinForms UI with SQL readback. **Verdict: 5/5 acceptance criteria and
4/4 `vis-gar-*` scenarios PASS; `feat-009` is `done`.**

Environment: branch `checkpoint-6-arreglos-y-mejoras` @ `94c0829` plus the uncommitted
feat-006/007/008/009 work; database **TPIntegrador** on the local SQL Server container
(`@@SERVERNAME` `03b0f931595f`) - the same authorized non-production database used by
feat-002/004/005/006/007/008, identified before any write; driver pywinauto 0.6.9 from
`%LOCALAPPDATA%\ServicioTecnicoDesktopTests`; display at 150% DPI (the designer 864x137 grid renders
as 1296x206 physical), session `admin`, Spanish with a switch to English for the list check. No
`UI.exe` was running when the session started, so the documented build was not blocked; the Checker's
own `UI.exe` (PID 73756) was closed gracefully through its own window at the end.

Checks actually run:

| Check | Command | Result |
| --- | --- | --- |
| Documented build | vswhere + `MSBuild .\TP_INTEGRADOR_2022.sln /t:Build /p:Configuration=Debug "/p:Platform=Any CPU" /m /v:m` | **PASS: exit 0**, six projects, `UI -> PRESENTATION\bin\Debug\UI.exe` refreshed |
| Harness | `powershell -NoProfile -File ".\init.ps1"` | PASS: 10 tasks, state/completion gates, 262 relative links, `git diff --check` |
| Git hygiene | `git status --short`, `git diff --stat`, `git diff --check` | Same 25 modified files and the three pre-existing untracked entries (`.codegraph/`, `.playwright-mcp/`, `skills-lock.json`); no new file; `git diff --check` exit 0 |
| App startup | `UI.exe` launch against TPIntegrador | Idempotent migrations/seeds only; the three new ES/EN keys (`Mensaje.GarantiaReingresoAbierto`, `Mensaje.GarantiaReaperturaBloqueada`, `OrdenDetalle.ReingresosGarantia`) appeared paired, `Columna.Numero/Fecha/Estado/Resultado` were reused (`Numero`/`Number`, `Fecha`/`Date`, `Estado`/`Status`, `Resultado`/`Result`) |

Source inspection confirmed every Builder claim: `PredicadoReingresoAbierto` is a single `public const`
fragment used by `ObtenerReingresoGarantiaAbierto` (TOP 1 over all siblings), by the locked guard of
`CrearConHistorial` (first statement of the batch, only for `tipo = Garantia` with a non-null origin,
under `TABLOCKX, HOLDLOCK`, before the only `INSERT INTO OrdenesServicio`, which lives in the `ELSE`
branch; conflict = `SELECT -1` and the repository returns `null`) and by
`EntregaRepository.CancelarEntregaConTransicion` (evaluated after its own locked read, before
`DELETE FROM Entregas` in the same transaction; conflict = `-1`, no DML). `ListarReingresosGarantia`
filters only `id_orden_origen` + `tipo_orden = 'Garantia'`, orders by `fecha_ingreso, id_orden` and
joins no evaluation. `OrdenServicioService` keeps every validation ahead of the pre-check and
rethrows the concrete `ReingresoGarantiaAbiertoException` with `throw;` in both methods. The form
translates by exception type (`BuscarReingresoAbierto` walks `InnerException`, never the message
text), the pre-check warns **before** the reason prompt, and `FrmOrdenServicioDetalle.cs` still
contains 0 SQL/repository references. The Designer adds a read-only `DGV_ReingresosGarantia`
(12,305, 864x137, anchored on four sides, `TAB_Garantia.AutoScroll = true`) and the caption at
(12,280) inside the Garantia tab; the button row ends at y=270 and no budget/testing control moved.
`GarantiaRepository.cs`, `EstadoEvaluacionGarantia.cs` and `Garantia.cs` are comment/exception-only
diffs; `FrmGarantias.cs`, `EvaluacionGarantia.cs`, `EvaluacionGarantiaRepository.cs` and
`GarantiaService.cs` are unmodified. No DDL or migration was performed by Builder or Checker.

Fixtures created through the real UI (all `AGENT_009_`, kept for review): cliente 3009
(`AGENT_009_CLI`), equipos 3006/3007/3009 (`AGENT_009_SN1/S N2/SN4`), original order **4012**
(Normal, approved Original 2032 with 30 days, coverage row **2003** 2026-10-13..2026-11-12, delivery
**3006**, zero re-entries at creation), warranty re-entries **4013** (A), **4014** (B), **4015** (C),
plus **4016** (Normal, approved Original of 0 days, delivered Reparado) and **4018** (delivered
No reparable). 4017 is a leftover Normal order left at Pendiente presupuesto: the first attempt at a
non-reparable diagnosis did not toggle `CHK_EsReparable` and was persisted as reparable, so 4018 was
created instead; 4017 was not modified afterwards.

- `vis-gar-block-parallel-reentry` - **passed**. The original 4012 Garantia tab was opened **before**
  A existed (empty grid, caption `Reingresos de garantia`, no error); A was created from that same
  window (`Reingreso creado: orden #4013.`). Six later attempts were refused: 3x while A was
  `Recibido` (same window), once from a freshly reopened window, once while A was
  `PendienteEvaluacionGarantia` and once while A was `AutorizadoReparacion` with an `Aceptada`
  evaluation. Each showed title `Error` and the exact text `Ya existe un reingreso de garantia
  abierto para esta orden. Cierre el reingreso existente antes de crear otro.`, with **no reason
  prompt**; after each one the count of `OrdenesServicio` rows with `id_orden_origen = 4012`, the
  original row, its `HistorialOrdenes` count and its `Entregas`/`Garantias` rows were byte-identical
  (concatenated snapshot comparison). A genuinely stale cached-grid window is not reachable because
  the order detail is modal (`ShowDialog`) and every creation path re-reads the DB pre-check; the
  reopened-window attempt covers that case.
- `vis-gar-allow-after-close` - **passed**. While A stayed open as `ListoRetiro`/`Reparado` with an
  `Aceptada` evaluation, creation was still refused. After delivering A (`Entregado`, delivery 3007)
  a new re-entry B (**4014**) was created. B was rejected with the withdrawal destination and a reason
  containing `pago` -> `ListoRetiro`/`GarantiaNoCubierta` + `Rechazada`, after which C (**4015**) was
  created; C was then rejected with the paid destination -> `PendientePresupuesto` (also closed).
  Original 4012 stayed `Entregado`/`Reparado` with coverage 2003 and delivery 3006 unchanged.
- `vis-gar-reentry-list` - **passed**. ES: caption `Reingresos de garantia`, headers
  `Numero`/`Fecha`/`Estado`/`Resultado`, rows `4013 10/13/2026 8:37 AM Entregado Reparado`,
  `4014 8:57 AM Listo retiro Garantia no cubierta`, `4015 9:04 AM Recibido -`; every number and
  reception date matched the `fecha_ingreso` readback, including a delivered re-entry and one without
  evaluation. EN after reopening the form in English: caption `Warranty re-entries`, headers
  `Number`/`Date`/`Status`/`Result`, rows `Delivered`/`Repaired`, `Ready for pickup`/`Warranty not
  covered`, `Received`. Empty case: order 4014 (no children) showed no rows, caption present, no
  error. The grid rendered fully at the default size and at 1400x1000.
- `vis-gar-regression` - **passed**. ENT-02: 4012 delivery created exactly one coverage row (2003)
  with the approved Original's 30 days; 4016 (0 days) created none; 4018 (No reparable) created none;
  delivering warranty order 4013 twice created none. GAR-05 accept -> `AutorizadoReparacion` +
  `Aceptada` (4013). GAR-06 paid rejection -> `PendientePresupuesto` + `Rechazada` with the
  `[pago: continua como reparacion paga]` wording (4015). GAR-07/GAR-08 withdrawal with `pago` in the
  reason -> `ListoRetiro`/`GarantiaNoCubierta` (4014). GAR-11 `Ver original` from 4014 opened
  `Orden #4012` (`Entregado`/`Reparado`) and left its snapshot unchanged. ENT-05 re-finalize +
  re-deliver -> `Entregado` (delivery 3010). The re-entry lifecycle ran end to end
  (`Recibido` -> `PendienteEvaluacionGarantia` -> `AutorizadoReparacion` -> `EnReparacion` ->
  `EnPruebas` -> `ListoRetiro`/`Reparado` -> `Entregado`).

Reverse edge (required by criterion 4): with A delivered and B open, `BTN_CancelarEntrega` on A
returned title `Error` and `No se puede reabrir este reingreso porque ya existe otro reingreso de
garantia abierto para la orden original.` and wrote nothing - delivery row 3007, state `Entregado`,
result `Reparado` and the 8 history rows were unchanged. The confirmation `No`, the cancelled reason
dialog and a blank reason committed nothing (blank showed `Complete los campos obligatorios de la
orden.`). With no open sibling (B and C closed) the same cancellation succeeded: delivery 3007 was
deleted, A moved to `EnPruebas`/`NULL` with 9 history rows, and A was re-finalized and re-delivered as
delivery 3010, creating no coverage row. At the end of the run original 4012, coverage 2003, delivery
3006 and its 9 history rows were byte-identical to the pre-verification baseline.

Raw evidence: `%LOCALAPPDATA%\ServicioTecnicoDesktopTests\evidence\feat009-20261013\`
(`feat009-log.md`, `state.json`, screenshots `s0-*`..`s4d-*`, grid crops) with the driver scripts
`...\scripts\sweep\feat009kit.py`, `feat009_s0_setup.py`, `feat009_s1_block.py`,
`feat009_s1b_block2.py`, `feat009_s2_allow.py`, `feat009_s2b_allow2.py`, `feat009_s3_list.py`,
`feat009_s3b/s3c/s3d_small*.py`, `feat009_s4_regression.py`, `feat009_s4b_regression2.py`,
`feat009_s4c_regression3.py`, `feat009_s4d_regression4.py`.

**Not re-tested / residual uncertainty (does not block the feature):** true simultaneous creation
(two threads/instances) was not run, so the functional meaning of `TABLOCKX, HOLDLOCK` and the
repository guard path isolated from the friendly pre-check remain source-inspected; all observed
refusals came from the pre-check because the warning appeared before the reason prompt, and the
`sentinel`/conflict-refresh branch of the form (only reachable under a race) was not exercised. The
900x600 check is a partial observation: at that window size (about 600x400 logical at 150% DPI) the
Garantia page scrolls to its maximum and only the caption is reachable while the grid body stays
clipped - consistent with this checkout's known 900x600 clipping limitation, not a data defect
(screenshots `s3c-*`/`s3d-*`); the grid renders fully at the default size and at 1400x1000.
## Builder report (feat-008, 2026-10-13)

Objective: with several repair interventions recorded, an earlier (non-current) intervention must
not offer a test registration that answers `Seleccione un registro`; only the current intervention
accepts new tests, and finalization, annulment and reopen keep evaluating the current intervention.
Scope: only the authorized `feat-008` files. No service, repository or domain change was needed.

| File | Method | Change |
| --- | --- | --- |
| [FrmOrdenServicioDetalle](PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.cs) | constructor | Creates the new label through `CrearAvisoPruebasIntervencion()` right after `InitializeComponent` |
| same | `CrearAvisoPruebasIntervencion` (new) | Programs `LBL_PruebaIntervencionAviso` (Tag `OrdenDetalle.PruebasIntervencionAnterior`, `AutoSize=false`, top/left/right anchor, `DGV_Pruebas` font) in the band above the test grid and takes about 34 logical px from the grid height; both values derive from the grid runtime height (never fixed coordinates) and the grid bottom edge plus the input positions stay unchanged. The Designer file is untouched |
| same | new fields | `_idIntervencionPruebaCargada` (intervention actually bound), `_cargandoPruebas` (load in progress), `LBL_PruebaIntervencionAviso` |
| same | `IntervencionPruebaSeleccionada` (new) | Resolves the combo integer `SelectedValue` against `_reparaciones`; null when there is no valid selection |
| same | `EsIntervencionActual` (new) | "Current" is the latest intervention by number, reusing `ObtenerIdUltimaIntervencion()` |
| same | `GrillaPruebasCoherente` (new) | Requires a finished load and `_idIntervencionPruebaCargada == selected intervention` |
| same | `HayIntervencionAbierta` (new) | Any intervention without `FechaFin` |
| same | `ActualizarAccionesPruebas` (new) | Computes selection/current/coherence once: the description and observations boxes, the result radios and `BTN_RegistrarPrueba` require edit permission + `EnPruebas` + current finished intervention + no open intervention + coherent grid; `BTN_FinalizarPruebas` requires current + coherent; `BTN_AnularPrueba` keeps its existing restriction; the intervention combo depends only on the view permission, so a historical selection never disables it; a historical selection shows the translated consultation-only reason. Reached from `AplicarPermisosDetalle`, `CargarPruebasDeCombo`, the row-selection handler and indirectly the reload/language paths |
| same | `CargarTabPruebas` | Clears the grid before rebuilding, no longer caches a failed `ListarPruebas` as an empty list (the selected intervention retries through the service) and keeps the reload default of the latest intervention |
| same | `CargarPruebasDeCombo` | Clears `_pruebasActuales`, the previous binding, `CurrentCell` and the row selection before replacing the data and again after binding (also in the null/invalid/error paths), records `_idIntervencionPruebaCargada`, shows the existing error and leaves the grid empty with the actions disabled on failure, and recomputes the actions at the end |
| same | `LimpiarGrillaPruebas`, `LimpiarSeleccionGrillaPruebas` (new) | Empty binding plus discarded selection/current cell; a row must be selected explicitly before annulling |
| same | `ActualizarBotonAnularPrueba` | Unchanged restriction (EnPruebas, edit permission, latest intervention, selected non-Anulada row of that intervention); now reached only through `ActualizarAccionesPruebas` |
| same | `AplicarPermisosDetalle` | Its testing-controls block delegates to `ActualizarAccionesPruebas()`; budget, repair, warranty and permission logic untouched |
| same | `BTN_RegistrarPrueba_Click` | Checks the selection and the current/finished/no-open preconditions before the description, replaces the two generic `Mensaje.SeleccioneRegistro` warnings with `Mensaje.PruebaIntervencionObligatoria` and `Mensaje.PruebasSoloIntervencionActual`, and keeps the service call, the success reset and the INSERT-only behaviour |
| same | `BTN_FinalizarPruebas_Click` | Defensive current-view eligibility check before the confirmation dialog; the service call and its no-valid-test rejection stay reachable on a loaded empty current grid |
| same | `BTN_AnularPrueba_Click` | Row/annulled checks preserved; only its historical-intervention warning now uses `Mensaje.PruebasSoloIntervencionActual` |
| same | `ActualizarTextos` | Translates the new label through its Tag in the existing language-refresh path |
| same | `CargarModoNuevo` | Resets `_idIntervencionPruebaCargada` together with the existing test state |
| [IdiomaRepository](INFRASTRUCTURE/Features/Idiomas/IdiomaRepository.cs) | `SembrarDatosIniciales` | Adds the paired ES/EN seeds `OrdenDetalle.PruebasIntervencionAnterior`, `Mensaje.PruebaIntervencionObligatoria` and `Mensaje.PruebasSoloIntervencionActual` with the existing `AgregarSeed` insert-if-missing convention, next to the testing labels; `Mensaje.SeleccioneRegistro` was not deleted or redefined |
| [business-flows.md](docs/agents/business-flows.md), [known-limitations.md](docs/agents/known-limitations.md) | testing section / new feat-008 section | Document the consultation-only historical view, the current-view action gating, the unchanged service guards and the layout/load boundaries; runtime acceptance labelled pending |

Intentionally untouched (no change needed for this feature): [OrdenServicioService](APPLICATION/Features/Ordenes/OrdenServicioService.cs)
(`RegistrarPrueba` still requires `EnPruebas`, no open intervention, a finished latest intervention
and that intervention as target; `FinalizarPruebas` reads only the latest intervention non-Anulada
tests; `AnularPrueba` works on the latest intervention),
[ReparacionRepository](INFRASTRUCTURE/Features/Reparaciones/ReparacionRepository.cs),
[Prueba](DOMAIN/Features/Reparaciones/Prueba.cs) and
[FrmOrdenServicioDetalle.Designer.cs](PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.Designer.cs).

Checks actually run:

| Check | Command | Result |
| --- | --- | --- |
| Build (documented command) | vswhere + `MSBuild .\TP_INTEGRADOR_2022.sln /t:Build /p:Configuration=Debug "/p:Platform=Any CPU" /m /v:m` | **exit 1**: the five libraries compiled and the UI compile step passed, but the copy to the output directory failed with `MSB3027`/`MSB3021` copying `APPLICATION.dll`/`REPOSITORY.dll` into `PRESENTATION\bin\Debug`, locked by the running `UI.exe` (PID 84192). Output-copy failure, not a source error |
| Build (compile proof) | same MSBuild on `.\PRESENTATION\UI.csproj` with `/p:OutputPath=%TEMP%\dsh-feat008-ui\` | **PASS: exit 0**; the six projects built (`ABSTRACTIONS`, `SERVICES`, `DOMAIN`, `REPOSITORY`, `APPLICATION`, `UI -> UI.exe`) with the output outside the repository; `PRESENTATION\obj\Debug\UI.exe` (06:11:17) is newer than both edited sources (06:10:21, 06:10:53), so the compile used the new code |
| Diff hygiene | `git status --short`, `git diff --stat`, `git diff --check` | Same modified set as before this task (20 files, no new ones) plus the preserved untracked `.codegraph/`, `.playwright-mcp/`, `skills-lock.json`; `git diff --check` exit 0; per-file LF/CRLF and BOM state preserved |
| Source inspection | grep over `PRESENTATION/Forms` for SQL/repository usage; call-site listing of the new helper | 0 SQL/repository matches; `ActualizarAccionesPruebas` is reached from `AplicarPermisosDetalle` (reload/language), `CargarPruebasDeCombo` (selector and reload) and `DGV_Pruebas_SelectionChanged`; the protected files timestamps predate this session |
| Harness | `powershell -NoProfile -File ".\init.ps1"` | PASS: 10 tasks, state/completion gates, relative links, `git diff --check` |

**Not verified (builder scope):** no application launch, no startup migrations/seeds, no database
read or write, no MSBuild output inside `PRESENTATION\bin` (locked), and none of the three
`vis-pru-*` scenarios. The label placement at other window sizes/DPI, the ES/EN texts, the
disabled/enabled control states, the cleared row selection and the persisted intervention of a new
test are proven by source inspection plus compilation only. This Builder report certifies nothing.

**Next action for the Checker:** close the running `UI.exe` normally, rebuild Debug/Any CPU, and run
`vis-pru-previous-intervention`, `vis-pru-current-intervention` and `vis-pru-switch-back` through the
real WinForms UI against the authorized non-production database on an order in `EnPruebas` with two
or more interventions (request and approve an additional budget so the order returns to
`EnReparacion` and a second intervention can be started and finished), with readback of
`Pruebas.id_reparacion` and of the finalization/annulment outcomes.

## Checker verification (feat-008, 2026-10-13) - independent, runtime

Environment: branch `checkpoint-6-arreglos-y-mejoras` plus the uncommitted feat-006/feat-007/feat-008
diff. The leftover `PRESENTATION\bin\Debug\UI.exe` (PID 84192) was closed gracefully through its own
window (pywinauto WM_CLOSE on `FrmPrincipal`; no taskkill/Stop-Process, no other process touched) and
the process exited. The documented build from vswhere discovery then passed: MSBuild
`TP_INTEGRADOR_2022.sln` Debug / Any CPU `/m`, six projects, `UI -> PRESENTATION\bin\Debug\UI.exe`,
exit 0 (incremental; the refreshed `UI.exe` 06:11:17 is newer than both edited sources,
FrmOrdenServicioDetalle.cs 06:10:21 and IdiomaRepository.cs 06:10:53). `init.ps1` PASS (10 tasks,
237 relative links, `git diff --check` clean). Driver: pywinauto 0.6.9 from
`%LOCALAPPDATA%\ServicioTecnicoDesktopTests`, real clicks and typing, admin session, separate Spanish
and English passes, window sizes 900x600 and 2000x1200, 150% display scaling.

**Database identity (checked before any write):** `TPIntegrador` on the local SQL Server container
`03b0f931595f` (`sqlserver-tp`, mssql/server:2022-latest, published on localhost:1433); the
`UrlDB` connection in `UI.exe.config` resolves to that server and database (credentials not
recorded here). Identity markers read back through the same connection: `DB_NAME()`=TPIntegrador,
`@@SERVERNAME`=03b0f931595f, and the feat-006/feat-007 fixtures still present (`AGENT_007_PART_A` 1
row, cliente documento `AGENT007D1` 1 row, orders 4007-4010 4 rows). It is the same non-production
database used by feat-002/004/005/006/007; no production or unidentified database was touched.

**Startup evidence:** launching the application ran the idempotent seeds. Readback of
`Palabras`/`Traducciones` shows the three new paired keys
(`OrdenDetalle.PruebasIntervencionAnterior`, `Mensaje.PruebaIntervencionObligatoria`,
`Mensaje.PruebasSoloIntervencionActual`) in ES and EN, and `Mensaje.SeleccioneRegistro` unchanged
("Seleccione un registro." / "Select a record.").

**Fixtures created through the UI (`AGENT_008_` prefix, all left in place):** cliente 3008
(AGENT008D1), equipo 3005 (AGENT_008_SN1) and order 4011, driven reception -> assignment ->
reparable diagnosis (30 days) -> original budget emit + approve (`Esperando respuesta` ->
`Autorizado reparacion`) -> repair 1 (id 3010) finished -> `En pruebas`; two tests registered on
intervention 1 (3009 Aprobada, 3010 RequiereRevision) and finalized with a revision result ->
`En reparacion`; repair 2 (id 3011) started and finished -> `En pruebas`, with intervention 2
(latest by number) current and an empty grid. Tests 3011-3017 were then created or annulled on
intervention 2. Nothing pre-existing was deleted, rewritten or cleaned up.

| Scenario | Result | Observed through the real UI and read back from the database |
| --- | --- | --- |
| vis-pru-previous-intervention | PASS | 06:37-06:43. With intervention 2 current, selecting intervention 1 by mouse (06:39) and by keyboard (UP, 06:52) left the grid with exactly intervention 1's two tests (3009 Aprobada, 3010 Requiere revision, all cells "N. 1") and no selected row; `TXT_PruebaDesc`, `TXT_PruebaObs`, `RDO_Aprobada`, `RDO_Fallida`, `BTN_RegistrarPrueba` and `BTN_FinalizarPruebas` reported IsEnabled=False, `BTN_AnularPrueba` was absent from the UIA tree (hidden) and the new label was visible in red with "Intervencion anterior: solo consulta. Seleccione la actual para registrar, anular o finalizar pruebas." (45 px high, above the grid, 1296 px wide). Real physical clicks through SetCursorPos/mouse_event on the disabled description box, the disabled failed-result radio and the disabled registration button, plus form-level typing of AGENT_008_SHOULD_NOT_PERSIST, Tab/Enter/Space and a UIA Invoke attempt (raised COMError) produced **no dialog at all** (no "Seleccione un registro"), the description kept an empty value and the order's test rows stayed at 2 (sqlcmd count before/after identical). Returning to intervention 2 re-enabled description/observations/radios/button and finalization, hid the label and left the annulment button visible but disabled. |
| vis-pru-current-intervention | PASS | 07:05-07:52 on order 4011. On intervention 2 (id 3011) the UI registered AGENT_008_IV2_APROBADA_1 (3011), AGENT_008_IV2_SWITCHBACK (3012) and AGENT_008_IV2_SWITCHBACK2 (3013) earlier and then AGENT_008_IV2_APROBADA_A (3014), AGENT_008_IV2_REVISION_B (3015, RequiereRevision) and AGENT_008_IV2_APROBADA_C (3016); every row read back with `id_reparacion`=3011 and the order stayed `En pruebas` after each registration (state label "En pruebas" and DB 4011\|EnPruebas\|NULL). The non-latest revision row (3015) was annulled: the regret pass clicked Yes on the confirmation and cancelled the reason dialog, leaving resultado RequiereRevision with an empty motivo; the confirmed pass stored resultado Anulada, motivo "AGENT_008 motivo anulacion revision" and fecha_anulacion 2026-10-13 10:14:10, displayed in the grid as Resultado Anulada with Motivo, Fecha anulacion and Usuario anulacion filled while the state stayed `En pruebas`. Finalization then produced `Listo retiro` / `Reparado` (DB 4011\|ListoRetiro\|Reparado) with the message "Pruebas finalizadas. La orden quedo lista para retiro (resultado: Reparado)."; reopening with a reason returned `En pruebas` with the result cleared (DB 4011\|EnPruebas\|NULL) and the six intervention-2 tests still attached (3011-3016); with all of them annulled the finalization was refused with "Error al realizar la operacion: No hay pruebas validas para finalizar las pruebas de la orden." and the state stayed `En pruebas`; registering AGENT_008_IV2_APROBADA_D (3017) then reached `Listo retiro` / `Reparado` again, and closing and reopening the order kept 7 intervention-2 rows (6 annulled) plus intervention 1's 2 untouched rows. |
| vis-pru-switch-back | PASS | 06:48-07:52. Eleven alternations between intervention 1 and intervention 2, by mouse combo selection and by keyboard (UP/DOWN or Ctrl+Home plus Down), including a switch made while a current-intervention row was selected and switches to intervention 2 with an empty grid (0 rows at 06:43:03, 06:48:35 and 06:49:53). Each switch left the combo on the chosen intervention, the grid showing only that intervention's rows, the reason label visible only on the historical view, registration and finalization enabled only on the current view, and annulment hidden on intervention 1 and visible-but-disabled on intervention 2 with no row selected. With a valid row selected on intervention 2 the annulment button became enabled; after switching to intervention 1 and back it was disabled again (row selection cleared), and an already annulled row could never enable annulment. AGENT_008_IV2_SWITCHBACK (3012) and AGENT_008_IV2_SWITCHBACK2 (3013) registered after returning to the current view stored `id_reparacion`=3011 while intervention 1 kept its same two rows (total +1); the same repeat after closing and reopening the detail form behaved identically. |

Criteria verdicts (all four verified; the full evidence strings are in [feature_list.json](feature_list.json)):

| # | Criterion | Verdict | Basis |
| --- | --- | --- | --- |
| 1 | Historical selection: misleading "Seleccione un registro" not reachable, action/inputs unavailable, reason visible | PASS | Disabled description/observations/radios/registration/finalization, hidden annulment, visible translated reason, no dialog and no write after physical clicks, typing, Tab/Enter/Space and a UIA Invoke attempt |
| 2 | Registration still possible for the current intervention, persisted against it, restored when switching back | PASS | Tests 3009/3010 on intervention 1 (3010) and 3011-3017 on intervention 2 (3011); registration and finalization re-enabled on every return to intervention 2 |
| 3 | Grid, row selection and enabled actions describe one coherent intervention after every switch | PASS | Eleven alternations with aligned combo/rows/reason/actions; selection cleared by each reload (annulment proxy) and re-established only by an explicit valid-row selection |
| 4 | Recording a test does not finish testing; finalization, annulment and reopen evaluate the current intervention's valid tests | PASS | State stayed `En pruebas` after every registration; the annulled revision row was excluded from finalization; PRU-03 refusal with the state unchanged; reopen kept the intervention-2 tests; refinalization reached `Listo retiro`/`Reparado`; the service messages are byte-identical to `HEAD` and the test methods are outside every uncommitted diff hunk |

Source inspection versus runtime evidence: the single decision point `ActualizarAccionesPruebas`
(requiring edit permission, `EnPruebas`, the current finished intervention, no open intervention and a
coherently loaded grid) is the only place that enables the description/observations boxes, the result
radios, `BTN_RegistrarPrueba` and `BTN_FinalizarPruebas`; `BTN_AnularPrueba` keeps its own
`ActualizarBotonAnularPrueba` restriction and is reached only from the helper, and
`BTN_ReabrirPruebas` (reopen, `Entrega` tab) keeps its unchanged `ListoRetiro`+`Reparado` gate. The
combo handler `CBO_PruebaReparacion_SelectedIndexChanged` still routes to `CargarPruebasDeCombo`,
`CargarTabPruebas` still runs from `CargarOrden`, `CargarModoNuevo` resets
`_idIntervencionPruebaCargada`, and `ActualizarTextos` translates the new label. `Mensaje.SeleccioneRegistro`
remains in use only for the annulment row/annulled checks. A grep of `PRESENTATION/Forms` for
`SqlConnection`/`SqlCommand`/`new *Repository(` returned 0 matches, so no form contains SQL or
repository calls. The Designer file's only uncommitted change is feat-007's `CBO_PRepuesto` (no
`LBL_PruebaIntervencionAviso`), and `ReparacionRepository.cs`/`Prueba.cs` still carry their 2026-09-30
timestamps.

Observations recorded, not fixed (no implementation file was changed by the Checker):

1. **Grid height at 150% scaling (direct effect of the label reservation, recorded):** the label takes
   about 51 px from the grid (grid 270 -> 219 px at the default 1404x1040 runtime window), so a
   seven-row test grid needed one keyboard scroll (Ctrl+Home plus Down) to reach the last row by UIA;
   the grid's bottom edge (581 px from the form origin) and every input position were identical at
   900x600, at the default size and at 2000x1200, and the label stayed visible and legible
   (45 px high) at all three sizes.
2. **Small-window clipping (pre-existing, not caused by feat-008):** at a 900x600 window the controls
   below the test grid (description, radios, observations and the register/finalize buttons) are cut
   off by the form edge, as they were before this change; the new label and the grid itself remain
   fully visible. This follows the same pattern as the "Ver inactivos" clipping already tracked for
   `feat-010`.
3. **UIA observability limit:** WinForms DataGridView rows expose no UIA selection pattern
   (`SelectionItemPattern`/`SelectionPattern` both absent), so the row selection was observed through
   the `BTN_AnularPrueba` enabled state (computed from `DGV_Pruebas.SelectedRows`), the database and
   the screenshots. A first probe pass reported four false negatives and one grid-read failure because
   of that limit and a reader bug; they were corrected in the same run (see the `S2b`/`S3b`/`S3d`
   script logs) and do not represent implementation defects.
4. **App state left for the owner:** the checker's `UI.exe` was closed gracefully at the end of the session
   (interface back in Spanish, order detail closed, no build output left locked); order 4011 remains in
   `Listo retiro`/`Reparado` with its `AGENT_008_` fixtures for review.

## Builder report (feat-007, 2026-10-13)

Objective: make the spare-part (`Repuesto`) item type selectable in an order budget with an explicit
catalogue-part choice, define its economics and stock effect from the existing infrastructure, and
keep approved budgets immutable. Scope: only the authorized `feat-007` files.

| File | Method | Change |
| --- | --- | --- |
| [TipoItemPresupuesto](DOMAIN/Features/Ordenes/TipoItemPresupuesto.cs) | class comment | Replaced the "infrastructure only / not enabled in emission" note with the quotation-only rule: a Repuesto item references a catalogue part and neither reserves nor moves stock |
| [DetallePresupuesto](DOMAIN/Features/Ordenes/DetallePresupuesto.cs) | `CrearNuevo` | Trailing optional `int? idRepuesto = null`; Repuesto is accepted only with a positive part id and stored on the object; labour/service keep a null reference and their existing rules |
| same | `ValidarParaGuardar`, `ValidarReglas` (new) | Re-runs the factory rules on any line, including objects hydrated by `CargarDesdeDB`, and checks `subtotal = cantidad * precio`, at most two decimals and a `decimal(18,2)`-representable amount |
| same | `DetallePresupuestoInvalidoException` (new, same file) | `ReglaNegocioException` subclass carrying `ClaveMensaje` so the UI translates by type; no new file and no project edit |
| [OrdenServicioService](APPLICATION/Features/Ordenes/OrdenServicioService.cs) | `ValidarItemsYCalcularSubtotal` | Accepts Repuesto, rejects null lines, calls `ValidarParaGuardar` per line and `ValidarRepuestoDelItem` before any budget write |
| same | `ValidarRepuestoDelItem` (new) | `_repuestoRepository.ObtenerPorId` must resolve an existing **active** part; no available-stock requirement and no selling-price substitution |
| same | `PublicarBorrador` | Loads `ListarDetalle`, revalidates it and rejects a stored subtotal that does not match the detail instead of repairing persisted economics |
| same | `EmitirPresupuesto`, `EmitirAdicional`, `CrearBorrador`, `PublicarBorrador` | `catch (ReglaNegocioException) { throw; }` keeps the concrete exception type; state guards, approval, additional-origin logic and consumption are untouched |
| [PresupuestoRepository](INFRASTRUCTURE/Features/Ordenes/PresupuestoRepository.cs) | `EmitirConDetalle`, `CrearBorrador` | Replaced the forced `NULL` detail value with nullable `@IdRepuesto<i>` parameters inside the existing single-transaction batches; DDL comments updated. No stock SQL, new column, index or constraint |
| [RepuestoRepository](INFRASTRUCTURE/Features/Repuestos/RepuestoRepository.cs) | `Inicializar` comment | Comment-only correction of the stale "not enabled in emission" note; no schema, index, SQL or code change |
| [IdiomaRepository](INFRASTRUCTURE/Features/Idiomas/IdiomaRepository.cs) | `SembrarDatosIniciales` | ES/EN seeds `TipoItem.Repuesto` ("Repuesto"/"Spare part") plus `Mensaje.PresupuestoRepuestoObligatorio`, `...RepuestoNoValido`, `...CantidadInvalida`, `...PrecioInvalido` and `...DetalleInvalido`; `OrdenDetalle.Repuesto` and `Columna.Repuesto` were reused |
| [FrmOrdenServicioDetalle.Designer.cs](PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.Designer.cs) | designer fields | Budget-only `CBO_PRepuesto` (DropDownList, at 288,175, size 270x22, initially hidden) in the `TXT_PDesc` slot, with `CBO_PTipo.SelectedIndexChanged` and `CBO_PRepuesto.SelectedIndexChanged` wired; no new dialog, repair-consumption combo untouched |
| [FrmOrdenServicioDetalle.cs](PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.cs) | `CargarComboTiposItem` | Third `ItemTipoItem` for `Repuesto` through `TraducirTipoItem`; the current selection survives a language refresh |
| same | `CargarComboRepuestosPresupuesto` (new) | `RepuestoService.Listar` supplies the parts; only active ones are offered (stock irrelevant) and `SelectedIndex = -1` forces an explicit choice; the map also resolves inactive parts already referenced by existing budgets |
| same | `CBO_PTipo_SelectedIndexChanged`, `AplicarTipoItemSeleccionado` (new) | Toggle textbox/picker by type and switch `LBL_PDesc.Tag` between `OrdenDetalle.DescripcionItem` and `OrdenDetalle.Repuesto` |
| same | `CBO_PRepuesto_SelectedIndexChanged` (new) | Prefills the editable price from `PrecioReferencia`; a reference price outside 0..10,000,000 is not clamped - it warns and leaves the field at zero - and a type or language refresh never overwrites an edited price |
| same | `BTN_AgregarItem_Click` | Requires the part for a Repuesto line, snapshots `codigo - descripcion` into the line description and passes the id to `CrearNuevo`; a failed add keeps the inputs and the list intact, a successful one resets picker/quantity/price |
| same | `ConfigurarColumnasDetalle`, `DGV_Detalle_CellFormatting`, `DescribirRepuestoItem` (new) | `IdRepuesto` column visible with header Tag `Columna.Repuesto`, showing the captured code/description and a blank cell for labour/service rows, with the numeric FK underneath |
| same | `AplicarPermisosDetalle` | Picker enabled only with `editaItems` **and** the Repuesto type; the description box is disabled in that case. Persisted budgets stay non-editable (`editaItems` still requires `_presupuesto == null`) |
| same | `MostrarError`, `BuscarDetalleInvalido` (new) | Translates `DetallePresupuestoInvalidoException` by `ClaveMensaje` while walking `InnerException`; never parses message text |
| guides | business-flows.md, persistence.md, known-limitations.md | Replaced the "reserved type / not enabled" statement and documented the price default and snapshot rule, zero-price lines, separate consumption costs, no reservation or movement, the unchanged annul windows and the `PublicarBorrador` revalidation, labelled runtime acceptance pending |

Checks actually run:

| Check | Command | Result |
| --- | --- | --- |
| Build (documented command) | vswhere + `MSBuild .\TP_INTEGRADOR_2022.sln /t:Build /p:Configuration=Debug "/p:Platform=Any CPU" /m /v:m` | ABSTRACTIONS..APPLICATION compiled, then **exit 1** with `MSB3027`/`MSB3021` copying `DOMAIN`/`APPLICATION`/`REPOSITORY.dll` into `PRESENTATION\bin\Debug`, which the running `UI.exe` (PID 69672) locks. Output-copy failure, not a source error |
| Build (compile proof) | same MSBuild on `.\PRESENTATION\UI.csproj` with `/p:OutputPath=%TEMP%\dsh-feat007-ui\` | **PASS: exit 0**; all six projects built, `UI -> UI.exe`, output written outside the repository |
| Diff hygiene | `git status --short`, `git diff --stat`, `git diff --check` | Only authorized files changed; `git diff --check` exit 0; per-file LF/CRLF and BOM state preserved (all touched files are BOM-less) |
| Harness | `powershell -NoProfile -File ".\init.ps1"` | PASS: 10 tasks, state/completion gates, 210 relative links, `git diff --check` |

**Not verified (builder scope):** no application launch, no startup migration, no database read or
write, no MSBuild output inside `PRESENTATION\bin` (locked), and none of the three
`vis-pre-repuesto-*` scenarios. Zero-stock quotation, rejection of an inactive or missing part at
emit time, the language refresh, the grid column and numeric-culture behaviour are proven by source
inspection plus compilation only.

**Next action for the Checker:** close the running `UI.exe` normally, rebuild Debug/Any CPU, then run
`vis-pre-repuesto-add`, `vis-pre-repuesto-emit` and `vis-pre-repuesto-regression` through the real
WinForms UI against the authorized non-production database, with readback of
`PresupuestoDetalle.id_repuesto`, the stock/movement tables and the ES/EN labels. This Builder report
certifies nothing.

## Checker verification (feat-007, 2026-10-13) - independent, runtime

Environment: branch `checkpoint-6-arreglos-y-mejoras` plus the uncommitted feat-006/feat-007 diff. The
leftover `PRESENTATION\bin\Debug\UI.exe` (PID 69672) was closed **gracefully through its own window**
(pywinauto window close, no taskkill/Stop-Process; only `FrmPrincipal` was open and the process exited).
The documented build from vswhere discovery then succeeded: MSBuild `TP_INTEGRADOR_2022.sln` Debug/Any
CPU `/m`, six projects, `UI -> PRESENTATION\bin\Debug\UI.exe`, exit 0. `init.ps1` PASS (10 tasks, 218
relative links, `git diff --check` clean); `git status --short` shows the same modified set as the
builder plus the preserved untracked `.codegraph/`, `.playwright-mcp/` and `skills-lock.json`. Driver:
pywinauto 0.6.9 from `%LOCALAPPDATA%\ServicioTecnicoDesktopTests`, real clicks and typing, admin
session, separate Spanish and English passes, window sizes 900x600 and 2000x1200.

**Database identity (checked before any write):** `TPIntegrador` on the local SQL Server container
`03b0f931595f` (`sqlserver-tp`, mssql/server:2022-latest, published on localhost:1433; `UI.exe.config`
`UrlDB` is `Server=.` / `Database=TPIntegrador` and no other SQL instance is installed or listening).
It is the same non-production database used by feat-002/004/005/006 (its fixtures are present:
`AGENT_PART_20261003B`, `AGENT_LOW_20261003B`, clientes 1002..3006, orders 1002..2026), and every row
this run created or changed reads back through that connection with the values the UI displayed, which
also proves the application writes there. No production or unidentified database was touched; no
credential or password hash is printed.

**Startup evidence:** launching the application ran the idempotent seeds and inserted
`TipoItem.Repuesto` plus the five `Mensaje.Presupuesto*` keys exactly once per language (ES/EN),
confirmed by readback of `Palabras`/`Traducciones`. That was the only feat-007 data change the startup
performed.

**Fixtures created through the UI (`AGENT_007_` prefix, all left in place):** parts
`AGENT_007_PART_A` (id 1004, PrecioReferencia 150.00, CostoActual 100.00, stock 5),
`AGENT_007_PART_Z` (id 1005, reference 0.00, stock 0, active) and `AGENT_007_PART_D` (id 1006,
reference 70.00, stock 3; deactivated for the blocked path and reactivated afterwards), cliente 3007
(`AGENT007D1`), equipo 3004 (`AGENT_007_SN1`) and orders 4007/4008/4009/4010, each taken through
reception, assignment and a reparable diagnosis to PendientePresupuesto. Nothing pre-existing was
deleted, rewritten or cleaned up; the two initial-stock adjustments of the new parts are the only
movements they ever had.

| Scenario | Result | Observed through the real UI and read back from the database |
| --- | --- | --- |
| vis-pre-repuesto-add | PASS | ES selector `Mano de obra / Servicio / Repuesto`, EN `Labor / Service / Spare part`; grid headers ES `Repuesto` vs `Tipo`, EN `Spare part` vs `Type`. On order 4007, Add with type Repuesto and no part gave `Seleccione un repuesto para el item del presupuesto.` (ES) and `Select a spare part for the estimate item.` (EN) with 0 grid rows and 0 `PresupuestoDetalle` rows. Choosing `AGENT_007_PART_A` (1004, reference 150.00) prefilled 150.00; quantity 2 and operator price 175.00 produced the row `AGENT_007_PART_A - ... / Repuesto / 2 / 175.00 / 350.00`; remove plus re-add reproduced it; five type switches in ES and five in EN kept the typed description; `AGENT_007_PART_Z` (1005, reference 0.00, stock 0) was accepted at 0.00/0.00. The saved draft stored detail 4016 (1004, 2, 175.00, 350.00) and 4017 (1005, 3, 0.00, 0.00), both unchanged after closing and reopening the order in ES and EN; on the persisted budget the type selector, Add, quantity and price were disabled. The picker rendered at 900x600 and 2000x1200. |
| vis-pre-repuesto-emit | PASS | Draft 2026 published through BTN_Emitir -> order 4007 EsperandoRespuesta, detail 4016-4019 byte-identical, header subtotal/total 1850.00. No stock effect: `MovimientosStock` 61 -> 61, `ReparacionRepuesto` 5 -> 5, `AGENT_007_PART_A` `1004|150.00|100.00|5|1` and `AGENT_007_PART_Z` `1005|0.00|10.00|0|1` unchanged, per-part movement and consumption lists unchanged; only the documented Borrador -> EsperandoRespuesta history row was added. Approve -> AutorizadoReparacion with the same values and still no movement. The master was then changed through the Inventario UI from 150.00/100.00 to 888.00/333.00 and the quoted line stayed 175.00/350.00 in the grid and in the database. Repair start plus consumption of `AGENT_007_PART_A` qty 1 moved stock 5 -> 4 and created `ReparacionRepuesto` 1007 with `costo_unitario` 333.00 (the cost at that moment) while the budget stayed identical. Blocked path: order 4008 draft 2027 (detail 4020, `AGENT_007_PART_D` 1 x 70.00) with the part deactivated -> publish blocked with `El repuesto seleccionado no existe o esta inactivo. Seleccione un repuesto activo.`; the order stayed PendientePresupuesto, 2027 stayed Borrador, detail 4020 was unchanged, counts and history did not change, and the inactive part line was still displayed by its captured reference. |
| vis-pre-repuesto-regression | PASS | Order 4009 direct emission of Mano de obra 1 x 1000.00 + Servicio 2 x 250.00 + Repuesto 2 x 70.00 with discount 140.00 -> Presupuesto 2028 subtotal 1640.00 / discount 140.00 / total 1500.00, detail 4021-4023 (4023 with `id_repuesto` 1006) and no stock change. Annul regrets (cancelled reason dialog; empty reason -> `Complete los campos obligatorios de la orden.`; No on the confirmation) all left 2028 Pendiente; the confirmed annul returned PendientePresupuesto with 2028 Anulado, detail retained and no stock reversal. Re-emission as a new original created 2029 (270.00, detail 4024 with `id_repuesto` 1006) while 2028 stayed Anulado; approve -> AutorizadoReparacion; repair started -> EnReparacion, where annulling the approved original was blocked with `No se puede anular el presupuesto aprobado porque la orden ya tiene reparaciones iniciadas.` and nothing changed. Additional neighbours: regret unchanged, request -> PendientePresupuesto, cancel -> EnReparacion, re-request -> PendientePresupuesto, part-containing additional draft 2030 (Borrador 190.00, detail 4026 `id_repuesto` 1006 1 x 90.00), publish -> EsperandoRespuesta, approve -> EnReparacion with no stock movement. Control probes on editable order 4010 (0 `PresupuestoDetalle` rows persisted): typed quantity 0 was coerced to 1 by the control; price 70.555 was refused by the domain rule (`Ingrese un precio unitario valido, no negativo y con hasta dos decimales.`) with no row written; typed -5 was coerced to 0.00 and the zero-price row was accepted; typed 99999999 was coerced to 10000000.00. |

Criteria verdicts (all five verified; full evidence strings are in [feature_list.json](feature_list.json)):

| # | Criterion | Verdict | Basis |
| --- | --- | --- | --- |
| 1 | Spare-part type available next to labour/service with ES/EN labels | PASS | ES/EN selector items, `Repuesto:`/`Spare part:` label, `Repuesto`/`Spare part` grid header |
| 2 | Part selection required; reference, quantity and price persisted and shown | PASS | prefill 150.00, override 175.00, detail 4016/4017/4020/4023/4024/4026 read back, grid row and non-editable persisted view |
| 3 | Invalid part/quantity/price blocked with a business message, no partial budget | PASS | no-part blocks in ES and EN with 0 rows written; inactive part on publish with no document/state/history change; 3-decimal price refused by the domain rule; quantity coerced by the control |
| 4 | Stock/cost effect documented and matching; approved economics never mutated | PASS | no movement or consumption at create, publish, approve, annul or re-emission; the quoted line stayed fixed after a master price/cost change; consumption snapshots its own cost (333.00); the guides were updated in the same scope and read against the observed behaviour |
| 5 | Labour/service lines, additional budget and annul/re-emit unchanged | PASS | direct mixed emission with discount, four annul regret/blocked paths, re-emission as a new original, request/cancel/re-request and a part-containing additional draft published and approved back to EnReparacion |

Source inspection versus runtime evidence: the typed `DetallePresupuestoInvalidoException` with its
`ClaveMensaje`, `ValidarParaGuardar`, `ValidarRepuestoDelItem` (active part, no stock requirement, no
price substitution) and the `PublicarBorrador` revalidation were confirmed in
[TipoItemPresupuesto](DOMAIN/Features/Ordenes/TipoItemPresupuesto.cs),
[DetallePresupuesto](DOMAIN/Features/Ordenes/DetallePresupuesto.cs),
[OrdenServicioService](APPLICATION/Features/Ordenes/OrdenServicioService.cs) and
[PresupuestoRepository](INFRASTRUCTURE/Features/Ordenes/PresupuestoRepository.cs) (nullable
`@IdRepuesto<i>` binding, no stock statement anywhere in that repository), and their effects were
observed at runtime. A grep of `PRESENTATION/Forms` for SQL keywords, `SqlConnection`/`SqlCommand` or
repository references returned 0 matches, so no form contains SQL or repository calls. The `Repuesto`
type constant and the `OrdenDetalle.Repuesto` / `Columna.Repuesto` translations were reused, as
claimed. Not re-tested: concurrency and the two source-only branches listed below.

Observations recorded, not fixed (no implementation file was changed by the Checker):

1. **Not reachable through the UI (recorded, not a defect):** a live ES/EN switch while the modal order
   detail is open. The `Idioma` menu reports enabled, but the click is rejected while the form runs its
   modal loop (error -2147220992), so the in-memory preservation across a live language switch, and the
   direct-emission branch with a part deactivated between adding the line and emitting, stay
   source-inspected. English behaviour was verified independently by reopening the form in English; the
   persisted values, the toggle, the blocked message and the grid headers were all observed in EN.
2. **Documentation drift (reported, not fixed, out of scope):** `docs/agents/business-test-catalogue.md`
   row PRE-01 and `IMPLEMENTACION_SERVICIO_TECNICO.md` still call the Repuesto budget item type
   reserved infrastructure that is not emit-enabled. Neither file is in `feat-007`'s authorized scope.
3. **Price-prefill edge (source-inspected):** an active part whose `PrecioReferencia` exceeds the price
   control maximum (10,000,000) cannot be prefilled; the picker warns instead of clamping. At runtime a
   typed 99999999 was coerced by the control to 10000000.00.

## Builder report (feat-006, 2026-10-13) - historical, superseded by its Checker section

Objective: make `documento` a unique customer identity across active and inactive rows, keep
`telefono`/`email` freely shareable, grandfather pre-existing duplicates, and stop user
administration from surfacing a raw index error. Scope: only the authorized `feat-006` files.

| File | Method | Change |
| --- | --- | --- |
| [Cliente](DOMAIN/Features/Clientes/Cliente.cs) | new `ClienteDocumentoDuplicadoException` | Business exception deriving from `ReglaNegocioException`, carrying the conflicting id, name, surname, documento and active flag plus a readable Spanish fallback message |
| [ClienteRepository](INFRASTRUCTURE/Features/Clientes/ClienteRepository.cs) | `Inicializar` | Separate guarded batch installs `UX_Clientes_Documento` only when the index is absent and no duplicate `documento` group exists (inactive rows included); duplicate databases skip it and still start |
| same | `Agregar`, `Modificar` | Ownership check and DML in one `ExecuteTransaction` batch under `TABLOCKX,HOLDLOCK`; a conflict executes no DML and returns the conflicting id negated; index errors 2601/2627 map to the typed exception |
| same | `ObtenerConflictoDocumento(documento, idExcluir)` | Parameterised lookup over all rows, deterministic `TOP 1 ... ORDER BY id_cliente`, returns no conflict when the edited row already owns the proposed documento (grandfathering, SQL equality) |
| [ClienteService](APPLICATION/Features/Clientes/ClienteService.cs) | `Crear`, `Modificar` | Conflict pre-check before any write or audit; `catch (ReglaNegocioException) { throw; }` preserves the concrete exception type |
| [FrmClientes](PRESENTATION/Forms/Clientes/FrmClientes.cs) | `MostrarError`, `MostrarDocumentoDuplicado`, `BuscarConflictoDocumento` | Typed conflict shown with `Mensaje.ClienteDocumentoDuplicado`/`...Inactivo`; other errors keep the generic wrapper; no SQL or message-text parsing |
| [UsuarioService](APPLICATION/Features/Usuarios/UsuarioService.cs) | `Modificar` | Username pre-check with self-id exclusion; `UsuarioYaExisteException` handled as in `Crear` |
| [UsuarioRepository](INFRASTRUCTURE/Features/Usuarios/UsuarioRepository.cs) | `Modificar` | 2601/2627 on `UX_Usuarios_Username` mapped to `UsuarioYaExisteException`, mirroring `Agregar` |
| [FrmAdministrarUsuarios](PRESENTATION/Forms/Auth/FrmAdministrarUsuarios.cs) | create/edit catches, `MostrarUsuarioDuplicado`, `EsUsuarioYaExiste` | Shows translated `Mensaje.UsuarioYaExiste` instead of the raw database error |
| [IdiomaRepository](INFRASTRUCTURE/Features/Idiomas/IdiomaRepository.cs) | `SembrarDatosIniciales` | ES/EN seeds for `Mensaje.ClienteDocumentoDuplicado`, `Mensaje.ClienteDocumentoDuplicadoInactivo` and `Mensaje.UsuarioYaExiste` (`AgregarSeed`, insert-if-missing) |

[FrmClienteEditar](PRESENTATION/Forms/Clientes/FrmClienteEditar.cs) needed no change: it still
validates the four required fields, and the parent list form displays the duplicate message after
the dialog closes.

Checks actually run:

| Check | Command | Result |
| --- | --- | --- |
| Build | vswhere + `MSBuild TP_INTEGRADOR_2022.sln /t:Build /p:Configuration=Debug "/p:Platform=Any CPU" /m` | PASS: six projects, `UI.exe` produced, exit code 0 |
| Whitespace | `git diff --check` | PASS (exit 0); readback confirmed LF files stay LF and CRLF files stay CRLF, all without BOM |
| Harness | `powershell -NoProfile -File ".\init.ps1"` | PASS (see the verification table below) |

**Not verified at builder time (historical; superseded by the Checker section below):** no
application launch, no startup migration, no database read or write, no concurrency exercise. The unique-index installation, the duplicate-free check and the
grandfathering path are proven by source inspection plus compilation only. Next action for the
Checker: run `vis-uniq-cliente-duplicate`, `vis-uniq-cliente-disabled`, `vis-uniq-cliente-edit` and
`vis-uniq-usuario-disabled` through the real WinForms UI against the authorized non-production
database and record the observed messages, row counts and readback.
## Checker verification (feat-006, 2026-10-13) - independent, runtime

Environment: branch `checkpoint-6-arreglos-y-mejoras` @ `94c0829` plus the builder diff (8 source
files, 3 guides, tracker). The running `PRESENTATION\bin\Debug\UI.exe` was reused (started 02:30);
its binaries were newer than every `.cs` file (newest source 02:25:41, newest binary 02:26:03) and
MSBuild Debug/Any CPU was re-run from vswhere discovery at 02:55 (all six projects, exit 0). Driver:
pywinauto 0.6.9 from `%LOCALAPPDATA%\ServicioTecnicoDesktopTests`, real clicks and typing, Spanish
and English, admin session, MDI child at 900x600 and 2000x1200 plus the default size.

**Database identity (checked before any write):** `TPIntegrador` on `localhost,1433`, server host
`03b0f931595f` (container), SQL Server Developer Edition. This is the same non-production database
used by feat-002/004/005 (its fixtures are still present: clientes 1002/2002/2003/3002/3003 and
orders 2002..2009). No production or unidentified database was touched. `UX_Clientes_Documento` was
**absent** after startup and stays absent: the database already holds 3 duplicate `documento` groups
(40500200, 45206329, 99910011), so `Inicializar` skipped the index by design and the transactional
guard was the enforcement path. The 8 pre-existing `Clientes` rows and 8 pre-existing users were
re-read after every step and are unchanged; nothing was deleted, merged or rewritten. Every created
record uses the `AGENT_006_` prefix: Clientes 3004/3005/3006 and usuarios 9/10 (all left active).

**Startup evidence:** launching the application ran the idempotent seeds and inserted the three new
translation keys exactly once per language (`Mensaje.ClienteDocumentoDuplicado`,
`Mensaje.ClienteDocumentoDuplicadoInactivo`, `Mensaje.UsuarioYaExiste` - ES and EN), i.e. the only
feat-006 data change the startup performed.

| Scenario | Result | Observed through the real UI and read back from the database |
| --- | --- | --- |
| vis-uniq-cliente-duplicate | PASS | AGENT_006_A created (id 3004, documento AGENT006D1). Two consecutive attempts with D1 blocked: `El documento 'AGENT006D1' ya pertenece a AGENT_006_A AGENT006 (ID 3004). Edite el registro existente.` No row added (`COUNT(*) documento=AGENT006D1` stayed 1). AGENT_006_C created with a distinct AGENT006D2 (id 3005). After closing and reopening the list: 8 pre-existing rows + 3004/3005/3006. EN (`Ingles`): `Document 'AGENT006D1' already belongs to AGENT_006_A AGENT006 (ID 3004). Edit the existing record.` |
| vis-uniq-cliente-disabled | PASS | Deactivation dialog dismissed with No -> `activo(3004)=1` and the grid unchanged; confirmed Yes -> `Operacion realizada exitosamente.` and `activo=0`. Creating with D1 blocked: `El documento 'AGENT006D1' ya pertenece a AGENT_006_A AGENT006 (ID 3004), un cliente inactivo. Marque 'Ver inactivos' para editarlo o reactivarlo.` With `Ver inactivos` ticked the disabled row is listed as Activo=0 and editing it while keeping D1 succeeded (telefono 555006001 -> 555006999; documento and activo=0 unchanged). AGENT_006_D (id 3006) then created with the same telefono 555006999 and the same email agent006a@test.local as A - readback shows both rows. Reactivating A returned `activo=1` and the repeated D1 attempt showed the active-owner message. |
| vis-uniq-cliente-edit | PASS | AGENT_006_C (id 3005, AGENT006D2) edited onto D1 blocked with the D1 owner message; immediately after and again after closing/reopening the list the database still showed 3005=AGENT006D2 and 3004=AGENT006D1 with exactly one row holding D1. Re-saving C with its own D2 succeeded, changing it to the free AGENT006D4 persisted, changing it back to D2 persisted; re-saving A with its own D1 also succeeded. ES and EN templates observed. |
| vis-uniq-usuario-disabled | PASS | Non-session AGENT_006_USR created (id 9); the elimination confirmation dismissed with No left `activo=1`, confirmed Yes -> `Usuario eliminado exitosamente.` and `activo=0`. Re-creating the username blocked with `El nombre de usuario 'AGENT_006_USR' ya existe, incluso si la cuenta esta inactiva. Edite o reactive la cuenta existente.` - a business message, not a raw SQL error - and `COUNT(*)` for that username stayed 1. Editing AGENT_006_USR2 (id 10) onto that username was blocked with the same message and USR2 was unchanged. Reactivation restored `activo=1`. EN: `Username 'AGENT_006_USR' already exists, even if the account is inactive. Edit or reactivate the existing account.` |

Criteria verdicts (all six verified; details and message texts above and in `feature_list.json`):

| # | Criterion | Verdict | Basis |
| --- | --- | --- | --- |
| 1 | Duplicate active documento rejected naming the existing record; distinct documento creates | PASS | Duplicate attempts blocked with owner name + ID 3004; C created with a distinct documento |
| 2 | A disabled cliente keeps reserving its documento; editing that record stays possible | PASS | Inactive-owner message; own edit saved while `activo` stayed 0 |
| 3 | Edit onto another record's documento rejected; own unchanged documento still saves | PASS | Blocked and unchanged after reopen; own documento re-saved and D4->D2 round trip persisted |
| 4 | Telefono and email remain non-unique | PASS | Clientes 3004 and 3006 share telefono 555006999 and email agent006a@test.local |
| 5 | Uniqueness enforced on the persistence path and validated in the service; no form contains SQL | PASS | Guarded `ExecuteTransaction` batches under `TABLOCKX,HOLDLOCK` plus service pre-check (source); index skipped safely on this duplicate-holding database (runtime); duplicate rejected with no DML (runtime row counts); grep of `PRESENTATION/Forms` for SELECT/INSERT/UPDATE/DELETE/SqlConnection/SqlCommand/repositories = 0 matches |
| 6 | User administration shows a clear business message instead of a raw database error | PASS | ES/EN `Mensaje.UsuarioYaExiste` observed on create and edit paths; no SQL text, no new row |

Repository checks actually run: MSBuild Debug/Any CPU via vswhere (six projects, UI.exe, exit 0);
`powershell -NoProfile -File ".\init.ps1"` PASS (9 tasks, gates, 202 links, `git diff --check`);
`git status --short` and `git diff --stat` (13 modified files, same set as the builder, plus the
preserved untracked `.codegraph/`, `.playwright-mcp/`, `skills-lock.json`); the Checker modified only
`feature_list.json` and `progress.md`; read-only `sqlcmd` readback through the external environment.

Scoped regression (affected rows only): CLI-01/04 create and edit (S1-S3); CLI-02 each required field
omitted -> `Nombre, apellido, documento y telefono son obligatorios.`, dialog stays open, 0 rows
created; CLI-05 two full deactivate/reactivate cycles on AGENT_006_D (3006) with `activo` read back
1->0->1 twice and the inactive row hidden from the active-only grid; CLI-07 regret on customer and
user deactivation (No -> no state change); CLI-08 persistence after closing and reopening the list;
filters (documento `AGENT006D1` -> 1 row, `AGENT006D` -> 3 rows, cleared -> 9 active rows,
`Ver inactivos` -> 11 rows with 2 inactive); user administration create/edit/disable/reactivate.
CLI-06 (deactivating a customer that owns active equipment) was not re-run: feat-006 changes no
equipment rule and no active-flag coupling, and no AGENT_006_ equipment exists.

Source inspection versus runtime evidence: the typed exception, the guarded index batch and the
2601/2627 mapping were confirmed in source and the guard behaviour was confirmed at runtime. The
`CREATE UNIQUE INDEX` branch itself, the 2601/2627 fallback and parallel execution were **not**
runtime-exercised (this database cannot install the index). No concurrency claim is made.

Observations recorded, not fixed (no implementation file was changed by the Checker):

1. Documentation drift: [the catalogue](docs/agents/business-test-catalogue.md), CLI-03 still says a
   duplicate documento is "Allowed; document is not unique", which feat-006 invalidated. The file was
   not in feat-006's authorized scope; the row needs a separate authorized docs update.
2. Pre-existing layout limit: at a narrow MDI child (900x600) the `PNL_Filtros` panel keeps its
   design width, so the `Ver inactivos` checkbox sits outside the visible form even though the
   inactive-owner message tells the operator to tick it. `FrmClientes.Designer.cs` was not modified
   by feat-006; this is unchanged legacy layout, observed and reported.
3. Minor, not reachable from the UI: `ClienteRepository.Modificar` returns success when `@Id` does
   not exist (the `UPDATE` affects 0 rows); `ClienteService.Modificar` throws
   `El cliente seleccionado no existe.` first, so the UI path cannot reach it.
## Sweep result (run 20261003B, completed) - historical

All 144 catalogue rows were exercised through the real WinForms UI with read-only SQL readback.

- **PASS:** CLI-01..08, EQP-01..07, CAT-01..05, PRO-01..03, STK-01..07, COM-01..06/08..10, ORD-01..10, DIA-01..07, PRE-01..13, ADI-01..11, REP-01..06/08..10, PRU-01..08, ENT-01..08, GAR-01..07/09..11, DSH-01..06, INF-01..10, HIST-01, LIST-01, LANG-01, PERM-01, INT-01..05.
- **RESOLVED (feat-005, verified 2026-10-05):**
  - COM-07: `AnularConfirmadaConStock` now validates `SUM(cantidad)` per part. Verified: aggregate 2 > stock 1 -> blocked (stock 1, no negative); with coverage 2 -> annul succeeds (stock 2 -> 0, 2 movements); repeat annul rejected; single-row COM-06 still blocked.
  - GAR-08: `EvaluarReingreso` takes an explicit `continuarPago` flag from the UI destination dialog. Verified: withdrawal + reason containing `pago` -> ListoRetiro/GarantiaNoCubierta; paid -> PendientePresupuesto; GAR-05/07 unchanged.
  - INF-11: report queries use `< DATEADD(day, 1, @Hasta)`. Verified: Desde=Hasta=2026-10-04 report equals the DB for that day and excludes later records; INF-07/INF-09 range values match the DB.
- **BLOCKED:** REP-07 partial return: the UI "Quitar consumo" returns the whole (part, cost) group; partial quantities are not exposed. Not a confirmed defect.
- Fix locations for the three defects are in the "Pending fixes recorded" table below and in [known limitations](docs/agents/known-limitations.md).
- Fixtures created through the UI (kept for review): type/brand/customer/equipment/supplier/parts AGENT_* and orders 2002..2009, 3002..3010, purchases 1..6, plus warranty/evaluation/consumption/test rows.
- Operational notes for the next run: order ids jumped (2xxx then 3xxx); `open_order` searches by number and verifies the header. Form steps take ~40s each; use command timeouts >= 1200s and read `sweep-log.md` (written incrementally). Relaunch/login via `scripts\sweep\sweepkit.ensure_login()`.

## Handoff for the next session

1. Read [AGENTS.md](AGENTS.md); run `powershell -NoProfile -File ".\init.ps1"`.
2. **Current state:** `feat-006`, `feat-007`, `feat-008`, `feat-009` and `feat-010` are `done`.
   `feat-006`..`feat-009` were verified through the real UI with database readback; `feat-010` was a
   **documentation-only** task (owner-authorized) whose Builder realigned the catalogue, the root
   implementation notes and the limitations register, and whose Checker verified all five criteria with
   wording, relative-link, `init.ps1` and `git diff --check` checks without repeating runtime tests.
   **No task is `in-progress`**, so the next session must start from a newly authorized feature.
3. Historical (feat-005): the fix diff and its UI+DB evidence are below; the three defects stay
   closed. Optionally push the pending commit, or investigate REP-07 partial-return UI separately.
4. If touching the customer or user identity paths again, re-run the `vis-uniq-*` scenarios plus
   the affected catalogue rows (CLI-01..08, PERM-01).

## Reference: catalogue phases and driver

- Phases: A (CLI, EQP, CAT, PRO, STK, PERM-01), B (COM), C (ORD, DIA, PRE, ADI, REP, PRU, ENT), D (GAR), E (DSH, INF), F (INT + closure).
- Desktop driver: external environment at `%LOCALAPPDATA%\ServicioTecnicoDesktopTests` (pywinauto 0.6.9). See [Desktop testing (local only)](docs/agents/desktop-testing.md).
- Fixtures use the `AGENT_TEST_<run-id>` prefix (masters `AGENT_`); records are kept unless the owner asks for cleanup.

## Verification Evidence

| Check | Command or procedure | Result | Boundary |
| --- | --- | --- | --- |
| Build | MSBuild `TP_INTEGRADOR_2022.sln` Debug/Any CPU | PASS: six projects, UI.exe produced | Compilation only |
| Desktop environment | external `check-environment.ps1` | PASS: windows-mcp 0.8.7, pywinauto 0.6.9, input desktop readable | Tooling only |
| Sweep A-F (run 20261003B) | pywinauto form flows + read-only SQL readback; see external `sweep-20261003B.md` | Complete: all 144 rows executed or recorded; defects later fixed in feat-005 | Full sweep; defects were findings |
| Fix COM-07 | Aggregate 2 > stock 1 annul (blocked); coverage 2 annul (reverses exactly); repeat; single-row COM-06 | PASS: no negative stock; state/movements correct | UI + DB |
| Fix GAR-08 | Withdrawal + reason containing `pago`; paid button; withdrawal without `pago`; accept | PASS: GarantiaNoCubierta / PendientePresupuesto / GarantiaNoCubierta / Autorizado | UI + DB |
| Fix INF-11 | Report Desde=Hasta=2026-10-04 with filter on; INF-07/INF-09 ranges | PASS: matches DB for that day and excludes later days | UI + DB |
| Fixtures | Orders 2002-2009/3002-3014, purchases 1-6/1002-1003, parts AGENT_PART/AGENT_LOW, customer CLI_20261003B, supplier AGENT_PROV_20261003B | Created through the UI; intentionally kept for review | Non-production DB |
| Harness verification | `powershell -NoProfile -File ".\init.ps1"` | PASS at end of session (2026-10-13): state/completion gates, relative links, `git diff --check` | Structural checks only |
| Build (feat-006) | vswhere + MSBuild `TP_INTEGRADOR_2022.sln` Debug/Any CPU, `/m` | PASS: six projects, UI.exe produced, exit 0 | Compilation only; no runtime or DB |
| Diff hygiene (feat-006) | `git status --short`, `git diff --stat`, `git diff --check` | 8 source files + 3 guides + tracker changed; readback kept LF/CRLF and BOM state per file; `git diff --check` exit 0 | Read-only Git; nothing staged |
| feat-006 behaviour | `vis-uniq-cliente-duplicate`, `vis-uniq-cliente-disabled`, `vis-uniq-cliente-edit`, `vis-uniq-usuario-disabled` | **NOT RUN** (builder scope, historical) | Superseded by the row below |
| feat-006 verification (Checker) | Four `vis-uniq-*` scenarios through the real UI (pywinauto, ES/EN) + read-only `sqlcmd` readback on TPIntegrador; MSBuild reload; `init.ps1` | PASS: 6/6 criteria, 4/4 scenarios; `UX_Clientes_Documento` absent by design (3 pre-existing duplicate groups); pre-existing rows unchanged (see the Checker section) | UI + DB; the index-install branch, the 2601/2627 fallback and concurrency were not exercised |
| Build (feat-007, documented command) | vswhere + MSBuild `TP_INTEGRADOR_2022.sln` Debug/Any CPU `/m` | Exit 1: `MSB3027`/`MSB3021` copying `DOMAIN`/`APPLICATION`/`REPOSITORY.dll` into the locked `PRESENTATION\bin\Debug` (running `UI.exe`, PID 69672); the five libraries compiled | Output lock, not a compile error |
| Build (feat-007, compile proof) | MSBuild `.\PRESENTATION\UI.csproj` Debug/Any CPU with `/p:OutputPath=%TEMP%\dsh-feat007-ui\` | PASS: six projects incl. `UI -> UI.exe`, exit 0 | Compilation only; output written outside the repository |
| Diff hygiene (feat-007) | `git status --short`, `git diff --stat`, `git diff --check` | Only authorized files changed (7 source files, 3 guides, tracker); `git diff --check` exit 0; per-file LF/CRLF and BOM state preserved | Read-only Git; nothing staged |
| feat-007 behaviour | `vis-pre-repuesto-add`, `vis-pre-repuesto-emit`, `vis-pre-repuesto-regression` | **NOT RUN** (builder scope) | Superseded by the row below |
| feat-007 verification (Checker) | Three `vis-pre-repuesto-*` scenarios through the real UI (pywinauto, ES and EN, real clicks/typing, 900x600 and 2000x1200) + read-only `sqlcmd` readback on TPIntegrador; MSBuild Debug/Any CPU after closing the leftover `UI.exe`; `init.ps1` | PASS: 5/5 criteria, 3/3 scenarios; spare-part detail rows 4016/4017/4020/4023/4024/4026; no stock movement or consumption from any budget path; the quoted line stayed fixed after a master price/cost change; consumption snapshots its own cost 333.00 | UI + DB; the two source-only branches, concurrency and the out-of-scope documentation drift were not exercised |

| Build (feat-008, documented command) | vswhere + MSBuild `TP_INTEGRADOR_2022.sln` Debug/Any CPU `/m /v:m` | Exit 1: `MSB3027`/`MSB3021` copying `APPLICATION.dll`/`REPOSITORY.dll` into the locked `PRESENTATION\bin\Debug` (running `UI.exe`, PID 84192); the five libraries and the UI compile step passed | Output lock, not a compile error |
| Build (feat-008, compile proof) | MSBuild `.\PRESENTATION\UI.csproj` Debug/Any CPU with `/p:OutputPath=%TEMP%\dsh-feat008-ui\` | PASS: six projects incl. `UI -> UI.exe`, exit 0; `PRESENTATION\obj\Debug\UI.exe` newer than both edited sources | Compilation only; output written outside the repository |
| Diff hygiene (feat-008) | `git status --short`, `git diff --stat`, `git diff --check` | 20 modified files (same set as before the task, no new ones), `git diff --check` exit 0, LF/CRLF and BOM preserved | Read-only Git; nothing staged |
| feat-009 (Checker) build + harness | vswhere + MSBuild Debug/Any CPU /m /v:m; init.ps1; git status --short, git diff --stat, git diff --check | **PASS**: build exit 0 with six projects and a refreshed `UI.exe`; init.ps1 PASS (10 tasks, 262 links); no new file, git diff --check exit 0 | Compilation and structure only |
| feat-009 (Checker) block/allow/reverse edge | Six refused creation attempts (Recibido x3 + reopened window + PendienteEvaluacionGarantia + AutorizadoReparacion/Aceptada), then delivery of A -> B, withdrawal rejection of B -> C, paid rejection of C, and BTN_CancelarEntrega on A with B open | **PASS**: every refusal showed the exact business text with no reason prompt and left the original byte-identical; allowed creations 4014/4015; the blocked cancellation wrote nothing while No/cancelled-reason/blank-reason committed nothing; with no open sibling the cancellation succeeded (EnPruebas/NULL, history +1) | UI + DB |
| feat-009 (Checker) list + regression | Garantia tab with three re-entries in ES and EN, the empty case, grid at default/1400x1000/900x600; delivery of 4012/4016/4018 and the warranty order 4013; GAR-05/06/07/08/10/11; original snapshot | **PASS**: rows matched echa_ingreso and the translated states, ES/EN labels correct, empty list error-free, ENT-02 positive/zero-day/non-repaired as documented and one coverage row only for 4012; 900x600 shows only the clipped caption (see the Checker section) | UI + DB; no concurrency run |
| feat-010 (Builder, documentation only) | Edits to the five authorized files plus `powershell -NoProfile -File ".\init.ps1"`, `git diff --check`, a manual relative-link check of the root implementation notes and a SHA-256 manifest of every protected file | PASS: tracker valid (`feat-010` the only `in-progress`, five criteria `verified: false`, `visual_scenarios: []`); catalogue rows keep six columns; 251 protected tracked files plus the three untracked entries byte-identical | Documentation only: no build, no launch, no database access and no runtime test repeated |
| feat-010 (Checker) | Re-read the corrected rows/sections against the dated feat-006..feat-009 Checker evidence; `powershell -NoProfile -File ".\init.ps1"`; `git diff --check`, `git status --short`, `git diff --stat`; manual relative-link sweep of the five authorized files; catalogue row-id/column audit; tracker JSON audit; SHA-256 manifest of the 251 protected tracked files plus the untracked inventory with modification-time comparison | **PASS**: 5/5 criteria; `init.ps1` PASS (10 tasks, 274 links); `git diff --check` exit 0; 0 broken links (root notes 1/1, catalogue 4/4, known-limitations 40/40, progress 57/57); exactly 7 catalogue rows changed with an identical 144-id sequence; protected manifest aggregate `979316b8...e14f4cc9` and no protected write later than 08:15:41 | Documentation only: no build, no launch, no database access, no sweep and no fixture command; the pre-edit manifest was not persisted, so non-modification rests on the mtime gap and content scope |

Historical (2026-10-03, `feat-002`): the budget, repair/tests/reopen, delivery and warranty
re-entry cycles were validated through the real forms against the non-production database;
orders 1002-1006 and `AGENT_` fixtures were intentionally kept. Detailed log and screenshots:
`%LOCALAPPDATA%\ServicioTecnicoDesktopTests\evidence\visual-validation-20261003.md`.

## Blockers / Not Re-tested

- COM-07, GAR-08 and INF-11 are **fixed and re-verified** (feat-005); no longer open.
- REP-07 (partial return) is BLOCKED: the UI "Quitar consumo" returns the whole (part, cost) group and does not expose a partial quantity. Not a confirmed defect.
- Known tool limitation remains: Windows-MCP omits controls with an empty accessible name; pywinauto by AutomationId was used. The report DateTimePicker is exposed as a Pane; the filter was enabled with Space + arrow keys.
- No repository-owned automated suite exists; this is desktop evidence recorded as a run log, not a reproducible CI gate.
- **feat-006 is verified at runtime (2026-10-13, Checker):** the duplicate-documento guard, the
  inactive reservation, the legacy-duplicate grandfathering, the shareable telefono/email and the
  duplicate-username message were exercised through the real UI with database readback; the four
  `vis-uniq-*` scenarios are `passed`. The builder-only statement that used to be here is historical.
- **Not re-tested for feat-006:** the `CREATE UNIQUE INDEX UX_Clientes_Documento` installation branch
  (this database already holds 3 duplicate documento groups, so the guard skipped it by design), the
  2601/2627 fallback, and parallel execution.
- **Pre-existing layout limit (observed, not a feat-006 regression):** at a 900x600 MDI child the
  `Ver inactivos` checkbox is clipped because `PNL_Filtros` keeps its design width; the
  inactive-owner message refers to that checkbox.
- **Documentation drift RESOLVED by feat-010 (2026-10-13, documentation only):** the CLI-03 drift
  observed here (a duplicate documento described as "Allowed; document is not unique") is corrected in
  `docs/agents/business-test-catalogue.md` to the enforced rule and its blocked path; the dated
  observation above stays historical evidence and no runtime test was repeated.
- **feat-007 is verified at runtime (2026-10-13, Checker):** the spare-part budget line, its blocked
  paths, the persisted part reference and the stock/cost separation were exercised through the real UI
  with database readback; the five acceptance criteria and the three `vis-pre-repuesto-*` scenarios
  passed. The builder-only statement that used to be here is historical.
- **Not reachable through the UI (recorded, not a defect):** a live ES/EN switch while the modal order
  detail is open; the language menu reports enabled but the click is rejected in the modal loop. The
  in-memory preservation across a live switch and the direct-emission branch with a part deactivated
  between adding the line and emitting stay source-inspected; English behaviour was verified by
  reopening the form in English.
- **Price-prefill edge (source-inspected):** an active part whose `PrecioReferencia` exceeds the price
  control maximum (10,000,000) cannot be prefilled; the picker warns. At runtime a typed 99999999 was
  coerced by the control to 10000000.00.
- **Closed environment blocker:** the `PRESENTATION\bin\Debug` output lock (leftover `UI.exe`, PID
  69672) was resolved by closing that instance through its own window; the documented solution build
  then passed with exit 0.
- **Additional documentation drift RESOLVED by feat-010 (2026-10-13, documentation only):** the PRE-01
  row and the root `IMPLEMENTACION_SERVICIO_TECNICO.md` statements that called the Repuesto budget item
  type reserved infrastructure not enabled for emission are corrected to the feat-007 quotation rule
  (active part required, no reservation or movement); the dated observation above stays historical.

- **feat-008 is implemented but NOT verified at runtime (2026-10-13, Builder):** compilation and
  source inspection only. The consultation-only historical view, the restored current-intervention
  actions, the cleared grid selection and the persisted intervention of a registered test were not
  exercised; the three `vis-pru-*` scenarios are `planned`.
- **Output lock still present for feat-008:** `PRESENTATION\bin\Debug\UI.exe` (PID 84192) locks the
  build output; it was not killed, and the compile proof used a temporary output directory. Close the
  application normally before the next documented solution build.
- **Superseded (2026-10-13, Checker):** eat-008 was verified afterwards (4/4 criteria, 3/3 scenarios) - see its Checker section; the two bullets above are historical.
- **feat-009 is verified at runtime (2026-10-13, Checker):** the single-open re-entry guard, the close-then-allow sequence, the reverse delivery-cancellation edge and the re-entry list were exercised through the real UI with database readback; 5/5 criteria and 4/4 scenarios passed. Construction fixtures 4012..4018 are kept for review.
- **Not re-tested for feat-009:** true simultaneous creation (the TABLOCKX, HOLDLOCK guard and the repository sentinel path isolated from the friendly pre-check are source-inspected only, because every observed refusal came from the pre-check) and the form's conflict-refresh branch.
- **Observed layout limit (not a feat-009 defect):** at a 900x600 window (about 600x400 logical at 150% DPI) the Garantia page scrolls to its maximum and only the re-entry caption is reachable while the grid body stays clipped; the grid renders fully at the default size and at 1400x1000. This matches the checkout's known 900x600 clipping limitation and belongs with the feat-010 documentation work.
- **Not re-tested for feat-008:** the label layout at window sizes or DPIs beyond the 900x600,
  default and 2000x1200 sizes its Checker recorded, and beyond the ES/EN rendering that run observed;
  the failed-read path (grid empty with disabled actions) and the dead helper
  `ObtenerUltimaPruebaNoAnulada` (unused already at `HEAD`) are source-inspected only.

## Fixes applied (feat-005)

| Finding | Change | File |
| --- | --- | --- |
| COM-07 | `AnularConfirmadaConStock` validates `SUM(cantidad)` per `id_repuesto` before reversing | [CompraRepository](INFRASTRUCTURE/Features/Compras/CompraRepository.cs) |
| GAR-08 | `EvaluarReingreso` takes explicit `continuarPago`; UI passes the destination decision | [OrdenServicioService](APPLICATION/Features/Ordenes/OrdenServicioService.cs), [FrmOrdenServicioDetalle](PRESENTATION/Forms/Ordenes/FrmOrdenServicioDetalle.cs) |
| INF-11 | All report range queries use `< DATEADD(day, 1, @Hasta)` | [ReporteRepository](INFRASTRUCTURE/Features/Reportes/ReporteRepository.cs) |

These are also recorded in [known limitations](docs/agents/known-limitations.md).

## Next Session / Recommended Next Step

1. Re-read [AGENTS.md](AGENTS.md), resolve the checkout, and compare fresh Git state with this record.
2. **`feat-009` is `done`** (independent Checker verification above: 5/5 criteria, 4/4 scenarios, real
   UI plus database readback). Its fixtures (cliente 3009, equipos 3006/3007/3009, orders 4012..4018,
   re-entries 4013/4014/4015) are intentionally left in place for review. **`feat-010` is `done`** (documentation only, owner-authorized): its Builder
   realigned `docs/agents/business-test-catalogue.md` (CLI-03, CLI-04, PRE-01, PRU-01, GAR-02, GAR-09,
   ENT-03), `IMPLEMENTACION_SERVICIO_TECNICO.md` and `docs/agents/known-limitations.md` (including the
   small-window clipping limitation). Its Checker verified all five criteria with documentation, link, tracker and
   diff checks only (no build, no runtime test, no database access; see its Checker section).
3. `feat-006` is closed (`done`): its independent Checker verification is in the section above and in
   `feature_list.json` (6/6 criteria, 4/4 scenarios).
4. Reproduce the feat-006 evidence with
   `%LOCALAPPDATA%\ServicioTecnicoDesktopTests\scripts\sweep\feat006_s1_duplicate.py` through
   `feat006_s7_regression.py`; the run log is
   `%LOCALAPPDATA%\ServicioTecnicoDesktopTests\evidence\feat006-20261013\feat006-log.md`.
5. Reproduce the feat-009 evidence with
   `%LOCALAPPDATA%\ServicioTecnicoDesktopTests\scripts\sweep\feat009_s0_setup.py` through
   `feat009_s4d_regression4.py` (state in `evidence\feat009-20261013\state.json`, log in
   `feat009-log.md`); the run needs an `EnPruebas`-free checkout and the authorized TPIntegrador
   database. Do not re-run any of these flows for `feat-010`: that task is documentation-only and
   repeats no runtime test.
6. Historical (feat-005): review its diff and evidence above; optionally push the pending commit or
   investigate REP-07 partial-return UI separately.

### feat-012 pre-Builder preservation fingerprints (Planner read-only snapshot)

- Tracked files excluding feature_list.json/progress.md: 254 files; SHA256 bb037144738522b2589b19ed1bb8a9010733f388bab69ed666ed80d96d758f5d.
- Existing diagrams (exclude the new G06 subtree on recheck): 56 files; SHA256 56da9854b1cfbb4ec21dd22135a8044c361d17c9f3412d10c5b266dffcc67a6d.
- Preexisting local tooling .codegraph/, .playwright-mcp/, skills-lock.json: 5 files; SHA256 c50b5b4f78bb2bfd9f1f17425c23b758b87929308fc505c929e631e6ca6f5251.
- Algorithm: repository-relative slash paths sorted by PowerShell Sort-Object -CaseSensitive; each row is path + TAB + lowercase SHA256 of its bytes; join rows with LF and append a trailing LF; SHA256 of UTF8 bytes without BOM.
- Prior passing feat-001..011 entries (explicit UTF8 ReadAllText, PowerShell ConvertTo-Json -InputObject array -Depth 30 -Compress; UTF8 SHA256): A6C936B7AEBA55CF659063E2ED7611FB47F05FED632869F5E4EA6079D67E4F27. No passing feature may be reopened or rewritten.
- Registration check: init.ps1 PASS (12 tasks, 292 links). These fingerprints establish preservation only, not diagram correctness; Planner source review is ongoing.

### feat-012 Planner result (2026-10-17; read-only stage complete)

- Exact 21-path scope accepted: nine PlantUML/Markdown pairs plus index under the new G06 folder, and the two existing harness state files; no application or prior-documentation mutation.
- Source inventory from six Compile manifests: DOMAIN 49, ABSTRACTIONS 11, APPLICATION 19, REPOSITORY 25, SERVICES 4, handwritten UI 61 (Program + 28 forms + 32 private nested helpers); 169 own semantic types total. Merge partial forms and explicitly exclude generated settings/resources/metadata, not business classes.
- Plan requires per-type Spanish characteristics/behaviours with source/member links, explicit relation/cardinality support, source-to-diagram inventory, actual concrete construction and current feat-006..011 rules. Select significant members without silently dropping types; distinguish conceptual FK associations from object navigation/ownership.
- Repository split planned as technical 9 (including reports), masters 5, orders 6, stock/purchases/warranties 5. Context-only repeated reference types must be labeled.
- No renderer identified by bounded local discovery (no plantuml/dot commands, no workspace PlantUML bundle, PLANTUML_JAR unset; Java alone is not PlantUML). Structural syntax fallback is allowed by the feature, but rendering/full syntax/visual-layout certification must not be claimed.
- Verification: independent class-set/member/relationship source review, all new Markdown links (not covered by init.ps1), all nine syntax structures and aliases, whitespace/secrets/path safety, scope/preservation fingerprints, JSON/prior-feature preservation, init.ps1 and git diff --check. No build/app/DB verification is required or authorized.
- Planner reports no human/product blocker and no file edits, installs, builds, app/DB access or nested delegation. Builder receives the complete actionable plan and must leave criteria false and status in-progress for the independent Checker.

### feat-012 Builder report (2026-10-17; documentation-only stage complete)

Objective: deliver the owner-authorized G06 academic package (professional Spanish, exact code identifiers) as nine current-source PlantUML class diagrams plus nine per-diagram class characteristics/behaviours descriptions and an index, without touching application source, configuration, SQL, the database, the existing diagrams or the preexisting dirty work. Scope actually written: exactly the 21 authorized paths - the 19 new files under `diagrams/G06-diagrama-de-clases-de-todos-los-modulos` (README.md, nine .puml and nine .md) plus this `progress.md` and `feature_list.json`. No build, no application launch, no database access, no installs, no remote rendering, no Git write, no extra repository script, plan, baseline or backlog.

| Check | Command / method | Result |
| --- | --- | --- |
| Startup | `git rev-parse --show-toplevel`, `git branch --show-current`, `git status --short`, `.\init.ps1` | `checkpoint-6-arreglos-y-mejoras` @ `5114582`; the same 17 dirty tracked paths and the 3 preexisting untracked entries; `init.ps1` PASS (12 tasks, 292 relative links, `git diff --check`). Additional untracked entry after the work: only the new G06 folder. |
| Source inventory | six `Compile Include` manifests, excluding `bin/`/`obj/` and `Properties/` | DOMAIN 49 types, ABSTRACTIONS 11, APPLICATION 19, REPOSITORY 25, SERVICES 4, UI 61 (Program + 28 forms + 32 private nested helpers) = 169 own types; UI 61 types correspond to 55 distinct names because six helper names repeat across forms. |
| Class coverage | every source type name compared against the union of the nine .puml declarations and against the backticked name in its companion .md | 169/169 represented in a diagram; 169/169 named in the matching Markdown; no .puml declares a type that the source does not declare. |
| Member fidelity | 760 distinct member names extracted from the nine .puml and matched against the concatenated source of the six projects; explicit checks for `int?`/`DateTime?` members, `static`/`abstract` classes, code constants and the shared SQL predicate | 760/760 present in the source; 10/10 nullable annotations, 13/13 static classes, `abstract class PermisoComponent`, `NombreRaizSistema`, the four `Resultado*` codes and `PredicadoReingresoAbierto` all match the source declaration. |
| Structural syntax (no renderer available) | one `@startuml`/one `@enduml` per file, brace balance, per-line quote parity ignoring comments, package aliases, no remote `!include`/URL | 9/9 files pass; 0 duplicate package aliases; 0 remote references. One multi-line note with a split quoted phrase was rewritten and re-verified. |
| Markdown links | all relative links in the 19 new files resolved from their own folder | 50 relative links checked, 0 broken; source links use `../../`; no absolute or machine-specific path. |
| Hygiene and secrets | regex scan for absolute/UNC paths, connection strings, password/secret literals and key material; per-file BOM/CRLF/trailing-space/final-newline | 0 absolute paths; 0 connection strings or credential literals; the only flagged token is the private field NAME `Passphrase` declared in `G06.5-services.puml` (no value). All 19 files are BOM-less UTF-8 with LF endings, no trailing spaces and a final newline. |
| Preservation | repository-relative slash-path + TAB + lowercase SHA256 rows, LF-joined with a trailing LF, SHA256 of UTF-8 without BOM, recomputed after the writes | tracked files excluding the two state files: 254 files, `BB037144738522B2589B19ED1BB8A9010733F388BAB69ED666ED80D96D758F5D` (matches); existing diagrams excluding the new subtree: 56 files, `56DA9854B1CFBB4EC21DD22135A8044C361D17C9F3412D10C5B266DFFCC67A6D` (matches); `.codegraph/`, `.playwright-mcp/`, `skills-lock.json`: 5 files, `C50B5B4F78BB2BFD9F1F17425C23B758B87929308FC505C929E631E6CA6F5251` (matches). |
| Prior features | first 11 feature entries serialized with explicit UTF-8 read plus `ConvertTo-Json -InputObject` array, `-Depth 30 -Compress`, SHA256 of UTF-8 | `A6C936B7AEBA55CF659063E2ED7611FB47F05FED632869F5E4EA6079D67E4F27` (matches the corrected fingerprint); no passing feature entry was reopened or rewritten. |
| New folder contents | recursive listing | exactly 19 files (9 `.puml`, 10 `.md`), no subfolders, no PNG/SVG or generated artefact. |

Deliverable contents: README index with the nine pair links, legend, per-project source-to-type inventory, abstraction criteria, explicit exclusions and snapshot/verification limits; G06.1 domain (49 types plus the domain relationships, composite permissions, exceptions and the FK-conceptual convention); G06.2 contracts (10 interfaces plus `TraduccionEditable`); G06.3 application (17 services, `DigitoVerificadorHelper`, `DatosUsuarioIncorrectosException`, the concrete-repository construction matrix); G06.4.1..4.4 repository split 9/5/5+1/6, with the result codes, the shared predicate, the composite unique indices and the transaction boundaries; G06.5 services (`SessionManager`, `SesionIdioma`, `EncryptionService`, `PasswordHasher`, API only, no secret values); G06.6 UI (Program, 28 forms as `IObservador`, 32 nested private helpers with their projections and the repeated-name accounting). Current rules reflected: customer document uniqueness including inactive records, spare-part budget item as quotation only with nullable `IdRepuesto`, current-intervention test registration with explicit finalization, 0..1 warranty coverage versus 0..N re-entries with one open visit, existence-based read-only original order with its typed exceptions and the `0/-1/-2/-3` result codes, and the distinction between conceptual FK associations and C# ownership.

Limitations and deviations (Builder/source/documentation checks only): no PlantUML renderer was identified in the environment (no `plantuml`/`dot` command, no workspace bundle, `PLANTUML_JAR` unset), so the nine diagrams were validated **structurally** (delimiters, brace/quote balance, aliases, no remote references) and not rendered; no visual layout, PNG or full PlantUML syntax certification is claimed. The validation was performed with read-only shell scripts and inline commands, not with any new repository script or tool. The repository split in G06.4.x is documentary only, and `ReparacionRepository` is documented inside the stock/purchases/warranties diagram while belonging physically to `Features/Reparaciones`; both facts are stated in the documents. The diagrams describe source structure and rules, not runtime behaviour, database contents or concurrency, and no criterion of feat-012 is claimed as verified.

Next action for the Checker: independently re-run the class-set difference against the six `Compile` manifests, the member/visibility/nullability and relationship/cardinality review, the structural `.puml` checks and the 50 new relative links, re-verify the three preservation digests plus the prior-feature digest, `git diff --check` and `init.ps1`, and only then decide each of the six acceptance criteria; no runtime, build or database evidence is required or authorized for this documentation feature.

## Checker verification (feat-012, 2026-10-17) - independent, documentation/source only

Verdict: **FAIL for completion** - `feat-012` stays `in-progress`. The Checker read `AGENTS.md`, the
harness skill, the exact `feat-012` entry, the current repository state, the Planner/Builder handoff and
the nine G06 pairs against the actual compiled source. No artifact was modified: application source,
configuration, SQL, `.csproj`, the old diagrams and the 19 new G06 files were only read; this Checker
wrote only `feature_list.json` and this file. No build, application launch, database access, install,
download, remote rendering or Git write was performed.

### Independently verified (PASS)

| Check | Method | Result |
| --- | --- | --- |
| Own-type inventory | `Compile Include` of the six `.csproj`, excluding `bin/obj` and generated `Properties` files, cross-checked with a brace-scoped parse of every `.cs` | DOMAIN 44 code files / 49 types (45 Compile entries including `AssemblyInfo`), ABSTRACTIONS 11/11, APPLICATION 19/19, REPOSITORY 25/25, SERVICES 4/4, UI 57 files / 61 types = 29 top-level + 32 nested; 169 total |
| Diagram coverage and ownership | every source type matched to a `.puml` class by name **and** nesting parent | 169/169 present, 0 phantom classes; nested helper ownership matches the source exactly (the 12 `FrmOrdenServicioDetalle` helpers and the other 20 in their own forms) |
| Member fidelity | 1165 documented members matched against the declared members of the owning class plus its base chain (case-exact) | 1165/1165 correct; 0 members borrowed from a sibling class or another file |
| Inheritance and dependencies | source base lists vs `.puml` generalizations/realizations; constructor fields and `new` sites vs the APPLICATION/UI dependency arrows | match (five `ReglaNegocioException` subclasses, `IEntity` contract chain, 28 `IObservador` forms, 10 repositories in `OrdenServicioService`, `EntregaRepository` reusing `OrdenServicioRepository.PredicadoReingresoAbierto`) |
| feat-006..feat-011 rules reflected | source spot checks (`Garantia.cs`, `OrdenServicio.cs`, `DetallePresupuesto.cs`, `OrdenServicioRepository`, `EntregaRepository`, `GarantiaService`, `OrdenServicioService`, `SessionManager`, `SesionIdioma`) | correct: document uniqueness exception, `int? IdRepuesto` quotation-only item, current-intervention tests, coverage 0..1 vs re-entries 0..N with one open, existence-based freeze, result codes `0/-1/-2/-3` matching the SQL literals, `CrearConHistorial` `out` code, no DI/repository interfaces |
| Structure of the nine `.puml` | one `@startuml`/`@enduml`, brace balance, package-alias uniqueness, no `!include`/URL | 9/9 pass, 0 duplicate aliases, 0 remote references |
| Markdown links | every relative link in the ten new `.md` resolved from its own folder | 50 checked, 0 broken, no absolute or machine-specific path |
| File hygiene and secrets | per-file BOM/endings/trailing space/final newline; regex scan for credentials, keys, absolute paths, IPs | 19/19 BOM-less UTF-8 + LF + final newline + no trailing whitespace; no secret values, only the private field name `Passphrase` in `G06.5-services.puml` |
| Preservation | recomputed the Planner fingerprints with the documented algorithm | tracked excluding the two state files: n254 `BB037144738522B2589B19ED1BB8A9010733F388BAB69ED666ED80D96D758F5D`; old diagrams: n56 `56DA9854B1CFBB4EC21DD22135A8044C361D17C9F3412D10C5B266DFFCC67A6D`; `.codegraph/`+`.playwright-mcp/`+`skills-lock.json`: n5 `C50B5B4F78BB2BFD9F1F17425C23B758B87929308FC505C929E631E6CA6F5251` - all match; `git status` shows only the preexisting dirty paths plus the new G06 folder; exactly 19 files in that folder |
| Prior features and harness | `feature_list.json` parse, prior-entry digest, `init.ps1`, `git diff --check` | 12 features, the 11 earlier ones `done` and unchanged; the reported prior digest `A6C936B7AEBA55CF659063E2ED7611FB47F05FED632869F5E4EA6079D67E4F27` reproduces over the prior feature **objects** (not over an array of ids as the fingerprint text worded it); `init.ps1` PASS (12 tasks, 292 relative links, `git diff --check` exit 0) |
| Renderer availability | bounded local lookup | no `plantuml`, no `dot`/Graphviz, no workspace jar, `PLANTUML_JAR` unset; Java alone is not a renderer, so the disclosed structural fallback under criterion 6 applies and no render/layout/syntax certification is claimed |

### Why it is not `done` (FAIL evidence)

1. **Criterion 3 - object ownership is misrepresented for identity-only relations.**
   `G06.1-domain.puml` lines 588-600 use `o--`/`*--` (object aggregation/composition) for 13
   relations that have no field in `DOMAIN`: `OrdenServicio` to `HistorialEstadoOrden`, `Diagnostico`,
   `Presupuesto`, `Entrega`, `Reparacion`, `Garantia` and itself by `id_orden_origen`;
   `Presupuesto *-- DetallePresupuesto`; `Compra *-- DetalleCompra`; `Reparacion` to
   `ReparacionRepuesto` and `Prueba`; `Repuesto` to `MovimientoStock`; `Garantia` to
   `EvaluacionGarantia`. A scan of `DOMAIN/**/*.cs` finds only two real collections
   (`Idioma.Traducciones`, `PermisoComponent._hijos`) and no reference-typed navigation property.
   This contradicts the document own legend (`README.md` lines 37-40: `o--`/`*--` require a real
   field or list, FK-conceptual edges are dotted), contradicts `G06.1-domain.md` lines 99-100
   ("estas relaciones se dibujan punteadas"), and contradicts `G06.1-domain.md` line 51 itself
   ("`Presupuesto` ... **No** tiene coleccion de detalles").
   `G06.1-domain.puml` line 582 (`FamiliaPermiso "0..1" o-- "0..*" PermisoComponent : Hijos`)
   attributes `Hijos` to the subclass although it is declared in `PermisoComponent`, and its `0..1`
   reading implies exclusive membership while `FamiliaPermiso.cs` states that the same
   permission/family may appear in different branches.

2. **Criterion 2 - wrong counts and wrong statements in the companion descriptions.**
   - `README.md` line 47 and line 57 and `G06.1-domain.md` line 10 declare 48 DOMAIN code files;
     `DOMAIN.csproj` declares 45 `Compile` entries (44 code files + `Properties/AssemblyInfo.cs`),
     and the README inventory table itself lists 44 rows.
   - `G06.4.3-repository-ordenes.md` line 9 says the diagram documents 6 types but lists and draws 5
     (README and `.puml`: 5).
   - `G06.4.4-repository-stock-compras-garantias.md` line 10 says 5 types but documents 6, including
     `ReparacionRepository` (README and `.puml`: 6).
   - `G06.1-domain.md` lines 12-14 name `Presupuesto.cs`, `Bitacora.cs` and `SQLHelper` as files that
     declare two types each; the actual multi-type files are `Cliente.cs`, `OrdenServicio.cs` and
     `DetallePresupuesto.cs` (two each) and `Garantia.cs` (three), while `SQLHelper.cs` belongs to
     REPOSITORY.
   - `G06.1-domain.md` line 30 says `CodigosPermiso` declares **59** `const string`; source and
     `G06.1-domain.puml` declare **57**.
   - `G06.6-ui.md` line 11 says six helper names repeat across forms; its own repeated-name table
     (lines 101-107) lists four (`ItemCliente`, `ItemProveedor`, `ItemEstado`, `ItemRepuesto`).
   - `G06.2-abstractions.md` lines 42 and 55 list `SesionIdioma` as an implementation of
     `IObservador`; the source declares `SesionIdioma : IObservado`.

3. **Residual uncertainty (not a separate failure).** The nested-class notation used in
   `G06.6-ui.puml` (`- class NodoPermiso { ... }` inside a form class body) passes the disclosed
   structural checks (balanced braces) but was **not** render-certified; no renderer exists locally
   and installing or remotely rendering was out of scope. The rest of the diagrams use plain
   PlantUML constructs.

### Criterion decision recorded in `feature_list.json`

| # | Criterion theme | Verified |
| --- | --- | --- |
| 1 | Nine current-source diagrams with G06 numbering and an index | true (delivered; the index carries the wrong DOMAIN file count reported above) |
| 2 | Companion Markdown characteristics/behaviours covering the types | false (type coverage is complete, but the counts/statements listed above are wrong) |
| 3 | Traceability of classes/members/inheritance/relations and FK-vs-ownership distinction | false (notation and ownership evidence above) |
| 4 | feat-006..feat-011 rules reflected | true |
| 5 | Preservation and documentation-only scope | true |
| 6 | Structural/links/state/preservation/diff/harness validation with the renderer limitation disclosed | true (validations ran and this evidence is recorded) |

### Boundaries and recommended next step

- Verified: source-to-document conformance, structure, links, hygiene, preservation, state and harness.
- Not verified and not authorised: build, application launch, database reads/writes, runtime behaviour,
  visual layout and full PlantUML syntax/rendering.
- Recommended next step: one repair round that (a) replaces the aggregation/composition arrows of the
  13 identity-only relations with the documented dotted FK-conceptual notation (or states an explicit
  notation rule that keeps object ownership distinct) and fixes the `FamiliaPermiso`/`Hijos` owner and
  multiplicity, and (b) corrects the seven count/statement errors listed above; then a fresh Checker
  re-verification of criteria 2 and 3. No source, configuration, SQL or database change is involved.
### feat-012 Builder report, retry round 1 (2026-10-17; narrow documentation repair)

Objective: repair the two Checker FAIL findings on the already-delivered G06 package - conceptual FK/permission notation and the inaccurate counts/statements - and remove the non-standard nested-class notation of the UI diagram. Documentation only: no source, configuration, SQL, database, build, application launch, install, download, remote rendering or Git write. Exactly 11 pre-existing authorized paths were modified: the nine G06 files named by the Planner plus `feature_list.json` and this `progress.md`. No file was added, deleted, moved or renamed; the other 10 G06 artifacts were left untouched.

| Repair | File | Change |
| --- | --- | --- |
| 13 identity-only relations | `G06.1-domain.puml`, `G06.1-domain.md` | `o--`/`*--` replaced by dotted `..` labelled `FK conceptual <actual column>`; `OrdenServicio` to history/diagnosis/budget/delivery/repair/warranty/self, `Presupuesto` to detail, `Compra` to detail, `Reparacion` to consumption/tests, `Repuesto` to movements and `Garantia` to evaluation. `Presupuesto` 1:1..* is annotated as the emitted budget (non-empty detail is required by `OrdenServicioService.ValidarItemsYCalcularSubtotal`, not by the class or the table); `Compra` 1:0..* covers the document lifecycle because `CompraRepository.QuitarItem` can delete the last item while creation and confirmation require at least one; the self relation is 0..1:0..* because `IdOrdenOrigen` is `int?` (exactly one origin applies to the warranty child, and one open child is a conditional rule, not a 0..1 lifetime cardinality). |
| Permission sharing | `G06.1-domain.puml`, `G06.1-domain.md`, `G06.2-abstractions.puml`, `G06.2-abstractions.md` | `FamiliaPermiso "0..1" o-- "0..*" PermisoComponent : Hijos` replaced by the ordinary self-association `PermisoComponent "0..*" --> "0..*" PermisoComponent : Hijos (lista real; no exclusiva)`; the same 0..1 defect of `IPermisoComponent` corrected to 0..*:0..* with a note that `Hijos` is a real `IList<IPermisoComponent>` declared in the base, that families accept children while simple permissions always reject them, and that the same component may appear in different branches. |
| File/type counts | `README.md`, `G06.1-domain.md` | DOMAIN now reports 45 `Compile` entries = 44 code files + `AssemblyInfo` (was 48); the multi-type files are `Cliente.cs`, `OrdenServicio.cs` and `DetallePresupuesto.cs` (two types each) and `Garantia.cs` (three), and `SQLHelper` is stated to belong to REPOSITORY (was `Presupuesto.cs`/`Bitacora.cs`/`SQLHelper`). |
| Other count facts | `G06.1-domain.md`, `G06.4.3-repository-ordenes.md`, `G06.4.4-repository-stock-compras-garantias.md`, `G06.6-ui.md` | `CodigosPermiso` 59 -> 57 constants; orders subgroup 6 -> 5 types; stock/purchases/warranties subgroup 5 -> 6 types (`ReparacionRepository` listed); UI repeats four helper names (was six) with 10 repeated declarations and six occurrences beyond the first; the relations row "10 formularios" corrected to 11 distinct parents (the 32-helper table itself already had 11). |
| Contract statement | `G06.2-abstractions.md` | `SesionIdioma` removed as an `IObservador` implementation in the relation row and in the implementation table: the source declares `SesionIdioma : IObservado` and the 28 forms implement `IObservador`; the architecture note now describes `IPermisoComponent` as a non-exclusive Composite. |
| UI notation | `G06.6-ui.puml`, `G06.6-ui.md`, `README.md` | The 32 `- class <Helper> { ... }` blocks nested inside a form body were rewritten as flat package-level declarations `class "FrmX.Ayudante" as UI_FrmX_Ayudante <<nested_private>> { ... }`, with every member line, visibility and nullability preserved and every helper kept in its own package and its own form. Header comment, README legend and the UI scope section describe the flat notation and state that the qualified name and the stereotype denote lexical private nesting, not data composition. No helper was merged with another, no short name was renamed and the 12 `FrmOrdenServicioDetalle` helpers stay distinct. |

| Check | Method | Result |
| --- | --- | --- |
| Source type inventory | `Compile Include` of the six `.csproj`, `bin/obj` and generated `Properties` excluded, brace-scoped parse of every `.cs` | 160 code files; DOMAIN 49, ABSTRACTIONS 11, APPLICATION 19, REPOSITORY 25, SERVICES 4, UI 57 files/61 types (29 top-level + 32 nested) = 169 types; all six counts match the recorded inventory |
| Type coverage | every source type compared with the union of the nine `.puml` declarations | 169/169 declared, 0 source types missing, 0 phantom types; per diagram: DOMAIN 49, ABSTRACTIONS 11, APPLICATION 19, repositories 9/5/5/6, SERVICES 4, UI 61 (29 + 32 qualified) |
| Member fidelity | 595 distinct member names extracted from the nine `.puml` and matched against the concatenated source of the six projects | 595/595 present as source tokens; 0 unknown names; the rewritten helper blocks kept their members byte-for-byte apart from indentation |
| UI helper mapping | flat declarations compared with a scoped parse of the 32 `private class` declarations and their enclosing form | 32/32 helpers, 32 unique aliases, 11 distinct parent forms, 0 missing and 0 extra; 12 `FrmOrdenServicioDetalle` helpers |
| Structural `.puml` validation | one `@startuml`/`@enduml`, brace balance, per-line quote parity, package-alias uniqueness per file, no `!include`/URL | 9/9 files pass; 0 duplicate aliases within a file; 0 remote references; 0 leftover nested `- class` lines |
| Markdown coverage and links | every `.puml` type named in its companion `.md`; every relative link resolved from its own folder | 0 unnamed types; 50 relative links checked, 0 broken, no absolute or machine-specific path; the 33-row helper table covers the 32 helpers and the four repeated-name rows sum to 10 |
| Corrected facts re-checked against source | `DOMAIN.csproj`, `CodigosPermiso`, `PermisoComponent`/`FamiliaPermiso`, `SesionIdioma`, `CompraRepository.QuitarItem`, `OrdenServicioService.ValidarItemsYCalcularSubtotal`, the 32 nested UI helpers | all corrected values reproduce: 45 `Compile` entries / 44 code files, 57 constants, 32 helpers with 11 parents and four repeated names, `SesionIdioma : IObservado`, last draft item deletable |
| Hygiene and secrets | per-file BOM/endings/trailing space/final newline; regex scan for credentials, keys and absolute paths | 19/19 G06 files are BOM-less UTF-8 with LF, no trailing whitespace and a final newline; 0 secret-pattern hits; 0 absolute-path hits; the G06 deliverable digest is `68236F4295339CEB14ACE9079A6944CB58257C1FB86FCD5EB4E87F09213AB920` |
| Preservation | recomputed the Planner fingerprints with the documented algorithm (tracked files via `git ls-files -z`) | tracked excluding the two state files: n254 `BB037144738522B2589B19ED1BB8A9010733F388BAB69ED666ED80D96D758F5D`; old diagrams: n56 `56DA9854B1CFBB4EC21DD22135A8044C361D17C9F3412D10C5B266DFFCC67A6D`; `.codegraph/`+`.playwright-mcp/`+`skills-lock.json`: n5 `C50B5B4F78BB2BFD9F1F17425C23B758B87929308FC505C929E631E6CA6F5251` - all match; `git status` shows only the preexisting dirty paths plus the untracked G06 folder, and the folder still holds exactly 19 files |
| Prior features and harness | `feature_list.json` parse, prior-feature digest, `init.ps1`, `git diff --check` | 12 features, the 11 earlier ones `done` and unchanged, prior-object digest `A6C936B7AEBA55CF659063E2ED7611FB47F05FED632869F5E4EA6079D67E4F27`; `init.ps1` PASS (12 tasks, 292 relative links, `git diff --check` exit 0) |
| Renderer availability | bounded local lookup (`Get-Command`, `PLANTUML_JAR`, workspace jars, Graphviz) | still no `plantuml` command, no `dot`, no workspace jar, `PLANTUML_JAR` unset (only Java is present), so the disclosed structural fallback under criterion 6 applies |

Limitations and deviations (Builder/source/documentation checks only): no PlantUML renderer exists locally, so the nine diagrams were validated structurally and against the source, never rendered; no visual layout, PNG or full PlantUML syntax certification is claimed, and the flat helper notation is standard PlantUML by construction but was not render-verified. No member-level audit was rebuilt from scratch: the 595 diagram member names and the 169-type inventory were re-derived and matched against the source, while the previously accepted 1165 member-ownership pairs and the 760-name Builder set were not recomputed; the helper rewrite preserved every member line, and the 32 helper members were checked against the new declarations. The `G06.6-ui.puml` dependency arrows were left exactly as they were (17 `..>` arrows plus the `Program ..> FrmPrincipal` edge), so `NodoPermiso.Permiso : PermisoComponent` still appears only as the documented external context type and not as an extra arrow. `feature_list.json` had mixed line endings from the earlier Checker write (62 CRLF among 696 LF); it was normalized to LF with an identical content hash (`348B3D9E972B60B5613D5B03DC998FBFA315EE28E7DB36E8821210754AA1F47D` before and after). No criterion of feat-012 is claimed as verified here: criteria 2 and 3 stay `false` and the feature stays `in-progress`.

Next action for the fresh Checker: independently re-run the class-set difference against the six `Compile` manifests, the member/visibility/nullability review, the corrected relation/cardinality and permission-sharing reading, the 32-helper flat mapping and the structural `.puml` checks, the 50 new relative links, the three preservation digests plus the prior-feature digest, `git diff --check` and `init.ps1`, and only then decide criteria 2 and 3. No runtime, build or database evidence is required or authorized for this documentation feature.


## Checker verification (feat-012, retry round 1, 2026-10-17) - independent, documentation/source only

Verdict: **PASS for completion** - all six acceptance criteria are met and `feat-012` is `done`. The fresh
Checker read `AGENTS.md`, the harness skill, the exact `feat-012` entry, the current repository state, the
Planner/Builder handoff and all nine G06 pairs against the actual source. No implementation file was
modified: application source, configuration, SQL, `.csproj`, the old diagrams and the 19 new G06 files
were only read; this Checker wrote only `feature_list.json` and this file. No build, application launch,
database access, install, download, remote rendering or Git write was performed.

| Check | Method | Result |
| --- | --- | --- |
| Startup and state | `git rev-parse --show-toplevel`, branch, `git status --short`, `init.ps1`, JSON parse | `checkpoint-6-arreglos-y-mejoras` @ `5114582`; the same 17 preexisting dirty tracked paths and 3 preexisting untracked entries plus the 19-file G06 folder; 12 features with the 11 earlier ones `done`; `init.ps1` PASS (12 tasks, 292 relative links, `git diff --check` exit 0) |
| Own-type inventory | six `Compile Include` manifests plus a brace-scoped parse of every `.cs` | DOMAIN 45 Compile = 44 code + `AssemblyInfo`, 49 types; ABSTRACTIONS 11; APPLICATION 19; REPOSITORY 25; SERVICES 4; UI 57 code files = 29 top-level + 32 nested = 61; 160 code files and 169 distinct types |
| Diagram coverage | every source type vs the nine `.puml` declarations (qualified helper names included) | 169/169 declared, 0 phantom, 0 missing; every helper parent matches the source form |
| Member ownership | 1058 `.puml` member occurrences resolved inside the owning type block | 0 members outside their owner; the 32 helper member lists resolve to their own source helper; 0 puml types unnamed in the companion `.md` |
| FK vs ownership and cardinalities | `G06.1-domain.puml`/`.md` vs DOMAIN fields, indexes and service guards | 13 identity-only relations are dotted `FK conceptual <column>`; `o--` only for the real collections (`Idioma.Traducciones`, `ObservadoresRegistrados`); `Traduccion --> IPalabra` is the only object reference; 0..1 for Diagnostico/Entrega/Garantia via `UX_Diagnostico_Orden`/`UX_Entrega_Orden`/`UX_Garantias_OrdenOriginal`; emitted detail 1..1..* via `ValidarItemsYCalcularSubtotal`; `Compra` 1..0..* because `QuitarItem` deletes the last draft line; self 0..1..0..* because `IdOrdenOrigen` is `int?` |
| Permission sharing | `PermisoComponent._hijos`/`Hijos`, `FamiliaPermiso.AgregarHijo`, `PermisoSimple.AgregarHijo`, `IPermisoComponent.Hijos` | base-owned non-exclusive `0..* --> 0..*` in DOMAIN and ABSTRACTIONS; no `0..1` and no exclusive diamond |
| Corrected counts and statements | recomputed from `DOMAIN.csproj`, `CodigosPermiso`, the four multi-type DOMAIN files, `SesionIdioma` and the 32 helper declarations | 57 constants; 5 orders / 6 stock repositories; 4 repeated helper names = 10 declarations + 6 extra occurrences; 11 parents; 12 `FrmOrdenServicioDetalle` helpers; `SesionIdioma : IObservado` and 28 `IObservador` forms; multi-type files `Cliente`/`OrdenServicio`/`DetallePresupuesto`/`Garantia`; `SQLHelper` in REPOSITORY - all reproduce |
| UI flat helper notation | 32 qualified `class "FrmX.Ayudante" as UI_... <<nested_private>>` declarations | 32 helpers, 32 unique aliases, 11 parents, 12 detail helpers, 0 leftover nested `- class` blocks, every member line resolves to source |
| feat-006..011 rules | source spot checks (`ClienteRepository`, `OrdenServicioRepository`, `EntregaRepository`, `OrdenServicioService`, `GarantiaService`, `DetallePresupuesto`, `IdiomaRepository`) | document uniqueness including inactive records; spare-part quotation-only nullable `IdRepuesto` with no stock effect; current-intervention tests plus explicit finalization; coverage 0..1 vs 0..N with one open visit; existence-based freeze (`TieneReingresosGarantia`) with the `0/-1/-2/-3` codes; no DI, no repository interface, no enum, no exclusive Composite child |
| Structure, links, hygiene | one `@startuml`/`@enduml`, brace/quote balance, aliases, remote references; every relative link; BOM/CRLF/trailing space/final newline; secret and absolute-path scan | 9/9 structural pass, 0 duplicate aliases, 0 remote references; 50/50 relative links; 19/19 BOM-less UTF-8 + LF + no trailing whitespace + final newline; 0 credential values (only the field name `Passphrase`, no value) and 0 absolute paths; G06 digest `68236F4295339CEB14ACE9079A6944CB58257C1FB86FCD5EB4E87F09213AB920` |
| Preservation and state | documented algorithm over `git ls-files`, the old diagrams and the tooling entries; prior-feature object digest | tracked excluding the two state files n254 `BB037144738522B2589B19ED1BB8A9010733F388BAB69ED666ED80D96D758F5D`; old diagrams n56 `56DA9854B1CFBB4EC21DD22135A8044C361D17C9F3412D10C5B266DFFCC67A6D`; tooling n5 `C50B5B4F78BB2BFD9F1F17425C23B758B87929308FC505C929E631E6CA6F5251`; prior-feature objects `A6C936B7AEBA55CF659063E2ED7611FB47F05FED632869F5E4EA6079D67E4F27` - all reproduce; exactly 19 files in the folder and no extra untracked path |
| Renderer | bounded local lookup | no `plantuml`, no `dot`, no workspace jar, `PLANTUML_JAR` unset (Java alone is not a renderer), so the disclosed structural fallback applies and no render/layout/full-syntax certification is claimed |

### Criteria decided

| # | Criterion theme | Verified |
| --- | --- | --- |
| 1 | Nine current-source diagrams with G06 numbering and an index | true |
| 2 | Companion Markdown characteristics/behaviours covering the types/constants, identifiers preserved and omissions documented | true |
| 3 | Traceability of classes/members/inheritance/relations/cardinalities; FK-conceptual vs object ownership | true |
| 4 | feat-006..feat-011 rules reflected | true |
| 5 | Preservation and documentation-only scope | true |
| 6 | Structural/links/state/preservation/diff/harness validation with the renderer limitation disclosed | true |

### Boundaries, non-blocking observations and next action

- Verified: source-to-document conformance, structure, links, hygiene, preservation, state and harness.
  Not verified and not authorised: build, application launch, database reads/writes, runtime behaviour,
  visual layout and full PlantUML rendering.
- Non-blocking: in `G06.6-ui.md` the `FilaOrden` and `FilaCompra` helper rows list a different
  significant-member subset than the `.puml` (both subsets are real source members); the pre-repair byte
  identity of the 10 G06 files the Builder reports untouched is not independently reproducible because no
  per-file baseline was recorded, although all of them were re-verified correct against source.
- Recommended next step: none for `feat-012`; the pending checkpoint-6 work (17 dirty tracked paths plus
  the new G06 folder) may be committed when the owner authorises a Git write.

## feat-014 Builder: archivo general de diagramas N (2026-10-27)

Objetivo autorizado: crear un unico directorio general `diagrams/N/` con un subdirectorio por caso y archivar alli los 129 cuerpos PlantUML ya presentados en el chat, sin regenerar, modernizar, corregir ni renderizar diagramas. Solo se autorizan 149 archivos nuevos en `diagrams/N/**` y la edicion de `feature_list.json` y `progress.md`.

Observacion del checkout: rama `main` en `aff17b416a351fe1d91a5f465994e1cbdb7b372c`. El estado inicial mostraba exactamente tres entradas no rastreadas preexistentes: `.codegraph/`, `.playwright-mcp/` y `skills-lock.json`. Las 13 tareas anteriores estaban `done`; `feat-014` no existia. No se ejecutaron escrituras Git ni cambios de rama.

Preservacion: antes de cualquier mutacion se capturo un manifiesto externo en TEMP con 280 registros y 13.930.341 bytes: los 275 archivos rastreados preexistentes excluyendo los dos archivos de estado, mas los cinco archivos de las tres raices no rastreadas de herramientas. Incluye los 21 archivos G06 rastreados. La captura tardo 89,286 s por la latencia de Google Drive; se uso hashing nativo SHA-256 y no se modifico ninguna herramienta. Esta baseline es la referencia de preservacion del Checker.

Extraccion: se leyo el archivo de sesion en modo solo lectura y se descomprimieron sus 1283-1286 frames concatenados ZSTD observados durante la etapa (el archivo crece con la sesion activa; los eventos seleccionados permanecen estables). Se filtraron exclusivamente los eventos `assistant/message` con seq 913, 987, 1048, 1126, 1197, 1285, 1359, 1451, 1587, 1729, 1809, 1905, 1977, 2062, 2140, 2211 y 2366; de `data.message.content` se conservaron unicamente las partes `text`, unidas con salto de linea. No se exportaron partes de razonamiento, tool calls, resultados, usuarios, credenciales ni el chat completo. Se obtuvieron 16 casos x 8 cuerpos mas el cuerpo extra N04: 129 cuerpos, 268.095 bytes UTF-8 canonicos (solo normalizacion CRLF a LF). El extra es `Relaciones principales de Gestión de órdenes`, sha256 `c74f23c887b05ec33d96c0a78fc9695bd7244186e35469047fe44efa650a16a3`.

Estructura entregada: `diagrams/N/` con 18 subdirectorios `N01`-`N18`, 129 archivos `.puml` (los 16 casos disponibles con los ocho nombres fijos y el extra solo en N04), 18 README de caso, un README raiz y `source-manifest.json`. Los casos 17 y 18 quedan solo con README pendiente, sin archivos PlantUML inventados ni vacios. El indice raiz y el manifiesto enlazan el contenido y conservan la numeracion original N01/N02 y el orden CU.

Estado: `feat-014` queda `in-progress` con dependencia `feat-013`; sus cinco criterios de aceptacion quedan sin verificar para la revision independiente del Checker. No se declara completada la tarea. Enlaces de referencia: [README del archivo N](diagrams/N/README.md) y [manifiesto de procedencia](diagrams/N/source-manifest.json).

Limites: no se ejecuto renderizado; `plantuml` y `dot` no estan en PATH, Java esta disponible pero no se encontro `PLANTUML_JAR` ni un jar en el workspace. La validacion es estructural y de fidelidad byte a byte, no una certificacion de render, layout ni sintaxis PlantUML completa. Los extractos publicos del chat del 18 al 20/10 son evidencia historica, no una nueva prueba de fuente actual ni certificacion de ejecucion. Una futura mejora de los diagramas sera una tarea separada y no forma parte de `feat-014`.

Checks de esta etapa: JSON de `feature_list.json` parseable con 14 tareas, solo `feat-014` en progreso y 151 rutas autorizadas; extraccion reproducible con 129 cuerpos y el total de bytes esperado; ninguna escritura fuera de alcance. La validacion final de estructura, enlaces, UTF-8/LF, manifiesto, preservacion e `init.ps1` se ejecuta despues de crear el archivo y se registra en el cierre del Builder.

### Cierre de verificacion del Builder (2026-10-27)

- Estructura y enlaces: 149/149 archivos nuevos; 129 `.puml`; 19 README; 18 carpetas de caso; 129 registros en `source-manifest.json`; 184 enlaces Markdown resueltos. El JSON parsea, las rutas del manifiesto son unicas y relativas a la raiz del repositorio, y N17/N18 solo contienen README, con cero `.puml`.
- Fidelidad: una reextraccion fresca del archivo de sesion (1318 frames, 2713 eventos en la ultima pasada; el archivo crece con la sesion activa) comparo los 129 cuerpos byte a byte contra el disco: 129/129 coinciden, 268.095 bytes UTF-8 en total, y el extra N04 conserva el sha256 `c74f23c887b05ec33d96c0a78fc9695bd7244186e35469047fe44efa650a16a3`. Los titulos y las horas UTC del manifiesto coinciden con el cuerpo y el evento.
- Higiene: 149/149 archivos UTF-8 sin BOM, solo LF, con LF final y sin espacios finales; cada `.puml` tiene un unico `@startuml` y un unico `@enduml`, titulo exacto y ninguna inclusion o URL remota. No se modifico ningun cuerpo PlantUML.
- Preservacion y alcance: 280/280 hashes de la baseline externa permanecen iguales (275 archivos rastreados salvo los dos de estado, mas los cinco archivos de las tres raices no rastreadas). Rama `main` en `aff17b4`; no hubo escrituras Git. El estado final esperado es dos archivos de estado modificados y 152 entradas no rastreadas: 149 dentro de `diagrams/N` y las tres raices preexistentes.
- Checks: `git diff --check` termino con codigo 0; `init.ps1` proceso `-ExecutionPolicy Bypass` dio PASS con 14 tareas, 296 enlaces relativos y `git diff --check`. No se ejecuto render, build, aplicacion, base de datos ni red. La sonda local encontro `plantuml` y `dot` fuera del PATH, Java 21 disponible, `PLANTUML_JAR` vacio y cero jar en el workspace; por tanto no se certifica sintaxis PlantUML completa ni legibilidad.
- Estado: `feat-014` sigue `in-progress`; sus cinco criterios continuan `false` y la decision corresponde al Checker independiente. No se hace commit, push ni cambio de rama.
