using System.Text.Json;

namespace SafetyFirst.Api.Features.Assignments;

public sealed class Assignment
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public Guid ScenarioVersionId { get; set; }
    public required string Title { get; set; }
    public DateTimeOffset? DueAt { get; set; }
    public JsonElement RetryPolicy { get; set; }
}
