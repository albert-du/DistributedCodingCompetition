
namespace DistributedCodingCompetition.ApiService.Client;

/// <inheritdoc/>
public class ProblemsService(HttpClient httpClient, ILogger<ProblemsService> logger) : IProblemsService
{
    private readonly ApiClient<ProblemsService> apiClient = new(httpClient, logger, "api/problems");

    /// <inheritdoc/>
    public Task<bool> TryAddProblemTestCaseAsync(Guid problemId, Guid testCaseId) =>
        apiClient.PostAsync($"/{problemId}/testcases/{testCaseId}");

    /// <inheritdoc/>
    public Task<(bool, ProblemResponseDTO?)> TryCreateProblemAsync(ProblemRequestDTO problem) =>
        apiClient.PostAsync<ProblemRequestDTO, ProblemResponseDTO>(data: problem);

    /// <inheritdoc/>
    public Task<bool> TryDeleteProblemAsync(Guid id) =>
        apiClient.DeleteAsync($"/{id}");

    /// <inheritdoc/>
    public Task<(bool, ProblemResponseDTO?)> TryReadProblemAsync(Guid id) =>
        apiClient.GetAsync<ProblemResponseDTO>($"/{id}");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<ProblemResponseDTO>?)> TryReadProblemsAsync(int page = 1, int count = 50) =>
        apiClient.GetAsync<PaginateResult<ProblemResponseDTO>>($"?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<SubmissionResponseDTO>?)> TryReadProblemSubmissionsAsync(Guid problemId, int page = 1, int count = 50) =>
        apiClient.GetAsync<PaginateResult<SubmissionResponseDTO>>($"/{problemId}/submissions?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<TestCaseResponseDTO>?)> TryReadProblemTestCasesAsync(Guid problemId, int page = 1, int count = 50) =>
        apiClient.GetAsync<PaginateResult<TestCaseResponseDTO>>($"/{problemId}/testcases?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<bool> TryUpdateProblemAsync(ProblemRequestDTO problem) =>
        apiClient.PutAsync($"/{problem.Id}", problem);
}