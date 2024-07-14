namespace DistributedCodingCompetition.Leaderboard.Services;

using DistributedCodingCompetition.ApiService.Models;

public class LiveReportingService(HttpClient httpClient) : ILiveReportingService
{
    public Task RefreshAsync(Leaderboard leaderboard)
    {
        var bodyStr = string.Join(';', leaderboard.Entries.Select(x => $"{x.UserId},{x.Points}"));
        return httpClient.PostAsJsonAsync($"refresh/{leaderboard.ContestId}?sync={DateTime.UtcNow:O}", bodyStr);
    }

    public async Task<IReadOnlyList<(Guid, int)>> GetLeadersAsync(Guid contestId)
    {
        var str = await httpClient.GetStringAsync($"leaders/{contestId}");
        return str.Trim('\"').Split(';').Select(x =>
        {
            var parts = x.Split(',');
            return (Guid.Parse(parts[0]), int.Parse(parts[1]));
        }).ToList();
    }
}
