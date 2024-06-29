namespace DistributedCodingCompetition.Web.Services;

using Microsoft.Extensions.Logging;
using DistributedCodingCompetition.AuthService.Models;

public class AuthService(HttpClient httpClient, ILogger<AuthService> logger, IModalService modalService, IApiService apiService) : IAuthService
{
    public async Task<Guid?> TryRegisterAsync(string email, string password)
    {
        try
        {
            (var success, var emailUser) = await apiService.TryUserByEmailAsync(email);
            if (!success)
                return null;
            
            if (emailUser != null)
            {
                modalService.ShowError("Cannot register", "Email already in use");
                return null;
            }
            var resp = await httpClient.PostAsync("/register?password=" + password, null);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<Guid>();
        }
        catch (Exception ex)
        {
            modalService.ShowError("Failed to register", ex.Message);
            logger.LogError(ex, "Failed to register");
            return null;
        }
    }

    public async Task<LoginResult?> TryLoginAsync(Guid id, string password, string userAgent, string ipAddress)
    {
        Dictionary<string, string> data = new()
        {
            { "id", id.ToString() },
            { "password", password },
            { "userAgent", userAgent },
            { "ipAddress", ipAddress }
        };
        FormUrlEncodedContent urlEncoded = new(data);
        var queryString = await urlEncoded.ReadAsStringAsync();
        try
        {
            var resp = await httpClient.PostAsync($"/login?{queryString}", null);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<LoginResult>();
        }
        catch (Exception ex)
        {
            modalService.ShowError("Failed to login", ex.Message);
            logger.LogError(ex, "Failed to login");
            return null;
        }
    }

    public async Task<Guid?> ValidateToken(string token)
    {
        try
        {
            var resp = await httpClient.PostAsync($"/validate?token={token}", null);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<Guid>();
        }
        catch (Exception ex)
        {
            modalService.ShowError("Failed to validate token", ex.Message);
            logger.LogError(ex, "Failed to validate token");
            return null;
        }
    }
}
