global using System.Net.Http.Json;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using DistributedCodingCompetition.ExecutionShared;

namespace DistributedCodingCompetition.CodeExecution.Client;

/// <summary>
/// Extension methods for dependency injection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add the DistributedCodingCompetition Code Execution Service to the application.
    /// </summary>
    /// <param name="serviceDescriptors"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IServiceCollection AddDistributedCodingCompetitionCodeExecution(this IServiceCollection serviceDescriptors, Uri apiAddress)
    {
        serviceDescriptors.AddSingleton<ICodeExecutionService, CodeExecutionService>();
        serviceDescriptors.AddHttpClient<ICodeExecutionService, CodeExecutionService>(client => client.BaseAddress = apiAddress);

        serviceDescriptors.AddSingleton<IExecutionManagementService, ExecutionManagementService>();
        serviceDescriptors.AddHttpClient<IExecutionManagementService, ExecutionManagementService>(client => client.BaseAddress = apiAddress);
        return serviceDescriptors;
    }

    /// <summary>
    /// Add the DistributedCodingCompetition Code Execution Service to the application.
    /// </summary>
    /// <param name="serviceDescriptors"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IServiceCollection AddDistributedCodingCompetitionCodeExecution(this IServiceCollection serviceDescriptors, string apiAddress) =>
        serviceDescriptors.AddDistributedCodingCompetitionCodeExecution(new Uri(apiAddress));
}