namespace DistributedCodingCompetition.CodeExecution;

using MongoDB.Driver;
using DistributedCodingCompetition.CodeExecution.Services;

/// <summary>
/// Seeds the database with a default ExecRunner for development purposes
/// </summary>
public static class Seeding
{
    /// <summary>
    /// seed the database with a default ExecRunner for development purposes
    /// </summary>
    /// <param name="execRunnerRepository"></param>
    /// <returns></returns>
    public static async Task SeedDataAsync(IExecRunnerRepository execRunnerRepository)
    {
        if ((await execRunnerRepository.GetExecRunnersAsync()).Any())
            await execRunnerRepository.CreateExecRunnerAsync(new()
            {
                Id = Guid.NewGuid(),
                Endpoint = "http://localhost:5227/",
                Key = "changeme",
            });
    }
}
