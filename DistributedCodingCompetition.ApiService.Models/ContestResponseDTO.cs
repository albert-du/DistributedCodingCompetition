namespace DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// DTO for a contest response.
/// </summary>
public sealed record ContestResponseDTO
{
    /// <summary>
    /// The id of the contest.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// The name of the contest.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The id of the owner of the contest.
    /// </summary>
    public required Guid OwnerId { get; init; }

    /// <summary>
    /// The name of the owner of the contest.
    /// </summary>
    public required string OwnerName { get; init; }

    /// <summary>
    /// The Description of the contest.
    /// </summary>
    public required string Description { get; init; } = string.Empty;

    /// <summary>
    /// The rendered description of the contest.
    /// </summary>
    public required string RenderedDescription { get; init; } = string.Empty;

    /// <summary>
    /// The UTC start time of the contest.
    /// </summary>
    public required DateTime StartTime { get; init; }

    /// <summary>
    /// The UTC end time of the contest.
    /// </summary>
    public required DateTime EndTime { get; init; }

    /// <summary>
    /// Status of the contest.
    /// </summary>
    public required bool Active { get; init; }

    /// <summary>
    /// Whether the contest is public.
    /// </summary>
    public required bool Public { get; init; }

    /// <summary>
    /// Whether new participants can join.
    /// </summary>
    public required bool Open { get; init; }

    /// <summary>
    /// Min age to participate in the contest.
    /// </summary>
    public required int MinimumAge { get; init; }

    /// <summary>
    /// Default points for a problem in the contest.
    /// </summary>
    public required int DefaultPointsForProblem { get; init; }

    /// <summary>
    /// The number of participants in the contest.
    /// </summary>
    public required int TotalParticipants { get; init; }

    /// <summary>
    /// The number of banned participants in the contest.
    /// </summary>
    public required int TotalBanned { get; init; }

    /// <summary>
    /// The number of admins in the contest.
    /// </summary>
    public required int TotalAdmins { get; init; }

    /// <summary>
    /// The number of problems in the contest.
    /// </summary>
    public required int TotalProblems { get; init; }

    /// <summary>
    /// The number of submissions in the contest.
    /// </summary>
    public required int TotalSubmissions { get; init; }

    /// <summary>
    /// The number of join codes in the contest.
    /// </summary>
    public required int TotalJoinCodes { get; init; }
}