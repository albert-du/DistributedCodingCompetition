namespace DistributedCodingCompetition.AuthService.Controllers;

using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using DistributedCodingCompetition.AuthService.Models;
using DistributedCodingCompetition.AuthService.Services;

/// <summary>
/// Authentication Endpoints
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController(IPasswordService passwordService, IMongoClient mongoClient, ITokenService tokenService) : ControllerBase
{
    // Services
    private readonly IMongoCollection<UserAuth> collection = mongoClient.GetDatabase("authdb").GetCollection<UserAuth>(nameof(UserAuth));

    // POST /register
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResult>> Register(string password, bool admin = false)
    {
        if (password.Length < 8)
            return BadRequest("Password must be at least 8 characters long");

        var hash = passwordService.HashPassword(password);
        UserAuth userAuth = new()
        {
            Id = Guid.NewGuid(),
            PasswordHash = hash,
            Admin = admin
        };
        await collection.InsertOneAsync(userAuth);
        return new RegisterResult { Id = userAuth.Id };
    }

    // POST login
    [HttpPost("login")]
    public async Task<ActionResult<LoginResult>> Login(Guid id, string password, string userAgent, string ipAddress)
    {
        var user = await collection.Find(u => u.Id == id).FirstOrDefaultAsync();
        if (user is null)
            return NotFound();

        (bool valid, string? newHash) = passwordService.VerifyPassword(password, user.PasswordHash);
        if (!valid)
        {
            user.LoginAttempts.Add(new()
            {
                Time = DateTime.UtcNow,
                UserAgent = userAgent,
                IP = ipAddress,
                Success = false,
                Error = "Invalid password"
            });
            await collection.ReplaceOneAsync(u => u.Id == id, user);
            return Unauthorized();
        }

        if (newHash is not null)
            user.PasswordHash = newHash;

        user.LoginAttempts.Add(new()
        {
            Time = DateTime.UtcNow,
            UserAgent = userAgent,
            IP = ipAddress,
            Success = true
        });
        await collection.ReplaceOneAsync(u => u.Id == id, user);
        return new LoginResult { Token = tokenService.GenerateToken(user), Admin = user.Admin };
    }

    [HttpGet("validate")]
    public ActionResult<Guid> ValidateToken(string token)
    {
        var id = tokenService.ValidateToken(token);
        return id is null ? Unauthorized() : id;
    }
}
