namespace DistributedCodingCompetition.Web.Services;

/// <summary>
/// Code persistence service
/// </summary>
/// <param name="httpClient"></param>
/// <param name="logger"></param>
public sealed class CodePersistenceService(HttpClient httpClient, ILogger<CodePersistenceService> logger) : ICodePersistenceService
{
    /// <summary>
    /// Read saved code for a user in a contest and problem
    /// </summary>
    /// <param name="contest"></param>
    /// <param name="problem"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<SavedCode?> TryReadCodeAsync(Guid contest, Guid problem, Guid user)
    {
        var response = await httpClient.GetAsync($"{contest}/{problem}/{user}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<SavedCode>();
    }

    /// <summary>
    /// Save code for a user in a contest and problem
    /// </summary>
    /// <param name="contest"></param>
    /// <param name="problem"></param>
    /// <param name="user"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    public async Task<bool> TrySaveCodeAsync(Guid contest, Guid problem, Guid user, SavedCode code)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync($"{contest}/{problem}/{user}", code);
            response.EnsureSuccessStatusCode();

            return true;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Failed to save code");
            return false;
        }
    }

}
