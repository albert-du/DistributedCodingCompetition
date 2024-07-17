namespace DistributedCodingCompetition.Leaderboard.Client;

using DistributedCodingCompetition.ApiService.Models;

/// <inheritdoc/>
public sealed class LeaderboardService : ILeaderboardService
{
    private readonly ILogger<LeaderboardService> _logger;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeaderboardService"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="httpClient"></param>
    internal LeaderboardService(ILogger<LeaderboardService> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    /// <inheritdoc/>
    public async Task<Leaderboard?> TryGetLeaderboardAsync(Guid contestId, int page)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Leaderboard>($"leaderboard/{contestId}/{page}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch leaderboard");
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<Leaderboard?> TryGetLiveLeaderboardAsync(Guid contestId)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Leaderboard>($"live/{contestId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch live leaderboard");
            return null;
        }
    }
}
