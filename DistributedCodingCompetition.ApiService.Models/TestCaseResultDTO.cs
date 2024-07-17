namespace DistributedCodingCompetition.ApiService.Models;

public sealed record TestCaseResultDTO
{
    public required Guid TestCaseId { get; init; }

    public required Guid SubmissionId { get; init; }

    public required string Output { get; init; }

    public bool Passed { get; init; }

    public required string? Error { get; init; }

    public required int ExecutionTime { get; init; }
}
