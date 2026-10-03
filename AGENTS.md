# Agent onboarding and safe contribution rules

This is a Windows desktop application for managing electronics repair services.
Read these instructions before changing or running anything in the repository.
They work with any AI harness; no memory service, plugin, or special framework is required.

## Start here

1. Resolve the checkout with `git rev-parse --show-toplevel`.
2. Read `git status --short` and `git branch --show-current`.
3. Read [the guide index](docs/agents/README.md).
4. Read [architecture](docs/agents/architecture.md) and the guide for your task.
5. Inspect the current source anchors before proposing a change.
6. State the authorized scope and verification plan before executing mutations.

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

## Current documentation handoff

These guides were inspected on `checkpoint-5-dashboard-reportes` at `47a0bf8`.
The local remote-tracking refs showed checkpoints 1 through 5 published.
That is a source snapshot, not a new runtime test certification.
The onboarding documents are intentionally LOCAL, UNCOMMITTED, and UNPUSHED.
Do not stage, commit, push, switch, or merge them without separate approval.
Preserve the preexisting deleted tracked temporary file and local tooling directories.
Later tasks must re-check branch/status rather than assume this snapshot is still current.

## Read by task

| Task | Required guide |
| --- | --- |
| Understand the product or locate a user-facing use case | [Use-case navigation](docs/agents/use-case-navigation.md) |
| Find a project, service, repository, or construction pattern | [Architecture](docs/agents/architecture.md) |
| Change an order, budget, repair, purchase, or warranty workflow | [Business flows](docs/agents/business-flows.md) |
| Change SQL, constraints, snapshots, or transactions | [Persistence](docs/agents/persistence.md) |
| Change permissions, login, translations, or integrity | [Security and localization](docs/agents/security-and-localization.md) |
| Build, validate desktop behavior, or prepare a checkpoint | [Development and testing](docs/agents/development-and-testing.md) |
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
- WinForms requires desktop/STA verification; browser Playwright is not its UI driver.
- Validate ES/EN, permissions, small/large windows, invalid input, and numeric culture behavior.
- Ask the user to close the app if build outputs are locked; do not kill user or IDE processes.
- For documentation-only changes, link/content/status checks suffice; do not launch the app.

## Git workflow

- Each checkpoint continues from the previous approved checkpoint branch.
- Creation/switching of branches, commits, pushes, and merging to `main` need explicit approval.
- Treat commit, push, and merge as separate approvals, not one implied action.
- Never force-push, delete branches, rebase published checkpoints, or discard dirty changes.
- Do not run `git add`, commit, push, switch, or merge for this local documentation handoff.

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
