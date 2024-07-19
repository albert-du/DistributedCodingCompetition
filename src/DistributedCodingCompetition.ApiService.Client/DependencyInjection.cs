global using System.Net.Http.Json;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.DependencyInjection;
global using DistributedCodingCompetition.ApiService.Models;

namespace DistributedCodingCompetition.ApiService.Client;
/// <summary>
/// Extension methods for dependency injection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add the DistributedCodingCompetition API Service to the application.
    /// </summary>
    /// <param name="serviceDescriptors"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IServiceCollection AddDistributedCodingCompetitionAPI(this IServiceCollection serviceDescriptors, Uri apiAddress)
    {
        serviceDescriptors.AddSingleton<IContestsService, ContestsService>();
        serviceDescriptors.AddHttpClient<IContestsService, ContestsService>(client => client.BaseAddress = apiAddress);

        serviceDescriptors.AddSingleton<IJoinCodesService, JoinCodesService>();
        serviceDescriptors.AddHttpClient<IJoinCodesService, JoinCodesService>(client => client.BaseAddress = apiAddress);

        serviceDescriptors.AddSingleton<IProblemsService, ProblemsService>();
        serviceDescriptors.AddHttpClient<IProblemsService, ProblemsService>(client => client.BaseAddress = apiAddress);

        serviceDescriptors.AddSingleton<ISubmissionsService, SubmissionsService>();
        serviceDescriptors.AddHttpClient<ISubmissionsService, SubmissionsService>(client => client.BaseAddress = apiAddress);

        serviceDescriptors.AddSingleton<ITestCasesService, TestCasesService>();
        serviceDescriptors.AddHttpClient<ITestCasesService, TestCasesService>(client => client.BaseAddress = apiAddress);

        serviceDescriptors.AddSingleton<IUsersService, UsersService>();
        serviceDescriptors.AddHttpClient<IUsersService, UsersService>(client => client.BaseAddress = apiAddress);

        return serviceDescriptors;
    }

    /// <summary>
    /// Add the DistributedCodingCompetition API Service to the application.
    /// </summary>
    /// <param name="serviceDescriptors"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IServiceCollection AddDistributedCodingCompetitionAPI(this IServiceCollection serviceDescriptors, string apiAddress) =>
        serviceDescriptors.AddDistributedCodingCompetitionAPI(new Uri(apiAddress));
}