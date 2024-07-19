namespace DistributedCodingCompetition.CodeExecution.Client;

/// <inheritdoc/>
public class CodeExecutionService(HttpClient httpClient, ILogger<CodeExecutionService> logger) : ICodeExecutionService
{
    /// <inheritdoc/>
    public async Task<ExecutionResult?> TryExecuteCodeAsync(ExecutionRequest request)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("execution", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ExecutionResult>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to execute code");
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<string>> AvailableLanguagesAsync()
    {
        try
        {
            var response = await httpClient.GetAsync("execution/languages");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IReadOnlyList<string>>() ?? throw new Exception("Failed to fetch available languages");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch available languages");
            return [];
        }
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<ExecutionResult>?> TryExecuteBatchAsync(IEnumerable<ExecutionRequest> request)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("execution/batch", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IReadOnlyList<ExecutionResult>>() ?? throw new Exception("Failed to execute batch");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to execute batch");
            return null;
        }
    }
}
