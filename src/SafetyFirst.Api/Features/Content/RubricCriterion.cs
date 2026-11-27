using System.Text.Json;

namespace SafetyFirst.Api.Features.Content;

public sealed class RubricCriterion
{
    public Guid Id { get; set; }
    public Guid RubricVersionId { get; set; }
    public required string AreaCode { get; set; }
    public required string RequirementCode { get; set; }
    public bool Essential { get; set; }
    public required string EvaluationType { get; set; }
    public JsonElement EvaluationConfig { get; set; }
    public required string Explanation { get; set; }
}
