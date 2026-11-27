using System.Text.Json;

namespace SafetyFirst.Api.Features.Content;

public sealed class HazardEntry
{
    public Guid Id { get; set; }
    public required string TopicCode { get; set; }
    public int VersionNo { get; set; }
    public required string Title { get; set; }
    public required string Category { get; set; }
    public JsonElement Content { get; set; }
    public JsonElement SourceReferences { get; set; }
    public JsonElement ReviewRecord { get; set; }
}
