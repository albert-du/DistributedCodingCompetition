namespace DistributedCodingCompetition.Web.Services;

using DistributedCodingCompetition.AuthService.Models;

public interface IAuthService
{
    Task<Guid?> TryRegister(string password);

    Task<LoginResult?> TryLogin(Guid id, string password, string userAgent, string ipAddress);

    Task<bool> ValidateToken(string token, Guid id);
}
