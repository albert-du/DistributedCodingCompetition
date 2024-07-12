namespace DistributedCodingCompetition.Leaderboard.Services;

using DistributedCodingCompetition.ApiService.Models;

public interface ILeaderboardService
{
    public IAsyncEnumerable<LeaderboardEntry> GetLeaderboardAsync();
}
