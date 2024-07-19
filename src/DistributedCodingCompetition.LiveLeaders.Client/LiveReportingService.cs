namespace DistributedCodingCompetition.LiveLeaders.Client;

/// <inheritdoc/>
public sealed class LiveReportingService(HttpClient httpClient) : ILiveReportingService
{
    /// <inheritdoc/>
    public Task ReportAsync(Guid contestId, Guid userId, int points) =>
        httpClient.PostAsync($"report/{contestId}/{userId}?points={points}&sync={DateTime.UtcNow:O}", null);

    /// <inheritdoc/>
    public Task RefreshAsync(Leaderboard leaderboard) =>
        httpClient.PostAsJsonAsync($"refresh/{leaderboard.ContestId}?sync={DateTime.UtcNow:O}", string.Join(';', leaderboard.Entries.Select(x => $"{x.UserId},{x.Points}")));

    /// <inheritdoc/>
    public async Task<IReadOnlyList<(Guid, int)>> GetLeadersAsync(Guid contestId)
    {
        var str = await httpClient.GetStringAsync($"leaders/{contestId}");
        return str.Trim('\"').Split(';', StringSplitOptions.RemoveEmptyEntries).Select(x =>
        {
            var parts = x.Split(',');
            return (Guid.Parse(parts[0]), int.Parse(parts[1]));
        }).ToList();
    }
}