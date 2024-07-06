namespace DistributedCodingCompetition.Judge.Services;

using DistributedCodingCompetition.ApiService.Models;
using DistributedCodingCompetition.ExecutionShared;

public interface ISubmissionService
{
    Task<Submission> ReadSubmissionAsync(Guid submissionId);
    Task UpdateSubmissionResults(Guid submissionId, IReadOnlyList<ExecutionResult> results);
}
