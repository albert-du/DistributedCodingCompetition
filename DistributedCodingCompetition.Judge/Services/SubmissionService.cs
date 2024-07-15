namespace DistributedCodingCompetition.Judge.Services;

using DistributedCodingCompetition.Models;

/// <inheritdoc/>
public class SubmissionService(HttpClient httpClient) : ISubmissionService
{
    /// <inheritdoc/>
    public async Task<Submission?> ReadSubmissionAsync(Guid submissionId)
    {
        return await httpClient.GetFromJsonAsync<Submission>($"api/submissions/{submissionId}");
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<Submission> ReadSubmissionsAsync(Guid problemId)
    {
        await foreach (var submission in httpClient.GetFromJsonAsAsyncEnumerable<Submission>($"api/problems/{problemId}/submissions"))
            if (submission is not null)
                yield return submission;
    }

    /// <inheritdoc/>
    public async Task UpdateSubmissionResults(Guid submissionId, IReadOnlyList<TestCaseResult> results, int maxScore, int score)
    {
        await httpClient.PostAsJsonAsync($"api/submissions/{submissionId}/results?possible={maxScore}&score={score}", results);
    }
}
