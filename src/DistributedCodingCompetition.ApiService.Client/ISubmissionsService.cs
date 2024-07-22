namespace DistributedCodingCompetition.ApiService.Client;

/// <summary>
/// Service for interacting with submissions.
/// </summary>
public interface ISubmissionsService
{
    /// <summary>
    /// Reads a paginated list of submissions.
    /// </summary>
    /// <param name="page">page starting at 1</param>
    /// <param name="count">number of results</param>
    /// <param name="contestId">optional contestId</param>
    /// <param name="problemId">optional problemId</param>
    /// <param name="userId">optional userId</param>
    /// <returns></returns>
    Task<(bool, PaginateResult<SubmissionResponseDTO>?)> TryReadSubmissionsAsync(int page = 1, int count = 50, Guid? contestId = null, Guid? problemId = null, Guid? userId = null);

    /// <summary>
    /// Reads a submission by its id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<(bool, SubmissionResponseDTO?)> TryReadSubmissionAsync(Guid id);

    /// <summary>
    /// Creates a submission.
    /// </summary>
    /// <param name="submission"></param>
    /// <returns></returns>
    Task<(bool, SubmissionResponseDTO?)> TryCreateSubmissionAsync(SubmissionRequestDTO submission);

    /// <summary>
    /// Updates a submission with results.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="possible"></param>
    /// <param name="results"></param>
    /// <param name="score"></param>
    /// <returns></returns>
    Task<(bool, SubmissionResponseDTO?)> TryUpdateSubmissionResultsAsync(Guid id, IReadOnlyList<TestCaseResultDTO> results, int possible, int score);

    /// <summary>
    /// Reads the results of a submission.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<(bool, IReadOnlyList<TestCaseResultDTO>?)> TryReadSubmissionResultsAsync(Guid id);

    /// <summary>
    /// Invalidates a submission.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="invalidate"></param>
    /// <returns></returns>
    Task<bool> TryInvalidateSubmissionAsync(Guid id, bool invalidate = true);

    /// <summary>
    /// Revalidates a submission.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> TryRevalidateSubmissionAsync(Guid id);

    /// <summary>
    /// Deletes a submission.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> TryDeleteSubmissionAsync(Guid id);
}