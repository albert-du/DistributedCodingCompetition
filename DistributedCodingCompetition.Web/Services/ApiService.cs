namespace DistributedCodingCompetition.Web.Services;

using DistributedCodingCompetition.ApiService.Models;

public class ApiService(HttpClient httpClient) : IApiService
{
    public async Task<User> UserByEmailAsync(string email)
    {
        var response = await httpClient.GetAsync($"api/users/email/{email}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<User>();
    }
}
