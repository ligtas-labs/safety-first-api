namespace SafetyFirst.Api.Features.Attempts;

public sealed class AssessmentResult
{
    public Guid Id { get; set; }
    public Guid AttemptId { get; set; }
    public required string ScorerVersion { get; set; }
    public decimal OverallRating { get; set; }
    public required string PassOrRetry { get; set; }
    public DateTimeOffset ScoredAt { get; set; }
}
