namespace DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// DTO for a submission response.
/// </summary>
public sealed record SubmissionResponseDTO
{
    /// <summary>
    /// The id of the submission.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// The id of the contest.
    /// </summary>
    public required Guid? ContestId { get; init; }

    /// <summary>
    /// The name of the contest.
    /// </summary>
    public required string ContestName { get; init; }

    /// <summary>
    /// The id of the problem.
    /// </summary>
    public required Guid ProblemId { get; init; }

    /// <summary>
    /// The name of the problem.
    /// </summary>
    public required string ProblemName { get; init; }

    /// <summary>
    /// The id of the user.
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// The name of the user.
    /// </summary>
    public required string UserName { get; init; }

    /// <summary>
    /// The language of the submission.
    /// </summary>
    public required string Language { get; init; }

    /// <summary>
    /// The source code of the submission.
    /// </summary>
    public required string Code { get; init; }

    /// <summary>
    /// The created at date of the submission.
    /// </summary>
    public required DateTime CreatedAt { get; init; }

    /// <summary>
    /// The judged at date of the submission.
    /// </summary>
    public required DateTime? JudgedAt { get; init; }

    /// <summary>
    /// The score of the submission.
    /// </summary>
    public required int Score { get; set; }

    /// <summary>
    /// The maximum possible score of the submission.
    /// </summary>
    public required int MaxPossibleScore { get; set; }

    /// <summary>
    /// The points of the submission.
    /// </summary>
    public required int Points { get; set; }

    /// <summary>
    /// Whether the submission is invalidated.
    /// </summary>
    public required bool Invalidated { get; set; }

    /// <summary>
    /// The test cases passed of the submission.
    /// </summary>
    public required int TestCasesPassed { get; set; }

    /// <summary>
    /// The total test cases of the submission.
    /// </summary>
    public required int TestCasesTotal { get; set; }
}