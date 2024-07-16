namespace DistributedCodingCompetition.ApiService.Models;

public sealed record ProblemResponseDTO
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required Guid OwnerId { get; init; }
    public required string OwnerName { get; init; }
    public required string TagLine { get; init; }
    public required string Description { get; init; }
    public required string RenderedDescription { get; init; }
    public required string? Difficulty { get; init; }
    public required int TestCaseCount { get; init; }
}