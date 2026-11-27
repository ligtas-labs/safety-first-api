namespace SafetyFirst.Api.Features.Content;

public sealed class ScenarioVersion
{
    public Guid Id { get; set; }
    public Guid ModuleId { get; set; }
    public Guid RubricVersionId { get; set; }
    public required string VariantCode { get; set; }
    public int VersionNo { get; set; }
    public required string PackageUrl { get; set; }
    public required string PackageSha256 { get; set; }
    public required string Status { get; set; }
    public DateTimeOffset? ReviewedAt { get; set; }
}
