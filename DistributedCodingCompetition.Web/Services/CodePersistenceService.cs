namespace DistributedCodingCompetition.Web.Services;

using DistributedCodingCompetition.Web.Models;

public class CodePersistenceService(HttpClient httpClient, ILogger<CodePersistenceService> logger) : ICodePersistenceService
{
    public async Task<SavedCode?> TryReadCodeAsync(Guid contest, Guid problem, Guid user)
    {
        var response = await httpClient.GetAsync($"{contest}/{problem}/{user}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<SavedCode>();
    }

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
