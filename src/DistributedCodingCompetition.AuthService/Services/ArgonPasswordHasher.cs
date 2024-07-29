using Microsoft.AspNetCore.Identity;

namespace DistributedCodingCompetition.AuthService.Services;

/// <summary>
/// Hashes and verifies passwords using Argon2.
/// </summary>
/// <param name="passwordService"></param>
public class ArgonPasswordHasher(IPasswordService passwordService) : IPasswordHasher<IdentityUser>
{
    /// <summary>
    /// Hashes a password using Argon2.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public string HashPassword(IdentityUser user, string password) =>
        passwordService.HashPassword(password);

    /// <summary>
    /// Verifies a password using Argon2.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="hashedPassword"></param>
    /// <param name="providedPassword"></param>
    /// <returns></returns>
    public PasswordVerificationResult VerifyHashedPassword(IdentityUser user, string hashedPassword, string providedPassword) =>
        passwordService.VerifyPassword(providedPassword, hashedPassword) switch
        {
            (false, _) => PasswordVerificationResult.Failed,
            (true, true) => PasswordVerificationResult.Success,
            (true, false) => PasswordVerificationResult.SuccessRehashNeeded,
        };
}
