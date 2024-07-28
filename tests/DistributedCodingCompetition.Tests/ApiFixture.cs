namespace DistributedCodingCompetition.Tests;

using DistributedCodingCompetition.ApiService.Client;
using DistributedCodingCompetition.AuthService.Client;
using DistributedCodingCompetition.CodeExecution.Client;
using DistributedCodingCompetition.Judge.Client;
using DotNet.Testcontainers.Builders;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net;

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
                .WithName($"TEST-PISTON-{Random.Shared.Next()}")
                .WithImage("ghcr.io/engineer-man/piston")
                .WithPortBinding(2000, true)
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
            var execPort = NextFreePort(5227);
            execRunner = Process.Start(new ProcessStartInfo
            {
                FileName = "dotnet",
                //Arguments = "run --urls=http://localhost:5227/ -- Piston=http://localhost:5228/",
                Arguments = $"run --urls=http://localhost:{execPort}/ -- Piston=http://localhost:{container.GetMappedPublicPort(2000)}/",
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

            await executionManagementService.CreateExecRunnerAsync(new("test", $"http://localhost:{execPort}/", true, 100, "changeme"));

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

    private static bool IsFree(int port)
    {
        IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
        IPEndPoint[] listeners = properties.GetActiveTcpListeners();
        int[] openPorts = listeners.Select(item => item.Port).ToArray<int>();
        return openPorts.All(openPort => openPort != port);
    }
    private static int NextFreePort(int port = 0)
    {
        port = (port > 0) ? port : Random.Shared.Next(1, 65535);
        while (!IsFree(port))
        {
            port += 1;
        }
        return port;
    }
}
