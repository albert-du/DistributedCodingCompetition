namespace DistributedCodingCompetition.Web.Controllers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DistributedCodingCompetition.AuthModels;

/// <summary>
/// Controller for handling authentication
/// </summary>
/// <param name="authService"></param>
[Route("htau")] // auth backwards
[ApiController]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>
    /// Logs in a user
    /// </summary>
    /// <param name="token">token</param>
    /// <returns>cookie</returns>
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(string token)
    {
        if (await authService.ValidateTokenAsync(token) is not ValidationResult result)
            return Unauthorized();

        List<Claim> claims = [new(ClaimTypes.NameIdentifier, result.Id.ToString())];

        if (result.Admin)
            claims.Add(new(ClaimTypes.Role, "Admin"));

        // cookie auth
        ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        AuthenticationProperties authProperties = new()
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

        return Ok();
    }

    /// <summary>
    /// Logs out a user
    /// </summary>
    /// <returns></returns>
    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }
}
