using System.Text.Json;

namespace DistributedCodingCompetition.ApiService.Client;

/// <summary>
/// A client for interacting with the DistributedCodingCompetition API.
/// </summary>
/// <typeparam name="TOwner">Owner Service</typeparam>
/// <param name="httpClient">httpclient to use</param>
/// <param name="logger">logger</param>
/// <param name="prefix">url prefix</param>
internal class ApiClient<TOwner>(HttpClient httpClient, ILogger<TOwner> logger, string prefix)
{
    /// <summary>
    /// Sends a GET request to the API.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <returns></returns>
    internal async Task<(bool, T?)> GetAsync<T>(string url = "")
    {
        var expanded = prefix + url;
        try
        {
            var result = await httpClient.GetFromJsonAsync<T>(expanded);
            logger.LogDebug("Successfully got {TYPE} from {URL}", typeof(T).Name, expanded);
            return (true, result);
        }
        catch (JsonException)
        {
            return (true, default);
        }
        catch (Exception ex)
        {


            logger.LogError(ex, "Failed to get {TYPE} from {URL}", typeof(T).Name, expanded);
            return (false, default);
        }
    }

    /// <summary>
    /// Sends a PUT request to the API.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TR"></typeparam>
    /// <param name="url"></param>
    /// <param name="data"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Sends a PUT request to the API.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <param name="data"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Sends a POST request to the API.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TR"></typeparam>
    /// <param name="url"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    internal async Task<(bool, TR?)> PostAsync<T, TR>(string url = "", T? data = default)
    {
        var expanded = prefix + url;
        try
        {
            var response = await httpClient.PostAsJsonAsync(expanded, data);
            response.EnsureSuccessStatusCode();
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

    /// <summary>
    /// Sends a POST request to the API.
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    internal async Task<bool> PostAsync(string url = "")
    {
        var expanded = prefix + url;
        try
        {
            var response = await httpClient.PostAsync(expanded, null);
            response.EnsureSuccessStatusCode();
            logger.LogDebug("Successfully posted to {URL}", expanded);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to post to {URL}", expanded);
            return false;
        }
    }

    /// <summary>
    /// Sends a DELETE request to the API.
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    internal async Task<bool> DeleteAsync(string url = "")
    {
        var expanded = prefix + url;
        try
        {
            var response = await httpClient.DeleteAsync(expanded);
            response.EnsureSuccessStatusCode();
            logger.LogDebug("Successfully deleted {URL}", expanded);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete {URL}", expanded);
            return false;
        }
    }
}