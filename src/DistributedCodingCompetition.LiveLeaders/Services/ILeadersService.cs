namespace DistributedCodingCompetition.LiveLeaders.Services;

/// <summary>
/// Service for accessing the leaders
/// </summary>
public interface ILeadersService
{
    /// <summary>
    /// Send the refreshed leaderboard to the live leaderboard service.
    /// </summary>
    /// <param name="contest">id</param>
    /// <param name="leaders">only takes 100</param>
    /// <param name="sync">leaderboard time</param>
    /// <returns></returns>
    Task RefreshLeaderboardAsync(Guid contest, IReadOnlyList<(Guid, int)> leaders, DateTime sync);

    /// <summary>
    /// Report that a submission has been judged.
    /// </summary>
    /// <param name="contest"></param>
    /// <param name="leader"></param>
    /// <param name="points"></param>
    /// <param name="sync"></param>
    /// <returns></returns>
    Task ReportJudgingAsync(Guid contest, Guid leader, int points, DateTime sync);

    /// <summary>
    /// Get the leaders for a contest.
    /// </summary>
    /// <param name="contest"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    Task<IReadOnlyList<(Guid, int)>> GetLeadersAsync(Guid contest, int count);
}
