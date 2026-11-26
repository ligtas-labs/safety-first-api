namespace SafetyFirst.Api.Features.Classes;

public sealed class Enrollment
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public Guid StudentId { get; set; }
    public required string Status { get; set; }
    public DateTimeOffset? JoinedAt { get; set; }
    public DateTimeOffset RequestedAt { get; set; }
    public Guid? ApprovedById { get; set; }
}
