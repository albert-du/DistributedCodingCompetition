
namespace DistributedCodingCompetition.ApiService.Client;

/// <inheritdoc/>
public sealed class TestCaseService : ITestCaseService
{
    private readonly ApiClient<TestCaseService> apiClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestCaseService"/> class.
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="logger"></param>
    internal TestCaseService(HttpClient httpClient, ILogger<TestCaseService> logger) =>
        apiClient = new(httpClient, logger, "api/testcases");

    /// <inheritdoc/>
    public Task<(bool, TestCaseResponseDTO?)> TryCreateTestCaseAsync(TestCaseRequestDTO testCase) =>
        apiClient.PostAsync<TestCaseRequestDTO, TestCaseResponseDTO>(data: testCase);

    /// <inheritdoc/>
    public Task<bool> TryDeleteTestCaseAsync(Guid id) =>
        apiClient.DeleteAsync($"/{id}");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<TestCaseResponseDTO>?)> TryReadProblemTestCasesAsync(int page = 1, int count = 50) =>
        apiClient.GetAsync<PaginateResult<TestCaseResponseDTO>>($"?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<(bool, TestCaseResponseDTO?)> TryReadTestCaseAsync(Guid id) =>
        apiClient.GetAsync<TestCaseResponseDTO>($"/{id}");

    /// <inheritdoc/>
    public Task<bool> TryUpdateTestCaseAsync(TestCaseRequestDTO testCase) =>
        apiClient.PutAsync($"/{testCase.Id}", testCase);
}