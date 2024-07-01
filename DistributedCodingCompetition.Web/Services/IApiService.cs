namespace DistributedCodingCompetition.Web.Services;

using DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// 
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
    Task<(bool, IReadOnlyList<User>?)> TryReadContestAdminAsync(Guid contestId);

}
