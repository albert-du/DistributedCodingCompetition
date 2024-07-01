namespace DistributedCodingCompetition.Web.Services;

using DistributedCodingCompetition.AuthService.Models;

public interface IAuthService
{
    Task<Guid?> TryRegisterAsync(string email, string password);

    Task<LoginResult?> TryLoginAsync(Guid id, string password, string userAgent, string ipAddress);

    Task<ValidationResult?> ValidateTokenAsync(string token);
}
