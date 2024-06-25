namespace DistributedCodingCompetition.CodeExecution.Services;

using DistributedCodingCompetition.CodeExecution.Models;

public interface IExecRunnerService
{
    Task RefreshExecRunnerAsync(ExecRunner runner);
}
