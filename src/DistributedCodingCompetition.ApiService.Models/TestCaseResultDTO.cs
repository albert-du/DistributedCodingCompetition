namespace DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// DTO for a test case result.
/// </summary>
public sealed record TestCaseResultDTO
{
    /// <summary>
    /// The id of the test case.
    /// </summary>
    public required Guid TestCaseId { get; init; }

    /// <summary>
    /// The id of the submission.
    /// </summary>
    public required Guid SubmissionId { get; init; }

    /// <summary>
    /// The input of the test case.
    /// </summary>
    public required string Output { get; init; }

    /// <summary>
    /// The output of the test case.
    /// </summary>
    public bool Passed { get; init; }

    /// <summary>
    /// The error of the test case.
    /// </summary>
    public required string? Error { get; init; }

    /// <summary>
    /// The execution time of the test case.
    /// </summary>
    public required int ExecutionTime { get; init; }
}