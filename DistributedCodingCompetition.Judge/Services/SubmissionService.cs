namespace DistributedCodingCompetition.Judge.Services;

using DistributedCodingCompetition.ApiService.Models;

public class SubmissionService(HttpClient httpClient) : ISubmissionService
{
    public async Task<Submission?> ReadSubmissionAsync(Guid submissionId)
    {
        return await httpClient.GetFromJsonAsync<Submission>($"api/submissions/{submissionId}");
    }

    public async Task UpdateSubmissionResults(Guid submissionId, IReadOnlyList<TestCaseResult> results)
    {
        await httpClient.PostAsJsonAsync($"api/submissions/{submissionId}/results", results);
    }
}
