namespace DistributedCodingCompetition.LiveLeaders.Client;

/// <summary>
/// Interface with external live reporting service
/// </summary>
public interface ILiveReportingService
{
    /// <summary>
    /// Report the points for the user in the contest.
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="userId"></param>
    /// <param name="points"></param>
    /// <returns></returns>
    Task ReportAsync(Guid contestId, Guid userId, int points);

    /// <summary>
    /// Refresh the leaderboard.
    /// </summary>
    /// <param name="leaderboard"></param>
    /// <returns></returns>
    Task RefreshAsync(Leaderboard leaderboard);

    /// <summary>
    /// Get the leaders of the contest.
    /// </summary>
    /// <param name="contestId"></param>
    /// <returns></returns>
    Task<IReadOnlyList<(Guid, int)>> GetLeadersAsync(Guid contestId);
}
