global using System.Net.Http.Json;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;

namespace DistributedCodingCompetition.CodePersistence.Client;

/// <summary>
/// Extension methods for dependency injection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add the DistributedCodingCompetition Code Persistence Service to the application.
    /// </summary>
    /// <param name="applicationBuilder"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddDistributedCodingCompetitionCodePersistence(this IHostApplicationBuilder applicationBuilder, Uri apiAddress)
    {
        applicationBuilder.Services.AddSingleton<ICodePersistenceService, CodePersistenceService>();
        applicationBuilder.Services.AddHttpClient<ICodePersistenceService, CodePersistenceService>(client => client.BaseAddress = apiAddress);
        return applicationBuilder;
    }

    /// <summary>
    /// Add the DistributedCodingCompetition Code Persistence Service to the application.
    /// </summary>
    /// <param name="applicationBuilder"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddDistributedCodingCompetitionCodePersistence(this IHostApplicationBuilder applicationBuilder, string apiAddress) =>
        applicationBuilder.AddDistributedCodingCompetitionCodePersistence(new Uri(apiAddress));
}