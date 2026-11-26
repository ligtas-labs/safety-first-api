using System.Text.Json;

namespace SafetyFirst.Api.Features.Content;

public sealed class RubricVersion
{
    public Guid Id { get; set; }
    public required string VersionLabel { get; set; }
    public JsonElement PassingRules { get; set; }
    public JsonElement ReviewRecord { get; set; }
    public DateTimeOffset? PublishedAt { get; set; }
}
