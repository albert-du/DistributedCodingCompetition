namespace DistributedCodingCompetition.ApiService.Models;

using Microsoft.EntityFrameworkCore;

public class ContestContext : DbContext
{
    public DbSet<Contest> Contests => Set<Contest>();
    public DbSet<Problem> Problems => Set<Problem>();
    public DbSet<Submission> Submissions => Set<Submission>();
    public DbSet<User> Users => Set<User>();
    public DbSet<JoinCode> JoinCodes => Set<JoinCode>();
    public DbSet<TestCase> TestCases => Set<TestCase>();
    public DbSet<TestCaseResult> TestCaseResults => Set<TestCaseResult>();
}
