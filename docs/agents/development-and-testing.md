# Build and validate without damaging user data

**Authorization boundary:** the read-only harness command below is suitable for documentation
work. Build, application launch, and database exercises need their own authorized scope.
A documentation-only task needs readback/link/status checks, not a desktop launch or seed run.

## Read-only harness verification

From the repository root, on Windows PowerShell 5.1 or later:

```powershell
powershell -NoProfile -File ".\init.ps1"
git status --short
```

[init.ps1](../../init.ps1) fails on invalid task fields/status, multiple active tasks, missing or
cyclic dependencies, unfinished dependencies of active/done tasks, missing completion evidence,
incomplete visual scenario gates, broken inline relative file links, or `git diff --check` errors.
It makes no file/Git writes, builds, application launches, package installations, or database calls.
Link checking covers inline file destinations in root harness Markdown and `docs/agents/*.md`,
not heading fragments, named C# methods, external URLs, or links inside code fences.
Git whitespace checking covers tracked diffs; read back newly added files too.
A pass verifies the harness structure, not the truth of recorded evidence or application behavior.

### Task tracker conventions

- `schema_version` is 1; `features` is an array. Follow the existing entry in [feature_list.json](../../feature_list.json).
- Each task has `id`, `name`, `description`, `type`, `authorized_files`, `dependencies`, `status`, `acceptance_criteria`, `visual_scenarios`, and `evidence`.
- `type` is `documentation` for harness/docs-only work or `behavior` for application changes. Use repository-relative paths for `authorized_files`; this records scope, not an automatic permission grant.
- Status is `not-started`, `in-progress`, `blocked`, or `done`. At most one task is `in-progress`; dependency IDs must exist and form an acyclic graph.
- Each acceptance criterion has a nonempty `description` and Boolean `verified`. Set it true only after the criterion is actually checked; all must be true for `done`.
- Behavior tasks need a nonempty visual matrix before activation. Each scenario has `id`, `description`, `status` (`planned`, `passed`, `failed`, `blocked`), and text `evidence` linking to a run record in [progress.md](../../progress.md) or an approved evidence artifact.
- `done` requires nonempty task evidence and all required scenarios `passed` with evidence. Missing desktop capability or DB authorization blocks acceptance; do not relabel a behavior task as documentation to bypass this gate.
- Keep the tracker limited to authorized work. Do not convert historical checkpoints into newly verified features or select a new application task automatically.

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

## Primary acceptance: visual state-machine flows

**Intended policy:** for application behavior changes, the agent's primary acceptance method
is interaction with the displayed WinForms forms through real desktop clicks and keyboard input.
Source inspection, compilation, and service/STA exercises support this gate; they do not replace it.
Check native desktop capability before promising autonomous execution. Browser Playwright is not
a desktop driver; `PerformClick`, UI Automation Invoke alone, or bitmap capture alone cannot prove
physical interaction. If the driver is unavailable, record the blocker. Human-assisted desktop
execution must be explicitly labeled with operator, steps, and observations; never claim the agent
performed those clicks. New tools/installations and scratch-DB writes need authorization.

### Plan and execute an affected-transition matrix

1. Inspect [business flows](business-flows.md), current form handlers/control guards, service/domain
   transitions, and repository effects. Separate current implementation from intended corrected
   behavior; record known discrepancies rather than inventing generic reversibility.
2. List the affected states and edges before activation. Include starting state/result, request
   origin, budget/repair/test/delivery/warranty state, permissions, and relevant stock/history.
   Test affected branches and neighboring transitions, not every product flow for every small task.
3. Reach starting states through the forms using owned scratch records. Direct DB setup or
   service calls are supplemental fixtures with explicit provenance, not proof of the UI path.
4. Observe labels, selected records, enabled/visible actions, and dialog behavior before clicking.
   Run the action and verify destination state/result plus related-record and stock/amount effects.
5. Dismiss each applicable confirmation with No/Cancel, cancel the reason dialog, and submit an
   empty required reason. Check that the operation was not committed and the UI can continue.
   Distinguish these abandoned attempts from a confirmed business cancellation or annulment.
