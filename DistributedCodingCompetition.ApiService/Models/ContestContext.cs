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
    public DbSet<Ban> Bans => Set<Ban>();
    public DbSet<ProblemPointValue> ProblemPointValues => Set<ProblemPointValue>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email).IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username).IsUnique();

        modelBuilder.Entity<User>()
            .HasOne(u => u.Ban).WithOne(b => b.User).HasForeignKey<Ban>(b => b.UserId);

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
