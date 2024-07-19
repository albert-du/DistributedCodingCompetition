global using System.Net.Http.Json;
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
    /// <param name="serviceDescriptors"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IServiceCollection AddDistributedCodingCompetitionLeaderboard(this IServiceCollection serviceDescriptors, Uri apiAddress)
    {
        serviceDescriptors.AddSingleton<ILeaderboardService, LeaderboardService>();
        serviceDescriptors.AddHttpClient<ILeaderboardService, LeaderboardService>(client => client.BaseAddress = apiAddress);
        return serviceDescriptors;
    }

    /// <summary>
    /// Add the DistributedCodingCompetition Leaderboard Service to the application.
    /// </summary>
    /// <param name="serviceDescriptors"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IServiceCollection AddDistributedCodingCompetitionLeaderboard(this IServiceCollection serviceDescriptors, string apiAddress) =>
        serviceDescriptors.AddDistributedCodingCompetitionLeaderboard(new Uri(apiAddress));
}