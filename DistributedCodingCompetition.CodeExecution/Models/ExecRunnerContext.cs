namespace DistributedCodingCompetition.CodeExecution.Models;

using Microsoft.EntityFrameworkCore;

public class ExecRunnerContext : DbContext
{
    public DbSet<ExecRunner> ExecRunners { get; set; } = null!;

    public ExecRunnerContext(DbContextOptions<ExecRunnerContext> options) : base(options)
    {

    }
}
