# Build and validate without damaging user data

**Authorization boundary:** commands below are recipes for a separately approved development
task. They were not executed to verify this documentation. A documentation-only task needs
passive readback/link/status checks, not a build, desktop launch, database connection, or seed run.

## Prerequisites and portable build discovery

Use Windows, Visual Studio/Build Tools with desktop .NET build support, and the
.NET Framework 4.7.2 targeting/developer pack. The projects are classic csproj, not modern SDK
projects. Use [TP_INTEGRADOR_2022.sln](../../TP_INTEGRADOR_2022.sln).
Do not change target frameworks or install new packages just to make a local build succeed.

PowerShell example, **only when building is authorized**, from repository root:

```powershell
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
if (-not (Test-Path -LiteralPath $vswhere)) { throw 'Install Visual Studio Installer / vswhere first.' }
$msbuild = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe'
$msbuild = @($msbuild)[0]
if (-not $msbuild) { throw 'No MSBuild installation was found.' }
& $msbuild '.\TP_INTEGRADOR_2022.sln' /t:Build /p:Configuration=Debug '/p:Platform=Any CPU' /m
if ($LASTEXITCODE -ne 0) { throw 'Build failed; inspect its output.' }
```

This discovers installation location instead of assuming a drive-specific MSBuild path.
A missing targeting pack and a locked output are different failures.
If UI.exe/dependencies are locked, ask the user to close the app normally and retry when
authorized. Never terminate their application, Visual Studio, or other processes automatically.

## Database setup is separate from build

Repositories read `ConfigurationManager.ConnectionStrings["UrlDB"]`.
Use an authorized local/private configuration; never print the existing connection value.
For explanation only, a credential-free placeholder configuration is:

```xml
<connectionStrings>
  <add name="UrlDB" providerName="System.Data.SqlClient"
       connectionString="Server=YOUR_SQL_SERVER;Database=YOUR_SCRATCH_DATABASE;Integrated Security=True;" />
</connectionStrings>
```

This is a placeholder, not a change to the checked-in configuration or a guaranteed authentication
mode. An authorized SQL Server database must preexist. Program.Main creates/migrates tables,
seeds roles/translations/demo users, and recalculates integrity. Opening the application is a
write operation before login. Do not run that startup against production without permission.
Docker SQL Server on localhost:1433 can be one local setup, not a product requirement.
Do not copy old seed SQL as a substitute for current repository initialization.

## Verification levels: name what actually happened

No repository-owned formal test project/suite was found in this source snapshot.
Previous external/ad hoc STA harness results and counts are historical observations,
not available reproducible tests or a current pass certification.
Do not add external validation dependencies merely to satisfy an agent's default workflow.

| Evidence level | What it proves / does not prove |
| --- | --- |
| Source inspection | Guards, call paths, SQL text; not successful execution or live schema |
| Build | Compilation and project references; not business behavior, layout, or data safety |
| Service/repository scratch-DB exercise | Data/state behavior for the calls made; not form wiring or desktop UX |
| STA form harness with PerformClick | Programmatic handler/control path; not physical hit-testing, focus, or every human interaction |
| Real desktop click/keyboard flow | Actual displayed form navigation, enabled/visible controls, dialogs, focus |
| Bitmap/DrawToBitmap capture | Visual layout evidence for the captured state; not functional interaction proof |

Global browser Playwright tools test web pages, not native WinForms. Their availability does not
create a web app or justify project Playwright packages. Use manual desktop verification or
already-available Windows UI automation; disclose tool limits instead of claiming browser E2E.

## Reproducible desktop/STA approach

After explicit approval, prepare an isolated scratch DB/configuration and record its identity
without credentials. Build the current checkout, then run the desktop app against that DB.
Record branch/commit, culture, language, account permission family, screen/DPI, and window size.
If creating an external harness, use an STA thread, initialize WinForms visual settings,
create/show forms on that thread, run/pump the message loop, invoke controls only on their
UI thread, and close/dispose forms. Arrange session/language explicitly if bypassing Main.
Avoid launching Main implicitly against default configuration.
Check persisted results after each step through authorized read queries or repository reads.
Use real clicks for wiring/hit-testing claims and PerformClick only with that evidence label.

### Minimum scenario matrix

- Customer creation: missing phone/name/document; independent deactivation of customer/equipment.
- Reception: active/customer-owned equipment; assignment to eligible/inactive/no-edit-permission user.
- Diagnosis: duplicate guard, reparable/non-reparable, normal/warranty branches.
- Budgets: draft/publication/delete; Original uniqueness; approve/reject; each allowed/blocked annul window.
- Additional requests from all three origins: approval/cancellation, old approved additions,
  draft removal, and annul-history heuristic. Recheck observations are preserved.
- Repairs: two costs for the same part, separate consumption rows, FIFO partial/full returns,
  insufficient stock, closed intervention guard, multiple intervention history.
- Tests: no-valid finalization error, mixed outcomes, annul a non-latest current test,
  explicit finish, reopen, delivery/cancel/re-delivery and warranty coexistence.
- Purchases: draft leaves stock unchanged; confirmation/cancel/annul; duplicate-part aggregate coverage.
- Warranty: positive/zero days, expiry timestamp, separate same-equipment re-entry,
  accepted/rejected/paid continuation, reason text containing `pago` with withdrawal selected.
- Reports: known totals in scratch fixtures, date-final-day records, delivered result exclusion,
  approval denominator and warranty acceptance definition, low-stock equality.
- UI: ES/EN, small/large windows and DPI, long labels/fields, no-permission account,
  empty/invalid input, comma/dot decimals, pending numeric edits before clicking actions.

NumericUpDown uses Value in these flows; a translation choice does not specify CultureInfo.
Culture/coercion behavior must be exercised, not inferred from historical reports or assumed
to be a confirmed custom parser/zero-coercion bug in the current source.

## Fixture ownership and cleanup

Prefer a disposable scratch DB. Otherwise obtain permission, use a unique prefix such as
`AGENT_TEST_<run-id>`, and record **every created ID**. A prefix alone is not deletion authority.
Never alter real stock directly or borrow existing customer/order IDs to simplify testing.
Create dedicated parts and stock via authorized domain workflows.

For approved cleanup, use a transaction and delete only recorded owned IDs, checking dependents
first. Typical dependency order: EvaluacionesGarantia, Garantias, Pruebas, ReparacionRepuesto,
MovimientosStock, Reparaciones, Entregas, PresupuestoDetalle, Presupuestos, Diagnosticos,
HistorialOrdenes, re-entry OrdenesServicio before their original orders, CompraDetalle,
Compras, Equipos, Clientes, Repuestos, Proveedores, then owned unused brands/types.
Actual FK relationships and shared records must be checked before this plan is executed.
Do not delete unrelated audit/permission/demo rows or weaken constraints to make cleanup work.
Rollback if ownership or FK assumptions fail. Dropping a disposable DB is also a separately
authorized destructive action, not an automatic cleanup shortcut.

## Git checkpoint and documentation verification

Checkpoints continue from the previously approved branch; current local tracking showed
checkpoint 1 through checkpoint 5 aligned with their origin refs. Creating/switching a branch,
committing, pushing, and merging to main each require approval. No force-push, published rebase,
branch deletion, or dirty-tree cleanup is implied by a checkpoint request.
For this onboarding handoff specifically: **new docs stay local; no add/commit/push/switch/merge**.

For docs-only review: read every new file, validate relative Markdown links and named anchors,
scan for secrets/absolute machine paths, distinguish source facts from policy, and compare
git status against the original dirty state. Report skipped runtime checks explicitly.
