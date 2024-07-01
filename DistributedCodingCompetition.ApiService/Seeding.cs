namespace DistributedCodingCompetition.ApiService;

using DistributedCodingCompetition.ApiService.Models;

public static class Seeding
{
    public static async Task SeedDataAsync(ContestContext context)
    {
        if (context.Contests.Any())
            return; // DB has been seeded

        context.Users.Add(new User
        {
            Id = Guid.Parse("134904d0-9515-4ceb-84d0-2cae5bf60f9d"),
            Username = "user1",
            FullName = "User One",
            Email = "user1@example.com",
            Birthday = new DateTime(2000, 1, 1).ToUniversalTime(),
            Creation = DateTime.UtcNow,
        });

        await context.SaveChangesAsync();
    }
}
