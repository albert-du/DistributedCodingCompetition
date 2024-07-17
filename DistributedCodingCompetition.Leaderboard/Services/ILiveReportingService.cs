namespace DistributedCodingCompetition.Leaderboard.Services;

using DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// Service for accessing the leaderboard
/// </summary>
public interface ILiveReportingService
{
    /// <summary>
    /// Refresh the leaderboard
    /// </summary>
    /// <param name="leaderboard"></param>
    /// <returns></returns>
    Task RefreshAsync(Leaderboard leaderboard);

    /// <summary>
    /// Get the leaders for a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <returns></returns>
    Task<IReadOnlyList<(Guid, int)>> GetLeadersAsync(Guid contestId);
}
