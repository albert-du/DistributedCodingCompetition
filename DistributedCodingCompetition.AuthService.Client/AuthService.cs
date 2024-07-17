namespace DistributedCodingCompetition.AuthService.Client;

/// <inheritdoc/>
public sealed class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthService> _logger;

    internal AuthService(HttpClient httpClient, ILogger<AuthService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Guid?> TryRegisterAsync(string email, string password)
    {
        try
        {
            var encodedPassword = Uri.EscapeDataString(password);
            var resp = await _httpClient.PostAsync("api/auth/register?password=" + encodedPassword, null);
            resp.EnsureSuccessStatusCode();
            var res = await resp.Content.ReadFromJsonAsync<RegisterResult>();
            return res?.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to register");
            return null;
        }
    }

    /// <inheritdoc/>
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
            var resp = await _httpClient.PostAsync($"api/auth/login?{queryString}", null);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<LoginResult>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to login");
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<ValidationResult?> ValidateTokenAsync(string token)
    {
        try
        {
            var resp = await _httpClient.PostAsync($"api/auth/validate?token={token}", null);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<ValidationResult>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate token");
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> TryChangePasswordAsync(Guid id, string oldPassword, string newPassword)
    {
        Dictionary<string, string> data = new()
        {
            { "id", id.ToString() },
            { "oldPassword", oldPassword },
            { "newPassword", newPassword }
        };
        FormUrlEncodedContent urlEncoded = new(data);
        var queryString = await urlEncoded.ReadAsStringAsync();
        try
        {
            var resp = await _httpClient.PostAsync($"api/auth/changePassword?{queryString}", null);
            resp.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to change password");
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> TryResetPasswordAsync(Guid id, string newPassword)
    {
        Dictionary<string, string> data = new()
        {
            { "id", id.ToString() },
            { "newPassword", newPassword }
        };
        FormUrlEncodedContent urlEncoded = new(data);
        var queryString = await urlEncoded.ReadAsStringAsync();
        try
        {
            var resp = await _httpClient.PostAsync($"api/auth/changePassword?{queryString}", null);
            resp.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to change password");
            return false;
        }
    }
}