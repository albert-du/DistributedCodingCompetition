namespace DistributedCodingCompetition.ApiService.Models;

using Microsoft.EntityFrameworkCore;

public class ContestContext(DbContextOptions<ContestContext> options) : DbContext(options)
{
    public DbSet<Contest> Contests => Set<Contest>();
    public DbSet<Problem> Problems => Set<Problem>();
    public DbSet<Submission> Submissions => Set<Submission>();
    public DbSet<User> Users => Set<User>();
    public DbSet<JoinCode> JoinCodes => Set<JoinCode>();
    public DbSet<TestCase> TestCases => Set<TestCase>();
    public DbSet<TestCaseResult> TestCaseResults => Set<TestCaseResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email).IsUnique();

        // modelBuilder.Entity<Contest>()
        //     .HasMany(c => c.Administrators).WithMany(u => u.AdministeredContests);

        // modelBuilder.Entity<Contest>()
        //     .HasMany(c => c.Participants).WithMany(u => u.EnteredContests);

        modelBuilder.Entity<JoinCode>()
            .HasIndex(j => j.Code).IsUnique();
    }
}
