namespace DistributedCodingCompetition.Web.Services;

/// <summary>
/// Code persistence service
/// </summary>
public interface ICodePersistenceService
{
    Task<SavedCode?> TryReadCodeAsync(Guid contest, Guid problem, Guid user);
    Task<bool> TrySaveCodeAsync(Guid contest, Guid problem, Guid user, SavedCode code);
}
