namespace DistributedCodingCompetition.ApiService.Client;

/// <summary>
/// Service for interacting with problems.
/// </summary>
public interface IProblemsService
{
    /// <summary>
    /// Reads a paginated list of problems.
    /// </summary>
    /// <param name="page"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    Task<(bool, PaginateResult<ProblemResponseDTO>?)> TryReadProblemsAsync(int page = 1, int count = 50);

    /// <summary>
    /// Reads a problem by its id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<(bool, ProblemResponseDTO?)> TryReadProblemAsync(Guid id);

    /// <summary>
    /// Reads a paginated list of submissions for a problem.
    /// </summary>
    /// <param name="problemId"></param>
    /// <param name="page"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    Task<(bool, PaginateResult<SubmissionResponseDTO>?)> TryReadProblemSubmissionsAsync(Guid problemId, int page = 1, int count = 50);

    /// <summary>
    /// Updates a problem.
    /// </summary>
    /// <param name="problem"></param>
    /// <returns></returns>
    Task<bool> TryUpdateProblemAsync(ProblemRequestDTO problem);

    /// <summary>
    /// Creates a problem.
    /// </summary>
    /// <param name="problem"></param>
    /// <returns></returns>
    Task<(bool, ProblemResponseDTO?)> TryCreateProblemAsync(ProblemRequestDTO problem);

    /// <summary>
    /// Deletes a problem.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> TryDeleteProblemAsync(Guid id);

    /// <summary>
    /// Reads the list of test cases for a problem.
    /// </summary>
    /// <param name="problemId"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    Task<(bool, PaginateResult<TestCaseResponseDTO>?)> TryReadProblemTestCasesAsync(Guid problemId, int page = 1, int count = 50);

    /// <summary>
    /// Adds a test case to a problem.
    /// </summary>
    /// <param name="problemId"></param>
    /// <param name="testCaseId"></param>
    /// <returns></returns>
    Task<bool> TryAddProblemTestCaseAsync(Guid problemId, Guid testCaseId);
}