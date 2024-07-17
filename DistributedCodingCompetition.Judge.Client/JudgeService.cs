namespace DistributedCodingCompetition.Judge.Client;

/// <inheritdoc/>
public sealed class JudgeService : IJudgeService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<JudgeService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="JudgeService"/> class.
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="logger"></param>
    internal JudgeService(HttpClient httpClient, ILogger<JudgeService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<string?> JudgeAsync(Guid submissionId)
    {
        var response = await _httpClient.PostAsync("evaluation?submissionId=" + submissionId, null);
        if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            return "Please wait before trying again";

        try
        {
            response.EnsureSuccessStatusCode();
            return null;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error while judging submission {SubmissionId}", submissionId);
            return $"An error occured while juding this submission: {ex.StatusCode}";
        }
    }

    /// <inheritdoc/>
    public async Task<string?> RejudgeAsync(Guid submissionId)
    {
        var response = await _httpClient.PostAsync("evaluation/rejudge?submissionId=" + submissionId, null);

        try
        {
            response.EnsureSuccessStatusCode();
            return null;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error while rejudging submission {SubmissionId}", submissionId);
            return $"An error occured while rejuding this submission: {ex.StatusCode}";
        }
    }

    /// <inheritdoc/>
    public async Task<string?> RejudgeProblemAsync(Guid problemId)
    {
        var response = await _httpClient.PostAsync("evaluation/problem?problemId=" + problemId, null);

        try
        {
            response.EnsureSuccessStatusCode();
            return null;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error while rejudging probkem {problemId}", problemId);
            return $"An error occured while rejuding this problem: {ex.StatusCode}";
        }
    }
}
