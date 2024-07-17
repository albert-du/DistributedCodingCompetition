namespace DistributedCodingCompetition.ApiService.Models;

public sealed record SubmissionRequestDTO
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Guid ContestId { get; init; }
    public required Guid ProblemId { get; init; }
    public required Guid UserId { get; init; }
    public required string Language { get; init; }
    public required string Code { get; init; }
}