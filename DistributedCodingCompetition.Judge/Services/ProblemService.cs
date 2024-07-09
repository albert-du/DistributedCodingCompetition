namespace DistributedCodingCompetition.Judge.Services;

using DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// Service for accessing problems
/// </summary>
/// <param name="httpClient"></param>
public class ProblemService(HttpClient httpClient) : IProblemService
{
    /// <summary>
    /// Read problem from external service
    /// </summary>
    /// <param name="problemId"></param>
    /// <returns></returns>
    public async Task<Problem?> ReadProblemAsync(Guid problemId)
    {
        return await httpClient.GetFromJsonAsync<Problem>($"api/problems/{problemId}");
    }

    /// <summary>
    /// Read test cases for a problem from external service
    /// </summary>
    /// <param name="problemId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<IReadOnlyList<TestCase>> ReadTestCasesAsync(Guid problemId)
    {
        return await httpClient.GetFromJsonAsync<IReadOnlyList<TestCase>>($"api/problems/{problemId}/testcases") ?? throw new Exception("Failed to fetch test cases");
    }
}
