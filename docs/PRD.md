# Safety First API PRD

The API supports O1 and O2 by distributing reviewed versions and validating scenario evidence. It supports O3 by owning confirmed results, repeat-safe persistence and authorized classroom reads. The development baseline includes PostgreSQL readiness, EF Core schema migrations and xUnit infrastructure. Authentication, evidence processing and classroom endpoints are planned.

## Sources and delivery status

Paper 33 is *OSH CASPTONE RESEARCH PAPER (33).pdf*, Chapters 1–3. Page numbers below refer to printed pages. C4 identifies the separate *Chapter 4 FOR PLANNING.pdf* draft. Paper 33 §1.3.2 defines O1 (repeatable 3D practice before OJT), O2 (reassessment after changing conditions) and O3 (expert-reviewed feedback and distinct corrective attempts). Chapter 2 §§2.1.6–8 and 2.4 inform the learning design. Paper 33 §3.8 reports needs-validation evidence. The draft user stories give implementation examples and the draft Gantt gives sequencing.

Safety First supplements Civil Engineering instruction and supervised practice. Simulation scores confer no certification or evidence of real-site competence. Rows are proposed backlog requirements unless marked implemented. Signed approvals and completed product evaluation were not verified. Appendix A–I entries appear in the Paper 33 contents but their bodies are absent from the available copy.

## Users and responsibilities

Students access their own assignments, attempts, results and comments. Teachers manage their own classes, approve requests, assign existing reviewed activities and read submitted results for those classes. Teacher provisioning is an operational action. Teachers cannot author scenarios or rubrics, override scores or see private practice. API enforces the rules regardless of client state.

## Requirements and acceptance

### Learning, evidence and progression

| ID | Capability and source | Acceptance criteria for this repo |
| --- | --- | --- |
| SF-01 | Touch exploration of reviewed fictional workplaces and inspection of objects, records and routes. 33 §1.7, §3.3 item 1 (printed p. 35); C4 §4.3.1. | Serve the requested published version and stable metadata. Unknown or retired downloads return an explicit unavailable state while historical evidence remains interpretable. |
| SF-02 | Search reviewed hazard entries and open linked practice. 33 §3.3 item 2 (pp. 35–36); C4 §4.3.2. | Return reviewed, sourced hazard records and practice links for the published package. Broken links fail the publishing fixture. |
| SF-03 | Complete choice-based HIRAC including exposed people, likelihood, severity, priority, control, rationale and residual risk. 33 §3.3 item 3 (pp. 36–37); C4 §4.6.2. | Validate option and inspection references against the pinned version. Unknown references fail without confirmation; valid omissions remain scorable evidence. |
| SF-04 | Respond to reviewed changes and resource constraints across planning, emergency preparedness and documentation. 33 §1.3.2, §3.3 items 3–4 (pp. 36–38); C4 §4.5.8. | Persist the sequence of condition changes and decisions. Impossible or incompatible events fail validation without finalizing a result. |
| SF-05 | Sequence tutorial, work at height and temporary works with provisional offline unlock and confirmed credit. 33 §3.3 item 5 (pp. 38–39); C4 §4.5.5, §4.7.2. | Derive confirmed credit from accepted prerequisites. A local provisional pass sent by the client confers no confirmed credit on its own. |
| SF-06 | Version reviewed rubrics and assess initial, corrective and independent attempts separately. 33 §3.3 items 6–7, Tables 6–7 (pp. 39–41); C4 §4.5.5, §4.5.7. | Confirm only reviewed rubric versions using shared fixtures. Score correct=1 and incorrect/omitted=0. Proposed rules use applicable-item percentages, bands 4=100%, 3=80–<100%, 2=60–<80%, 1=<60%, mean of applicable ratings and pass only with mean ≥3, no applicable rating <2, and all essentials met. A corrective attempt has a separate ID. |
| SF-07 | Explain criterion-level results, omissions and corrective practice. 33 §3.3 items 7–8 (pp. 41–42); C4 §4.3.7–8. | Return item outcomes, area counts and reasons tied to the rubric, including essential omissions. Incompatible evidence returns a failure reason, not a score. |
| SF-08 | Show learner-owned progress, separated attempt types and practice recommendations. 33 §3.3 item 8 (p. 42), §3.6; C4 §4.4.1. | Return the requesting student's own confirmed history with distinct attempt kinds. A different learner's record is inaccessible. |

### Access, classroom and synchronization

