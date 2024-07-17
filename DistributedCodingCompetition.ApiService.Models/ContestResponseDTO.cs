namespace DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// DTO for a contest response.
/// </summary>
public sealed record ContestResponseDTO
{
    /// <summary>
    /// The id of the contest.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// The name of the contest.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The id of the owner of the contest.
    /// </summary>
    public required Guid OwnerId { get; set; }

    /// <summary>
    /// The name of the owner of the contest.
    /// </summary>
    public required string OwnerName { get; set; }

    /// <summary>
    /// The Description of the contest.
    /// </summary>
    public required string Description { get; set; } = string.Empty;

    /// <summary>
    /// The rendered description of the contest.
    /// </summary>
    public required string RenderedDescription { get; set; } = string.Empty;

    /// <summary>
    /// The UTC start time of the contest.
    /// </summary>
    public required DateTime StartTime { get; set; }

    /// <summary>
    /// The UTC end time of the contest.
    /// </summary>
    public required DateTime EndTime { get; set; }

    /// <summary>
    /// Status of the contest.
    /// </summary>
    public required bool Active { get; set; }

    /// <summary>
    /// Whether the contest is public.
    /// </summary>
    public required bool Public { get; set; }

    /// <summary>
    /// Whether new participants can join.
    /// </summary>
    public required bool Open { get; set; }

    /// <summary>
    /// Min age to participate in the contest.
    /// </summary>
    public required int MinimumAge { get; set; }

    /// <summary>
    /// Default points for a problem in the contest.
    /// </summary>
    public required int DefaultPointsForProblem { get; set; }
}