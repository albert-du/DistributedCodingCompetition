namespace DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// Leaderboard representation.
/// </summary>
public sealed record Leaderboard
{
    /// <summary>
    /// UTC creation time of the leaderboard.
    /// </summary>
    public DateTime Creation { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// The id of the contest.
    /// </summary>
    public required Guid ContestId { get; init; }

    /// <summary>
    /// The name of the contest.
    /// </summary>
    public required string ContestName { get; init; }

    /// <summary>
    /// The number of entries in the leaderboard.
    /// </summary>
    public required int Count { get; init; }

    /// <summary>
    /// The entries in the leaderboard.
    /// </summary>
    public required IReadOnlyList<LeaderboardEntry> Entries { get; init; } = [];
}

/// <summary>
/// Leaderboard entry representation.
/// </summary>
/// <param name="UserId">User Id</param>
/// <param name="Username">username</param>
/// <param name="Points">points earned</param>
/// <param name="Rank">rank assigned</param>
public sealed record LeaderboardEntry(Guid UserId, string Username, int Points, int Rank);