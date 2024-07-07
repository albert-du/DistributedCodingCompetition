namespace DistributedCodingCompetition.Web.Services;

using DistributedCodingCompetition.Web.Models;

public interface ICodePersistenceService
{
    Task<SavedCode?> TryReadCodeAsync(Guid contest, Guid problem, Guid user);
    Task<bool> TrySaveCodeAsync(Guid contest, Guid problem, Guid user, SavedCode code);
}
