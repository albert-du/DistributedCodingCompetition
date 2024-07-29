//using MongoDB.Bson.Serialization.Attributes;

//namespace DistributedCodingCompetition.AuthService.Models;

///// <summary>
///// One login attempt, successful or not.
///// </summary>
//public record LoginAttempt
//{
//    /// <summary>
//    /// Ip address of request.
//    /// </summary>
//    public required string IP { get; init; } = string.Empty;

//    /// <summary>
//    /// Browser user agent from the request.
//    /// </summary>
//    public required string UserAgent { get; init; } = string.Empty;
    
//    /// <summary>
//    /// Time of the login attempt.
//    /// </summary>
//    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
//    public required DateTime Time { get; init; }
    
//    /// <summary>
//    /// Status
//    /// </summary>
//    public required bool Success { get; init; }
    
//    /// <summary>
//    /// Error
//    /// </summary>
//    public string? Error { get; init; }
//}
