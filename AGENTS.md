# Agent onboarding and safe contribution rules

This is a Windows desktop application for managing electronics repair services.
Read these instructions before changing or running anything in the repository.
They work with any AI harness; no memory service, plugin, or special framework is required.

## Startup Workflow

1. Resolve the checkout with `git rev-parse --show-toplevel`.
2. Read `git status --short` and `git branch --show-current`.
3. Read [feature_list.json](feature_list.json) and [progress.md](progress.md); confirm the authorized task and next step.
4. Run `powershell -NoProfile -File ".\init.ps1"` ([read-only checks](init.ps1)); this does not verify the application.
5. Read [the guide index](docs/agents/README.md), [architecture](docs/agents/architecture.md), and the guide for your task.
6. Inspect current source anchors; state the authorized scope and verification plan before mutations.

## Task state and scope

- **One feature at a time:** only one task may be `in-progress`; dependencies must be done first.
- Record only authorized tasks, file scope, acceptance criteria, and applicable visual scenarios in `feature_list.json`.
- Use `not-started`, `in-progress`, `blocked`, or `done`; record evidence and next action in `progress.md`.
- A failed baseline check is a blocker to report, not authorization to repair unrelated files or install tools.

## Authorization comes before execution

- Do not modify files or execute write operations without an explicitly authorized scope.
- Preserve preexisting tracked changes, deletions, untracked files, and local tooling folders.
- Do not restore, delete, stage, or include another person's changes without permission.
- Documentation-only work must not change source, configuration, SQL, or database data.
- A build and an application launch are separate actions with different risks.
- Application startup creates/migrates tables, seeds data, and recalculates user integrity digits.
- Never launch against production or an unidentified database to inspect the UI.
- Obtain explicit permission before database writes, startup migrations, or test cleanup.
- Do not print connection credentials, password hashes, encryption secrets, or raw secrets.
- If a file already exists, read it before editing; do not overwrite user instructions.

## Historical documentation evidence

The application guides were inspected on `checkpoint-5-dashboard-reportes` at `47a0bf8`.
They were committed in `297b993`; that does not renew their source/runtime certification.
Current task evidence belongs in `progress.md`. Re-check branch/status at every restart;
historical branch, remote, or dirty-state descriptions are not the current checkout state.

## Read by task

| Task | Required guide |
| --- | --- |
| Understand the product or locate a user-facing use case | [Use-case navigation](docs/agents/use-case-navigation.md) |
| Find a project, service, repository, or construction pattern | [Architecture](docs/agents/architecture.md) |
| Change an order, budget, repair, purchase, or warranty workflow | [Business flows](docs/agents/business-flows.md) |
| Change SQL, constraints, snapshots, or transactions | [Persistence](docs/agents/persistence.md) |
| Change permissions, login, translations, or integrity | [Security and localization](docs/agents/security-and-localization.md) |
| Build, validate desktop behavior, or prepare a checkpoint | [Development and testing](docs/agents/development-and-testing.md) |
| Prepare or run the optional local desktop-testing tools | [Desktop testing (local only)](docs/agents/desktop-testing.md) |
| Assess risk or choose a future fix | [Known limitations](docs/agents/known-limitations.md) |

## Product boundaries

- Implemented scope covers masters, orders/diagnosis/budgets, stock/repairs/tests,
  suppliers/purchases/warranties, and dashboard/reports across five checkpoints.
- The product is not an invoicing, payment, cash-management, or AFIP system.
- There is no implemented mobile client, AI diagnosis, or report export feature.
- Approved budget amounts are commitments, not collected revenue.
- Do not call checkpoint 5 final product completion or imply all limitations are resolved.

## Architecture guardrails

- Keep changes small and explicit: KISS, existing feature folders, existing responsibilities.
- All six projects target .NET Framework 4.7.2; the UI is Windows Forms.
- Use [TP_INTEGRADOR_2022.sln](TP_INTEGRADOR_2022.sln), not the stale `.slnx` path map.
- Physical `PRESENTATION` contains assembly/namespace `UI`.
- Physical `INFRASTRUCTURE` contains assembly/namespace `REPOSITORY`.
- Forms call application services; do not add direct SQL or repository calls to forms.
- Application services currently construct concrete repositories with `new`.
- There are no repository interfaces or dependency-injection container to extend.
- Do not introduce EF, IoC, CQRS, Mediator, speculative layers, or target changes without approval.
- Existing ABSTRACTIONS contracts do not make this a fully inverted/hexagonal architecture.
- Current source takes precedence over historical checkpoint prose and old diagrams.
- Do not assume Krypton is installed; current controls are ordinary WinForms controls.

## Preserve business invariants

