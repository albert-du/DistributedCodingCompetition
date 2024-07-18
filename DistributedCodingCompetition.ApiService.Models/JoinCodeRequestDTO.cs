namespace DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// DTO for a join code request.
/// </summary>
public sealed record JoinCodeRequestDTO
{
    /// <summary>
    /// The id of the join code.
    /// Required for updates.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// The name of the join code.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// The id of the contest the join code is for.
    /// </summary>
    public Guid? ContestId { get; init; }

    /// <summary>
    /// The id of the creator of the join code.
    /// </summary>
    public Guid? CreatorId { get; init; }

    /// <summary>
    /// The code for the join code.
    /// </summary>
    public string? Code { get; init; }

    /// <summary>
    /// Whether the join code is for an admin.
    /// </summary>
    public bool? Admin { get; init; }

    /// <summary>
    /// Whether the join code is active.
    /// </summary>
    public bool? Active { get; init; }

    /// <summary>
    /// Whether the join code should close after use.
    /// </summary>
    public bool? CloseAfterUse { get; init; }

    /// <summary>
    /// The UTC date and time the join code will end.
    /// </summary>
    public DateTime? Expiration { get; init; }
}