namespace SafetyFirst.Api.Features.Content;

public sealed class LearningModule
{
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Title { get; set; }
    public int SequenceNo { get; set; }
    public required string Kind { get; set; }
}