| ID | Capability and source | Acceptance criteria for this repo |
| --- | --- | --- |
| SF-09 | Authenticate students and provision teachers operationally; scope first deployment to one school with multiple classes. 33 §1.7, §3.6; C4 §4.1, §4.5.6. | Refuse unauthorized and expired sessions and student self-promotion. Confirm protected operations under two distinct roles. |
| SF-10 | Manage classes and approve enrollment requests from class codes. 33 §3.3 item 9 (p. 42); C4 §4.6.4, §4.7.1. | Create pending membership through a valid code and grant class access only after approval. Cross-class teacher and invalid-code requests fail. |
| SF-11 | Assign published activities and show eligible work and status. 33 §3.3 item 9 (p. 42), §3.6; C4 §4.6.4. | Accept assignment of a published compatible version by its class teacher. A revoked membership cannot read or submit against it. |
| SF-12 | Durably save and synchronize attempt evidence with repeat-safe server acceptance. 33 §1.7.2, §3.6.I; C4 §4.5.6, §4.7.2–3. | Use a reviewed transaction and proposed unique learner/client-ID key. Simulate lost response, concurrent duplicate and changed payload; only one matching result is retained. Renewed auth can retry the same evidence. |
| SF-13 | Explicitly submit an eligible confirmed attempt and restrict teacher review to that class. 33 §3.3 item 9 (p. 42); C4 §4.5.6, §4.7.4. | Link only a confirmed eligible owned attempt to an accessible assignment. Prove a different class and confirmed-but-private practice stay absent from teacher responses. |
| SF-14 | Store classroom comments separately from assessment and return feedback to the student. 33 §3.3 item 9 (p. 42), §3.6; C4 §4.7.4. | Store comments independently of results. Read the new comment as its student and confirm the score is unchanged; reject unauthorized edits. |
| SF-15 | Control account/record access and retain historical versions and finalized evidence. 33 §3.6; C4 §4.3.11, §4.5.4, §4.5.6. | Retain pinned scenario/rubric references, result history and archived records. Reject ordinary changes to finalized evidence. Recovery and archive policy require approval. |

### Quality, research and operations

| ID | Capability and source | Acceptance criteria for this repo |
| --- | --- | --- |
| SF-16 | Verify product quality, accessibility to intended devices and operational behavior. 33 §1.7, §3.1–2, §3.8.5; C4 §4.5.3. | Exercise authorized/forbidden requests, database outage/recovery and duplicate retry with redacted traces. Availability and response-time budgets require approval. |
| SF-17 | Keep approved study procedures and records separate from classroom use. 33 §3.2, §3.10–11. | Keep study responses and identifiable records outside normal teacher queries and repository fixtures. Verify development fixtures are synthetic. |
| SF-OPS-01 | Expose API readiness for coordinated deployment. Engineering baseline; C4 §4.5.3; Linear LIG-7. | Implemented. GET /health/ready returns 503 for missing configuration or Npgsql connection failure and 200 once the configured connection opens. Exercise with no configuration and with a disposable Neon connection when available. |

## Linear delivery scope

Paper 33 Chapter 1 defines the pre-OJT practice objectives and supplementary-learning scope. Chapter 2 motivates guided practice, changing conditions and explanatory feedback. Chapter 3 defines the modules and proposed evaluation and reports needs validation. Chapter 4 specifies the proposed platform and workflows. Linear tracks delivery and acceptance evidence for these requirements.

