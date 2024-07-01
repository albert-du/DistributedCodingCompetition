namespace DistributedCodingCompetition.AuthService;

using MongoDB.Driver;
using DistributedCodingCompetition.AuthService.Models;
using DistributedCodingCompetition.AuthService.Services;

public static class Seeding
{
    public static async Task SeedDataAsync(IMongoCollection<UserAuth> collection, IPasswordService passwordService)
    {
        if (await collection.CountDocumentsAsync(FilterDefinition<UserAuth>.Empty) > 0)
            return; // DB has been seeded

        await collection.InsertOneAsync(new UserAuth
        {
            Id = Guid.Parse("134904d0-9515-4ceb-84d0-2cae5bf60f9d"),
            PasswordHash = passwordService.HashPassword("password"),
        });
    }
}