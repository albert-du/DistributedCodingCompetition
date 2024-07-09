namespace DistributedCodingCompetition.ApiService;

using DistributedCodingCompetition.ApiService.Models;

public static class Seeding
{
    public static async Task SeedDataAsync(ContestContext context)
    {
        if (context.Contests.Any())
            return; // DB has been seeded

        User user1 = new()
        {
            Id = Guid.Parse("134904d0-9515-4ceb-84d0-2cae5bf60f9d"),
            Username = "user1",
            FullName = "User One",
            Email = "user1@example.com",
            Birthday = new DateTime(2000, 1, 1).ToUniversalTime(),
            Creation = DateTime.UtcNow,
        };
        
        User user2 = new()
        {
            Id = Guid.Parse("234904d0-9515-4ceb-84d0-2cae5bf60f9e"),
            Username = "user2",
            FullName = "User Two",
            Email = "user2@example.com",
            Birthday = new DateTime(2000, 1, 1).ToUniversalTime(),
            Creation = DateTime.UtcNow,
        };

        TestCase testCase = new()
        {
            Id = Guid.Parse("82720217-26d3-4c5c-82c8-fb047b5383e1"),
            Input = "1 2",
            Output = "3",
            Description = "Add two numbers",
            Sample = true,
            Weight = 100,
        };

        Problem problem = new()
        {
            Id = Guid.Parse("bcd30243-b2bf-4e4f-bf84-44ff02041bc2"),
            Name = "Problem 1",
            Description = "First problem",
            TestCases = [testCase],
            Owner = user1,
        };

        JoinCode joinCode = new()
        {
            Id = Guid.Parse("d830da9c-c6fb-464d-89f2-869cd91082a8"),
            ContestId = Guid.Parse("134904d0-9515-4ceb-84d0-2cae5bf60f9d"),
            Code = "12345678",
            Name = "Join code 1",
            Active = true,
            Creation = DateTime.UtcNow,
            Expiration = DateTime.UtcNow.AddDays(1),
            CloseAfterUse = false,
            CreatorId = user1.Id,
        };

        Contest contest = new()
        {
            Id = Guid.Parse("134904d0-9515-4ceb-84d0-2cae5bf60f9d"),
            Name = "Contest 1",
            Description = "First contest",
            RenderedDescription = "First contest",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddDays(1),
            Administrators = [user1],
            Problems = [problem],
            Public = true,
            Owner = user1,
            JoinCodes = [joinCode],
            Participants = [user2],
        };

        context.Users.Add(user1);
        context.Users.Add(user2);
        context.TestCases.Add(testCase);
        context.Problems.Add(problem);
        context.Contests.Add(contest);
        context.JoinCodes.Add(joinCode);

        await context.SaveChangesAsync();
    }
}
