namespace DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// DTO for a contest creation or update request.
/// </summary>
public sealed record ContestRequestDTO
{
    /// <summary>
    /// The id of the contest.
    /// Required for updates.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// The name of the contest.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The Description of the contest.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The rendered description of the contest.
    /// </summary>
    public string? RenderedDescription { get; set; }

    /// <summary>
    /// The UTC start time of the contest.
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// The UTC end time of the contest.
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Status of the contest.
    /// False to archive the contest.
    /// </summary>
    public bool? Active { get; set; }

    /// <summary>
    /// The id of the owner of the contest.
    /// </summary>
    public Guid? OwnerId { get; set; }

    /// <summary>
    /// Whether the contest is public.
    /// </summary>
    public bool? Public { get; set; }

    /// <summary>
    /// Whether new participants can join.
    /// </summary>
    public bool? Open { get; set; }

    /// <summary>
    /// Min age to participate in the contest.
    /// </summary>
    public int? MinimumAge { get; set; }

    /// <summary>
    /// Default points for a problem in the contest.
    /// </summary>
    public int? DefaultPointsForProblem { get; set; }
}
