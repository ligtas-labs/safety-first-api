using Microsoft.EntityFrameworkCore;

namespace SafetyFirst.Api.Data;

public sealed class SafetyFirstDbContext(DbContextOptions<SafetyFirstDbContext> options)
    : DbContext(options);
