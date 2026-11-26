<div align="center">

# Safety First API

[![ASP.NET Core 10](https://img.shields.io/badge/ASP.NET_Core-10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet)
[![PostgreSQL Neon](https://img.shields.io/badge/PostgreSQL-Neon-4169E1?style=for-the-badge&logo=postgresql&logoColor=white)](https://neon.com/)
[![Docker image](https://img.shields.io/badge/Docker-image-2496ED?style=for-the-badge&logo=docker&logoColor=white)](https://www.docker.com/)
[![pnpm 10.25](https://img.shields.io/badge/pnpm-10.25-F69220?style=for-the-badge&logo=pnpm&logoColor=white)](https://pnpm.io/)

</div>

[About](#about) · [Local development](#local-development) · [Configuration](#configuration) · [Testing](#testing) · [Repository layout](#repository-layout) · [Deployment](#deployment) · [Contributing](#contributing)

## About

Safety First API is the ASP.NET Core backend for the web and Android apps. The current foundation exposes PostgreSQL readiness and development API documentation. Authentication, classrooms and assessment persistence are planned.

| Area | Stack |
| --- | --- |
| API | .NET 10, ASP.NET Core controllers and C# |
| API documentation | ASP.NET Core OpenAPI and Scalar |
| Readiness database | PostgreSQL (Neon), accessed via Npgsql |
| Delivery and Git hooks | Docker, GitHub Actions, pnpm, Husky and Commitlint |

## Local development

Install the [.NET 10 SDK](https://dotnet.microsoft.com/download), [Node.js 24](https://nodejs.org/) and [Git](https://git-scm.com/). Install pnpm for Git hooks with `npm install --global pnpm@10.25.0`.

```sh
git clone https://github.com/ligtas-labs/safety-first-api.git
cd safety-first-api
pnpm install --frozen-lockfile
dotnet restore SafetyFirst.slnx --locked-mode
dotnet build SafetyFirst.slnx
dotnet run --project src/SafetyFirst.Api --launch-profile http
```

| Local endpoint | Purpose |
| --- | --- |
| `http://localhost:5299/scalar/v1` | Interactive API reference in Development |
| `http://localhost:5299/openapi/v1.json` | OpenAPI document in Development |
| `http://localhost:5299/health/ready` | Database readiness probe |

## Configuration

| Setting | Purpose |
| --- | --- |
| `ConnectionStrings__DefaultConnection` | Set this process environment variable to a disposable PostgreSQL connection string for readiness checks. |
| `DATABASE_URL` | Pooled Neon URL for the API and readiness check when `ConnectionStrings__DefaultConnection` is unset. |
| `DATABASE_URL_UNPOOLED` | Direct Neon URL for EF Core design-time commands and migrations. |

Keep connection values out of source and logs. Use separate database credentials per environment. The HTTP launch profile sets the Development environment.

To link a local checkout, run `neon link --no-config` and select the intended project and branch. For manual setup, copy [`.env.example`](.env.example) to `.env.local` and fill in both URLs from Neon Console. The API reads the ignored `.env.local` or `.env` file. From the repository root, use `dotnet run --project src/SafetyFirst.Api --no-launch-profile -- --postgres-version` to verify the configured runtime connection. Run `dotnet tool restore` before using `dotnet ef`.

For schema changes, see the [EF Core migrations guide](docs/migrations.md).

## Testing

Run the xUnit suite from the repository root.

```sh
dotnet test SafetyFirst.slnx
```

To verify a Release build, run:

```sh
dotnet build SafetyFirst.slnx -c Release
dotnet test SafetyFirst.slnx -c Release --no-build
```

### Watch the API and tests

Open two terminals in the repository root. In the first terminal, start the API with hot reload.

```sh
dotnet watch --project src/SafetyFirst.Api/SafetyFirst.Api.csproj run --launch-profile http
```

In the second terminal, start the test watcher to rerun the xUnit suite when code changes.

```sh
dotnet watch --project tests/SafetyFirst.Api.Tests/SafetyFirst.Api.Tests.csproj test
```

The readiness unit tests call the controller directly and run without an API server, PostgreSQL, or credentials. Press `Ctrl+C` in either terminal to stop its watcher.

### Check the running API

With the API running on the `http` launch profile, check readiness from another terminal.

```powershell
curl.exe -i http://localhost:5299/health/ready
```

Expect HTTP 503 when the database is unconfigured or unreachable and HTTP 200 when the configured test PostgreSQL database is reachable. For new behavior, follow [the test-first workflow](CONTRIBUTING.md#test-driven-development).

## Repository layout

| Path | Purpose |
| --- | --- |
| `SafetyFirst.slnx` | .NET solution |
| `src/SafetyFirst.Api/Program.cs` | Service registration and routing |
| `src/SafetyFirst.Api/Features/Health/` | PostgreSQL readiness endpoint |
| `src/SafetyFirst.Api/Properties/launchSettings.json` | Local launch profiles |
| `deploy/` and `.github/workflows/` | Compose, Caddy, release script and CI |
| `docs/` | Product requirements, design and deployment runbook |

The solution currently has one API project organized by feature. See the [maintainable .NET solution structure reference](https://dev.to/mashrulhaque/how-to-design-a-maintainable-net-solution-structure-for-growing-teams-284n) for dependency direction as the solution grows.

## Deployment

Build the image locally with `docker build -t safety-first-api:local .`. The API and web run as Docker images on the Ubuntu VPS behind Caddy. The API uses Neon PostgreSQL. See the [deployment runbook](docs/deployment.md) for configuration, release verification and rollback.

## Contributing

Read [CONTRIBUTING.md](CONTRIBUTING.md) for the Linear issue workflow, testing expectations and PR checklist. For agent-assisted work, use [AGENTS.md](AGENTS.md) and the [optional skills instructions](CONTRIBUTING.md#optional-agent-skills).

## Project links

[Web](https://github.com/ligtas-labs/safety-first-web) · [API](https://github.com/ligtas-labs/safety-first-api) · [Mobile](https://github.com/ligtas-labs/safety-first-mobile) · [Linear handbook](https://linear.app/ligtas-labs/document/developer-handbook-api-2efbc528f877)

[Product requirements](docs/PRD.md) · [Technical design](docs/DESIGN.md) · [Contributing](CONTRIBUTING.md) · [Agent instructions](AGENTS.md)

## Deployed API reference

Scalar and OpenAPI are enabled automatically in local Development. In
Production they are disabled by default. To publish both endpoints, install
the updated `deploy/compose.yaml` and set `API_DOCS_ENABLED=true` in
`/opt/safety-first/.env`. Release an API image that supports
`ApiDocs:Enabled`, then install, validate, and reload the updated Caddy
configuration.

Verify HTTP 200 from both endpoints after deployment.

- [Scalar API reference](https://safety-first-api.michaelconstantino.dev/scalar/v1)
- [OpenAPI document](https://safety-first-api.michaelconstantino.dev/openapi/v1.json)

To disable the endpoints, set `API_DOCS_ENABLED=false` and recreate the API
container.