- States/types/results are static `const string` values, not C# enums.
- Do not add an unrestricted order-state dropdown or bypass transition methods.
- Keep customer and equipment active flags independent; historical orders retain their customer FK.
- A technician is an active user with effective `ORDENES_EDITAR`, not a literal role-name test.
- Diagnosis is at most one per order; budgets and repair interventions are one-to-many.
- Do not mutate approved budget economics or invent generic budget versioning.
- Separate selling price from stock acquisition/consumption cost snapshots.
- Preserve each consumption row and its original cost; returns follow FIFO by consumption ID.
- Recording a test does not finish testing; explicit finalization evaluates all valid current tests.
- A warranty re-entry is a new order; never reset the original delivered order.
- Read rollback windows before enabling cancellation, annulment, reopening, or re-delivery.

## Security and localization cautions

- Permission checks for the acting user are largely UI-side, not a universal backend boundary.
- Effective permission descendants may be shared by several families.
- Composite permission membership is not exclusive UML composition/ownership.
- Forms observe `SesionIdioma`; use translation keys in `Tag` and existing update conventions.
- Keep generated technical documentation/comments professional and English by default.
- Preserve existing ES/EN UI conventions when extending the application.
- DVH/DVV verification currently covers `Usuarios`, not all business tables.
- Do not silently recalculate integrity to erase evidence of a mismatch.
- Demo account seeds have weak default passwords; they are not production credentials.

## Verification contract

- Distinguish **verified implementation** (source inspection), **runtime evidence**,
  **intended policy**, and **not re-tested** behavior in every report.
- Source inspection proves code paths, not live database contents or concurrency correctness.
- No repository-owned formal automated test suite was found in this snapshot.
- Historical ad hoc harness counts are not a reproducible current passing gate.
- **Primary behavior acceptance:** exercise the real WinForms UI with clicks/keyboard; test affected forward, reverse, blocked, and repeated state-transition cycles using [the visual matrix](docs/agents/development-and-testing.md#primary-acceptance-visual-state-machine-flows).
- **Scope acceptance to the task:** verify only what the task changes/adds/removes and its direct effects. Do not sweep the whole product for every change.
- **Full or partial flow sweeps run only when the user explicitly requests them**, using [the business test catalogue](docs/agents/business-test-catalogue.md) and its phases. A sweep is a separate authorized task, not part of routine feature work.
- Verify state/result, controls, related records, and persistence after reopening; distinguish dialog dismissal from a committed business reversal.
- Build/service/STA checks support visual acceptance; screenshots or `PerformClick` alone do not prove real desktop interaction. Browser Playwright is not a WinForms driver.
- Confirm native desktop capability and authorized scratch DB before execution. Missing visual evidence keeps behavior acceptance blocked; disclose assisted manual evidence explicitly.
- Validate ES/EN, permissions, small/large windows, invalid input, and numeric culture behavior.
- Ask the user to close the app if build outputs are locked; do not kill user or IDE processes.
- For documentation-only changes, link/content/status checks suffice; do not launch the app.

## Definition of Done

- Authorized scope and acceptance criteria are satisfied; required checks actually ran and evidence is recorded.
- Behavior tasks have passing evidence for every required visual scenario (the affected ones, not the whole catalogue), including reversal/blocking paths; skipped or unavailable visual checks do not count as passes.
- Relevant guides and task state are updated; preexisting user work is preserved and the next session is restartable.
- `init.ps1` passes; its structural result does not certify runtime behavior or the truth of manually recorded evidence.

## End of Session

1. Update task status and verified criteria in `feature_list.json`; use `blocked` for missing required verification.
2. Update `progress.md`: current objective, branch/commit, files, checks/results, blockers, and recommended next step. Keep older evidence dated or labeled historical.
3. Re-run `init.ps1`, review the diff and Git status, and report checks skipped. A restartable handoff does not require a clean working tree or a commit.

## Git workflow

- Each checkpoint continues from the previous approved checkpoint branch.
- Creation/switching of branches, commits, pushes, and merging to `main` need explicit approval.
- Treat commit, push, and merge as separate approvals, not one implied action.
- Never force-push, delete branches, rebase published checkpoints, or discard dirty changes.
- Updating harness state does not authorize `git add`, commit, push, switch, or merge.

## Keep these guides current

1. Trace the changed form handler, service method, domain rule, and repository batch.
2. Update the relevant guide in the same authorized work scope as the behavior change.
3. Use relative source links plus method names, not brittle source line numbers.
4. Re-check constraints, cardinalities, state guards, and transaction boundaries independently.
5. Label future recommendations as intended policy; never describe them as implemented.
6. Validate every relative Markdown link and scan additions for secrets.
7. Report files changed, checks run, checks skipped, and unresolved uncertainty.

Optional code-intelligence tools can accelerate inspection when available and healthy.
If a tool reports a broken/stale index, read current source directly and disclose the fallback.
Do not repair/reindex a broken index during a documentation-only task without authorization.
