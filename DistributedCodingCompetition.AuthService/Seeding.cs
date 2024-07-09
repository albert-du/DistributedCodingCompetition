namespace DistributedCodingCompetition.AuthService;

using MongoDB.Driver;
using DistributedCodingCompetition.AuthService.Models;
using DistributedCodingCompetition.AuthService.Services;

public static class Seeding
{
    /// <summary>
    /// Seeds the database with some initial data for testing purposes
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="passwordService"></param>
    /// <returns></returns>
    public static async Task SeedDataAsync(IMongoCollection<UserAuth> collection, IPasswordService passwordService)
    {
        await collection.InsertOneAsync(new UserAuth
        {
            Id = Guid.Parse("134904d0-9515-4ceb-84d0-2cae5bf60f9d"),
            PasswordHash = passwordService.HashPassword("password"),
        });

        await collection.InsertOneAsync(new UserAuth
        {
            Id = Guid.Parse("234904d0-9515-4ceb-84d0-2cae5bf60f9e"),
            PasswordHash = passwordService.HashPassword("password"),
        });
    }
}