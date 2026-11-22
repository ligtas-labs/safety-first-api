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

Keep connection values out of source and logs. Use separate database credentials per environment. The HTTP launch profile sets the Development environment.

## Testing

```sh
dotnet build SafetyFirst.slnx -c Release
dotnet test SafetyFirst.slnx -c Release --no-build
```

The foundation has no automated test project. The test command currently discovers no tests. For a readiness smoke check, start the API and run `curl -i http://localhost:5299/health/ready`. Expect HTTP 503 with no configured connection, then restart with a reachable test PostgreSQL connection and expect HTTP 200. For new behavior, follow [the test-first workflow](CONTRIBUTING.md#test-driven-development).

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
