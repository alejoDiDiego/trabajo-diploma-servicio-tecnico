# Business test catalogue (on-demand sweeps)

**Purpose:** the maintained catalogue of business-flow scenarios for this application. It is
used for two different things:

- **Scoped acceptance (default for every task):** a feature verifies only what it changes and
  its direct effects. Use the affected rows of this catalogue, not the whole document.
- **Full or partial sweeps (only when the user explicitly requests them):** execute one phase
  (A-F) or the requested module sections. Never run the full catalogue automatically.

Administration flows (users, permissions, translations, activity log, change control, DVH/DVV
integrity) are out of scope here: they were already exercised. Only `PERM-01` is kept as a
light cross-check that business buttons respect permissions.

## How to read and execute

- Tables list `ID | Scenario | Steps | Expected (including reversal/regret/blocked/repeat) |
  Cross-module effects | Notes`.
- Expectations summarize inspected source and the [business flows](business-flows.md) guide.
  Re-confirm current guards before each sweep; current source wins over this catalogue.
- **Regret** means abandoning an operation through its dialogs (No/Cancel, empty required
  reason) and verifying nothing was committed. **Blocked** means the action is hidden/disabled
  or the guard rejects it with no side effect. **Repeat** means running the cycle at least twice.
- Rows marked `Defect:` describe a confirmed source defect. Record the observed behavior as a
  finding; do not treat the defect as a pass and do not fix it without explicit authorization.
- Fixtures: create records with `AGENT_TEST_<run-id>` (and `AGENT_` for masters), record every
  ID, keep records for review unless the owner asks for cleanup.
