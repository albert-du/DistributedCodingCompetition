namespace DistributedCodingCompetition.ApiService.MigrationService;

using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using OpenTelemetry.Trace;
using DistributedCodingCompetition.ApiService.Data.Models;
using DistributedCodingCompetition.ApiService.Data.Contexts;

/// <summary>
/// Worker service for migrating the database.
/// </summary>
/// <param name="serviceProvider"></param>
/// <param name="hostApplicationLifetime"></param>
/// <param name="configuration"></param>
public sealed class Worker(IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime, IConfiguration configuration, ILogger<Worker> logger) : BackgroundService
{
    public const string ActivitySourceName = "API Migrations";

    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);
        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ContestContext>();

            await EnsureDatabaseAsync(dbContext, cancellationToken);
            await RunMigrationAsync(dbContext, cancellationToken);

            if (Convert.ToBoolean(configuration["Seed"]))
                await SeedDataAsync(dbContext, cancellationToken);
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
            throw;
        }
        if (Convert.ToBoolean(configuration["ExitAfterMigration"]))
            hostApplicationLifetime.StopApplication();
    }

    private static async Task EnsureDatabaseAsync(ContestContext dbContext, CancellationToken cancellationToken)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Create the database if it does not exist.
            // Do this first so there is then a database to start a transaction against.
            if (!await dbCreator.ExistsAsync(cancellationToken))
                await dbCreator.CreateAsync(cancellationToken);
        });
    }

    private static async Task RunMigrationAsync(ContestContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }

    private async Task SeedDataAsync(ContestContext dbContext, CancellationToken cancellationToken)
    {
        logger.LogInformation("Seeding data");
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
            Input = "1\n2\n",
            Output = "3",
            Description = "Add two numbers",
            Sample = true,
            Weight = 100,
        };

        Problem problem = new()
        {
            Id = Guid.Parse("bcd30243-b2bf-4e4f-bf84-44ff02041bc2"),
            Name = "Problem 1",
            TagLine = "add two integers from stdin",
            Description = "add two integers from stdin",
            RenderedDescription = "add two integers from stdin",
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

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Seed the database
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            await dbContext.Users.AddAsync(user1, cancellationToken);
            await dbContext.Users.AddAsync(user2, cancellationToken);
            await dbContext.TestCases.AddAsync(testCase, cancellationToken);
            await dbContext.Problems.AddAsync(problem, cancellationToken);
            await dbContext.Contests.AddAsync(contest, cancellationToken);
            await dbContext.JoinCodes.AddAsync(joinCode, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }
}
