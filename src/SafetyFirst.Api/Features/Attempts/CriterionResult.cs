namespace SafetyFirst.Api.Features.Attempts;

public sealed class CriterionResult
{
    public Guid AssessmentId { get; set; }
    public Guid CriterionId { get; set; }
    public bool Applicable { get; set; }
    public short? Earned { get; set; }
    public required string FeedbackCode { get; set; }
}
