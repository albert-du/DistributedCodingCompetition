namespace DistributedCodingCompetition.Judge.Services;

public class LiveReportingService(HttpClient httpClient) : ILiveReportingService
{
    public Task ReportAsync(Guid contestId, Guid userId, int points) =>
        httpClient.PostAsync($"report/{contestId}/{userId}?points={points}&sync={DateTime.UtcNow:O}", null);

}
