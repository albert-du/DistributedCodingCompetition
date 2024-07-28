namespace DistributedCodingCompetition.Tests;

using DistributedCodingCompetition.ApiService.Client;
using DistributedCodingCompetition.AuthService.Client;
using DistributedCodingCompetition.CodeExecution.Client;
using DistributedCodingCompetition.Judge.Client;
using DotNet.Testcontainers.Builders;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

public record struct APIs(IAuthService AuthService,
                          IUsersService UsersService,
                          IContestsService ContestsService,
                          IJoinCodesService JoinCodesService,
                          IProblemsService ProblemsService,
                          ITestCasesService TestCasesService,
                          ISubmissionsService SubmissionsService,
                          IJudgeService JudgeService,
                          ICodeExecutionService CodeExecutionService,
                          IExecutionManagementService ExecutionManagementService);

public class ApiFixture : IAsyncDisposable
{
    private readonly Task<APIs> services;

    public Task<APIs> APIs => services;

    DistributedApplication? app;

    Process? execRunner;
    //Process? piston;

    public ApiFixture()
    {
        services = Task.Run(async () =>
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
            var judgeHttpClient = app.CreateHttpClient("judge");
            var executionHttpClient = app.CreateHttpClient("codeexecution");

            // create a container for the piston service
            var container = new ContainerBuilder()
                .WithName("TEST-PISTON")
                .WithImage("ghcr.io/engineer-man/piston")
                .WithPortBinding(2001, 2000)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(2000))
                .WithTmpfsMount("/piston/jobs")
                .Build();

            await container.StartAsync().ConfigureAwait(false);

            // new process, start an execrunner and a piston simulating service
            //piston = Process.Start(new ProcessStartInfo
            //{
            //    FileName = "dotnet",
            //    Arguments = "run --urls=http://localhost:5228/",
            //    WorkingDirectory = Path.GetFullPath($"{Environment.CurrentDirectory}\\..\\..\\..\\..\\PistonSimulator\\"),
            //});

            execRunner = Process.Start(new ProcessStartInfo
            {
                FileName = "dotnet",
                //Arguments = "run --urls=http://localhost:5227/ -- Piston=http://localhost:5228/",
                Arguments = "run --urls=http://localhost:5227/ -- Piston=http://localhost:2001/",
                WorkingDirectory = Path.GetFullPath($"{Environment.CurrentDirectory}\\..\\..\\..\\..\\..\\src\\DistributedCodingCompetition.ExecRunner\\"),
            });

            // Build service

            AuthService authService = new(authHttpClient, loggerFactory.CreateLogger<AuthService>());

            UsersService usersService = new(httpClient, loggerFactory.CreateLogger<UsersService>());
            ContestsService contestsService = new(httpClient, loggerFactory.CreateLogger<ContestsService>());
            JoinCodesService joinCodesService = new(httpClient, loggerFactory.CreateLogger<JoinCodesService>());
            ProblemsService problemsService = new(httpClient, loggerFactory.CreateLogger<ProblemsService>());
            TestCasesService testCasesService = new(httpClient, loggerFactory.CreateLogger<TestCasesService>());
            SubmissionsService submissionsService = new(httpClient, loggerFactory.CreateLogger<SubmissionsService>());

            JudgeService judgeService = new(judgeHttpClient, loggerFactory.CreateLogger<JudgeService>());

            CodeExecutionService codeExecutionService = new(executionHttpClient, loggerFactory.CreateLogger<CodeExecutionService>());
            ExecutionManagementService executionManagementService = new(executionHttpClient, loggerFactory.CreateLogger<ExecutionManagementService>());

            // clear the seeded exec runners and add a new one
            var execRunners = await executionManagementService.ListExecRunnersAsync();
            await Task.WhenAll(execRunners.Select(x => executionManagementService.DeleteExecRunnerAsync(x.Id)));

            await executionManagementService.CreateExecRunnerAsync(new("test", "http://localhost:5227/", true, 100, "changeme"));

            // wait 8 seconds for the database migrations to run
            await Task.Delay(8000);
            return new APIs(authService, usersService, contestsService, joinCodesService, problemsService, testCasesService, submissionsService, judgeService, codeExecutionService, executionManagementService);
        });
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        if (app is not null)
            await app.DisposeAsync();

        execRunner?.Kill();
        //piston?.Kill();

        execRunner?.Dispose();
        //piston?.Dispose();
    }
}
