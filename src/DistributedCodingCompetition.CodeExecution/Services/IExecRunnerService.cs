namespace DistributedCodingCompetition.CodeExecution.Services;

using DistributedCodingCompetition.CodeExecution.Models;
using DistributedCodingCompetition.ExecutionShared;

/// <summary>
/// Service for managing exec runners.
/// </summary>
public interface IExecRunnerService
{
    /// <summary>
    /// Refresh the exec runner.
    /// </summary>
    /// <param name="runner"></param>
    /// <returns></returns>
    Task<RunnerStatus?> RefreshExecRunnerAsync(ExecRunner runner);

    /// <summary>
    /// Execute code.
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ExecutionResult> ExecuteCodeAsync(ExecRunner runner, ExecutionRequest request);

    /// <summary>
    /// Fetch available packages.
    /// </summary>
    /// <param name="runner"></param>
    /// <returns></returns>
    Task<IReadOnlyList<string>> FetchAvailablePackagesAsync(ExecRunner runner);

    /// <summary>
    /// Set packages.
    /// </summary>
    /// <param name="execRunner"></param>
    /// <param name="packages"></param>
    /// <returns></returns>
    Task SetPackagesAsync(ExecRunner execRunner, IEnumerable<string> packages);
}
