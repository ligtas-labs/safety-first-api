using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SafetyFirst.Api.Features.Health;
using Xunit;

namespace SafetyFirst.Api.Tests;

public sealed class HealthControllerTests
{
    [Fact]
    public async Task Get_ReturnsServiceUnavailable_WhenDatabaseIsNotConfigured()
    {
        var configuration = new ConfigurationBuilder().Build();
        var controller = new HealthController(configuration, new SuccessfulProbe());

        var result = await controller.Get(CancellationToken.None);

        var status = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status503ServiceUnavailable, status.StatusCode);
    }

    [Fact]
    public async Task Get_ReturnsOk_WhenDatabaseConnectionSucceeds()
    {
        const string connectionString = "Host=readiness.test;Database=synthetic";
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = connectionString
            })
            .Build();
        var probe = new SuccessfulProbe();
        var controller = new HealthController(configuration, probe);

        var result = await controller.Get(CancellationToken.None);

        Assert.IsType<OkResult>(result);
        Assert.Equal(connectionString, probe.ConnectionString);
    }

    private sealed class SuccessfulProbe : IPostgresReadinessProbe
    {
        public string? ConnectionString { get; private set; }

        public Task OpenAsync(string connectionString, CancellationToken cancellationToken)
        {
            ConnectionString = connectionString;
            return Task.CompletedTask;
        }
    }
}
