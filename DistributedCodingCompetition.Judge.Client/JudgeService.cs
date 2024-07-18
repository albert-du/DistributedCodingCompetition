namespace DistributedCodingCompetition.Judge.Client;

/// <inheritdoc/>
public sealed class JudgeService(HttpClient httpClient, ILogger<JudgeService> logger) : IJudgeService
{
    /// <inheritdoc/>
    public async Task<string?> JudgeAsync(Guid submissionId)
    {
        var response = await httpClient.PostAsync("evaluation?submissionId=" + submissionId, null);
        if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            return "Please wait before trying again";

        try
        {
            response.EnsureSuccessStatusCode();
            return null;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error while judging submission {SubmissionId}", submissionId);
            return $"An error occurred while judging this submission: {ex.StatusCode}";
        }
    }

    /// <inheritdoc/>
    public async Task<string?> RejudgeAsync(Guid submissionId)
    {
        var response = await httpClient.PostAsync("evaluation/rejudge?submissionId=" + submissionId, null);

        try
        {
            response.EnsureSuccessStatusCode();
            return null;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error while rejudging submission {SubmissionId}", submissionId);
            return $"An error occurred while rejudging this submission: {ex.StatusCode}";
        }
    }

    /// <inheritdoc/>
    public async Task<string?> RejudgeProblemAsync(Guid problemId)
    {
        var response = await httpClient.PostAsync("evaluation/problem?problemId=" + problemId, null);

        try
        {
            response.EnsureSuccessStatusCode();
            return null;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error while rejudging problem {problemId}", problemId);
            return $"An error occurred while rejudging this problem: {ex.StatusCode}";
        }
    }
}
