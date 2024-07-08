namespace DistributedCodingCompetition.Web;

using DistributedCodingCompetition.ExecutionShared;

/// <summary>
/// Client for executing code.
/// </summary>
/// <param name="httpClient"></param>
public sealed class CodeExecutionClient(HttpClient httpClient)
{
    /// <summary>
    /// Execute code.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<ExecutionResult> ExecuteCodeAsync(ExecutionRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("/execution", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ExecutionResult>() ?? throw new Exception("Failed to execute code");
    }

    /// <summary>
    /// Get available languages.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<IReadOnlyList<string>> AvailableLanguagesAsync()
    {
        var response = await httpClient.GetAsync("/execution/languages");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IReadOnlyList<string>>() ?? throw new Exception("Failed to fetch available languages");
    }
}
