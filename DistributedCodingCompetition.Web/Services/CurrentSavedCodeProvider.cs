namespace DistributedCodingCompetition.Web.Services;

using Microsoft.AspNetCore.Components;
using DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// Current saved code provider
/// Accesses the current saved code for the current user and problem
/// </summary>
/// <param name="userStateService"></param>
/// <param name="navigationManager"></param>
/// <param name="codePersistenceService"></param>
public sealed class CurrentSavedCodeProvider(IUserStateService userStateService, NavigationManager navigationManager, ICodePersistenceService codePersistenceService) : ICurrentSavedCodeProvider
{
    /// <summary>
    /// Get the current saved code for the current user and problem
    /// </summary>
    /// <returns></returns>
    public async Task<SavedCode?> GetCurrentSavedCodeAsync()
    {
        var user = await userStateService.UserAsync();

        // Extract the contest and problem from the URL
        var segments = navigationManager.ToBaseRelativePath(navigationManager.Uri).Split('/');
        if (user is null ||
            segments.Length < 4 ||
            segments[0] != "contest" ||
            segments[2] != "solve" ||
            !Guid.TryParse(segments[1], out var contest) ||
            !Guid.TryParse(segments[3], out var problem))
            return null;

        Console.WriteLine("Reading code");


        return await codePersistenceService.TryReadCodeAsync(contest, problem, user.Id);
    }

    /// <summary>
    /// Try to save the current code for the current user and problem
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public async Task<bool> TrySaveCurrentCodeAsync(SavedCode code)
    {
        var user = await userStateService.UserAsync();

        // Extract the contest and problem from the URL
        var segments = navigationManager.ToBaseRelativePath(navigationManager.Uri).Split('/');
        if (user is null ||
            segments.Length < 4 ||
            segments[0] != "contest" ||
            segments[2] != "solve" ||
            !Guid.TryParse(segments[1], out var contest) ||
            !Guid.TryParse(segments[3], out var problem))
            return false;
        Console.WriteLine("Saving code");
        return await codePersistenceService.TrySaveCodeAsync(contest, problem, user.Id, code);
    }
}
