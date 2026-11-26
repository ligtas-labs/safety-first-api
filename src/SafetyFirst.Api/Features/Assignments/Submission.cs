namespace SafetyFirst.Api.Features.Assignments;

public sealed class Submission
{
    public Guid Id { get; set; }
    public Guid AssignmentId { get; set; }
    public Guid AttemptId { get; set; }
    public DateTimeOffset SubmittedAt { get; set; }
    public required string LateStatus { get; set; }
    public Guid StudentId { get; set; }
}
