namespace DistributedCodingCompetition.LiveLeaders.Services;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

/// <inheritdoc/>
public class LeadersService(ILogger<LeadersService> logger, IDistributedCache cache) : ILeadersService
{
    /// <summary>
    /// Send the refreshed leaderboard to the live leaderboard service.
    /// </summary>
    /// <param name="contest">id</param>
    /// <param name="leaders">only takes 100</param>
    /// <param name="sync">leaderboard time</param>
    /// <returns></returns>
    public async Task RefreshLeaderboardAsync(Guid contest, IReadOnlyList<(Guid, int)> leaders, DateTime sync)
    {
        logger.LogInformation("Refreshing leaderboard for {Contest}", contest);
        DistributedCacheEntryOptions options = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };
        var newLeaders = leaders.Select(x => x.Item1);
        var oldLeaders = (await cache.GetStringAsync($"leaderboard:{contest}"))?.Split(';')?.Select(Guid.Parse) ?? [];
        var removed = oldLeaders.Except(newLeaders);

        await cache.SetStringAsync($"leaderboard:{contest}", string.Join(";", newLeaders), options);
        await cache.SetStringAsync($"leaderboard:{contest}:sync", sync.ToString("O"), options);
        foreach (var (leader, points) in leaders)
            await cache.SetAsync($"leaderboard:{contest}:{leader}", BitConverter.GetBytes(points), options);

        foreach (var leader in removed)
            await cache.RemoveAsync($"leaderboard:{contest}:{leader}");
    }

    /// <inheritdoc/>
    public async Task ReportJudgingAsync(Guid contest, Guid leader, int points, DateTime sync)
    {
        logger.LogInformation("Reporting judging for {Leader} in {Contest}", leader, contest);
        DistributedCacheEntryOptions options = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        // make sure the sync is newer than the leaderboard
        var lastSync = await cache.GetStringAsync($"leaderboard:{contest}:sync");
        if (lastSync == null || DateTime.Parse(lastSync) > sync)
            return;

        // get the current points
        var current = await cache.GetAsync($"leaderboard:{contest}:{leader}");
        if (current == null)
            // if the leader is not being tracked, ignore the submission
            return;

        // update the points
        await cache.SetAsync($"leaderboard:{contest}:{leader}", BitConverter.GetBytes(BitConverter.ToInt32(current) + points), options);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<(Guid, int)>> GetLeadersAsync(Guid contest, int count)
    {
        logger.LogInformation("Getting leaders for {Contest}", contest);
        var keys = await cache.GetStringAsync($"leaderboard:{contest}");
        if (keys == null)
            return [];

        var leaders = keys.Split(';').Select(Guid.Parse).ToArray();

        var tasks = new Task<byte[]?>[leaders.Length];

        for (var i = 0; i < leaders.Length; i++)
            tasks[i] = cache.GetAsync($"leaderboard:{contest}:{leaders[i]}");

        var points = new (Guid, int)[leaders.Length];

        for (var i = 0; i < leaders.Length; i++)
        {
            var result = await tasks[i];
            if (result != null)
                points[i] = (leaders[i], BitConverter.ToInt32(result));
        }

        return points.OrderByDescending(x => x.Item2).Take(count).ToList();
    }
}
