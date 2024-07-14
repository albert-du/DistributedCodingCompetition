namespace DistributedCodingCompetition.Web.Services;

using DistributedCodingCompetition.ApiService.Models;

public sealed class LeaderboardService(ILogger<LeaderboardService> logger, HttpClient httpClient) : ILeaderboardService
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

    public async Task<Leaderboard?> TryGetLiveLeaderboardAsync(Guid contestId)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<Leaderboard>($"leaderboard/{contestId}/live");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch live leaderboard");
            return null;
        }
    }
}
