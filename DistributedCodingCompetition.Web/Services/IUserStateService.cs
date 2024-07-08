namespace DistributedCodingCompetition.Web.Services;

using DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// Get the current user.
/// </summary>
public interface IUserStateService
{
    /// <summary>
    /// Get the current user.
    /// </summary>
    /// <returns></returns>
    Task<User?> UserAsync();

    /// <summary>
    /// Update the current user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task UpdateUserAsync(User user);

    /// <summary>
    /// Event that is triggered when the user changes.
    /// </summary>
    event EventHandler<User?> OnChange;
}