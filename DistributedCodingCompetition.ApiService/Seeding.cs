﻿namespace DistributedCodingCompetition.ApiService;

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

        TestCase testCase = new()
        {
            Id = Guid.Parse("134904d0-9515-4ceb-84d0-2cae5bf60f9d"),
            Input = "1 2",
            Output = "3",
            Description = "Add two numbers",
            Sample = true,
            Weight = 100,
        };

        Problem problem = new()
        {
            Id = Guid.Parse("134904d0-9515-4ceb-84d0-2cae5bf60f9d"),
            Name = "Problem 1",
            Description = "First problem",
            TestCases = [testCase],
            Owner = user1,
        };

        Contest contest = new()
        {
            Id = Guid.Parse("134904d0-9515-4ceb-84d0-2cae5bf60f9d"),
            Name = "Contest 1",
            Description = "First contest",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddDays(1),
            Administrators = [user1],
            Problems = [problem],
            Public = true,
            Owner = user1,
        };

        context.Users.Add(user1);
        context.TestCases.Add(testCase);
        context.Problems.Add(problem);
        context.Contests.Add(contest);

        await context.SaveChangesAsync();
    }
}
