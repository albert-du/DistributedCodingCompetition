namespace DistributedCodingCompetition.Judge.Services;

using DistributedCodingCompetition.ApiService.Models;
using DistributedCodingCompetition.ExecutionShared;

public class SubmissionService(HttpClient httpClient) : ISubmissionService
{
    public async Task<Submission> ReadSubmissionAsync(Guid submissionId)
    {

    }

    public async Task UpdateSubmissionResults(Guid submissionId, IReadOnlyList<ExecutionResult> results)
    {

    }

}
