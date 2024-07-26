namespace DistributedCodingCompetition.Tests;

public class CodeExecutionTest(ApiFixture fixture) : IClassFixture<ApiFixture>
{
    [Fact]
    public async Task TestCodeExecution()
    {
        var apis = await fixture.APIs;
        var executionManagementService = apis.ExecutionManagementService;
        var codeExecutionService = apis.CodeExecutionService;

        var execRunners = await executionManagementService.ListExecRunnersAsync();
        Assert.NotEmpty(execRunners);
        var firstRunner = execRunners[0];
        Assert.NotNull(firstRunner);
        // make sure can read available packages
        var availablePackages = await executionManagementService.AvailablePackagesAsync(firstRunner.Id);
        Assert.NotNull(availablePackages);
        Assert.True(availablePackages.Any());
        // find the first python package
        var pythonPackage = availablePackages.FirstOrDefault(p => p.StartsWith("python"));
        Assert.NotNull(pythonPackage);

        // set the packages for the first runner
        await executionManagementService.SetPackagesAsync(firstRunner.Id, [pythonPackage]);
        var installed = await executionManagementService.InstalledPackagesAsync(firstRunner.Id);
        Assert.NotNull(installed);
        Assert.Contains(pythonPackage, installed);
    }
}