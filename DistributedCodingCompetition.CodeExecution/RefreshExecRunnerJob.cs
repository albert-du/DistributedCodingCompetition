namespace DistributedCodingCompetition.CodeExecution;

using Quartz;
using DistributedCodingCompetition.CodeExecution.Services;
using DistributedCodingCompetition.CodeExecution.Models;

public class RefreshExecRunnerJob(IExecRunnerService execRunnerService, IRefreshEventService refreshEventService, IServiceScopeFactory serviceScopeFactory) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ExecRunnerContext>();
        await Task.Delay(15000);
        foreach (var runner in db.ExecRunners)
            await execRunnerService.RefreshExecRunnerAsync(runner);
        await db.SaveChangesAsync();
        refreshEventService.Refresh(this, [.. db.ExecRunners]);
    }
}