| Capability | Linear epic | PRD requirements |
| --- | --- | --- |
| Accounts and authentication | [LIG-124](https://linear.app/ligtas-labs/issue/LIG-124/epic-user-authentication-and-account-management), [LIG-126](https://linear.app/ligtas-labs/issue/LIG-126/authapi-link-neon-identities-and-provision-safety-first-accounts) | SF-09 |
| Simulation | [LIG-128](https://linear.app/ligtas-labs/issue/LIG-128/epic-core-simulation-playthrough-and-exploration) | SF-01, SF-04–06 |
| Hazard reference | [LIG-129](https://linear.app/ligtas-labs/issue/LIG-129/epic-hazard-database-and-safety-reference) | SF-02 |
| HIRAC and safety decisions | [LIG-130](https://linear.app/ligtas-labs/issue/LIG-130/epic-hirac-and-safety-decision-making) | SF-03–04 |
| Modules and assignments | [LIG-131](https://linear.app/ligtas-labs/issue/LIG-131/epic-learning-module-and-assignment-management) | SF-05, SF-11, SF-13 |
| Practice scenarios | [LIG-132](https://linear.app/ligtas-labs/issue/LIG-132/epic-practice-scenarios-and-learning-activities) | SF-01, SF-04–06 |
| Assessment and feedback | [LIG-133](https://linear.app/ligtas-labs/issue/LIG-133/epic-rubric-based-assessment-scoring-and-feedback) | SF-06–07, SF-12 |
| Progress and monitoring | [LIG-134](https://linear.app/ligtas-labs/issue/LIG-134/epic-learning-progress-tracking-and-student-monitoring) | SF-08, SF-13 |
| Classes and enrollment | [LIG-135](https://linear.app/ligtas-labs/issue/LIG-135/epic-class-and-enrollment-management) | SF-10, SF-15 |
| Offline learning and synchronization | [LIG-136](https://linear.app/ligtas-labs/issue/LIG-136/epic-offline-learning-and-synchronization) | SF-05, SF-12, SF-15 |
| Submission review and feedback | [LIG-137](https://linear.app/ligtas-labs/issue/LIG-137/epic-submission-review-and-teacher-feedback) | SF-13–15 |

### Account acceptance

Validate the agreed environment-specific Neon credential and resolve its subject to the existing stable Safety First UUID. An authenticated but unprovisioned identity must receive an explicit onboarding state. Eligible student activation must be idempotent under retries and concurrent requests. Operational teacher provisioning and role enforcement stay server-owned. Verify collisions, role escalation, token rejection, and identity consistency across clients against disposable PostgreSQL.

All three components use the same environment-specific Neon Auth identity. A class code requests membership after account activation. Student eligibility, email verification and cross-device sign-out remain open. Test with synthetic student and teacher accounts in a shared non-production environment, with the API available before client smoke tests.

### Planning acceptance details

Serve reviewed module objectives and prerequisites. Validate assignment updates, deadlines, archive behavior, approved enrollment and membership revocation at the API. Expose class completion and commonly missed requirements from eligible submissions. Restrict comment edits and removal to the author with current access. Preserve linked historical records after archiving.

SF-14 covers comment authoring and ownership. Apply the same access boundary to comment reads as to the associated submission.

## Cross-app workflow

Mobile pins the content and rubric versions and saves an attempt ID, attempt kind, relevant parent attempt ID, and ordered evidence locally before upload. On reconnection it retries pending evidence under the same learner and client attempt ID. API validates ownership, content compatibility and evidence, persists results, and returns the existing confirmation on a matching repeat. A changed payload for the same ID produces a conflict. Mobile holds on timeout, pauses for renewed authentication, and retains rejected or discrepant evidence with its reason. Only server acknowledgment establishes confirmed credit.

Synchronization and classroom submission are distinct. Confirmed practice remains private. An explicit submission identifies the assignment and a confirmed attempt. API checks learner ownership, current enrollment and assignment eligibility before linking it. Web shows the result to the authorized teacher after the submission succeeds and the view refreshes. Teacher comments are separate records. The study protocol controls access to research instruments and outcomes separately.

This is the proposed behavior from Chapter 4 §§4.5.4–7 and 4.7.1–4. Endpoint names, payload shape and canonical comparison, credential validation and client handoff, retry timing, offline session expiry, deadline/resubmission policies, and archive/recovery semantics require a shared decision.

## Data and access

The development baseline defines a PostgreSQL schema and migrations. See [technical design](DESIGN.md#initial-database-schema) and [migration instructions](migrations.md). Planned feature workflows use schema groups for identity and role, class/enrollment, assignment/submission, versioned content/rubric, ordered attempt events, assessment and criterion results, and teacher comments. Only the API accesses PostgreSQL. Use HTTPS at the proxy, environment-scoped credentials, an exact browser-origin CORS policy before connecting web, and access checks for every protected resource.

## Verification evidence

Capture status codes and redacted OpenAPI examples, synthetic record counts, versioned rubric fixture outcomes, and cross-class and retry traces. Confirm trusted proxy/CORS behavior and recovery on a disposable database. Deployment design is Ubuntu VPS with Caddy and Docker images for API/web, and Neon PostgreSQL.

## Study evidence and open decisions

Use synthetic accounts and reviewed fixture data for development. Paper 33 §§3.2 and 3.10–11 describe content/functional verification, a rubric-based pilot, pretest, post-test and usability assessment. These are distinct from the needs survey and ordinary classroom scoring. Study participation is voluntary with informed consent and confidential withdrawal handling. Coded individual study records are limited to the research team and adviser, with aggregate reporting. The paper's August 2027 disposal date concerns the specified survey/interview materials. Confirm the applicable retention and disposal rules for platform and pilot records before collection.

Confirm approved pilot instruments, eligibility and participant counts, consent and withdrawal handling, linkage/retention, reviewer authorization, and the current ethics/panel/adviser approval records. Leave evaluation outcomes open until measured. Confirm reviewed criterion applicability, essential items, accepted alternatives, guidance rules, weights or rounding, and golden fixtures before releasing assessment logic.
