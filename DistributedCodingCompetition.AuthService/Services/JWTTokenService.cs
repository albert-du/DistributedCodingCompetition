namespace DistributedCodingCompetition.AuthService.Services;

using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using DistributedCodingCompetition.AuthService.Models;
using Microsoft.IdentityModel.Tokens;

public class JWTTokenService(IConfiguration configuration, ILogger<JWTTokenService> logger) : ITokenService
{
    byte[] key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? "CHANGEKEYNOW");
    public async Task<string> GenerateTokenAsync(UserAuth userAuth)
    {
        var userKey = Combine(key, Convert.FromBase64String(userAuth.UserSecret));
        JwtSecurityTokenHandler tokenHandler = new();
        SigningCredentials signingCredentials = new(new SymmetricSecurityKey(userKey), SecurityAlgorithms.HmacSha512Signature);
        List<Claim> claims = [
            new(ClaimTypes.NameIdentifier, userAuth.Id.ToString()),
        ];
        if (userAuth.Admin)
            claims.Add(new(ClaimTypes.Role, "Admin"));
        JwtSecurityToken token = new(claims: claims, notBefore: DateTime.UtcNow, expires: DateTime.UtcNow.AddDays(30));
    }

    public async Task InvalidateUserTokensAsync(UserAuth userAuth)
    {
        _logger.LogInformation($"Invalidating tokens for user {userAuth.Username}");
    }

    public async Task<UserAuth?> ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var username = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            var admin = jwtToken.Claims.First(x => x.Type == ClaimTypes.Role).Value == "Admin";
            return new UserAuth
            {
                Username = username,
                Admin = admin
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to validate token");
            return null;
        }
    }
    private static byte[] Combine(byte[] a1, byte[] a2)
    {
        byte[] ret = new byte[a1.Length + a2.Length];
        Array.Copy(a1, 0, ret, 0, a1.Length);
        Array.Copy(a2, 0, ret, a1.Length, a2.Length);
        return ret;
    }
}
