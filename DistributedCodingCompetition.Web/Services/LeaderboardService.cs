using DistributedCodingCompetition.ApiService.Models;

namespace DistributedCodingCompetition.Web.Services;

public class LeaderboardService(ILogger<LeaderboardService> logger, HttpClient httpClient) : ILeaderboardService
{
    public async Task<Leaderboard?> TryGetLeaderboardAsync(Guid contestId, int page)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<Leaderboard>($"leaderboard/{contestId}/{page}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch leaderboard");
            return null;
        }
    }

}
