namespace DistributedCodingCompetition.Judge.Client;

/// <summary>
/// Interface with external judge service
/// </summary>
public interface IJudgeService
{
    /// <summary>
    /// Starts judging the submission with the specified id.
    /// </summary>
    /// <param name="submissionId">Submission's identifier to use</param>
    /// <returns>string with error if any</returns>
    Task<string?> JudgeAsync(Guid submissionId);

    /// <summary>
    /// Rejudge the submission with the specified id.
    /// </summary>
    /// <param name="submissionId"></param>
    /// <returns></returns>
    Task<string?> RejudgeAsync(Guid submissionId);

    /// <summary>
    /// Rejudge the problem with the specified id.
    /// </summary>
    /// <param name="problemId"></param>
    /// <returns></returns>
    Task<string?> RejudgeProblemAsync(Guid problemId);
}
