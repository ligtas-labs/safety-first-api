namespace SafetyFirst.Api.Features.Assignments;

public sealed class TeacherFeedback
{
    public Guid Id { get; set; }
    public Guid SubmissionId { get; set; }
    public Guid TeacherId { get; set; }
    public required string Body { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
