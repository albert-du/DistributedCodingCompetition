namespace DistributedCodingCompetition.Web.Services;

using DistributedCodingCompetition.Web.Models;
using Microsoft.AspNetCore.Components;

public class CurrentSavedCodeProvider(IUserStateService userStateService, NavigationManager navigationManager, ICodePersistenceService codePersistenceService) : ICurrentSavedCodeProvider
{
    public async Task<SavedCode?> GetCurrentSavedCodeAsync()
    {
        var user = await userStateService.UserAsync();
        var segments = navigationManager.ToBaseRelativePath(navigationManager.Uri).Split('/');
        if (user is null ||
            segments.Length != 4 ||
            segments[0] != "contest" ||
            segments[2] != "problem" ||
            !Guid.TryParse(segments[1], out var contest) ||
            !Guid.TryParse(segments[3], out var problem))
            return null;

        return await codePersistenceService.TryReadCodeAsync(contest, problem, user.Id);
    }

    public async Task<bool> TrySaveCurrentCodeAsync(SavedCode code)
    {
        var user = await userStateService.UserAsync();
        var segments = navigationManager.ToBaseRelativePath(navigationManager.Uri).Split('/');
        if (user is null ||
            segments.Length != 4 ||
            segments[0] != "contest" ||
            segments[2] != "problem" ||
            !Guid.TryParse(segments[1], out var contest) ||
            !Guid.TryParse(segments[3], out var problem))
            return false;
        return await codePersistenceService.TrySaveCodeAsync(contest, problem, user.Id, code);
    }

    private
}
