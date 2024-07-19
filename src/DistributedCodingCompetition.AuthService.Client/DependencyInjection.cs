global using System.Net.Http.Json;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using DistributedCodingCompetition.AuthService.Models;

namespace DistributedCodingCompetition.AuthService.Client;

/// <summary>
/// Extension methods for dependency injection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add the DistributedCodingCompetition Auth Service to the application.
    /// </summary>
    /// <param name="serviceDescriptors"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IServiceCollection AddDistributedCodingCompetitionAuth(this IServiceCollection serviceDescriptors, Uri apiAddress)
    {
        serviceDescriptors.AddSingleton<IAuthService, AuthService>();
        serviceDescriptors.AddHttpClient<IAuthService, AuthService>(client => client.BaseAddress = apiAddress);
        return serviceDescriptors;
    }

    /// <summary>
    /// Add the DistributedCodingCompetition Auth Service to the application.
    /// </summary>
    /// <param name="serviceDescriptors"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IServiceCollection AddDistributedCodingCompetitionAuth(this IServiceCollection serviceDescriptors, string apiAddress) =>
        serviceDescriptors.AddDistributedCodingCompetitionAuth(new Uri(apiAddress));
}