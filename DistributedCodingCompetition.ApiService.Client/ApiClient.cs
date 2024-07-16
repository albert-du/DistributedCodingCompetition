namespace DistributedCodingCompetition.ApiService.Client;

internal class ApiClient<TOwner>(HttpClient httpClient, ILogger<TOwner> logger, string prefix)
{
    internal async Task<(bool, T?)> GetAsync<T>(string url = "")
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

    internal async Task<(bool, TR?)> PutAsync<T, TR>(string url = "", T? data = default)
    {
        var expanded = prefix + url;
        try
        {
            var response = await httpClient.PutAsJsonAsync(expanded, data);
            var result = await response.Content.ReadFromJsonAsync<TR>();
            response.EnsureSuccessStatusCode();

            logger.LogDebug("Successfully put {TYPE} to {URL}", typeof(T).Name, expanded);
            return (true, result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to put {TYPE} to {URL}", typeof(T).Name, expanded);
            return (false, default);
        }
    }

    internal async Task<bool> PutAsync<T>(string url = "", T? data = default)
    {
        var expanded = prefix + url;
        try
        {
            var response = await httpClient.PutAsJsonAsync(expanded, data);
            response.EnsureSuccessStatusCode();
            logger.LogDebug("Successfully put {TYPE} to {URL}", typeof(T).Name, expanded);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to put {TYPE} to {URL}", typeof(T).Name, expanded);
            return false;
        }
    }

    internal async Task<(bool, TR?)> PostAsync<T, TR>(string url = "", T? data = default)
    {
        var expanded = prefix + url;
        try
        {
            var response = await httpClient.PostAsJsonAsync(expanded, data);
            var result = await response.Content.ReadFromJsonAsync<TR>();
            logger.LogDebug("Successfully posted {TYPE} to {URL}", typeof(T).Name, expanded);
            return (true, result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to post {TYPE} to {URL}", typeof(T).Name, expanded);
            return (false, default);
        }
    }
}