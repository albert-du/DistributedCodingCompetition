namespace DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// User model
/// </summary>
public class User
{
    /// <summary>
    /// Id of the user
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Unique username of the user
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Email address of the user.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Full name of the user.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Birthday of the user.
    /// </summary>
    public DateTime Birthday { get; set; }

    /// <summary>
    /// User account creation time
    /// </summary>
    public DateTime Creation { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// If ban exists, this is the id of the ban
    /// </summary>
    public Guid? BanId { get; set; }

    /// <summary>
    /// Ban navigation property
    /// </summary>
    public Ban? Ban { get; set; }

    /// <summary>
    /// Submissions navigation property
    /// </summary>
    public ICollection<Submission> Submissions { get; set; } = [];

    /// <summary>
    /// Entered contests navigation property
    /// </summary>
    public ICollection<Contest> EnteredContests { get; set; } = [];

    /// <summary>
    /// Administered contests navigation property
    /// </summary>
    public ICollection<Contest> OwnedContests { get; set; } = [];

    /// <summary>
    /// Administered contests navigation property
    /// </summary>
    public ICollection<Contest> AdministeredContests { get; set; } = [];

    /// <summary>
    /// Banned contests navigation property
    /// </summary>
    public ICollection<Contest> BannedContests { get; set; } = [];

    /// <summary>
    /// Problems navigation property
    /// </summary>
    public ICollection<Problem> Problems { get; set; } = [];

    public ICollection<Ban> IssuedBans { get; set; } = [];

    internal UserResponseDTO Serialize() =>
        new()
        {
            Id = Id,
            Username = Username,
            Email = Email,
            FullName = FullName,
            CreatedAt = Creation,
            Banned = BanId.HasValue
        };
}
