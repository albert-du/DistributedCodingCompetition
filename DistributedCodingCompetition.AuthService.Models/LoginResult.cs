namespace DistributedCodingCompetition.AuthService.Models;

/// <summary>
/// Successful login result.
/// </summary>
public sealed record LoginResult
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
