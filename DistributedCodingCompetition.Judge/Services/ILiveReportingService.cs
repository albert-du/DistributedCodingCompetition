namespace DistributedCodingCompetition.Judge.Services;

public interface ILiveReportingService
{
    Task ReportAsync(Guid contestId, Guid userId, int points);
}
