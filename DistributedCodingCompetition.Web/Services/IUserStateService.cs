namespace DistributedCodingCompetition.Web.Services;

/// <summary>
/// Get the current user.
/// </summary>
public interface IUserStateService
{
    /// <summary>
    /// Get the current user.
    /// </summary>
    /// <returns></returns>
    Task<UserResponseDTO?> UserAsync();
}