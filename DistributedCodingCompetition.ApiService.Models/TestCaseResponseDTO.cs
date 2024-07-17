namespace DistributedCodingCompetition.ApiService.Models;

public sealed record TestCaseResponseDTO
{
    public required Guid Id { get; init; }
    public required Guid ProblemId { get; init; }
    public required string ProblemName { get; init; }
    public required string Input { get; init; }
    public required string Output { get; init; }
    public required string Description { get; init; }
    public required bool Sample { get; init; }
    public required bool Active { get; init; }
    public required int Weight { get; init; }
}