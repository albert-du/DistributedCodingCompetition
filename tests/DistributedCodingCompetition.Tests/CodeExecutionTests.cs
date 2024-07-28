using DistributedCodingCompetition.ExecutionShared;

namespace DistributedCodingCompetition.Tests;

public class CodeExecutionTest(ApiFixture fixture) : IClassFixture<ApiFixture>
{
    [Fact]
    public async Task TestCodeExecution()
    {
        var services = await fixture.APIs;
        var executionManagementService = services.ExecutionManagementService;
        var codeExecutionService = services.CodeExecutionService;

        var execRunners = await executionManagementService.ListExecRunnersAsync();
        Assert.NotEmpty(execRunners);
        var firstRunner = execRunners[0];
        Assert.NotNull(firstRunner);
        // make sure can read available packages
        var availablePackages = await executionManagementService.AvailablePackagesAsync(firstRunner.Id);
        Assert.NotNull(availablePackages);
        Assert.True(availablePackages.Any());

        // make sure nothing's installed yet
        var installed = await executionManagementService.InstalledPackagesAsync(firstRunner.Id);
        Assert.NotNull(installed);

        // find the first python package, fine last so python 3
        var pythonPackage = availablePackages.LastOrDefault(p => p.StartsWith("python"));
        Assert.NotNull(pythonPackage);

        // set the packages for the first runner
        await executionManagementService.SetPackagesAsync(firstRunner.Id, [pythonPackage]);
        // give it a few seconds to install
        await Task.Delay(10000);
        installed = await executionManagementService.InstalledPackagesAsync(firstRunner.Id);
        Assert.NotNull(installed);
        Assert.Contains(pythonPackage, installed);
    }

    [Fact]
    public async Task TestCodeExecutionService()
    {
        var services = await fixture.APIs;
        var codeExecutionService = services.CodeExecutionService;

        var languages = await codeExecutionService.AvailableLanguagesAsync();
        Assert.NotEmpty(languages);
        var python = languages.FirstOrDefault(l => l.StartsWith("python"));
        Assert.NotNull(python);

        ExecutionRequest request = new()
        {
            Language = python,
            Code = "print('hello world')"
        };
        var result = await codeExecutionService.TryExecuteCodeAsync(request);
        Assert.NotNull(result);
        Assert.Equal("hello world", result.Output);
    }

    [Fact]
    public async Task TestBatchCodeExecutionService()
    {
        var services = await fixture.APIs;
        var codeExecutionService = services.CodeExecutionService;

        var languages = await codeExecutionService.AvailableLanguagesAsync();
        Assert.NotEmpty(languages);
        var python = languages.FirstOrDefault(l => l.StartsWith("python"));
        Assert.NotNull(python);
        List<ExecutionRequest> requests =
        [
            new()
            {
                Language = python,
                Code = "print('hello world')"
            },
            new()
            {
                Language = python,
                Code = "print('goodbye world')"
            }
        ];
        var results = await codeExecutionService.TryExecuteBatchAsync(requests);
        Assert.NotNull(results);
        Assert.Equal(2, results.Count);
        Assert.Equal("hello world", results[0].Output);
        Assert.Equal("goodbye world", results[1].Output);
    }
}