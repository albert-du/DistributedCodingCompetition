namespace DistributedCodingCompetition.CodeExecution.Services;

using DistributedCodingCompetition.CodeExecution.Models;
using DistributedCodingCompetition.ExecutionShared;

/// <summary>
/// Manages an active cache of exec runners.
/// </summary>
public interface IActiveRunnersService
{
    /// <summary>
    /// Immediately triggers a refresh of the exec runners.
    /// </summary>
    /// <returns></returns>
    Task IndexExecRunnersAsync();

    /// <summary>
    /// Find an exec runner by language.
    /// </summary>
    /// <param name="language"></param>
    /// <returns></returns>
    Task<ExecRunner?> FindExecRunnerAsync(string language);

    /// <summary>
    /// Balance requests across exec runners.
    /// </summary>
    /// <param name="requests"></param>
    /// <returns></returns>
    Task<IReadOnlyList<(ExecutionRequest request, ExecRunner? runner)>> BalanceRequestsAsync(IReadOnlyCollection<ExecutionRequest> requests);

    /// <summary>
    /// Reads all languages available from exec runners.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<string>> GetLanguagesAsync();
}