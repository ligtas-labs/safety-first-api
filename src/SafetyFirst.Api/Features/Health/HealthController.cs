using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace SafetyFirst.Api.Features.Health;

[ApiController]
public sealed class HealthController(IConfiguration config, IPostgresReadinessProbe probe) : ControllerBase
{
    [HttpGet("/health/ready")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var connectionString = config.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString)) return StatusCode(503);
        try
        {
            await probe.OpenAsync(connectionString, cancellationToken);
            return Ok();
        }
        catch (NpgsqlException) { return StatusCode(503); }
    }
}
