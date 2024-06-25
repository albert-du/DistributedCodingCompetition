namespace DistributedCodingCompetition.CodeExecution;

using Quartz;
using DistributedCodingCompetition.CodeExecution.Services;
using DistributedCodingCompetition.CodeExecution.Models;

public class RefreshExecRunnerJob(IExecRunnerService execRunnerService, ExecRunnerContext db) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        foreach (var runner in db.ExecRunners)
            await execRunnerService.RefreshExecRunnerAsync(runner);
        await db.SaveChangesAsync();
    }
}
