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
        var controller = new HealthController(configuration);

        var result = await controller.Get(CancellationToken.None);

        var status = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status503ServiceUnavailable, status.StatusCode);
    }
}
