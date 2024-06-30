namespace DistributedCodingCompetition.AuthService.Services;

using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using DistributedCodingCompetition.AuthService.Models;
using MongoDB.Driver;

/// <summary>
/// Service for generating and validating JWT tokens.
/// </summary>
/// <param name="configuration"></param>
/// <param name="logger"></param>
public class JWTTokenService(IConfiguration configuration, ILogger<JWTTokenService> logger, IMongoClient mongoClient) : ITokenService
{
    private readonly IMongoCollection<UserAuth> collection = mongoClient.GetDatabase("authdb").GetCollection<UserAuth>(nameof(UserAuth));

    // key for signing tokens
    // min size 512 bits
    private readonly byte[] key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"] ?? "CHANGE ME SERIOUSLY CHANGE ME CHANGE ME SERIOUSLY CHANGE ME CHANGE ME SERIOUSLY CHANGE ME");

    /// <summary>
    /// Generate a JWT token.
    /// </summary>
    /// <param name="userAuth"></param>
    /// <returns></returns>
    public string GenerateToken(UserAuth userAuth)
    {
        // log if key not set
        if (configuration["Jwt:Key"] is null)
            logger.LogWarning("JWT Key not set, using default key.");

        JwtSecurityTokenHandler tokenHandler = new();

        SigningCredentials signingCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature);

        List<Claim> claims = [new(ClaimTypes.NameIdentifier, userAuth.Id.ToString())];

        // add roles
        if (userAuth.Admin)
            claims.Add(new(ClaimTypes.Role, "Admin"));

        JwtSecurityToken token = new(claims: claims, notBefore: DateTime.UtcNow, expires: DateTime.UtcNow.AddDays(7), signingCredentials: signingCredentials);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Validate a JWT token.
    /// </summary>
    /// <param name="token"></param>
    /// <returns>null if invalid</returns>
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

            var id = Guid.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            // check user not invalid
            var user = collection.Find(x => x.Id == id).FirstOrDefault();
            if (user is null)
                return null;

            if (jwtToken.ValidFrom < user.MinTokenTime)
                return null;

            return id;
        }
        catch
        {
            // invalid token
            return null;
        }
    }
}