6. Exercise legal reversals, illegal/out-of-window reversals, and permission-denied paths. A
   hidden/disabled action is valid UI blocking evidence; do not bypass it and call that a UI test.
   Separately exercise backend guards when needed and authorized. After an error, inspect actual
   persisted outcome before retrying: a post-commit error does not prove rollback.
7. Close/reopen the detail to check persisted state, history, and related records; use authorized
   read queries/repository readback for effects not visible in forms. Continue forward after the
   reversal and repeat applicable cycles to find stale controls, wrong selection, duplicate
   records, lost observations, or a stranded workflow.

### Reversal and continuation scenarios

These are scenario families, not a claim that every action is reversible. Confirm exact guards
and expected outcomes from current source and the authorized requirement for each task. Consult
[known limitations](known-limitations.md); the scenario list does not certify those defects as fixed.

| Scenario family | Required observations when affected |
| --- | --- |
| Budget emit -> annul -> re-emit; approve/reject -> eligible annul | Order and document states, result clearing, observations, totals, and blocking after later activity |
| Additional request -> cancel -> resume -> request again | All affected origins (authorization/repair/tests), draft removal, pending-document blocking, retained prior additions, correct continuation |
| Additional approval after a tests-origin pause | Current flow returns to repair; do not assume approval restores tests or passes them automatically |
| Register/annul tests -> finalize -> revision or ready for pickup | Valid current tests only, no-valid-test refusal, mixed results, annul a non-latest test, controls after each step |
| Ready for pickup -> reopen tests -> finalize again | State/result reset, retained history, renewed forward progress |
| Deliver -> cancel delivery -> continue -> re-deliver | Destination depends on result; delivery record, warranty/re-entry coexistence, no assumption that old coverage dates refresh |
| Deactivate -> reactivate/reincorporate a master | Active-only selectors, independent customer/equipment flags, historical references, repeated cycles |
| Purchase draft -> cancel; confirm -> eligible annul | Stock/movements, reversal attempts with insufficient aggregate stock and duplicate-part details, documented guard discrepancies, retained purchase history |
| Warranty re-entry -> diagnosis -> accepted/rejected/paid continuation | New linked order and untouched original, branch-specific controls/results, repeat visits; re-entry does not reset the original order |

### Visual run evidence and completion

Record each required scenario in `visual_scenarios` and keep a run record in `progress.md`:

| Scenario / run | Environment and fixture IDs | Initial state and controls | Clicks/keys/dialog choices | Expected state and side effects | Observed state, persistence, and continuation | Evidence / result |
| --- | --- | --- | --- | --- | --- | --- |
| Use the tracker scenario ID | Branch/commit and local diff, scratch DB identity without credentials, ES/EN, culture, permissions, DPI/window, driver/operator | Include origin and related document/intervention states | Reproducible sequence, including dismissed and confirmed operations | From source/approved requirement | Include reopen/readback and repeated-cycle observations | Actual screenshot/log references and passed/failed/blocked status |

Capture critical before/after and blocked/dismissed states when desktop capture is available,
without exposing secrets or real customer data. Screenshots support the action log; they are not
standalone interaction evidence. Record failed and blocked scenarios as well as successful ones.
Report exercised transitions, origins, legal/illegal reversals, and remaining gaps, not just a test
count. Missing required visual evidence keeps the behavior task blocked, even if compilation passes.

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

Checkpoints continue from the previously approved branch. Inspect current branch/status rather
than reusing the historical checkpoint-5 tracking observation. The original guides were committed
in `297b993`; that is not a fresh runtime certification. Creating/switching a branch, committing,
pushing, and merging to main each require approval. No force-push, published rebase, branch
deletion, or dirty-tree cleanup is implied by a checkpoint or harness-state update.

For docs-only review: read every new file, validate relative Markdown links and named anchors,
scan for secrets/absolute machine paths, distinguish source facts from policy, and compare
git status against the original dirty state. Report skipped runtime checks explicitly.
Before ending, update [feature_list.json](../../feature_list.json) and [progress.md](../../progress.md)
with checked criteria, actual evidence, blockers, and a concrete next step; re-run `init.ps1`.
