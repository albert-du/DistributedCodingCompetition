namespace DistributedCodingCompetition.Web.Services;

using DistributedCodingCompetition.ApiService.Models;
using System.Net;

public class ApiService(HttpClient httpClient, ILogger<ApiService> logger) : IApiService
{
    public async Task<(bool,User?)> TryUserByEmailAsync(string email)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/users/email/{email}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return (true ,null);
            response.EnsureSuccessStatusCode();
            return (true, await response.Content.ReadFromJsonAsync<User>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch user by email");

            return (false, null);
        }
    }

    public async Task<bool> TryCreateUserAsync(User user)
    {
        var response = await httpClient.PostAsJsonAsync("api/users", user);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<User>() ?? throw new Exception("Failed to create user");
    }
}
