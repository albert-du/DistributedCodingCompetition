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
}
