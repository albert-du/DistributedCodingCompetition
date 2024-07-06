﻿namespace DistributedCodingCompetition.Judge.Services;

using DistributedCodingCompetition.ExecutionShared;

public interface ICodeExecutionService
{
    Task<ExecutionResult> ExecuteCodeAsync(ExecutionRequest request);
    Task<IReadOnlyList<ExecutionResult>> ExecuteBatchAsync(IEnumerable<ExecutionRequest> request);

    Task<IReadOnlyList<string>> AvailableLanguagesAsync();
}
