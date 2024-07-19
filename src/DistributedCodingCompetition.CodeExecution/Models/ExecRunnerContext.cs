namespace DistributedCodingCompetition.CodeExecution.Models;

using Microsoft.EntityFrameworkCore;

public class ExecRunnerContext(DbContextOptions<ExecRunnerContext> options) : DbContext(options)
{
    public DbSet<ExecRunner> ExecRunners => Set<ExecRunner>();
}
