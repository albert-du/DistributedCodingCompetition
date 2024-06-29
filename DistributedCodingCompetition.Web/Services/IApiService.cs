namespace DistributedCodingCompetition.Web.Services;

using DistributedCodingCompetition.ApiService.Models;

public interface IApiService
{
    Task<bool> TryUserByEmailAsync(string email, out User? user);
}
