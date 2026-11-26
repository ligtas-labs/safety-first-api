namespace SafetyFirst.Api.Features.Attempts;

public sealed class Attempt
{
    public Guid Id { get; set; }
    public Guid LearnerId { get; set; }
    public Guid ScenarioVersionId { get; set; }
    public Guid? RetryOfId { get; set; }
    public required string Kind { get; set; }
    public Guid DeviceId { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset CompletedAt { get; set; }
    public DateTimeOffset ReceivedAt { get; set; }
    public required string ValidationStatus { get; set; }
    public required string PayloadHash { get; set; }
    public Guid ClientAttemptId { get; set; }
}
