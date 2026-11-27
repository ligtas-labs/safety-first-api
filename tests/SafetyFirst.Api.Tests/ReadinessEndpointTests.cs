using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SafetyFirst.Api.Features.Health;
using Xunit;

namespace SafetyFirst.Api.Tests;

public sealed class ReadinessEndpointTests
{
    [Fact]
    public Task Get_Returns503_WithoutConfiguredDatabase() =>
        AssertStatusAsync(null, HttpStatusCode.ServiceUnavailable);

    [LocalPostgresFact]
    public Task Get_Returns200_WithReachablePostgres()
    {
        var value = Environment.GetEnvironmentVariable(LocalPostgresFactAttribute.PortVariable);
        Assert.True(ushort.TryParse(value, out var port) && port > 0,
            "SAFETYFIRST_TEST_POSTGRES_PORT must be a valid local port.");

        var connectionString = new NpgsqlConnectionStringBuilder
        {
            Host = "127.0.0.1",
            Port = port,
            Database = "safetyfirst_test",
            Username = "safetyfirst_test",
            Password = "safetyfirst_test",
            SslMode = SslMode.Disable,
            Pooling = false,
            Timeout = 5
        }.ConnectionString;

        return AssertStatusAsync(connectionString, HttpStatusCode.OK);
    }

    private static async Task AssertStatusAsync(string? connectionString, HttpStatusCode expectedStatus)
    {
        var builder = WebApplication.CreateEmptyBuilder(new WebApplicationOptions
        {
            ApplicationName = typeof(HealthController).Assembly.GetName().Name!,
            EnvironmentName = "Testing"
        });
        builder.Configuration.Sources.Clear();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["ConnectionStrings:DefaultConnection"] = connectionString
        });
        builder.WebHost.UseTestServer();

        await using var app = Program.BuildApplication(builder);
        await app.StartAsync();
        using var client = app.GetTestClient();
        using var response = await client.GetAsync("https://localhost/health/ready");

        Assert.Equal(expectedStatus, response.StatusCode);
    }
}

internal sealed class LocalPostgresFactAttribute : FactAttribute
{
    public const string PortVariable = "SAFETYFIRST_TEST_POSTGRES_PORT";

    public LocalPostgresFactAttribute()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(PortVariable)))
        {
            Skip = $"Set {PortVariable} to the port of a disposable local PostgreSQL instance.";
        }
    }
}
