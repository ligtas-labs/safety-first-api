using Microsoft.EntityFrameworkCore;
using SafetyFirst.Api.Data;
using SafetyFirst.Api.Features.Health;
using Scalar.AspNetCore;

var printPostgresVersion = args is ["--postgres-version"];
var builder = WebApplication.CreateBuilder(printPostgresVersion ? [] : args);

if (string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("DefaultConnection")))
{
    builder.Configuration["ConnectionStrings:DefaultConnection"] = NeonConnection.Read("DATABASE_URL");
}

var app = BuildApplication(builder);

if (printPostgresVersion)
{
    await using var host = app;
    await using var scope = app.Services.CreateAsyncScope();
    var db = scope.ServiceProvider.GetRequiredService<SafetyFirstDbContext>();
    var version = await db.Database.SqlQueryRaw<string>("SELECT version() AS \"Value\"").SingleAsync();
    Console.WriteLine($"Postgres version: {version}");
    return;
}

app.Run();

public partial class Program
{
    public static WebApplication BuildApplication(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<SafetyFirstDbContext>(options => options.UseNpgsql(connectionString));
        builder.Services.AddSingleton<IPostgresReadinessProbe, PostgresReadinessProbe>();
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        var app = builder.Build();
        if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("ApiDocs:Enabled"))
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        return app;
    }
}
