namespace DistributedCodingCompetition.Judge.Services;

public interface IProblemPointValueService
{
    Task<int> GetPointMaxAsync(Guid contestId, Guid problemId);
}