- Evidence: per-step log and screenshots in the external evidence directory; a compact summary
  in `progress.md`. Use the run-record format of
  [development and testing](development-and-testing.md#visual-run-evidence-and-completion).
- Environment and drivers: see [Desktop testing (local only)](desktop-testing.md).

## Execution phases

| Phase | Modules | Depends on |
| --- | --- | --- |
| A | CLI, EQP, CAT, PRO, STK, PERM-01 | environment ready |
| B | COM (purchases) and its stock/cost effects | A (part, supplier) |
| C | ORD, DIA, PRE, ADI, REP, PRU, ENT | A; STK for part consumption |
| D | GAR (warranty and re-entry) | C (a delivered, warranted order) |
| E | DSH, INF (dashboard and reports) | B, C, D fixtures |
| F | INT (integration chains) and closure | all previous |

Cross-cutting checks (`HIST-01`, `LIST-01`, `LANG-01`) run inside the phases that touch them;
`HIST-01` applies after every transition of the order chain.

## Main flow (golden path)

Canonical end-to-end sequence; every step's expected state is detailed in its module rows.
A full sweep runs it at least once; the superset with additional, re-entry and reversals is
`INT-02`.

| Step | Action | Expected state |
| --- | --- | --- |
| 1 | Create active customer, type, brand and equipment | Masters active and selectable |
| 2 | Create order with the active, owned equipment | Recibido |
| 3 | Assign an eligible technician | Recibido |
| 4 | Diagnose as reparable with warranty days | PendientePresupuesto |
| 5 | Emit the Original budget | EsperandoRespuesta |
| 6 | Approve the budget | AutorizadoReparacion |
| 7 | Start and finish the repair (consumption optional) | EnPruebas |
| 8 | Register approved tests and finalize | ListoRetiro / Reparado |
| 9 | Deliver | Entregado / Reparado; warranty when days > 0 |
| 10 | Re-enter by warranty (if created), diagnose and accept | New Garantia order -> AutorizadoReparacion; original untouched |

The golden path alone is not sufficient evidence: each sweep must also exercise the blocked,
regret and reversal branches of the modules it touches.

---

## CLI - Customers

| ID | Scenario | Steps | Expected | Cross-module effects | Notes |
| --- | --- | --- | --- | --- | --- |
| CLI-01 | Create customer | Fill name, surname, document, phone | Record appears active in grid | Selectable for equipment/orders | — |
| CLI-02 | Missing required fields | Try create with each of name/surname/document/phone empty | Blocked with message; no record | — | Confirm current field rules in source |
| CLI-03 | Duplicate document | Create second customer with same document | Allowed; document is not unique | — | — |
| CLI-04 | Edit customer | Change name/phone/email/address | Grid and combos update | Existing orders keep historical FK | — |
| CLI-05 | Deactivate -> reactivate -> deactivate | Run the full cycle | Inactive hidden from new-equipment/order combos; grid keeps history; reactivation restores selectability | Equipment stays active (independent flags) | Repeat cycle twice |
| CLI-06 | Deactivate with active equipment | Deactivate customer that owns active equipment | Customer inactive; equipment remains active; history intact | Order creation with that customer blocked | — |
| CLI-07 | Regret on deactivate/reactivate | Dismiss confirmation | No state change | — | — |
| CLI-08 | Reopen persistence | Close and reopen the list | States persist | — | — |

## EQP - Equipment

| ID | Scenario | Steps | Expected | Cross-module effects | Notes |
| --- | --- | --- | --- | --- | --- |
| EQP-01 | Create equipment | Select active customer/type/brand, fill model/serial | Record appears active | Selectable in orders for that customer | — |
| EQP-02 | Inactive references | Try each: inactive customer, inactive type, inactive brand | Blocked with message | — | — |
| EQP-03 | Edit equipment | Change model/serial/color and (if allowed) owner | Grid updates; existing orders keep original customer FK | Warranty re-entry owner mismatch scenario GAR-04 | — |
| EQP-04 | Deactivate -> reactivate | Full cycle | Inactive excluded from order combos; history kept | — | — |
| EQP-05 | Independence | Deactivate customer only; deactivate equipment only | Flags independent in both directions | — | — |
| EQP-06 | Order requires ownership | Create order selecting equipment that does not belong to the customer | Equipment not offered; guard blocks | — | — |
| EQP-07 | Multiple orders | Create several orders for the same equipment | Allowed; each order keeps its own history | — | — |

## CAT - Catalogues (equipment types, brands)

| ID | Scenario | Steps | Expected | Cross-module effects | Notes |
| --- | --- | --- | --- | --- | --- |
| CAT-01 | Create | Create type and brand | Appear active | Selectable in equipment | — |
| CAT-02 | Duplicate name | Repeat the same name | Rejected by unique constraint with message | — | — |
| CAT-03 | Edit | Rename | Grid and combos update | Existing equipment keeps reference | — |
| CAT-04 | Deactivate -> reactivate | Full cycle | Inactive excluded from new equipment; history kept | — | — |
| CAT-05 | Deactivate in use | Deactivate a type/brand used by active equipment | Record current guard behavior as observed | — | Confirm source guard before judging |

## ORD - Orders: reception, assignment, cancellation

| ID | Scenario | Steps | Expected | Cross-module effects | Notes |
| --- | --- | --- | --- | --- | --- |
| ORD-01 | Create order | Active customer + active owned equipment + problem text | Recibido; initial history entry | — | Confirm required reception fields in source |
| ORD-02 | Missing problem/reception data | Try create with empty required fields | Blocked with message | — | — |
| ORD-03 | Assign technician (eligible) | Select active user with effective ORDENES_EDITAR | Assignment saved | Diagnosis can start | — |
| ORD-04 | Assign ineligible | Inactive user or user without permission | Not offered / blocked | — | — |
| ORD-05 | Reassign | Assign a second eligible technician | Replaces assignment; history kept | — | — |
| ORD-06 | Assign after delivery | Try on Entregado | Blocked | — | — |
| ORD-07 | Modify reception | Edit problem/state fields before delivery | Saved | After delivery blocked | ModificarRecepcion lacks the standard session guard: note if observed |
| ORD-08 | Cancel allowed states | From the orders list (BTN_Cancelar): confirm + required reason at Recibido, EnDiagnostico, PendientePresupuesto, EsperandoRespuesta, AutorizadoReparacion | ListoRetiro + Cancelado each time | Later steps blocked on that order | Cancellation exists only in the list, not in the detail; repeat per state |
| ORD-09 | Cancel blocked states | Try at EnReparacion, EnPruebas, ListoRetiro, Entregado, PendienteEvaluacionGarantia | Blocked with no change | — | — |
| ORD-10 | Cancellation has no reversal | Look for un-cancel action after ORD-08 | No reversal exists; verify blocked | — | — |

## DIA - Diagnosis

| ID | Scenario | Steps | Expected | Cross-module effects | Notes |
| --- | --- | --- | --- | --- | --- |
| DIA-01 | Start | Recibido + assigned technician -> start | EnDiagnostico | — | Without technician blocked |
| DIA-02 | Duplicate | Try to start/finalize a second diagnosis | At most one diagnosis; blocked | — | — |
| DIA-03 | Finalize reparable (Normal) | Description + reparable | PendientePresupuesto | Enables budget | — |
| DIA-04 | Finalize reparable (Garantia order) | Re-entry order diagnosis | PendienteEvaluacionGarantia | Goes to GAR evaluation | — |
| DIA-05 | Finalize non-reparable | Unchecked reparable | ListoRetiro + NoReparable | Delivery path without budget; no warranty | — |
| DIA-06 | Estimated days 0 | Finalize with 0 | Stored as null; no warranty implication | Warranty days come from the budget, not here | — |
| DIA-07 | No reversal | After finalize, look for edit/annul | Fields disabled; no annul exists | — | — |

## PRE - Original budget

| ID | Scenario | Steps | Expected | Cross-module effects | Notes |
| --- | --- | --- | --- | --- | --- |
| PRE-01 | Draft | Add Mano de obra/Servicio items, quantities, price, discount, warranty days | Draft in memory; save draft persists | — | Repuesto item type is reserved infrastructure, not emit-enabled |
| PRE-02 | Remove item / delete draft | Remove an item; delete the draft | Draft detail updated; deletion removes only draft+detail | — | — |
| PRE-03 | Emit | Emit a valid draft | Pendiente; order EsperandoRespuesta | Approved amount calculation starts here | — |
| PRE-04 | Original uniqueness | Try a second non-annulled Original | Blocked | — | — |
| PRE-05 | Approve | Approve with response medium/obs | AutorizadoReparacion | Repair can start | — |
| PRE-06 | Reject | Reject with required reason | ListoRetiro + PresupuestoRechazado | Delivery without repair | Reason required |
| PRE-07 | Annul pending | Annul while EsperandoRespuesta, with reason | PendientePresupuesto; budget Anulado | Re-emit allowed | — |
| PRE-08 | Annul approved Original | Annul in AutorizadoReparacion with no interventions | Reverts to PendientePresupuesto | Blocked if any intervention started | — |
| PRE-09 | Annul rejected Original | Annul after PRE-06 with no delivery | PendientePresupuesto + NULL result; observations combined | Blocked if later non-annulled Original/Adicional exists | Check `CombineObservacion` |
| PRE-10 | Regret | Cancel the reason dialog; dismiss the confirmation | No change; order continues | — | Repeat for annul/approve/reject |
| PRE-11 | Re-emit after annul | Select "(nuevo original)", add item, emit | EsperandoRespuesta again | Amounts recalculated | — |
| PRE-12 | Amounts | Verify authorized total sums approved Originals+Adicionales after PRE-11 | Snapshot totals consistent | Dashboard/report MontoAprobados | — |
| PRE-13 | Draft persistence | Save draft, close the detail, reopen the order | Draft and its items persist; publish still works | Reopen check | — |

## ADI - Additional budget

| ID | Scenario | Steps | Expected | Cross-module effects | Notes |
| --- | --- | --- | --- | --- | --- |
| ADI-01 | Request from Autorizado | Reason + no pending additional | Order pauses to PendientePresupuesto | Repair blocked while paused | — |
| ADI-02 | Request from EnReparacion | Same | Same pause | — | — |
| ADI-03 | Request from EnPruebas | Same | Same pause | — | — |
| ADI-04 | Emit/publish additional | Emit the additional draft | EsperandoRespuesta | — | — |
| ADI-05 | Approve | Approve additional | Resumes origin: Autorizado, EnReparacion, or EnReparacion when origin was EnPruebas | Tests must be redone if origin was EnPruebas | Do not assume instant pass |
| ADI-06 | Reject | Reject with reason | ListoRetiro + PresupuestoRechazado | — | Confirm current destination in source |
| ADI-07 | Cancel request | Cancel before decision | Origin restored; additional drafts removed | Blocked if a Pendiente additional exists; prior approved/rejected do not block | Repeat request after cancel |
| ADI-08 | Annul approved additional | Annul with later repair activity | Blocked by `HasActividadPosterior` | — | Timestamps read separately: not atomic |
| ADI-09 | Old approved additions | Keep a previous approved additional and run a new cycle | Previous addition retained; new cycle works | — | — |
| ADI-10 | Origin heuristic | Vary history text/sequences | Origin inferred from history/observations | — | Defect: `TryObtenerOrigenSolicitud` is a text heuristic, not an explicit request ID |
| ADI-11 | Regret | Cancel dialogs in ADI-01/04/05/06/07 | No change | — | — |

## REP - Repairs and consumption

| ID | Scenario | Steps | Expected | Cross-module effects | Notes |
| --- | --- | --- | --- | --- | --- |
| REP-01 | Start first intervention | From AutorizadoReparacion | EnReparacion + intervention #1 | — | — |
| REP-02 | Open intervention guard | Try to start another with an open one | Blocked | — | — |
| REP-03 | New intervention | Close current, start again from EnReparacion | Numbered next intervention | History kept | — |
| REP-04 | Consume part | Open intervention + stock | Stock decreases; separate consumption row with current cost | Movements + reports | — |
| REP-05 | Same part, two costs | Change master cost, consume again | Two rows with different snapshot costs | — | — |
| REP-06 | Insufficient stock | Try to consume more than stock | Blocked transactionally | — | — |
| REP-07 | Partial return | Return part of a consumption | Oldest consumption ID reduced first; remaining rows keep costs | Stock increases | — |
| REP-08 | Full return | Return everything | Rows deleted; movement history retained | — | — |
| REP-09 | Finalize intervention | Work text required | EnPruebas | — | Empty work text blocked |
| REP-10 | Inactive part | Deactivate a part, open consumption combo | Inactive not offered; history intact | — | — |

## PRU - Tests

| ID | Scenario | Steps | Expected | Cross-module effects | Notes |
| --- | --- | --- | --- | --- | --- |
| PRU-01 | Register | EnPruebas + latest finished intervention; approved/failed | Persisted; order state unchanged | — | — |
| PRU-02 | Annul any current test | Annul a non-latest, non-annulled test with reason | Annulled; excluded from finalization | — | Not only the most recent |
| PRU-03 | Finalize without valid tests | Annul all and finalize | Error; state unchanged | — | — |
| PRU-04 | Finalize all approved | One or more approved valid tests | ListoRetiro + Reparado | Delivery/warranty path | — |
| PRU-05 | Finalize with failure | Include a failed/revision test | EnReparacion + NULL result | Repair must run again | — |
| PRU-06 | Reopen | ListoRetiro+Reparado, reason | EnPruebas + NULL result | Result cleared; history kept | — |
| PRU-07 | Repeat cycle | Finalize again after reopen, twice | Stable repeated transitions | — | — |
| PRU-08 | Regret | Dismiss confirm / cancel reason in annul/finalize/reopen | No change | — | — |

## ENT - Delivery

| ID | Scenario | Steps | Expected | Cross-module effects | Notes |
| --- | --- | --- | --- | --- | --- |
| ENT-01 | Deliver | ListoRetiro + receiver name | Entregado + delivery record | — | Receiver required |
| ENT-02 | Warranty auto-creation | Normal + Reparado + approved Original with warranty days > 0 | Warranty created with delivery and end dates | GAR re-entry available | No warranty for other results, zero days, or warranty orders |
| ENT-03 | Cancel delivery (repaired) | Reason | EnPruebas + NULL result; delivery physically deleted | Warranty/re-entries remain | Re-delivery skips warranty insertion; dates are not refreshed |
| ENT-04 | Cancel delivery (other result) | Reason on a NoReparable delivery | ListoRetiro keeping its result | — | — |
| ENT-05 | Re-deliver | Deliver again after cancel | Entregado again | — | — |
| ENT-06 | Regret | Dismiss confirm / cancel reason | No change | — | — |
| ENT-07 | Delivered lock | Try edits on Entregado | Blocked | — | — |
| ENT-08 | Quick delivery from list | Select a ListoRetiro order in the orders list, press Entregar | Detail opens on the Entrega tab; completing it delivers normally | — | Alternative UI path; only for ListoRetiro |

## STK - Parts and stock

| ID | Scenario | Steps | Expected | Cross-module effects | Notes |
| --- | --- | --- | --- | --- | --- |
| STK-01 | Create part | Code, description, cost, selling price, minimum and initial stock | Active part in grid with initial stock | Selectable in consumption/purchases; low-stock KPI | Confirm initial-stock movement recording in source |
| STK-02 | Duplicate code | Repeat code | Rejected | — | — |
| STK-03 | Edit | Change cost/price/minimum | Grid updates | Old consumption costs unchanged | — |
| STK-04 | Deactivate -> reactivate | Full cycle | Inactive excluded from consumption/purchase combos; history kept | — | — |
| STK-05 | Manual adjustment | Stock adjustment form with reason | Movement recorded; stock changed | Movements list; dashboard low-stock | Confirm required reason in source |
| STK-06 | Movements list | Read after STK-05/REP/COM | Movements consistent with operations | — | — |
| STK-07 | Low stock | Stock <= minimum | KPI and low-stock query include the part | DSH, INF | Equality boundary |

## PRO - Suppliers

| ID | Scenario | Steps | Expected | Cross-module effects | Notes |
| --- | --- | --- | --- | --- | --- |
| PRO-01 | Create/edit | Reason/social name and data | Grid updates | Selectable in purchases | — |
| PRO-02 | Duplicate social name | Repeat | Rejected | — | — |
| PRO-03 | Deactivate -> reactivate | Full cycle | Inactive not selectable for new purchases; history kept | — | — |

## COM - Purchases

| ID | Scenario | Steps | Expected | Cross-module effects | Notes |
| --- | --- | --- | --- | --- | --- |
| COM-01 | Draft | Add items, including repeated rows for the same part | Draft persists; stock unchanged | — | — |
| COM-02 | Remove item | Remove a detail row | Draft detail updated | — | — |
| COM-03 | Cancel draft | Cancel | Persists as Cancelada; no stock change | — | Regret variant: dismiss confirmation |
| COM-04 | Confirm | Confirm draft | Stock increases; Compra movements; master cost set to purchase cost | STK, reports, consumption cost | — |
| COM-05 | Annul confirmed | Reason + sufficient stock | Cancelada + negative adjustments linked to the purchase | Stock decreases; previous master cost is NOT restored | — |
| COM-06 | Annul blocked | Insufficient stock | Blocked | — | — |
| COM-07 | Duplicate-part aggregate | Repeated rows for one part, annul with enough stock for each row but not the total | Blocked; stock unchanged and never negative | — | Fixed (feat-005): `AnularConfirmadaConStock` validates the summed quantity per part |
| COM-08 | Regret | Dismiss confirm / cancel reason | No change | — | — |
| COM-09 | Reports | Read purchases-by-supplier after COM-04/05 | Counts/totals match confirmed purchases | INF | — |
| COM-10 | List quick actions | Confirm, cancel and annul directly from the purchases list | Same behavior as the detail form | — | Alternative UI path |

## GAR - Warranties and re-entry

| ID | Scenario | Steps | Expected | Cross-module effects | Notes |
| --- | --- | --- | --- | --- | --- |
| GAR-01 | List/filter | Read after ENT-02 | Warranty row with dates; filter works | — | — |
| GAR-02 | Re-entry | From a delivered Normal order with valid warranty, reason + confirm | New order Tipo Garantia, Recibido; original untouched | ORD/DIA/ENT on the new order | — |
| GAR-03 | Zero days / expired | Deliver with warranty days 0, or wait past end date | No warranty / `ObtenerVigente` false; re-entry blocked | — | Defect: no start-date check in `ObtenerVigente` |
| GAR-04 | Owner changed | Change equipment owner, then re-entry from the delivered order | UI passes the historical customer; service validates current owner -> re-entry can fail | — | Confirmed source-path finding; do not claim a universal failure |
| GAR-05 | Evaluate accepted | Re-entry + reparable diagnosis -> accept with reason | AutorizadoReparacion without commercial budget | Repair path | — |
| GAR-06 | Evaluate rejected, paid | Choose the paid destination button | PendientePresupuesto (ordinary paid budget) | PRE flow | Decision is explicit (`continuarPago`), not text-based (feat-005) |
| GAR-07 | Evaluate rejected, withdrawal | Reject with reason without `pago` | ListoRetiro + GarantiaNoCubierta | Delivery | — |
| GAR-08 | Withdrawal reason containing `pago` | Choose withdrawal but include `pago` in the text | ListoRetiro + GarantiaNoCubierta | — | Fixed (feat-005): the explicit destination decides |
| GAR-09 | Multiple re-entries | Create a second re-entry while warranty is valid | Confirm actual behavior in source | — | Warranty row is unique per original |
| GAR-10 | Warranty order delivery | Deliver the warranty order after repair+tests | No new warranty is created | — | — |
| GAR-11 | View original | From a re-entry, press Ver original | Shows the original delivered order; original unchanged | — | — |

## DSH - Dashboard

| ID | Scenario | Steps | Expected | Cross-module effects | Notes |
| --- | --- | --- | --- | --- | --- |
| DSH-01 | Open KPI | With known fixtures | All non-Entregado, including ListoRetiro and cancelled/rejected | — | — |
| DSH-02 | Repair KPI | EnReparacion + EnPruebas | Counts only those states | — | — |
| DSH-03 | Waiting/ready KPIs | EsperandoRespuesta; ListoRetiro | Match fixtures | — | — |
| DSH-04 | Warranty KPI | Non-delivered Tipo Garantia orders | Matches fixtures | — | — |
| DSH-05 | Low stock | Active parts stock <= minimum | Matches STK-07 | — | — |
| DSH-06 | Refresh | Advance an order, press refresh | KPIs change accordingly | — | — |

## INF - Reports

| ID | Scenario | Steps | Expected | Cross-module effects | Notes |
| --- | --- | --- | --- | --- | --- |
| INF-01 | Orders by state | Known fixture set | Non-delivered, reception-date filter | — | — |
| INF-02 | Orders by result | Fixtures with ListoRetiro results | Delivered outcomes excluded | — | Confirmed definition; not a bug unless requirements change |
| INF-03 | Repairs by technician | Intervention fixtures | Count by start date | — | Not distinct orders |
| INF-04 | Average reception/delivery | Delivered fixtures | AVG days | — | — |
| INF-05 | Approval rate | Approved/rejected fixtures | approved/(approved+rejected) | — | Pending excluded |
| INF-06 | Most-used parts | Consumption + returns | Remaining quantities after FIFO returns | — | — |
| INF-07 | Purchases by supplier | COM fixtures | Confirmada count and total | — | — |
| INF-08 | Re-entries / warranty rate | GAR fixtures | Re-entry count; accepted/(accepted+rejected) | — | Label vs definition mismatch noted |
| INF-09 | Approved amount | PRE fixtures | Sum approved totals both types | — | Not billing/collection |
| INF-10 | Low stock | STK-07 | Active parts stock <= minimum | — | — |
| INF-11 | Final-day date boundary | Record on the selected final day with the date filter on | Included; later days excluded | — | Fixed (feat-005): `< DATEADD(day, 1, @Hasta)` |

## PERM-01 - Business permissions cross-check (light)

| ID | Scenario | Steps | Expected | Cross-module effects | Notes |
| --- | --- | --- | --- | --- | --- |
| PERM-01 | Read-only user | Create a test user with read-only permissions (admin form is fixture setup only), sign in, open business lists/forms | Business actions hidden/disabled (create/edit/emit/deliver); read-only navigation works | — | Permission checks are largely UI-side; a hidden/disabled action is valid UI evidence |

## Cross-cutting checks

| ID | Scenario | Steps | Expected | Notes |
| --- | --- | --- | --- | --- |
| HIST-01 | Order history | After every transition, read the Historial tab | One entry per transition with acting user and timestamp, matching the performed actions | Different from Bitacora (administration, out of scope) |
| LIST-01 | Orders list filters and quick paths | Filter by customer/state/type, search by number/problem, toggle delivered; cancel and quick-deliver from the list | Filters narrow the grid; cancellation only for allowed states; quick delivery only ListoRetiro | See ORD-08 and ENT-08 |
| LANG-01 | ES/EN switch | Switch Idioma > Espanol/Ingles on key business lists and details | Labels, buttons and headers update; entered values and state preserved | Translation administration is out of scope; this checks only the business UI |

## INT - Integration chains

| ID | Scenario | Steps | Expected | Cross-module effects | Notes |
| --- | --- | --- | --- | --- | --- |
| INT-01 | Purchase-to-report chain | Supplier + part -> draft purchase -> confirm -> consume in repair -> partial return -> low-stock check -> annul purchase coverage -> reports and movements | Stock, costs, movements, reports consistent at each step | STK, REP, COM, DSH, INF | — |
| INT-02 | Full order chain with additional | Customer/equipment -> order -> diagnosis -> Original approve -> request additional -> cancel request -> request again -> approve -> repair with consumption -> tests (annul + revision cycle) -> deliver -> warranty -> re-entry -> accept -> repair -> tests -> deliver warranty order -> dashboard/reports | Every state and side effect consistent; original order untouched by re-entry | ORD..GAR, DSH, INF | Longest chain; run last |
| INT-03 | Deactivation mid-flow | Deactivate customer/equipment/part/supplier during the chain | Blocked selectors; history preserved; reactivate and continue | CLI, EQP, STK, PRO | Both directions of independence |
| INT-04 | Cancellation matrix | Cancel orders at each allowed state; attempt each blocked state | ORD-08/09 consistency across the fixture set | — | — |
| INT-05 | Known-defect confirmation | Run COM-07, GAR-08, INF-11 on purpose | Observed behavior matches the documented defect notes | — | Findings, not passes |

## Closure checklist for a sweep

- The main flow (golden path) was executed at least once during the sweep.
- Every executed row has an evidence line (result, observed state, screenshot reference).
- Failed/blocked rows are recorded as failed/blocked; they are not silently skipped.
- New findings are added to [known limitations](known-limitations.md) only with owner approval.
- The sweep summary and next step are written to `progress.md`.
