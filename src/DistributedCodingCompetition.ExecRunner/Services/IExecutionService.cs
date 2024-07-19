using DistributedCodingCompetition.ExecutionShared;

namespace DistributedCodingCompetition.ExecRunner.Services;

public interface IExecutionService
{
    Task<ExecutionResult> ExecuteCodeAsync(ExecutionRequest request);
}
