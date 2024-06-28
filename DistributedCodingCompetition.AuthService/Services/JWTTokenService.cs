namespace DistributedCodingCompetition.AuthService.Services;

using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using DistributedCodingCompetition.AuthService.Models;
using Microsoft.IdentityModel.Tokens;

public class JWTTokenService(IConfiguration configuration, ILogger<JWTTokenService> logger) : ITokenService
{
    byte[] key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"] ?? "CHANGEKEYNOW");
    public string GenerateToken(UserAuth userAuth)
    {
        JwtSecurityTokenHandler tokenHandler = new();

        SigningCredentials signingCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature);

        List<Claim> claims = [new(ClaimTypes.NameIdentifier, userAuth.Id.ToString())];

        if (userAuth.Admin) claims.Add(new(ClaimTypes.Role, "Admin"));

        JwtSecurityToken token = new(claims: claims, notBefore: DateTime.UtcNow, expires: DateTime.UtcNow.AddDays(7), signingCredentials: signingCredentials);
        return tokenHandler.WriteToken(token);
    }

    public Guid? ValidateToken(string token)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        try
        {
            tokenHandler.ValidateToken(token, new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return Guid.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        }
        catch
        {
            return null;
        }
    }
}
