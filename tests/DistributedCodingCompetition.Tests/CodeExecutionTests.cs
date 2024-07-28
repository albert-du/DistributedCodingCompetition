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


        await executionManagementService.SetPackagesAsync(firstRunner.Id, ["python=3.12.0"]);

        var start = DateTime.UtcNow;
        while (!(await executionManagementService.InstalledPackagesAsync(firstRunner.Id)).Contains("python=3.12.0"))
        {
            if (DateTime.UtcNow - start > TimeSpan.FromMinutes(1))
                throw new Exception("Timed out waiting for language install");
            await Task.Delay(1000);
        }

        ExecutionRequest request = new()
        {
            Language = "python=3.12.0",
            Code = "print('hello world')"
        };
        var result = await codeExecutionService.TryExecuteCodeAsync(request);
        Assert.NotNull(result);
        Assert.Equal("hello world\n", result.Output);

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
        Assert.Equal("hello world\n", results[0].Output);
        Assert.Equal("goodbye world\n", results[1].Output);
    }
}