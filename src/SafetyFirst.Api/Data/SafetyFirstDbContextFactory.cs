using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SafetyFirst.Api.Data;

public sealed class SafetyFirstDbContextFactory : IDesignTimeDbContextFactory<SafetyFirstDbContext>
{
    public SafetyFirstDbContext CreateDbContext(string[] args)
    {
        var connectionString = NeonConnection.Read("DATABASE_URL_UNPOOLED")
            ?? throw new InvalidOperationException("Set DATABASE_URL_UNPOOLED for EF Core migrations.");

        var options = new DbContextOptionsBuilder<SafetyFirstDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new SafetyFirstDbContext(options);
    }
}
