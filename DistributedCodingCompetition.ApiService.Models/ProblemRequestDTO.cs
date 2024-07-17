namespace DistributedCodingCompetition.ApiService.Models;

public sealed record ProblemRequestDTO
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string? Name { get; init; }
    public Guid? OwnerId { get; init; }
    public string? TagLine { get; init; }
    public string? Description { get; init; }
    public string? RenderedDescription { get; init; }
    public string? Difficulty { get; init; }
}