namespace DistributedCodingCompetition.Web.Services;

/// <summary>
/// Represents the current saved code provider
/// </summary>
public interface ICurrentSavedCodeProvider
{
    /// <summary>
    /// Get the current saved code
    /// </summary>
    /// <returns></returns>
    Task<SavedCode?> GetCurrentSavedCodeAsync();

    /// <summary>
    /// Save the current code
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    Task<bool> TrySaveCurrentCodeAsync(SavedCode code);
}
