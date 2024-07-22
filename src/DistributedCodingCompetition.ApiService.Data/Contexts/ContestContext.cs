namespace DistributedCodingCompetition.ApiService.Data.Contexts;

using Microsoft.EntityFrameworkCore;
using DistributedCodingCompetition.ApiService.Data.Models;

/// <summary>
/// Data context for contest API
/// </summary>
/// <param name="options"></param>
public class ContestContext(DbContextOptions<ContestContext> options) : DbContext(options)
{
    /// <summary>
    /// Contests
    /// </summary>
    public DbSet<Contest> Contests => Set<Contest>();

    /// <summary>
    /// Problems
    /// </summary>
    public DbSet<Problem> Problems => Set<Problem>();

    /// <summary>
    /// Submissions
    /// </summary>
    public DbSet<Submission> Submissions => Set<Submission>();

    /// <summary>
    /// Users
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Join Codes
    /// </summary>
    public DbSet<JoinCode> JoinCodes => Set<JoinCode>();

    /// <summary>
    /// Test Cases
    /// </summary>
    public DbSet<TestCase> TestCases => Set<TestCase>();

    /// <summary>
    /// Test Case Results
    /// </summary>
    public DbSet<TestCaseResult> TestCaseResults => Set<TestCaseResult>();

    /// <summary>
    /// System Bans
    /// </summary>
    public DbSet<Ban> Bans => Set<Ban>();

    /// <summary>
    /// Problem Point Values
    /// </summary>
    public DbSet<ProblemPointValue> ProblemPointValues => Set<ProblemPointValue>();

    /// <summary>
    /// create the ef data model
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email).IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username).IsUnique();

        modelBuilder.Entity<User>()
            .HasOne(u => u.Ban).WithOne(b => b.User).HasForeignKey<Ban>(b => b.UserId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.IssuedBans).WithOne(b => b.Issuer);

        modelBuilder.Entity<Contest>()
            .HasOne(c => c.Owner).WithMany(u => u.OwnedContests);

        modelBuilder.Entity<Contest>()
            .HasMany(c => c.Administrators).WithMany(u => u.AdministeredContests);

        modelBuilder.Entity<Contest>()
            .HasMany(c => c.Participants).WithMany(u => u.EnteredContests);

        modelBuilder.Entity<Contest>()
            .HasMany(c => c.Banned).WithMany(u => u.BannedContests);

        modelBuilder.Entity<JoinCode>()
            .HasIndex(j => j.Code).IsUnique();
    }
}
