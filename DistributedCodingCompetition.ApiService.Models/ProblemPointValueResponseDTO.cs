namespace DistributedCodingCompetition.ApiService.Models;

public sealed record ProblemPointValueResponseDTO
{
    public required Guid Id { get; init; }
    public required Guid ProblemId { get; init; }
    public required Guid ContestId { get; init; }
    public required int Points { get; init; }
}
