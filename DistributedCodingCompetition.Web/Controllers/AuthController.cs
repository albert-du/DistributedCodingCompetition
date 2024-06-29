namespace DistributedCodingCompetition.Web.Controllers;

using System.Security.Claims;
using DistributedCodingCompetition.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(Guid id, string password)
    {
        var result = await authService.TryLoginAsync(id, password, Request.Headers["User-Agent"].ToString(), Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown");
        if (result is null)
            return Unauthorized();

        List<Claim> claims = [
            new (ClaimTypes.NameIdentifier, id.ToString()),
        ];
        if (result.Admin)
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));

        // cookie auth
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
        };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

        return Ok();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }
}
