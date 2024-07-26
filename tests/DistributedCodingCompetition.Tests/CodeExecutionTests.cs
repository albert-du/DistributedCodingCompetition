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
        var firstRunner = execRunners.First();
        Assert.NotNull(firstRunner);
        // set the packages for the first runner
        await executionManagementService.SetPackagesAsync(firstRunner.Id, ["python=3.12"]);
        var availablePackages = await executionManagementService.InstalledPackagesAsync(firstRunner);
    }
}