namespace SafetyFirst.Api.Features.Identity;

public sealed class UserAccount
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string DisplayName { get; set; }
    public required string Role { get; set; }
    public DateTimeOffset? DisabledAt { get; set; }
    public string? NeonAuthSubject { get; set; }
}
