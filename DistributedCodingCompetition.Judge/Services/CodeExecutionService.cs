namespace DistributedCodingCompetition.Judge.Services;

using DistributedCodingCompetition.ExecutionShared;

/// <summary>
/// Code execution service
/// </summary>
/// <param name="httpClient"></param>
public class CodeExecutionService(HttpClient httpClient) : ICodeExecutionService
{
    /// <summary>
    /// Call execution endpoint to execute code
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<ExecutionResult> ExecuteCodeAsync(ExecutionRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("execution", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ExecutionResult>() ?? throw new Exception("Failed to execute code");
    }

    /// <summary>
    /// Get available languages for execution
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<IReadOnlyList<string>> AvailableLanguagesAsync()
    {
        var response = await httpClient.GetAsync("execution/languages");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IReadOnlyList<string>>() ?? throw new Exception("Failed to fetch available languages");
    }

    /// <summary>
    /// Batch execute multiple requests
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<IReadOnlyList<ExecutionResult>> ExecuteBatchAsync(IEnumerable<ExecutionRequest> request)
    {
        var response = await httpClient.PostAsJsonAsync("execution/batch", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IReadOnlyList<ExecutionResult>>() ?? throw new Exception("Failed to execute batch");
    }
}