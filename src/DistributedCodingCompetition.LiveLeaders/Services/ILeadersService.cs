namespace DistributedCodingCompetition.LiveLeaders.Services;

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

    Task ReportJudgingAsync(Guid contest, Guid leader, int points, DateTime sync);

    Task<IReadOnlyList<(Guid, int)>> GetLeadersAsync(Guid contest, int count);
}
