# Safety First API technical design

## Implemented foundation and feature layout

`Program.cs` owns service registration and controller mapping. `Features/Health/HealthController.cs` is the readiness endpoint. Add controller, request/response models and workflow-specific services together in `Features/<Feature>/` when that workflow is implemented. Candidate features are Identity, Content, Attempts, Classes, Assignments and Feedback. The shared EF Core context lives under `Data/`, and migration files live under `Migrations/`. See the [migration workflow](migrations.md).

## Planned integration

Use ASP.NET Core controllers and proposed EF Core persistence on Neon. Validate actor, membership and pinned versions before an evidence transaction. Compare a repeated attempt payload against the accepted evidence and keep teacher comments separate from results.

See the [PRD](PRD.md) for stable requirement IDs, the proposed cross-app workflow and unresolved contracts. Add planned folders with their first implementation.

## Verification

Keep controllers and workflow-specific types under their feature. Validate route, authorized actor, denied actor and synthetic persistence behavior. Keep Program.cs focused on startup and registrations. See the [README](../README.md) for current commands and the [PRD](PRD.md) for acceptance checks.

## Initial database schema

[`CreateInitialSchema`](migrations.md) implements the [one-school ERD](https://lucid.app/lucidchart/c1f70c64-9531-4c95-94ec-4a30783af7f8/edit?page=NhkHYIGS2bJr).

| Records | Relationships and constraints |
| --- | --- |
| Users, classes, enrollments | App UUIDs own application records. A nullable, unique Neon Auth subject links provisioned accounts. Email is unique. Each class has a teacher and each student has at most one enrollment per class. |
| Modules, scenario versions, hazard entries, scenario hazards, rubric versions, rubric criteria | Scenario versions pin a rubric version. Module codes, module/variant/version combinations and hazard topic/version combinations are unique. A scenario links to a hazard at most once. |
| Attempts, attempt events, assessment results, criterion results | Each attempt belongs to a learner and scenario version. The learner/client-attempt pair uniquely identifies uploads and event sequence numbers are unique within an attempt. A composite foreign key requires any retry parent to belong to the same learner. Each attempt can have one assessment and each assessment can have one result per criterion. |
| Assignments, submissions, teacher feedback | An assignment pins a scenario version. Each student can submit once per assignment. A composite foreign key requires the submission student to own the attempt. Feedback references the submission and its teacher author. |

Foreign keys restrict deletion of referenced records. The API must verify account roles, teacher ownership, approved enrollment, publication status, assignment and attempt version compatibility, confirmed grading and criterion membership in the pinned rubric. An offline attempt against the assignment's pinned version may still qualify when a newer version is published. Teachers see attempts through explicit submissions in their own classes. The sync service must compare retried payloads and persist accepted evidence and assessment atomically. Neon Auth provisioning and sign-in belong to the auth issue.
