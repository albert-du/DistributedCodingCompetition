namespace DistributedCodingCompetition.Web.Services;

using DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// API service for interacting with the contest API service.
/// </summary>
public interface IApiService
{
    Task<bool> TryCreateUserAsync(User user);
    Task<(bool, User?)> TryReadUserAsync(Guid id);
    Task<(bool, User?)> TryReadUserByEmailAsync(string email);
    Task<(bool, User?)> TryReadUserByUsername(string username);
    Task<bool> TryUpdateUserAsync(User user);

    Task<(bool, Contest?)> TryReadContestByJoinCodeAsync(string code);

    Task<(bool, Contest?)> TryReadContestAsync(Guid id);
    Task<(bool, IReadOnlyList<JoinCode>?)> TryReadJoinCodesAsync(Guid contestId);
    Task<(bool, IReadOnlyList<User>?)> TryReadContestAdminsAsync(Guid contestId);
    Task<(bool, Contest?)> TryUpdateContestAsync(Contest contest);
    Task<(bool, IReadOnlyList<Submission>?)> TryReadContestSubmissionsAsync(Guid contestId, int count, int page);
    Task<(bool, ContestRole?)> TryReadUserContestRoleAsync(Guid contestId, Guid userId);
    Task<bool> TryCreateProblemAsync(Problem problem);
}
