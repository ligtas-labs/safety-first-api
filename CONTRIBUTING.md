# Contributing to Safety First API

Start with the [README](README.md) for setup and supported commands. Use the [PRD](docs/PRD.md) and [technical design](docs/DESIGN.md) for requirements and design decisions.

## Project Structure

```text
safety-first-api/
├── .github/                              # GitHub automation and PR templates
│   └── workflows/                        # CI and deployment workflows
│       ├── image.yml                     # Builds and publishes the API image
│       └── release.yml                   # Runs a manually triggered VPS release
├── deploy/                               # VPS deployment files
│   ├── Caddyfile                         # Reverse proxy configuration
│   ├── compose.yaml                      # Web and API service definitions
│   └── release.sh                        # Deploys the selected images to the VPS
├── docs/                                 # Requirements, design, and development guides
│   └── deployment.md                     # Release, verification, and rollback runbook
├── src/                                  # Application source
│   └── SafetyFirst.Api/                  # ASP.NET Core API project
│       ├── Features/                     # Feature-owned endpoints and related code
│       │   └── Health/                   # Database readiness feature
│       │       └── HealthController.cs   # Handles GET /health/ready
│       ├── Properties/                   # Local run settings
│       │   └── launchSettings.json       # Development HTTP and HTTPS launch profiles
│       ├── Program.cs                    # API entry point and service registration
│       ├── SafetyFirst.Api.csproj        # Target framework and NuGet dependencies
│       ├── SafetyFirst.Api.http          # Example HTTP requests
│       ├── appsettings.json              # Default application configuration
│       ├── appsettings.Development.json  # Development configuration overrides
│       └── packages.lock.json            # Locked NuGet dependency versions
├── Dockerfile                            # API image build
└── SafetyFirst.slnx                      # .NET solution entry point
```

## Ticket and branch workflow

1. Pick up the Linear issue and confirm ownership, affected repos, dependencies and acceptance criteria. Use stable PRD requirement IDs where they apply.
2. Create new ticket branches from an updated `development` using `<type>/<lowercase-linear-id>-<short-slug>`. Use `feature/`, `fix/`, `refactor/`, `setup/` or `docs/`. Continue work on an existing ticket branch when one is assigned. Use a descriptive branch name for unticketed work.
3. Stage one logical change, inspect `git diff --cached`, and use Conventional Commits. Example: `docs(contributing): explain the test-first workflow`.
4. Open ticket PRs into `development`. Use a descriptive title, follow the PR template, and include the Linear ID in the branch name and the Linear Issue section. For cross-repo work, identify dependencies and merge order. Request teammate review and wait for applicable CI.
5. Verify the integrated result on `development` and record evidence in Linear before closing the ticket. For a sprint release, open a `development` → `main` PR, use a merge commit, and include applicable artifact versions, migration and rollback steps. Tag the accepted release. Merge urgent changes on `main` back into `development`.

## Test-driven development

1. **Red**. Turn one acceptance criterion into a test at the public interface. Run it and confirm the expected failure. For a bug, reproduce the existing behavior first.
2. **Green**. Make the smallest implementation that passes. Run the same test again.
3. **Refactor**. Clean up with the test green, then run the relevant suite and [repository checks](README.md#testing). Repeat for the next criterion.

For new API behavior, start with a request and expected status, response contract and side effect. Cover allowed and denied actors for protected routes and use an isolated test PostgreSQL instance for persistence and retry checks. The current solution has no automated test project. Add the relevant test project to `SafetyFirst.slnx` with the first behavior that needs it and report the number of tests executed. See the [readiness smoke check](README.md#testing) for the current foundation. For docs, visual or device checks, record the environment, steps, expected result and observed result. Use synthetic fixtures. Record failures and any checks still pending.

## Code organization

Keep a controller, request and response models, and workflow-specific services together under `src/SafetyFirst.Api/Features/<Feature>/`. Keep `Program.cs` focused on startup and dependency registration. Shared database configuration lives under `Data/`. EF Core stores migration files under `Migrations/`. See the [migration workflow](docs/migrations.md) for review and deployment steps.

Use PascalCase for C# types and methods, camelCase for locals and parameters, and the `Async` suffix for async methods. Keep business rules independent of HTTP and storage details. The solution currently contains one project. If separate Domain, Application or Infrastructure projects become necessary, keep dependencies pointing inward and justify the split in the implementing change.

Reference: [Maintainable .NET solution structure](https://dev.to/mashrulhaque/how-to-design-a-maintainable-net-solution-structure-for-growing-teams-284n).

## Pull request checklist

- [ ] Summarize the observed behavior and applicable requirement IDs.
- [ ] Record checks run, observed results and any remaining verification. Include screenshots, device details or migration evidence where relevant.
- [ ] Update affected setup instructions, contracts or configuration examples.
- [ ] Keep credentials, participant data and generated files out of the diff and evidence.

## Optional agent skills

Use [AGENTS.md](AGENTS.md) for repository instructions. The team bundle is optional for local agent-assisted work. List the available skills, install a selected one in this checkout, then start a new agent session and invoke it by name.

```sh
npx skills add MJayConstantino/skills --list
npx skills add MJayConstantino/skills --skill verification-before-completion --agent codex claude-code
```

Review the skill before use. Keep participant records and secrets out of agent prompts. Review agent changes and run the relevant checks before submitting a PR.
