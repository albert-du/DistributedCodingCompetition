namespace DistributedCodingCompetition.AuthService.Services;

using DistributedCodingCompetition.AuthService.Models;

/// <summary>
/// Service for generating and validating tokens
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generate a token for a user.
    /// </summary>
    /// <param name="userAuth"></param>
    /// <returns></returns>
    Task<string> GenerateTokenAsync(UserAuth userAuth);
    
    /// <summary>
    /// Invalidate all tokens for a user.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task InvalidateUserTokensAsync(UserAuth userId);

    /// <summary>
    /// Validate a token and return the user it belongs to.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<UserAuth?> ValidateToken(string token);
}
