namespace DistributedCodingCompetition.Web.Services;

using DistributedCodingCompetition.Web.Models;

public interface ICurrentSavedCodeProvider
{
    Task<SavedCode?> GetCurrentSavedCodeAsync();

    Task<bool> TrySaveCurrentCodeAsync(SavedCode code);
}
