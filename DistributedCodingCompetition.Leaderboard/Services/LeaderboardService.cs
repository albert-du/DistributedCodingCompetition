namespace DistributedCodingCompetition.Leaderboard.Services;

using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using DistributedCodingCompetition.ApiService.Models;

public class LeaderboardService(ILogger<LeaderboardService> logger, HttpClient httpClient, IDistributedCache distributedCache) : ILeaderboardService
{
    private static readonly DistributedCacheEntryOptions options = new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) };

    public async Task<Leaderboard?> GetLeaderboardAsync(Guid contest, int page)
    {
        // check the cache for the leaderboard page
        var bytes = await distributedCache.GetStringAsync($"{contest}:{page}");
        if (bytes != null)
            // deserialize the leaderboard page
            return JsonSerializer.Deserialize<Leaderboard>(bytes);

        // fetch the leaderboard from the api
        var start = DateTime.UtcNow;
        var response = await httpClient.GetFromJsonAsync<Leaderboard>($"api/contests/{contest}/leaderboard");
        logger.LogInformation("Fetched leaderboard in {Elapsed}", DateTime.UtcNow - start);
        if (response == null)
            return null;

        // cache the leaderboard
        List<LeaderboardEntry> entries = new(50);
        Leaderboard? leaderboardPage = null;
        var p = 1;
        foreach (var entry in response.Entries)
        {
            entries.Add(entry);
            if (entries.Count == 50)
            {
                // cache the leaderboard page
                Leaderboard leaderboard = new() { Creation = response.Creation, ContestId = response.ContestId, ContestName = response.ContestName, Count = response.Count, Entries = [.. entries] };

                if (p == page)
                    leaderboardPage = leaderboard;

                await distributedCache.SetStringAsync($"{contest}:{p++}", JsonSerializer.Serialize(leaderboard), options);
                entries.Clear();
            }
        }
        if (entries.Count > 0)
        {
            // cache remaining entries
            Leaderboard leaderboard = new() { Creation = response.Creation, ContestId = response.ContestId, ContestName = response.ContestName, Count = response.Count, Entries = [.. entries] };

            if (p == page)
                leaderboardPage = leaderboard;

            await distributedCache.SetStringAsync($"{contest}:{p}", JsonSerializer.Serialize(leaderboard), options);
        }

        return leaderboardPage;
    }
}
