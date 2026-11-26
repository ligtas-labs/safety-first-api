# Safety First API technical design

## Implemented foundation and feature layout

`Program.cs` owns service registration and controller mapping. `Features/Health/HealthController.cs` is the readiness endpoint. Add controller, request/response models and workflow-specific services together in `Features/<Feature>/` when that workflow is implemented. Candidate features are Identity, Content, Attempts, Classes, Assignments and Feedback. The shared EF Core context lives under `Data/`, and migration files live under `Migrations/`. See the [migration workflow](migrations.md).

## Planned integration

Use ASP.NET Core controllers and proposed EF Core persistence on Neon. Validate actor, membership and pinned versions before an evidence transaction. Compare a repeated attempt payload against the accepted evidence and keep teacher comments separate from results.

See the [PRD](PRD.md) for stable requirement IDs, the proposed cross-app workflow and unresolved contracts. Add planned folders with their first implementation.

## Verification

Keep controllers and workflow-specific types under their feature. Validate route, authorized actor, denied actor and synthetic persistence behavior. Keep Program.cs focused on startup and registrations. See the [README](../README.md) for current commands and the [PRD](PRD.md) for acceptance checks.
