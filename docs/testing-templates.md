# xUnit starter templates

Copy the relevant example into a new `.cs` file under `tests/SafetyFirst.Api.Tests`. Name the test after the behavior and the condition that triggers it. Run it from the repository root with `dotnet test SafetyFirst.slnx`.

## Controller unit test

Use this pattern to test a controller result with synthetic configuration and a fake dependency. This example checks the readiness result when no connection string is provided.

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SafetyFirst.Api.Features.Health;
using Xunit;

namespace SafetyFirst.Api.Tests;

public sealed class ReadinessUnitExample
{
    [Fact]
    public async Task Get_Returns503_WhenDatabaseIsNotConfigured()
    {
        var config = new ConfigurationBuilder().Build();
        var controller = new HealthController(config, new UnexpectedDatabaseProbe());

        var result = await controller.Get(CancellationToken.None);

        Assert.Equal(503, Assert.IsType<StatusCodeResult>(result).StatusCode);
    }

    private sealed class UnexpectedDatabaseProbe : IPostgresReadinessProbe
    {
        public Task OpenAsync(string connectionString, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("The controller should return before opening a connection.");
    }
}
```

For the success case, supply a synthetic connection string with `AddInMemoryCollection`, return `Task.CompletedTask` from the fake probe, and assert `OkResult`. The existing [health controller tests](../tests/SafetyFirst.Api.Tests/HealthControllerTests.cs) show both cases.

## HTTP integration test

Use this pattern to test a real request against two explicitly started test API instances. The first runs without database configuration. The second connects to disposable PostgreSQL. The example is opt-in so the ordinary test suite can run without Docker.

```csharp
using System.Net;
using Xunit;

namespace SafetyFirst.Api.Tests;

public sealed class ReadinessHttpExample
{
    [LocalReadinessTheory]
    [InlineData(15299, HttpStatusCode.ServiceUnavailable)]
    [InlineData(15300, HttpStatusCode.OK)]
    public async Task Get_ReturnsExpectedStatus(int port, HttpStatusCode expectedStatus)
    {
        using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
        using var response = await client.GetAsync($"http://127.0.0.1:{port}/health/ready");

        Assert.Equal(expectedStatus, response.StatusCode);
    }
}

internal sealed class LocalReadinessTheoryAttribute : TheoryAttribute
{
    public LocalReadinessTheoryAttribute()
    {
        if (Environment.GetEnvironmentVariable("SAFETYFIRST_RUN_READINESS_HTTP_TESTS") != "1")
        {
            Skip = "Start the disposable test containers and opt in to the HTTP tests.";
        }
    }
}
```

With Docker running in Linux container mode and ports 15299 and 15300 free, run the following from the repository root in PowerShell. The API image excludes local `.env` files. The first container receives no database settings, and the second receives only the synthetic credentials below.

```powershell
docker build -t safety-first-api:readiness-test .
docker network create safetyfirst-readiness-test
docker run --rm -d --name safetyfirst-readiness-db --network safetyfirst-readiness-test -e POSTGRES_DB=safetyfirst_test -e POSTGRES_USER=safetyfirst_test -e POSTGRES_PASSWORD=safetyfirst_test postgres:16
docker run --rm -d --name safetyfirst-readiness-no-db --network safetyfirst-readiness-test -p 127.0.0.1:15299:8080 safety-first-api:readiness-test
docker run --rm -d --name safetyfirst-readiness-with-db --network safetyfirst-readiness-test -p 127.0.0.1:15300:8080 -e 'ConnectionStrings__DefaultConnection=Host=safetyfirst-readiness-db;Database=safetyfirst_test;Username=safetyfirst_test;Password=safetyfirst_test' safety-first-api:readiness-test
```

Wait for `docker exec safetyfirst-readiness-db pg_isready -h 127.0.0.1 -U safetyfirst_test -d safetyfirst_test` to report that it is accepting connections. Then run the example.

```powershell
$env:SAFETYFIRST_RUN_READINESS_HTTP_TESTS = '1'
dotnet test SafetyFirst.slnx --filter FullyQualifiedName~ReadinessHttpExample
Remove-Item Env:SAFETYFIRST_RUN_READINESS_HTTP_TESTS
```

Stop the containers and remove the test network when finished. PostgreSQL uses the disposable container's storage. For tests that write rows, seed synthetic data and reset that data between test cases.

```powershell
docker stop safetyfirst-readiness-with-db safetyfirst-readiness-no-db
docker stop safetyfirst-readiness-db
docker network rm safetyfirst-readiness-test
```
