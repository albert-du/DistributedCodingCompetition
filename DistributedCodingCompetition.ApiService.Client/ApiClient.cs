namespace DistributedCodingCompetition.ApiService.Client;

internal class ApiClient<TOwner>(HttpClient httpClient, ILogger<TOwner> logger, string prefix)
{
    internal async Task<(bool, T?)> Get<T>(string url = string.Empty)
    {
        var expanded = prefix + url;
        try
        {
            var result = await httpClient.GetFromJsonAsync<T>(expanded);
            logger.LogDebug("Successfully got {TYPE} from {URL}", typeof(T).Name, expanded);
            return (true, result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get {TYPE} from {URL}", typeof(T).Name, expanded);
            return (false, default);
        }
    }
}