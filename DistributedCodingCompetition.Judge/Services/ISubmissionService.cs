namespace DistributedCodingCompetition.Judge.Services;

using DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// Service for accessing submissions
/// </summary>
public interface ISubmissionService
{
    /// <summary>
    /// Read a submission
    /// </summary>
    /// <param name="submissionId"></param>
    /// <returns></returns>
    Task<Submission?> ReadSubmissionAsync(Guid submissionId);
    
    /// <summary>
    /// Update a submission with scoring information
    /// </summary>
    /// <param name="submissionId"></param>
    /// <param name="results"></param>
    /// <param name="maxScore"></param>
    /// <param name="score"></param>
    /// <returns></returns>
    Task UpdateSubmissionResults(Guid submissionId, IReadOnlyList<TestCaseResult> results, int maxScore, int score);
}
