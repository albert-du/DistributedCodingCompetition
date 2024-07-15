namespace DistributedCodingCompetition.Web.Services;

/// <summary>
/// Service for accessing the leaderboard
/// </summary>
public interface ILeaderboardService
{
    /// <summary>
    /// Get the leaderboard at a page
    /// page is 1-indexed
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    Task<Leaderboard?> TryGetLeaderboardAsync(Guid contestId, int page);

    /// <summary>
    /// Get the live leaderboard
    /// </summary>
    /// <param name="contestId"></param>
    /// <returns></returns>
    Task<Leaderboard?> TryGetLiveLeaderboardAsync(Guid contestId);
}
