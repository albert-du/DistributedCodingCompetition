namespace DistributedCodingCompetition.CodeExecution;

using DistributedCodingCompetition.CodeExecution.Models;

public static class Seeding
{
    /// <summary>
    /// seed the database with a default ExecRunner for development purposes
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static async Task SeedDataAsync(ExecRunnerContext context)
    {
        if (context.ExecRunners.Any())
            return; // DB has been seeded
        await context.ExecRunners.AddAsync(new()
        {
            Id = Guid.NewGuid(),
            Endpoint = "http://localhost:5227/",
            Key = "changeme",
        });
        await context.SaveChangesAsync();
    }
}
