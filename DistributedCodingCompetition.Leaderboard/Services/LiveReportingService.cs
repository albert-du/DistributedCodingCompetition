namespace DistributedCodingCompetition.Leaderboard.Services;

using DistributedCodingCompetition.ApiService.Models;

public class LiveReportingService(HttpClient httpClient) : ILiveReportingService
{
    public Task RefreshAsync(Leaderboard leaderboard) =>
        httpClient.PostAsJsonAsync($"report/{leaderboard.ContestId}?sync={DateTime.UtcNow:O}", leaderboard.Entries.Select(x => (x.UserId, x.Points)));

    public async Task<IReadOnlyList<(Guid, int)>> GetLeadersAsync(Guid contestId) =>
        await httpClient.GetFromJsonAsync<IReadOnlyList<(Guid, int)>>($"leaders/{contestId}") ?? [];

}
