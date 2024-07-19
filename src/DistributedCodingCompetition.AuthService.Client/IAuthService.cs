namespace DistributedCodingCompetition.AuthService.Client;

/// <summary>
/// Authentication Service
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<Guid?> TryRegisterAsync(string email, string password);

    /// <summary>
    /// Try to login a user
    /// </summary>
    /// <param name="id"></param>
    /// <param name="password"></param>
    /// <param name="userAgent"></param>
    /// <param name="ipAddress"></param>
    /// <returns></returns>
    Task<LoginResult?> TryLoginAsync(Guid id, string password, string userAgent, string ipAddress);

    /// <summary>
    /// Validate a token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<ValidationResult?> ValidateTokenAsync(string token);

    /// <summary>
    /// Change a user's password
    /// </summary>
    /// <param name="id"></param>
    /// <param name="oldPassword"></param>
    /// <param name="newPassword"></param>
    /// <returns></returns>
    Task<bool> TryChangePasswordAsync(Guid id, string oldPassword, string newPassword);

    /// <summary>
    /// Resets a user's password
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newPassword"></param>
    /// <returns></returns>
    Task<bool> TryResetPasswordAsync(Guid id, string newPassword);
}
