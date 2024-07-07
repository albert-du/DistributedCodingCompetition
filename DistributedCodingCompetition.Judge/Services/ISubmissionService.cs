namespace DistributedCodingCompetition.Judge.Services;

using DistributedCodingCompetition.ApiService.Models;

public interface ISubmissionService
{
    Task<Submission?> ReadSubmissionAsync(Guid submissionId);
    Task UpdateSubmissionResults(Guid submissionId, IReadOnlyList<TestCaseResult> results, int maxScore, int score);
}
