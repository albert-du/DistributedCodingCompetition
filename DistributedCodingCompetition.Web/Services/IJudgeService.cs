namespace DistributedCodingCompetition.Web.Services;

public interface IJudgeService
{
    /// <summary>
    /// Starts judging the submission with the specified id.
    /// </summary>
    /// <param name="submissionId">Submission's identifier to use</param>
    /// <returns>string with error if any</returns>
    Task<string?> JudgeAsync(Guid submissionId);
}
