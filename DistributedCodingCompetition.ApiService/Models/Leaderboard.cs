namespace DistributedCodingCompetition.ApiService.Models;

public sealed record Leaderboard
{
    public DateTime Creation { get; init; } = DateTime.UtcNow;
    public required Guid ContestId { get; init; }
    public required string ContestName { get; init; }
    public required int Count { get; init; }
    public required IReadOnlyList<LeaderboardEntry> Entries { get; init; } = [];
}

public sealed record LeaderboardEntry(Guid UserId, string Username, int Points, int Rank);