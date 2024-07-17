namespace DistributedCodingCompetition.ApiService.Models;

public sealed record SubmissionResponseDTO
{
    public required Guid Id { get; init; }
    public required Guid? ContestId { get; init; }
    public required string ContestName { get; init; }
    public required Guid ProblemId { get; init; }
    public required string ProblemName { get; init; }
    public required Guid UserId { get; init; }
    public required string UserName { get; init; }
    public required string Language { get; init; }
    public required string Code { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime? JudgedAt { get; init; }
    public required int Score { get; set; }
    public required int MaxPossibleScore { get; set; }
    public required int Points { get; set; }
    public required bool Invalidated { get; set; }
    public required int TestCasesPassed { get; set; }
    public required int TestCasesTotal { get; set; }
}