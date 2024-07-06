namespace DistributedCodingCompetition.Judge.Services;

using DistributedCodingCompetition.ApiService.Models;

public class ProblemService(HttpClient httpClient) : IProblemService
{
    public async Task<Problem?> ReadProblemAsync(Guid problemId)
    {
        return await httpClient.GetFromJsonAsync<Problem>($"api/problems/{problemId}");
    }

    public async Task<IReadOnlyList<TestCase>> ReadTestCasesAsync(Guid problemId)
    {
        return await httpClient.GetFromJsonAsync<IReadOnlyList<TestCase>>($"api/problems/{problemId}/testcases") ?? throw new Exception("Failed to fetch test cases");
    }
}
