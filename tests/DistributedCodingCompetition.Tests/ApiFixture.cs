namespace DistributedCodingCompetition.Tests;

using DistributedCodingCompetition.ApiService.Client;
using DistributedCodingCompetition.AuthService.Client;
using Microsoft.Extensions.Logging;

public record struct APIs(IAuthService AuthService, IUsersService UsersService, IContestsService ContestsService, IJoinCodesService JoinCodesService, IProblemsService ProblemsService, ITestCasesService TestCasesService);

public class ApiFixture : IAsyncDisposable
{
    readonly Task<APIs> apis;

    public Task<APIs> APIs => apis;

    DistributedApplication? app;

    public ApiFixture()
    {
        apis = Task.Run(async () =>
        {
            // Arrange
            var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.DistributedCodingCompetition_AppHost>();
            app = await appHost.BuildAsync();
            await app.StartAsync();

            using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
                .SetMinimumLevel(LogLevel.Trace)
                .AddConsole());

            // Act
            var httpClient = app.CreateHttpClient("apiservice");
            var authHttpClient = app.CreateHttpClient("authentication");

            // Build service

            AuthService authService = new(authHttpClient, loggerFactory.CreateLogger<AuthService>());
            UsersService usersService = new(httpClient, loggerFactory.CreateLogger<UsersService>());
            ContestsService contestsService = new(httpClient, loggerFactory.CreateLogger<ContestsService>());
            JoinCodesService joinCodesService = new(httpClient, loggerFactory.CreateLogger<JoinCodesService>());
            ProblemsService problemsService = new(httpClient, loggerFactory.CreateLogger<ProblemsService>());
            TestCasesService testCasesService = new(httpClient, loggerFactory.CreateLogger<TestCasesService>());

            // wait 8 seconds for the database migrations to run
            await Task.Delay(8000);
            return new APIs(authService, usersService, contestsService, joinCodesService, problemsService, testCasesService);
        });
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        if (app is not null)
            await app.DisposeAsync();
    }
}
