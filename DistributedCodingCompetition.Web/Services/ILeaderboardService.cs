using DistributedCodingCompetition.ApiService.Models;

namespace DistributedCodingCompetition.Web.Services;

public interface ILeaderboardService
{
    /// <summary>
    /// Get the leaderboard at a page
    /// page is 1-indexed
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    Task<IReadOnlyList<LeaderboardEntry>?> TryGetLeaderboardAsync(Guid contestId, int page);
}
