global using System.Net.Http.Json;
global using Microsoft.Extensions.Hosting;
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
    /// <param name="applicationBuilder"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddDistributedCodingCompetitionAuth(this IHostApplicationBuilder applicationBuilder, Uri apiAddress)
    {
        applicationBuilder.Services.AddSingleton<IAuthService, AuthService>();
        applicationBuilder.Services.AddHttpClient<IAuthService, AuthService>(client => client.BaseAddress = apiAddress);
        return applicationBuilder;
    }

    /// <summary>
    /// Add the DistributedCodingCompetition Auth Service to the application.
    /// </summary>
    /// <param name="applicationBuilder"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddDistributedCodingCompetitionAuth(this IHostApplicationBuilder applicationBuilder, string apiAddress) =>
        applicationBuilder.AddDistributedCodingCompetitionAuth(new Uri(apiAddress));
}