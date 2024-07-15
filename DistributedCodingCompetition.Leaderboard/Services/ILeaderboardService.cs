namespace DistributedCodingCompetition.Leaderboard.Services;

using DistributedCodingCompetition.Models;

/// <summary>
/// Service for accessing the leaderboard
/// </summary>
public interface ILeaderboardService
{
    /// <summary>
    /// Get the leaderboard for a contest
    /// </summary>
    /// <param name="contest"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    Task<Leaderboard?> GetLeaderboardAsync(Guid contest, int page);
}
