namespace DistributedCodingCompetition.AuthService.Models;

/// <summary>
/// Successful validation result.
/// </summary>
public sealed record ValidationResult
{
    /// <summary>
    /// Id.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Admin status.
    /// </summary>
    public bool Admin { get; set; }
}
