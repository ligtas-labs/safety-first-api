using Microsoft.EntityFrameworkCore;
using SafetyFirst.Api.Data;
using Scalar.AspNetCore;

var printPostgresVersion = args is ["--postgres-version"];
var builder = WebApplication.CreateBuilder(printPostgresVersion ? [] : args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    connectionString = NeonConnection.Read("DATABASE_URL");
    builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
}

builder.Services.AddDbContext<SafetyFirstDbContext>(options => options.UseNpgsql(connectionString));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

if (printPostgresVersion)
{
    await using var host = app;
    await using var scope = app.Services.CreateAsyncScope();
    var db = scope.ServiceProvider.GetRequiredService<SafetyFirstDbContext>();
    var version = await db.Database.SqlQueryRaw<string>("SELECT version() AS \"Value\"").SingleAsync();
    Console.WriteLine($"Postgres version: {version}");
    return;
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("ApiDocs:Enabled"))
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
