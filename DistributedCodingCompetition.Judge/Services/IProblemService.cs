namespace DistributedCodingCompetition.Judge.Services;

using DistributedCodingCompetition.ApiService.Models;

public interface IProblemService
{
    Task<Problem?> ReadProblemAsync(Guid problemId);
    Task<IReadOnlyList<TestCase>> ReadTestCasesAsync(Guid problemId);
}
