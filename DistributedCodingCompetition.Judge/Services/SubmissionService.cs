namespace DistributedCodingCompetition.Judge.Services;

using DistributedCodingCompetition.ApiService.Models;

/// <inheritdoc/>
public class SubmissionService(HttpClient httpClient) : ISubmissionService
{
    /// <inheritdoc/>
    public async Task<Submission?> ReadSubmissionAsync(Guid submissionId)
    {
        return await httpClient.GetFromJsonAsync<Submission>($"api/submissions/{submissionId}");
    }

    /// <inheritdoc/>
    public async Task UpdateSubmissionResults(Guid submissionId, IReadOnlyList<TestCaseResult> results, int maxScore, int score)
    {
        await httpClient.PostAsJsonAsync($"api/submissions/{submissionId}/results?possible={maxScore}&score={score}", results);
    }
}
