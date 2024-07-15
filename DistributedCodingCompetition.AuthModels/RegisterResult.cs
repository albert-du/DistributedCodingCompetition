namespace DistributedCodingCompetition.AuthModels;

/// <summary>
/// Result of a successful registration attempt.
/// </summary>
public record RegisterResult
{
    /// <summary>
    /// Id of the new auth registry.
    /// </summary>
    public required Guid Id { get; init; }
}
