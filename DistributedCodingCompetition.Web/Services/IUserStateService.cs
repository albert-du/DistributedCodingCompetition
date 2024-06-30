namespace DistributedCodingCompetition.Web.Services;

using DistributedCodingCompetition.ApiService.Models;

public interface IUserStateService
{
    Task<User?> UserAsync();
    Task UpdateUserAsync(User user);
    event EventHandler<User?> OnChange;
}