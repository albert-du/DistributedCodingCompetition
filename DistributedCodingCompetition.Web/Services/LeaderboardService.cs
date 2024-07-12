using DistributedCodingCompetition.ApiService.Models;

namespace DistributedCodingCompetition.Web.Services;

public class LeaderboardService(ILogger<LeaderboardService> logger, HttpClient httpClient) : ILeaderboardService
{
    public async Task<IReadOnlyList<LeaderboardEntry>?> TryGetLeaderboardAsync(Guid contestId, int page)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<IReadOnlyList<LeaderboardEntry>>($"api/leaderboard/{contestId}/{page}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch leaderboard");
            return null;
        }
    }

}
