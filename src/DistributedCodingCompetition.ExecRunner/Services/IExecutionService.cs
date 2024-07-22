namespace DistributedCodingCompetition.ExecRunner.Services;

using DistributedCodingCompetition.ExecutionShared;

/// <summary>
/// Service for executing code on piston
/// </summary>
public interface IExecutionService
{
    /// <summary>
    /// Execute Code
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ExecutionResult> ExecuteCodeAsync(ExecutionRequest request);
}
