namespace DistributedCodingCompetition.CodeExecution;

using Quartz;
using DistributedCodingCompetition.CodeExecution.Services;
using DistributedCodingCompetition.CodeExecution.Models;

public class RefreshExecRunnerJob : IJob
{
    private readonly IExecRunnerService _execRunnerService;

    public RefreshExecRunnerJob(IExecRunnerService execRunnerService)
    {
        _execRunnerService = execRunnerService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        var runner = dataMap.Get("runner") as ExecRunner;
        if (runner == null)
        {
            return;
        }

        await _execRunnerService.RefreshExecRunnerAsync(runner);
    }
}
