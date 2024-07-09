namespace DistributedCodingCompetition.Judge.Services;

using DistributedCodingCompetition.ExecutionShared;

/// <summary>
/// Code execution service
/// </summary>
public interface ICodeExecutionService
{
    /// <summary>
    /// Execute one code request
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ExecutionResult> ExecuteCodeAsync(ExecutionRequest request);

    /// <summary>
    /// Execute Many code requests
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<IReadOnlyList<ExecutionResult>> ExecuteBatchAsync(IEnumerable<ExecutionRequest> request);

    /// <summary>
    /// Get the available languages for execution
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyList<string>> AvailableLanguagesAsync();
}
