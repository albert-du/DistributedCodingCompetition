namespace DistributedCodingCompetition.Leaderboard.Client;

using DistributedCodingCompetition.ApiService.Models;

/// <inheritdoc/>
public sealed class LeaderboardService(ILogger<LeaderboardService> logger, HttpClient httpClient) : ILeaderboardService
{
    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public async Task<Leaderboard?> TryGetLiveLeaderboardAsync(Guid contestId)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<Leaderboard>($"live/{contestId}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch live leaderboard");
            return null;
        }
    }
}
