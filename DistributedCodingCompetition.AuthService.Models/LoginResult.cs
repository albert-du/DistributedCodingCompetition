namespace DistributedCodingCompetition.AuthModels;

/// <summary>
/// Sucessful login result.
/// </summary>
public record LoginResult
{
    /// <summary>
    /// Token to use for validation.
    /// </summary>
    public required string Token { get; set; }

    /// <summary>
    /// Admin status.
    /// </summary>
    public required bool Admin { get; set; }
}
