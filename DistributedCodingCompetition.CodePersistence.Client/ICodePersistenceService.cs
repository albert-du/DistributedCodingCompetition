namespace DistributedCodingCompetition.CodePersistence.Client;

/// <summary>
/// Code persistence service
/// </summary>
public interface ICodePersistenceService
{
    /// <summary>
    /// Try to read the code for a user
    /// </summary>
    /// <param name="contest"></param>
    /// <param name="problem"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<SavedCode?> TryReadCodeAsync(Guid contest, Guid problem, Guid user);
    
    /// <summary>
    /// Try to save the code for a user
    /// </summary>
    /// <param name="contest"></param>
    /// <param name="problem"></param>
    /// <param name="user"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    Task<bool> TrySaveCodeAsync(Guid contest, Guid problem, Guid user, SavedCode code);
}
