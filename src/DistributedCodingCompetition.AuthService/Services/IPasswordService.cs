namespace DistributedCodingCompetition.AuthService.Services;

/// <summary>
/// Password Hashing implementation
/// </summary>
public interface IPasswordService
{
    /// <summary>
    /// Hashes password.
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    string HashPassword(string password);

    /// <summary>
    /// Verifies password against hash.
    /// </summary>
    /// <param name="password"></param>
    /// <param name="hash"></param>
    /// <returns>Bool, true if passing, and optionally a new hash if needed.</returns>
    (bool Success, bool NeedsRehash) VerifyPassword(string password, string hash);
}
