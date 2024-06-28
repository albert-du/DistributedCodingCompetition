namespace DistributedCodingCompetition.CodeExecution;

using DistributedCodingCompetition.CodeExecution.Models;

public static class Seeding
{
    public static async Task SeedDataAsync(ExecRunnerContext context)
    {
        if (context.ExecRunners.Any())
            return; // DB has been seeded
        await context.ExecRunners.AddAsync(new()
        {
            Id = Guid.NewGuid(),
            Endpoint = "http://localhost:7110/",
            Key = "changeme",
        });
        await context.SaveChangesAsync();
    }
}
