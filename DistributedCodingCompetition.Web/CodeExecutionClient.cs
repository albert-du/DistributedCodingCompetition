namespace DistributedCodingCompetition.Web;

using DistributedCodingCompetition.ExecutionShared;
public class CodeExecutionClient(HttpClient httpClient)
{
    public async Task<ExecutionResult> ExecuteCodeAsync(ExecutionRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("api/execution", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ExecutionResult>() ?? throw new Exception("Failed to execute code");
    }

    public async Task<IReadOnlyList<string>> AvailableLanguagesAsync()
    {
        var response = await httpClient.GetAsync("api/execution/languages");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IReadOnlyList<string>>() ?? throw new Exception("Failed to fetch available languages");
    }
}
