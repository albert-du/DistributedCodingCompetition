
using System.Text;

namespace DistributedCodingCompetition.ApiService.Client;

/// <inheritdoc />
public class SubmissionsService(HttpClient httpClient, ILogger<SubmissionsService> logger) : ISubmissionsService
{
    private readonly ApiClient<SubmissionsService> apiClient = new(httpClient, logger, "api/submissions");

    /// <inheritdoc />
    public Task<(bool, SubmissionResponseDTO?)> TryCreateSubmissionAsync(SubmissionRequestDTO submission) =>
        apiClient.PostAsync<SubmissionRequestDTO, SubmissionResponseDTO>(string.Empty, submission);

    /// <inheritdoc />
    public Task<bool> TryDeleteSubmissionAsync(Guid id) =>
        apiClient.DeleteAsync($"/{id}");

    /// <inheritdoc />
    public Task<bool> TryInvalidateSubmissionAsync(Guid id, bool invalidate = true) =>
        apiClient.PostAsync($"/{id}/validate?valid={!invalidate}");

    /// <inheritdoc />
    public Task<(bool, SubmissionResponseDTO?)> TryReadSubmissionAsync(Guid id) =>
        apiClient.GetAsync<SubmissionResponseDTO>($"/{id}");

    /// <inheritdoc />
    public Task<(bool, PaginateResult<SubmissionResponseDTO>?)> TryReadSubmissionsAsync(int page = 1, int count = 50, Guid? contestId = null, Guid? problemId = null, Guid? userId = null)
    {
        StringBuilder sb = new($"?page={page}&count={count}");
        if (contestId is not null) sb.Append($"&contestId={contestId}");
        if (problemId is not null) sb.Append($"&problemId={problemId}");
        if (userId is not null) sb.Append($"&userId={userId}");
        return apiClient.GetAsync<PaginateResult<SubmissionResponseDTO>>(sb.ToString());
    }

    /// <inheritdoc />
    public Task<bool> TryRevalidateSubmissionAsync(Guid id) =>
        TryInvalidateSubmissionAsync(id, false);

    /// <inheritdoc />
    public Task<(bool, SubmissionResponseDTO?)> TryUpdateSubmissionResultsAsync(Guid id, IEnumerable<TestCaseResultDTO> results, int possible, int score) =>
        apiClient.PostAsync<IEnumerable<TestCaseResultDTO>, SubmissionResponseDTO>($"{id}/results?possible={possible}&score={score}", results);

    public Task<(bool, IReadOnlyList<TestCaseResultDTO>?)> TryReadSubmissionResultsAsync(Guid id) =>
        apiClient.GetAsync<IReadOnlyList<TestCaseResultDTO>>($"{id}/results");
}