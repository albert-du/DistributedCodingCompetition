namespace DistributedCodingCompetition.Web.Services;

using System.Net;
using Microsoft.Extensions.Logging;
using DistributedCodingCompetition.AuthService.Models;
using System.Web;

public class AuthService(HttpClient httpClient, ILogger<AuthService> logger, IModalService modalService, IApiService apiService) : IAuthService
{
    public async Task<Guid?> TryRegisterAsync(string email, string password)
    {
        try
        {
            (var success, var emailUser) = await apiService.TryReadUserByEmailAsync(email);
            if (!success)
                return null;
            
            if (emailUser != null)
            {
                modalService.ShowError("Cannot register", "Email already in use");
                return null;
            }
            var encodedPassword = Uri.EscapeDataString(password);
            var resp = await httpClient.PostAsync("api/auth/register?password=" + encodedPassword, null);
            resp.EnsureSuccessStatusCode();
            var res = await resp.Content.ReadFromJsonAsync<RegisterResult>();
            return res?.Id;
        }
        catch (Exception ex)
        {
            modalService.ShowError("Failed to register", "An error occurred while trying to register");
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
            var resp = await httpClient.PostAsync($"api/auth/login?{queryString}", null);
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

    public async Task<ValidationResult?> ValidateTokenAsync(string token)
    {
        try
        {
            var resp = await httpClient.PostAsync($"api/auth/validate?token={token}", null);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<ValidationResult>();
        }
        catch (Exception ex)
        {
            modalService.ShowError("Failed to validate token", ex.Message);
            logger.LogError(ex, "Failed to validate token");
            return null;
        }
    }
}
