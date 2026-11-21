# Safety First API

ASP.NET Core 10 controllers provide the shared backend. The current build exposes PostgreSQL readiness. Authentication, classrooms and assessment persistence are planned.

## Local development

Install the .NET 10 SDK, Node.js 24 and pnpm 10.25.0. From the repository root:

```sh
pnpm install --frozen-lockfile
dotnet restore SafetyFirst.slnx
dotnet build SafetyFirst.slnx
dotnet run --project src/SafetyFirst.Api --launch-profile http
```

Open `http://localhost:5299/scalar/v1` or `http://localhost:5299/openapi/v1.json` in Development. Request `GET http://localhost:5299/health/ready` to check the configured database. Expect 503 without a connection and 200 when the configured PostgreSQL connection opens. Set `ConnectionStrings__DefaultConnection` via your local secret store or process environment using a disposable Neon connection. Keep the value out of source and logs.

For the CI-equivalent .NET checks, run `dotnet build SafetyFirst.slnx -c Release` and `dotnet test SafetyFirst.slnx -c Release --no-build`. The foundation has no test projects. For image packaging use `docker build -t safety-first-api:local .`.

## Repository layout

```text
src/SafetyFirst.Api/
  Program.cs                      # service registration and routing
  Features/Health/                # HealthController, GET /health/ready
  Properties/launchSettings.json
deploy/                           # Compose, Caddy and release script
docs/                             # PRD, design and deployment runbook
.github/workflows/                # image and release workflows
```

## Deployment

API and web run as Docker images on the Ubuntu VPS behind Caddy. Neon PostgreSQL is the API database. See the [deployment runbook](docs/deployment.md).

[Product requirements](docs/PRD.md) · [Technical design](docs/DESIGN.md) · [Contributing](CONTRIBUTING.md) · [Agent instructions](AGENTS.md)
