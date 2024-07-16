namespace DistributedCodingCompetition.ApiService.Client;

/// <summary>
/// Service for interacting with contests.
/// </summary>
public interface IContestsService
{
    /// <summary>
    /// Reads a paginated list of contests.
    /// </summary>
    /// <param name="page">starting a 1</param>
    /// <param name="count">number of contests to see</param>
    /// <returns></returns>
    Task<(bool, PaginateResult<ContestResponseDTO>?)> TryReadContestsAsync(int page, int count = 50);

    /// <summary>
    /// Reads a contest by its id.
    /// </summary>
    /// <param name="id">Contest's id</param>
    /// <returns>success, contest model</returns>
    Task<(bool, ContestResponseDTO?)> TryReadContestAsync(Guid id);

    /// <summary>
    /// Reads a contest by its join code code.
    /// </summary>
    /// <param name="code">code</param>
    /// <returns>success, contest</returns>
    Task<(bool, ContestResponseDTO?)> TryReadContestByJoinCodeAsync(string code);

    /// <summary>
    /// Reads a paginated list of admins for a contest.
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="page"></param>
    /// <param name="count"></param>
    /// <returns>success, admins</returns>
    Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadContestAdmins(Guid contestId, int page, int count);

    /// <summary>
    /// Reads a paginated list of banned users for a contest.
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="page"></param>
    /// <param name="count"></param>
    /// <returns>success, banned</returns>
    Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadContestBanned(Guid contestId, int page, int count);

    /// <summary>
    /// Reads a paginated list of participants for a contest.
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="page"></param>
    /// <param name="count"></param>
    /// <returns>success, participants</returns>
    Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadContestParticipants(Guid contestId, int page, int count);

    /// <summary>
    /// Reads a list of join codes for a contest.
    /// </summary>
    /// <param name="contestId"></param>
    /// <returns>success, join codes</returns>
    Task<(bool, IReadOnlyList<JoinCodeResponseDTO>?)> TryReadContestJoinCodesAsync(Guid contestId);

    /// <summary>
    /// Reads a user's role in a contest.
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="userId"></param>
    /// <returns>success, roll if exists</returns>
    Task<(bool, ContestRole?)> TryReadContestUserRoleAsync(Guid contestId, Guid userId);

    /// <summary>
    /// Reads a paginated list of public contests.
    /// </summary>
    /// <param name="page"></param>
    /// <param name="count"></param>
    /// <returns>success, contests</returns>
    Task<(bool, PaginateResult<ContestResponseDTO>?)> TryReadPublicContestsAsync(int page, int count);

    /// <summary>
    /// Reads a list of problems for a contest.
    /// </summary>
    /// <param name="contestId"></param>
    /// <returns>success, list of problems</returns>
    Task<(bool, IReadOnlyList<ProblemResponseDTO>?)> TryReadContestProblemsAsync(Guid contestId);

    /// <summary>
    /// Reads a user's solve status for a contest.
    /// </summary>
    /// <param name="contestId">Contest Id</param>
    /// <param name="userId">User Id</param>
    /// <returns>success, list of solve statuses</returns>
    Task<(bool, IReadOnlyList<ProblemUserSolveStatus>?)> TryReadContestUserSolveStatusAsync(Guid contestId, Guid userId);

    /// <summary>
    /// Reads a user's solve status for a problem in a contest.
    /// </summary>
    /// <param name="contestId">contest id</param>
    /// <param name="problemId">problem id</param>
    /// <param name="userId">user id</param>
    /// <returns>success, solve status</returns>
    Task<(bool, ProblemUserSolveStatus?)> TryReadContestProblemUserSolveStatusAsync(Guid contestId, Guid problemId, Guid userId);

    /// <summary>
    /// Reads a list of problem point values for a contest.
    /// </summary>
    /// <param name="contestId"></param>
    /// <returns>success, point values</returns>
    Task<(bool, IReadOnlyList<ProblemPointValueResponseDTO>?)> TryReadContestProblemPointValuesAsync(Guid contestId);

    /// <summary>
    /// Reads a problem point value for a contest.
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="problemId"></param>
    /// <returns></returns>
    Task<(bool, ProblemPointValueResponseDTO?)> TryReadContestProblemPointValueAsync(Guid contestId, Guid problemId);

    /// <summary>
    /// Updates a problem point value for a contest.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    Task<(bool, ProblemPointValueResponseDTO?)> TryUpdateContestProblemPointValueAsync(ProblemPointValueRequestDTO data);

    /// <summary>
    /// Creates a problem point value for a contest.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    Task<(bool, ProblemPointValueResponseDTO?)> TryCreateContestProblemPointValueAsync(ProblemPointValueRequestDTO data);

    /// <summary>
    /// DO NOT CALL FROM CLIENT APP CODE
    /// 
    /// This endpoint is intended to be used by the leaderboard service only.
    /// </summary>
    /// <param name="contestId"></param>
    /// <returns></returns>
    Task<(bool, Leaderboard?)> TryReadContestLeaderboardAsync(Guid contestId); 

    /// <summary>
    /// Updates a user's role in a contest.
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="userId"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    Task<bool> TryUpdateUserContestRoleAsync(Guid contestId, Guid userId, ContestRole role);
}