namespace DistributedCodingCompetition.AuthService.Controllers;

using DistributedCodingCompetition.AuthService.Models;
using DistributedCodingCompetition.AuthService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Security.Cryptography;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMongoCollection<UserAuth> collection;
    private readonly IPasswordService _passwordService;

    public AuthController(IPasswordService passwordService, IMongoClient mongoClient)
    {
        _passwordService = passwordService;
        var db = mongoClient.GetDatabase("authdb");
        collection = db.GetCollection<UserAuth>(nameof(UserAuth));

    }

    // POST /register
    [HttpPost]
    public async Task<ActionResult<RegisterResult>> Register(string password, bool admin = false)
    {
        if (password.Length < 8)
            return BadRequest("Password must be at least 8 characters long");

        var hash = _passwordService.HashPassword(password);
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
    public async Task<ActionResult<LoginResult>> Login(Guid id, string password)
    {
        var user = await collection.Find(u => u.Id == id).FirstOrDefaultAsync();
        if (user is null)
            return NotFound();

        (bool valid, string? newHash) = _passwordService.VerifyPassword(password, user.PasswordHash);
        if (!valid)
            return Unauthorized();

        if (newHash is not null)
        {
            user.PasswordHash = newHash;
            await collection.ReplaceOneAsync(u => u.Id == id, user);
        }

        return new LoginResult { Token = user.Id, Admin = user.Admin };
    }
}
