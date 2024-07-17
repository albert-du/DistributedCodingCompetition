global using System.Net.Http.Json;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;

namespace DistributedCodingCompetition.Leaderboard.Client;

/// <summary>
/// Extension methods for dependency injection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add the DistributedCodingCompetition Leaderboard Service to the application.
    /// </summary>
    /// <param name="applicationBuilder"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddDistributedCodingCompetitionLeaderboard(this IHostApplicationBuilder applicationBuilder, Uri apiAddress)
    {
        applicationBuilder.Services.AddSingleton<ILeaderboardService, LeaderboardService>();
        applicationBuilder.Services.AddHttpClient<ILeaderboardService, LeaderboardService>(client => client.BaseAddress = apiAddress);
        return applicationBuilder;
    }

    /// <summary>
    /// Add the DistributedCodingCompetition Leaderboard Service to the application.
    /// </summary>
    /// <param name="applicationBuilder"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddDistributedCodingCompetitionLeaderboard(this IHostApplicationBuilder applicationBuilder, string apiAddress) =>
        applicationBuilder.AddDistributedCodingCompetitionLeaderboard(new Uri(apiAddress));
}