namespace DistributedCodingCompetition.CodeExecution;

using Quartz;
using DistributedCodingCompetition.CodeExecution.Services;
using DistributedCodingCompetition.CodeExecution.Models;

/// <summary>
/// Cron Job to refresh the exec runner instances
/// </summary>
/// <param name="execRunnerService"></param>
/// <param name="refreshEventService"></param>
/// <param name="serviceScopeFactory"></param>
public class RefreshExecRunnerJob(IExecRunnerService execRunnerService, IRefreshEventService refreshEventService, IServiceScopeFactory serviceScopeFactory) : IJob
{
    /// <summary>
    /// Refresh the index of exec runner instances
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Execute(IJobExecutionContext context)
    {
        await Task.Delay(5000);
        using var scope = serviceScopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ExecRunnerContext>();
        foreach (var runner in db.ExecRunners)
            await execRunnerService.RefreshExecRunnerAsync(runner);
        await db.SaveChangesAsync();
        refreshEventService.Refresh(this, [.. db.ExecRunners]);
    }
}
