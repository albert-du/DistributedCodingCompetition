namespace DistributedCodingCompetition.AuthService.Models;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Security.Cryptography;

public class UserAuth
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public Guid Id { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime PasswordChangeTime { get; set; } = DateTime.UtcNow;
    public string UserSecret { get; private set; } = RandomString();
    public bool Active { get; set; } = true;
    public bool Admin { get; set; }
    public List<LoginAttempt> LoginAttempts { get; set; } = [];

    public void RefreshSecret() =>
        UserSecret = RandomString();

    private static string RandomString(int length = 32) =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(length));
}
