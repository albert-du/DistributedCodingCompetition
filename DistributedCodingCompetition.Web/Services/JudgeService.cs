namespace DistributedCodingCompetition.Web.Services;

/// <summary>
/// Service to judge submissions
/// </summary>
/// <param name="httpClient"></param>
/// <param name="logger"></param>
public class JudgeService(HttpClient httpClient, ILogger<JudgeService> logger) : IJudgeService
{
    /// <summary>
    /// Request judge to evaluate submission
    /// </summary>
    /// <param name="submissionId"></param>
    /// <returns></returns>
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
            return $"An error occured while juding this submission: {ex.StatusCode}";
        }
    }

    /// <summary>
    /// Rejudge the submission with the specified id.
    /// </summary>
    /// <param name="submissionId"></param>
    /// <returns></returns>
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
            return $"An error occured while rejuding this submission: {ex.StatusCode}";
        }
    }

    /// <summary>
    /// Rejudge the problem with the specified id.
    /// </summary>
    /// <param name="problemId"></param>
    /// <returns></returns>
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
            logger.LogError(ex, "Error while rejudging probkem {problemId}", problemId);
            return $"An error occured while rejuding this problem: {ex.StatusCode}";
        }
    }
}
