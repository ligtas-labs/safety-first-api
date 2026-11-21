# Safety First API

Install .NET 10 SDK and pnpm 10.25.0 for commit hooks. From the root run `pnpm install --frozen-lockfile`, `dotnet restore SafetyFirst.slnx`, `dotnet build SafetyFirst.slnx`, then `dotnet run --project src/SafetyFirst.Api --launch-profile http`. Open http://localhost:5299/scalar/v1 for the interactive API reference and http://localhost:5299/openapi/v1.json for the schema. Both are currently mapped in Development. Update endpoint schemas, authorization expectations, errors and examples in the implementing PR. Run `dotnet test SafetyFirst.slnx` when test projects exist and report the executed test count. Production API origin: https://safety-first-api.michaelconstantino.dev. Add an explicit web-origin CORS policy and test browser preflight before connecting web. Use separate Neon databases or branches and credentials per environment.


## Product scope

Accept learner evidence and make authorized submissions available to teachers.

[Requirements](docs/PRD.md) · [Design](docs/DESIGN.md) · [Contributing](CONTRIBUTING.md) · [Agent instructions](AGENTS.md) · [Deployment](docs/deployment.md)
