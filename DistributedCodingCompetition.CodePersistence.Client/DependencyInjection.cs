global using System.Net.Http.Json;
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
    /// <param name="serviceDescriptors"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IServiceCollection AddDistributedCodingCompetitionCodePersistence(this IServiceCollection serviceDescriptors, Uri apiAddress)
    {
        serviceDescriptors.AddSingleton<ICodePersistenceService, CodePersistenceService>();
        serviceDescriptors.AddHttpClient<ICodePersistenceService, CodePersistenceService>(client => client.BaseAddress = apiAddress);
        return serviceDescriptors;
    }

    /// <summary>
    /// Add the DistributedCodingCompetition Code Persistence Service to the application.
    /// </summary>
    /// <param name="serviceDescriptors"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IServiceCollection AddDistributedCodingCompetitionCodePersistence(this IServiceCollection serviceDescriptors, string apiAddress) =>
        serviceDescriptors.AddDistributedCodingCompetitionCodePersistence(new Uri(apiAddress));
}