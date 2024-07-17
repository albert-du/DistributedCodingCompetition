namespace DistributedCodingCompetition.ApiService.Client;

/// <summary>
/// Extension methods for dependency injection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add the DistributedCodingCompetition API Service to the application.
    /// </summary>
    /// <param name="applicationBuilder"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddDistributedCodingCompetitionAPI(this IHostApplicationBuilder applicationBuilder, Uri apiAddress)
    {
        applicationBuilder.Services.AddSingleton<IContestsService, ContestsService>();
        applicationBuilder.Services.AddHttpClient<IContestsService, ContestsService>(client => client.BaseAddress = apiAddress);

        applicationBuilder.Services.AddSingleton<IJoinCodesService, JoinCodesService>();
        applicationBuilder.Services.AddHttpClient<IJoinCodesService, JoinCodesService>(client => client.BaseAddress = apiAddress);

        applicationBuilder.Services.AddSingleton<IProblemsService, ProblemsService>();
        applicationBuilder.Services.AddHttpClient<IProblemsService, ProblemsService>(client => client.BaseAddress = apiAddress);

        applicationBuilder.Services.AddSingleton<ISubmissionsService, SubmissionsService>();
        applicationBuilder.Services.AddHttpClient<ISubmissionsService, SubmissionsService>(client => client.BaseAddress = apiAddress);

        applicationBuilder.Services.AddSingleton<ITestCasesService, TestCasesService>();
        applicationBuilder.Services.AddHttpClient<ITestCasesService, TestCasesService>(client => client.BaseAddress = apiAddress);

        applicationBuilder.Services.AddSingleton<IUsersService, UsersService>();
        applicationBuilder.Services.AddHttpClient<IUsersService, UsersService>(client => client.BaseAddress = apiAddress);

        return applicationBuilder;
    }

    /// <summary>
    /// Add the DistributedCodingCompetition API Service to the application.
    /// </summary>
    /// <param name="applicationBuilder"></param>
    /// <param name="apiAddress"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddDistributedCodingCompetitionAPI(this IHostApplicationBuilder applicationBuilder, string apiAddress) =>
        applicationBuilder.AddDistributedCodingCompetitionAPI(new Uri(apiAddress));
}