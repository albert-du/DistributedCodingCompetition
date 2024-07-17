global using System.Net.Http.Json;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using DistributedCodingCompetition.ApiService.Models;

namespace DistributedCodingCompetition.LiveLeaders.Client;

/// <summary>
/// Extension methods for dependency injection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add the DistributedCodingCompetition LiveLeaders Service to the application.
    /// </summary>
    /// <param name="serviceDescriptors"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IServiceCollection AddDistributedCodingCompetitionLiveLeaders(this IServiceCollection serviceDescriptors, Uri apiAddress)
    {
        serviceDescriptors.AddSingleton<ILiveReportingService, LiveReportingService>();
        serviceDescriptors.AddHttpClient<ILiveReportingService, LiveReportingService>(client => client.BaseAddress = apiAddress);
        return serviceDescriptors;
    }

    /// <summary>
    /// Add the DistributedCodingCompetition LiveLeaders Service to the application.
    /// </summary>
    /// <param name="serviceDescriptors"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IServiceCollection AddDistributedCodingCompetitionLiveLeaders(this IServiceCollection serviceDescriptors, string apiAddress) =>
        serviceDescriptors.AddDistributedCodingCompetitionLiveLeaders(new Uri(apiAddress));
}