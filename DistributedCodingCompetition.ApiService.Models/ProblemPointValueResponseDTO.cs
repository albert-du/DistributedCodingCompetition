namespace DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// DTO for a problem point value response.
/// </summary>
public sealed record ProblemPointValueResponseDTO
{
    /// <summary>
    /// The id of the problem point value.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// The id of the problem.
    /// </summary>
    public required Guid ProblemId { get; init; }

    /// <summary>
    /// The id of the contest.
    /// </summary>
    public required Guid ContestId { get; init; }

    /// <summary>
    /// The points for the problem.
    /// </summary>
    public required int Points { get; init; }
}
