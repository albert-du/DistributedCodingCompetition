namespace DistributedCodingCompetition.ApiService;

using DistributedCodingCompetition.ApiService.Models;

public static class Seeding
{
    public static async Task SeedDataAsync(ContestContext context)
    {
        if (context.Contests.Any())
            return; // DB has been seeded

        await context.SaveChangesAsync();
    }
}
