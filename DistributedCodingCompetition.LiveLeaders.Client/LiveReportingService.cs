namespace DistributedCodingCompetition.LiveLeaders.Client;

/// <inheritdoc/>
public sealed class LiveReportingService : ILiveReportingService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<LiveReportingService> _logger;

    internal LiveReportingService(HttpClient httpClient, ILogger<LiveReportingService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc/>
    public Task ReportAsync(Guid contestId, Guid userId, int points) =>
        _httpClient.PostAsync($"report/{contestId}/{userId}?points={points}&sync={DateTime.UtcNow:O}", null);

    /// <inheritdoc/>
    public Task RefreshAsync(Leaderboard leaderboard) =>
        _httpClient.PostAsJsonAsync($"refresh/{leaderboard.ContestId}?sync={DateTime.UtcNow:O}", string.Join(';', leaderboard.Entries.Select(x => $"{x.UserId},{x.Points}")));

    /// <inheritdoc/>
    public async Task<IReadOnlyList<(Guid, int)>> GetLeadersAsync(Guid contestId)
    {
        var str = await _httpClient.GetStringAsync($"leaders/{contestId}");
        return str.Trim('\"').Split(';', StringSplitOptions.RemoveEmptyEntries).Select(x =>
        {
            var parts = x.Split(',');
            return (Guid.Parse(parts[0]), int.Parse(parts[1]));
        }).ToList();
    }
}