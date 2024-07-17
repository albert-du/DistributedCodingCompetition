namespace DistributedCodingCompetition.CodeExecution.Client;

/// <inheritdoc/>
public class CodeExecutionService : ICodeExecutionService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CodeExecutionService> _logger;

    /// <summary>
    /// Create a new instance of the DistributedCodingCompetition Code Execution Service.
    /// </summary>
    /// <param name="httpClient"></param>
    internal CodeExecutionService(HttpClient httpClient, ILogger<CodeExecutionService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<ExecutionResult?> TryExecuteCodeAsync(ExecutionRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("execution", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ExecutionResult>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to execute code");
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<string>> AvailableLanguagesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("execution/languages");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IReadOnlyList<string>>() ?? throw new Exception("Failed to fetch available languages");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch available languages");
            return [];
        }
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<ExecutionResult>?> TryExecuteBatchAsync(IEnumerable<ExecutionRequest> request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("execution/batch", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IReadOnlyList<ExecutionResult>>() ?? throw new Exception("Failed to execute batch");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to execute batch");
            return null;
        }
    }
}
