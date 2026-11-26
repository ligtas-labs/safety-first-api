using System.Text.Json;

namespace SafetyFirst.Api.Features.Attempts;

public sealed class AttemptEvent
{
    public Guid Id { get; set; }
    public Guid AttemptId { get; set; }
    public int SequenceNo { get; set; }
    public required string EventType { get; set; }
    public JsonElement Payload { get; set; }
    public long ElapsedMs { get; set; }
}
