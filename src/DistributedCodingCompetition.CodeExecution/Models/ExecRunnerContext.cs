namespace DistributedCodingCompetition.CodeExecution.Models;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Data context for ExecRunners
/// </summary>
/// <param name="options"></param>
public class ExecRunnerContext(DbContextOptions<ExecRunnerContext> options) : DbContext(options)
{
    /// <summary>
    /// Execution Runners
    /// </summary>
    public DbSet<ExecRunner> ExecRunners => Set<ExecRunner>();
}
