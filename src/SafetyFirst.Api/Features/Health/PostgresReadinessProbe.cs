using Npgsql;

namespace SafetyFirst.Api.Features.Health;

public interface IPostgresReadinessProbe
{
    Task OpenAsync(string connectionString, CancellationToken cancellationToken);
}

internal sealed class PostgresReadinessProbe : IPostgresReadinessProbe
{
    public async Task OpenAsync(string connectionString, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
    }
}
