namespace DistributedCodingCompetition.Web.Services;

using DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// API service for interacting with the contest API service.
/// </summary>
public interface IApiService
{
    /// <summary>
    /// Create a new User.
    /// </summary>
    /// <param name="user">User to create</param>
    /// <returns>success</returns>
    Task<bool> TryCreateUserAsync(User user);

    /// <summary>
    /// Read a User by their ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>success in reaching endpoint; null if doesn't exist</returns>
    Task<(bool, User?)> TryReadUserAsync(Guid id);

    /// <summary>
    /// Read a User by their email.
    /// </summary>
    /// <param name="email"></param>
    /// <returns>success in reaching endpoint; null if doesn't exist</returns>
    Task<(bool, User?)> TryReadUserByEmailAsync(string email);

    /// <summary>
    /// Read a User by their username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns>success in reaching endpoint; null if doesn't exist</returns>
    Task<(bool, User?)> TryReadUserByUsername(string username);

    /// <summary>
    /// Update a User.
    /// </summary>
    /// <param name="user"></param>
    /// <returns>success</returns>
    Task<bool> TryUpdateUserAsync(User user);

    /// <summary>
    /// Read contest by join code.
    /// </summary>
    /// <param name="code"></param>
    /// <returns>success in reaching endpoint; null if doesn't exist</returns>
    Task<(bool, Contest?)> TryReadContestByJoinCodeAsync(string code);

    /// <summary>
    /// Read a contest by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>success in reaching endpoint; null if doesn't exist</returns>
    Task<(bool, Contest?)> TryReadContestAsync(Guid id);

    /// <summary>
    /// Read join codes for a contest.
    /// </summary>
    /// <param name="contestId"></param>
    /// <returns>success in reaching endpoint; null or emtpy if doesn't exist</returns>
    Task<(bool, IReadOnlyList<JoinCode>?)> TryReadJoinCodesAsync(Guid contestId);

    /// <summary>
    /// success in reaching endpoint; null if doesn't exist
    /// </summary>
    /// <param name="contestId"></param>
    /// <returns>success in reaching endpoint; null if doesn't exist</returns>
    Task<(bool, IReadOnlyList<User>?)> TryReadContestAdminsAsync(Guid contestId);

    /// <summary>
    /// Update Contest
    /// </summary>
    /// <param name="contest"></param>
    /// <returnssuccess in reaching endpoint; null if doesn't exist></returns>
    Task<(bool, Contest?)> TryUpdateContestAsync(Contest contest);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="count"></param>
    /// <param name="page">start at 0</param>
    /// <returns></returns>
    Task<(bool, IReadOnlyList<Submission>?)> TryReadContestSubmissionsAsync(Guid contestId, int count, int page);

    /// <summary>
    /// Read a user's contest role.
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="userId"></param>
    /// <returns>success in reaching endpoint; null if doesn't exist</returns>
    Task<(bool, ContestRole?)> TryReadUserContestRoleAsync(Guid contestId, Guid userId);

    /// <summary>
    /// Create a new problem
    /// </summary>
    /// <param name="problem"></param>
    /// <returns>success in reaching endpoint; null if doesn't exist</returns>
    Task<(bool, Guid?)> TryCreateProblemAsync(Problem problem);

    /// <summary>
    /// Create a new contest
    /// </summary>
    /// <param name="contest"></param>
    /// <returns>success in reaching endpoint; null if doesn't exist</returns>
    Task<(bool, Guid?)> TryCreateContestAsync(Contest contest);

    /// <summary>
    /// Update Problem
    /// </summary>
    /// <param name="contest"></param>
    /// <returns>success in reaching endpoint; null if doesn't exist></returns>
    Task<(bool, Problem?)> TryUpdateProblemAsync(Problem contest);

    /// <summary>
    /// Read a problem by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<(bool, Problem?)> TryReadProblemAsync(Guid id);

    /// <summary>
    /// Read a problem by its name.
    /// </summary>
    /// <param name="problemId"></param>
    /// <returns></returns>
    Task<(bool, IReadOnlyList<TestCase>?)> TryReadProblemTestCases(Guid problemId);
}
