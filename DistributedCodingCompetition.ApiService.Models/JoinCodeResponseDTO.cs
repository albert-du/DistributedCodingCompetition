namespace DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// DTO for a join code response.
/// </summary>
public sealed record JoinCodeResponseDTO
{
    /// <summary>
    /// The id of the join code.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// The name of the join code.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The id of the contest the join code is for.
    /// </summary>
    public required Guid ContestId { get; init; }

    /// <summary>
    /// The name of the contest the join code is for.
    /// </summary>
    public required string ContestName { get; init; }

    /// <summary>
    /// The id of the creator of the join code.
    /// </summary>
    public required Guid CreatorId { get; init; }

    /// <summary>
    /// The name of the creator of the join code.
    /// </summary>
    public required string CreatorName { get; init; }

    /// <summary>
    /// The code for the join code.
    /// </summary>
    public required string Code { get; init; }

    /// <summary>
    /// Whether the join code is for an admin.
    /// </summary>
    public required bool Admin { get; init; }

    /// <summary>
    /// Whether the join code is active.
    /// </summary>
    public required bool Active { get; init; }

    /// <summary>
    /// Whether the join code will close after use.
    /// </summary>
    public required bool CloseAfterUse { get; init; }

    /// <summary>
    /// The number of times the join code has been used.
    /// </summary>
    public required int Uses { get; init; }

    /// <summary>
    /// The UTC time the join code was created.
    /// </summary>
    public required DateTime CreatedAt { get; init; }
}