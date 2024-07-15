namespace DistributedCodingCompetition.Judge.Services;

/// <summary>
/// Service for accessing problems
/// </summary>
public interface IProblemService
{
    /// <summary>
    /// Read a problem
    /// </summary>
    /// <param name="problemId"></param>
    /// <returns></returns>
    Task<Problem?> ReadProblemAsync(Guid problemId);

    /// <summary>
    /// Read all problems
    /// </summary>
    /// <param name="problemId"></param>
    /// <returns></returns>
    Task<IReadOnlyList<TestCase>> ReadTestCasesAsync(Guid problemId);
}
