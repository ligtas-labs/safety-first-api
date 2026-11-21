using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace SafetyFirst.Api.Controllers;

[ApiController]
public sealed class ReadinessController(IConfiguration config) : ControllerBase
{
    [HttpGet("/health/ready")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var connectionString = config.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString)) return StatusCode(503);
        try
        {
            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync(cancellationToken);
            return Ok();
        }
        catch (NpgsqlException) { return StatusCode(503); }
    }
}
