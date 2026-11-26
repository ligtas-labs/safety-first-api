using DotNetEnv;
using Npgsql;

namespace SafetyFirst.Api.Data;

internal static class NeonConnection
{
    public static string? Read(string variable)
    {
        Env.NoClobber().TraversePath().LoadMulti([".env.local", ".env"]);
        var value = Environment.GetEnvironmentVariable(variable);
        if (string.IsNullOrWhiteSpace(value)) return null;

        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri) ||
            (uri.Scheme != "postgresql" && uri.Scheme != "postgres") ||
            string.IsNullOrEmpty(uri.Host))
        {
            throw new InvalidOperationException($"{variable} must be a PostgreSQL URL.");
        }

        var credentials = uri.UserInfo.Split(':', 2);
        if (credentials.Length != 2 || credentials[0].Length == 0 || uri.AbsolutePath.Length < 2)
        {
            throw new InvalidOperationException($"{variable} must contain credentials and a database name.");
        }

        return new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.Port == -1 ? 5432 : uri.Port,
            Database = Uri.UnescapeDataString(uri.AbsolutePath[1..]),
            Username = Uri.UnescapeDataString(credentials[0]),
            Password = Uri.UnescapeDataString(credentials[1]),
            SslMode = SslMode.VerifyFull,
            ChannelBinding = ChannelBinding.Require,
        }.ConnectionString;
    }
}
