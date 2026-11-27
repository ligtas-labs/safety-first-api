namespace SafetyFirst.Api.Features.Classes;

public sealed class Classroom
{
    public Guid Id { get; set; }
    public Guid TeacherId { get; set; }
    public required string Name { get; set; }
    public required string Term { get; set; }
    public DateTimeOffset? ArchivedAt { get; set; }
    public string? JoinCodeHash { get; set; }
    public DateTimeOffset? CodeExpiresAt { get; set; }
}
