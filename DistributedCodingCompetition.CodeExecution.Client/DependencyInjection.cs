global using System.Net.Http.Json;
global using Microsoft.Extensions.Hosting;
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
    /// <param name="applicationBuilder"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddDistributedCodingCompetitionCodeExecution(this IHostApplicationBuilder applicationBuilder, Uri apiAddress)
    {
        applicationBuilder.Services.AddSingleton<ICodeExecutionService, CodeExecutionService>();
        applicationBuilder.Services.AddHttpClient<ICodeExecutionService, CodeExecutionService>(client => client.BaseAddress = apiAddress);
        return applicationBuilder;
    }

    /// <summary>
    /// Add the DistributedCodingCompetition Code Execution Service to the application.
    /// </summary>
    /// <param name="applicationBuilder"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddDistributedCodingCompetitionCodeExecution(this IHostApplicationBuilder applicationBuilder, string apiAddress) =>
        applicationBuilder.AddDistributedCodingCompetitionCodeExecution(new Uri(apiAddress));
}