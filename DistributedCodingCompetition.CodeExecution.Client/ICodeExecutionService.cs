namespace DistributedCodingCompetition.CodeExecution.Client;

/// <summary>
/// Service for executing code.
/// </summary>
public interface ICodeExecutionService
{
    /// <summary>
    /// Execute one code request
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ExecutionResult?> TryExecuteCodeAsync(ExecutionRequest request);

    /// <summary>
    /// Execute Many code requests
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<IReadOnlyList<ExecutionResult>?> TryExecuteBatchAsync(IEnumerable<ExecutionRequest> request);

    /// <summary>
    /// Get the available languages for execution
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyList<string>> AvailableLanguagesAsync();
}
