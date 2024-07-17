namespace DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// DTO for a submission request.
/// </summary>
public sealed record SubmissionRequestDTO
{
    /// <summary>
    /// The id of the submission.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// The id of the contest.
    /// </summary>
    public required Guid ContestId { get; init; }

    /// <summary>
    /// The id of the problem.
    /// </summary>
    public required Guid ProblemId { get; init; }

    /// <summary>
    /// The id of the user.
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// The language of the submission.
    /// </summary>
    public required string Language { get; init; }

    /// <summary>
    /// The code of the submission.
    /// </summary>
    public required string Code { get; init; }
}