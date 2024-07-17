namespace DistributedCodingCompetition.Judge.Client;

public sealed class JudgeService : IJudgeService
{
    public async Task<string?> JudgeAsync(Guid submissionId);

    public async Task<string?> RejudgeAsync(Guid submissionId);

    public async Task<string?> RejudgeProblemAsync(Guid problemId);
}
