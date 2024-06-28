namespace DistributedCodingCompetition.AuthService.Models;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

/// <summary>
/// Auth data for a user.
/// </summary>
public class UserAuth
{
    /// <summary>
    /// Id of the user.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public Guid Id { get; set; }

    /// <summary>
    /// Hashed password.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;
    
    /// <summary>
    /// Last changed password time.
    /// </summary>
    public DateTime PasswordChangeTime { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Do not validate any token from before this time.
    /// </summary>
    public DateTime MinTokenTime { get; private set; } = DateTime.UtcNow;
    
    /// <summary>
    /// User is active.
    /// </summary>
    public bool Active { get; set; } = true;

    /// <summary>
    /// User is adminstrator.
    /// </summary>
    public bool Admin { get; set; }

    /// <summary>
    /// Login attempts.
    /// </summary>
    public List<LoginAttempt> LoginAttempts { get; set; } = [];

    /// <summary>
    /// Invalidate all tokens.
    /// </summary>
    public void RefreshToken()=>
        MinTokenTime = DateTime.UtcNow;
}
