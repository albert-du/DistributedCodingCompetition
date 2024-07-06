namespace DistributedCodingCompetition.Judge.Services;

using DistributedCodingCompetition.ExecutionShared;
public class CodeExecutionService(HttpClient httpClient) : ICodeExecutionService
{
    public async Task<ExecutionResult> ExecuteCodeAsync(ExecutionRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("execution", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ExecutionResult>() ?? throw new Exception("Failed to execute code");
    }

    public async Task<IReadOnlyList<string>> AvailableLanguagesAsync()
    {
        var response = await httpClient.GetAsync("execution/languages");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IReadOnlyList<string>>() ?? throw new Exception("Failed to fetch available languages");
    }

    public async Task<IEnumerable<ExecutionResult>> ExecuteBatchAsync(IEnumerable<ExecutionRequest> request)
    {
        var response = await httpClient.PostAsJsonAsync("execution/batch", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<ExecutionResult>>() ?? throw new Exception("Failed to execute batch");
    }
}