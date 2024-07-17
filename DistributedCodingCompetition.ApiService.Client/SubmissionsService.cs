namespace DistributedCodingCompetition.ApiService.Client;

/// <inheritdoc />
public class SubmissionsService : ISubmissionsService
{
    private readonly ApiClient<SubmissionsService> apiClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubmissionsService"/> class.
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="logger"></param>
    internal SubmissionsService(HttpClient httpClient, ILogger<SubmissionsService> logger) =>
        apiClient = new(httpClient, logger, "api/submissions");

    
}