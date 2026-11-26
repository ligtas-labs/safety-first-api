# EF Core migrations

Safety First uses the EF Core model as the source of truth for its application schema. EF compares the current model with the checked-in model snapshot when generating a migration. PostgreSQL tracks applied migrations in `__EFMigrationsHistory`. The initial migration has an empty `Up()` and establishes a baseline without creating application tables. [EF Core migration files](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing)

## Generate and review a migration

Run the following from `src/SafetyFirst.Api` after adding or changing a mapped entity. Replace `AddClassrooms` with a name that describes the schema change.

```powershell
dotnet tool restore
dotnet ef migrations add AddClassrooms
```

The design-time factory requires `DATABASE_URL_UNPOOLED`. It loads the direct Neon URL from the process environment, then falls back to the ignored `.env.local` or `.env` file. Generating a migration writes local files without applying SQL to Neon.

| File in `Migrations/` | What to review |
| --- | --- |
| `<timestamp>_AddClassrooms.cs` | Start here. `Up()` lists the operations that apply the change. `Down()` lists the operations EF uses if the migration is rolled back. |
| `<timestamp>_AddClassrooms.Designer.cs` | Generated metadata that describes the model at this migration. |
| `SafetyFirstDbContextModelSnapshot.cs` | Generated schema model that EF compares against the next time you add a migration. EF updates this existing file. |

Generate one migration per coherent schema change or PR. Review the entity and mapping edits first, then the migration's `Up()` and `Down()`. Check for unexpected drops, renames represented as drop-and-add, and data backfills for new required columns. Commit the migration and its designer file together with the updated snapshot and model changes. If you need to regenerate an unapplied, unshared migration, run `dotnet ef migrations remove`, fix the model, and generate it again. Once a migration has been applied to a shared database, make further changes in a new migration.

To see exactly what SQL a single migration generates, name its predecessor and the new migration. Omit `--output` to print the SQL or write it to `obj/` to review a local copy.

```powershell
dotnet ef migrations script InitialCreate AddClassrooms --output obj/AddClassrooms.sql
```

For LIG-115, generate the SQL from the repository root and verify the expected tables, constraints, and additive operations.

```powershell
dotnet ef migrations script InitialCreate CreateInitialSchema --project src/SafetyFirst.Api --output obj/CreateInitialSchema.sql
./scripts/Test-InitialSchema.ps1 -SqlPath obj/CreateInitialSchema.sql
```

On an isolated Neon branch, run `dotnet ef database update CreateInitialSchema` against a database at `InitialCreate` and against a separate empty database. Run `scripts/Test-InitialSchema.sql` with a PostgreSQL client on each migrated database. The fixture rolls back its synthetic records and checks the pinned scenario, grade and feedback read, cross-owner submission rejection, duplicate submission rejection, and attempt retry uniqueness.

## Push schema changes to Neon

Point `DATABASE_URL_UNPOOLED` at the direct endpoint for the intended Neon branch and database. A process-level value takes precedence over the local files, so check which configuration you are using before updating. Apply the migration on the Neon `development` branch first.

```powershell
dotnet ef migrations list
dotnet ef database update
```

`database update` applies pending migrations and records their IDs. For a reviewed release, you can generate an idempotent script with `dotnet ef migrations script --idempotent --output obj/release.sql`. It uses the target database's migration history to gate pending migrations. Review the SQL and follow the [deployment runbook](deployment.md) before applying it to production. Stop to review any proposed reset, drop, or data loss before proceeding. [Applying EF Core migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying)

## Pull the schema from Neon

Use EF Core reverse engineering when you need to see how existing tables map to C#. Use the direct endpoint for the intended branch and a read-only database role. Supply its Npgsql keyword-format connection string through the process setting `ConnectionStrings__SchemaInspection`. Replace `public.classrooms` with a table that already exists.

```powershell
dotnet ef dbcontext scaffold "Name=ConnectionStrings:SchemaInspection" Npgsql.EntityFrameworkCore.PostgreSQL --table public.classrooms --context SchemaInspectionContext --output-dir obj/SchemaInspection --no-onconfiguring
```

The command reads the schema and writes a scratch context and entity classes under the ignored `obj/` directory. Compare them with your existing mappings. Scaffolding does not update `SafetyFirstDbContext`, its snapshot, or migration history. If a shared database already has an out-of-band schema change, reconcile that change with the team before generating or applying a migration that would attempt to repeat it. [EF Core reverse engineering](https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/)
