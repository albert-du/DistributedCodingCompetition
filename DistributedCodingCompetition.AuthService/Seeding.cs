namespace DistributedCodingCompetition.AuthService;

using MongoDB.Driver;
using DistributedCodingCompetition.AuthService.Models;
using DistributedCodingCompetition.AuthService.Services;

public static class Seeding
{
    public static async Task SeedDataAsync(IMongoCollection<UserAuth> collection, IPasswordService passwordService)
    {
        await collection.InsertOneAsync(new UserAuth
        {
            Id = Guid.Parse("134904d0-9515-4ceb-84d0-2cae5bf60f9d"),
            PasswordHash = passwordService.HashPassword("password"),
        });
    }
}