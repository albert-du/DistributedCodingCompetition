namespace DistributedCodingCompetition.Models;

/// <summary>
/// Represents a ban
/// </summary>
public class Ban
{
    /// <summary>
    /// Id of the ban
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Id of the user
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Navigation property for the user
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Start of the ban
    /// </summary>
    public DateTime Start { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// End of the ban
    /// </summary>
    public DateTime? End { get; set; }

    /// <summary>
    /// Reason for the ban
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// Id of the issuer
    /// </summary>
    public Guid IssuerId { get; set; }

    /// <summary>
    /// Navigation property for the issuer
    /// </summary>
    public User? Issuer { get; set; }

    /// <summary>
    /// Whether the ban is active, set to false to withdraw the ban
    /// </summary>
    public bool Active { get; set; } = true;
}