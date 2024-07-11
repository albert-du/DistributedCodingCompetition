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
    Task<(bool, JoinCode?)> TryReadJoinCodeAsync(Guid contestId);
    Task<(bool, JoinCode?)> TryReadJoinCodeAsync(string code);

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

    /// <summary>
    /// Updates a user's status within a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="userId"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    Task<bool> TryUpdateUserContestRoleAsync(Guid contestId, Guid userId, ContestRole role);

    /// <summary>
    /// Read a contest's participants.
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="count"></param>
    /// <param name="page">start at 1</param>
    /// <returns></returns>
    Task<(bool, IReadOnlyList<User>?)> TryReadContestParticipantsAsync(Guid contestId, int count, int page);

    /// <summary>
    /// Read a contest's banned users.
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    Task<(bool, IReadOnlyList<User>?)> TryReadContestBannedAsync(Guid contestId, int count, int page);

    /// <summary>
    /// Reads the contests that a user is an admin of.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    Task<(bool, IReadOnlyList<Contest>?)> TryReadUserAdministratedContestsAsync(Guid userId, int count, int page);

    /// <summary>
    /// Reads the contests that a user is a participant of.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    Task<(bool, IReadOnlyList<Contest>?)> TryReadUserEnteredContestsAsync(Guid userId, int count, int page);

    /// <summary>
    /// Reads publically joinable contests.
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    Task<(bool, IReadOnlyList<Contest>?)> TryReadPublicContestsAsync(int count, int page);

    /// <summary>
    /// Read a contest join codes.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<(bool, IReadOnlyList<JoinCode>?)> TryReadContestJoinCodesAsync(Guid id);

    /// <summary>
    /// Delete a join code.
    /// </summary>
    /// <param name="joinCodeId"></param>
    /// <returns></returns>
    Task<bool> TryDeleteJoinCodeAsync(Guid joinCodeId);

    /// <summary>
    /// Update a join code 
    /// </summary>
    /// <param name="joinCode"></param>
    /// <returns></returns>
    Task<bool> TryUpdateJoinCodeAsync(JoinCode joinCode);

    /// <summary>
    /// Create a new join code.
    /// </summary>
    /// <param name="joinCode"></param>
    /// <returns></returns>
    Task<(bool, Guid?)> TryCreateJoinCodeAsync(JoinCode joinCode);

    /// <summary>
    /// Add a problem to a contest.
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="problem"></param>
    /// <returns></returns>
    Task<bool> TryAddProblemToContestAsync(Guid contestId, Problem problem);

    /// <summary>
    /// Create a new problem test case.
    /// </summary>
    /// <param name="testCase"></param>
    /// <returns></returns>
    Task<(bool, Guid?)> TryCreateProblemTestCaseAsync(TestCase testCase);

    /// <summary>
    /// Read a problem's test cases.
    /// </summary>
    /// <param name="contestId"></param>
    /// <returns></returns>
    Task<(bool, IReadOnlyList<Problem>?)> TryReadContestProblemsAsync(Guid contestId);

    /// <summary>
    /// Delete a problem.
    /// </summary>
    /// <param name="problemId"></param>
    /// <returns></returns>
    Task<bool> TryDeleteProblemAsync(Guid problemId);


    /// <summary>
    /// Add a test case to a problem.
    /// </summary>
    /// <param name="problemId"></param>
    /// <param name="testCase"></param>
    /// <returns></returns>
    Task<bool> TryAddTestCaseToProblemAsync(Guid problemId, TestCase testCase);

    /// <summary>
    /// Delete a test case.
    /// </summary>
    /// <param name="testCaseId"></param>
    /// <returns></returns>
    Task<bool> TryDeleteTestCaseAsync(Guid testCaseId);

    /// <summary>
    /// Read a test case.
    /// </summary>
    /// <param name="testCaseId"></param>
    /// <returns></returns>
    Task<(bool, TestCase?)> TryReadTestCaseAsync(Guid testCaseId);

    /// <summary>
    /// Update a test case.
    /// </summary>
    /// <param name="testCase"></param>
    /// <returns></returns>
    Task<bool> TryUpdateTestCaseAsync(TestCase testCase);

    /// <summary>
    /// Read a contest's problems.
    /// </summary>
    /// <param name="joinCodeId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<bool> TryJoinContestAsync(Guid joinCodeId, Guid userId);

    /// <summary>
    /// Read a user's solve status for a contest.
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<(bool, IReadOnlyList<ProblemUserSolveStatus>?)> TryReadUserSolveStatusForContestAsync(Guid contestId, Guid userId);


    /// <summary>
    /// Read a user's solve status for a problem.
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="userId"></param>
    /// <param name="problemId"></param>
    /// <returns></returns>
    Task<(bool, ProblemUserSolveStatus?)> TryReadUserSolveStatusAsync(Guid contestId, Guid userId, Guid problemId);

    /// <summary>
    /// Create a new submission.
    /// Does not solve automatically
    /// </summary>
    /// <param name="submission"></param>
    /// <returns></returns>
    Task<bool> TryCreateSubmissionAsync(Submission submission);

    /// <summary>
    /// Read submissions from a user for a problem.
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="problemId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<(bool, IReadOnlyList<Submission>?)> TryReadUserProblemSubmissionsAsync(Guid contestId, Guid problemId, Guid userId, int page, int pageSize);

    /// <summary>
    /// Read a submission.
    /// </summary>
    /// <param name="submission"></param>
    /// <returns></returns>
    Task<bool> TryUpdateSubmissionAsync(Submission submission);

    /// <summary>
    /// Read a submission.
    /// </summary>
    /// <param name="submissionId"></param>
    /// <returns></returns>
    Task<(bool, Submission?)> TryReadSubmissionAsync(Guid submissionId);

    /// <summary>
    /// Read a submission's test case results.
    /// </summary>
    /// <param name="submissionId"></param>
    /// <returns></returns>
    Task<(bool, IReadOnlyList<TestCaseResult>?)> TryReadTestCaseResultsAsync(Guid submissionId);


    /// <summary>
    /// Read a user's submissions for a contest.
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="userId"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(bool, IReadOnlyList<Submission>?)> TryReadUserContestSubmissionsAsync(Guid contestId, Guid userId, int page, int pageSize);
}
