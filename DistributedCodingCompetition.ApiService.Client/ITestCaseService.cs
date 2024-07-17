namespace DistributedCodingCompetition.ApiService.Client;

/// <summary>
/// Service for interacting with test cases.
/// </summary>
public interface ITestCasesService
{
    /// <summary>
    /// Reads a paginated list of test cases.
    /// </summary>
    /// <param name="page"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    Task<(bool, PaginateResult<TestCaseResponseDTO>?)> TryReadProblemTestCasesAsync(int page = 1, int count = 50);

    /// <summary>
    /// Reads a test case by its id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<(bool, TestCaseResponseDTO?)> TryReadTestCaseAsync(Guid id);

    /// <summary>
    /// Updates a test case.
    /// </summary>
    /// <param name="testCase"></param>
    /// <returns></returns>
    Task<bool> TryUpdateTestCaseAsync(TestCaseRequestDTO testCase);

    /// <summary>
    /// Creates a test case.
    /// </summary>
    /// <param name="testCase"></param>
    /// <returns></returns>
    Task<(bool, TestCaseResponseDTO?)> TryCreateTestCaseAsync(TestCaseRequestDTO testCase);

    /// <summary>
    /// Deletes a test case.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> TryDeleteTestCaseAsync(Guid id);
}