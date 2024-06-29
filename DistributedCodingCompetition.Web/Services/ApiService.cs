namespace DistributedCodingCompetition.Web.Services;

using DistributedCodingCompetition.ApiService.Models;
using System.Net;

public class ApiService(HttpClient httpClient) : IApiService
{
    public async Task<User?> TryUserByEmailAsync(string email)
    {
        var response = await httpClient.GetAsync($"api/users/email/{email}");
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<User>();
    }
}
