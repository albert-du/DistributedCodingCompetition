namespace DistributedCodingCompetition.Web.Services;

using DistributedCodingCompetition.ApiService.Models;

public interface IApiService
{
    Task<User> UserByEmailAsync(string email);
}
