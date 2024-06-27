namespace DistributedCodingCompetition.CodeExecution.Services;

using DistributedCodingCompetition.CodeExecution.Models;
using DistributedCodingCompetition.ExecutionShared;

public interface IExecRunnerService
{
    Task RefreshExecRunnerAsync(ExecRunner runner);

    Task<ExecutionResult> ExecuteCodeAsync(ExecRunner runner, ExecutionRequest request);
    Task<IReadOnlyList<string>> FetchAvailablePackagesAsync(ExecRunner runner);
    Task SetPackagesAsync(ExecRunner execRunner, IEnumerable<string> packages);
}
