namespace DistributedCodingCompetition.Web.Services;

using DistributedCodingCompetition.ApiService.Models;

public interface IApiService
{
    Task<(bool,User?)> TryUserByEmailAsync(string email);
    Task<bool> TryCreateUserAsync(User user);
}
