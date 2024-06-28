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
    public DateTime MinTokenTime { get; private set; } = DateTime.UtcNow;
    public bool Active { get; set; } = true;
    public bool Admin { get; set; }
    public List<LoginAttempt> LoginAttempts { get; set; } = [];

    public void RefreshToken()=>
        MinTokenTime = DateTime.UtcNow;
}
