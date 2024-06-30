namespace DistributedCodingCompetition.Web.Services;

using DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// 
/// </summary>
public interface IApiService
{
    Task<bool> TryCreateUserAsync(User user);
    Task<(bool, User?)> TryReadUserAsync(Guid id);
    Task<(bool,User?)> TryReadUserByEmailAsync(string email);
    Task<(bool,User?)> TryReadUserByUsername(string username);
    Task<bool> TryUpdateUserAsync(User user);
}
