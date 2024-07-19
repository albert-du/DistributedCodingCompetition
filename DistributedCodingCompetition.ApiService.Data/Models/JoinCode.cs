namespace DistributedCodingCompetition.ApiService.Data.Models;

/// <summary>
/// Code to join a contest.
/// </summary>
public class JoinCode
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Unique identifier of the contest.
    /// </summary>
    public Guid ContestId { get; set; }

    /// <summary>
    /// Contest the code is for.
    /// </summary>
    public Contest Contest { get; set; } = null!;

    /// <summary>
    /// The code to join the contest.
    /// </summary>
    public string Code { get; set; } = Utils.RandomString(8);

    /// <summary>
    /// Name of the code.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Whether the code is active. Use this to manually turn it off temporarily.
    /// </summary>
    public bool Active { get; set; } = true;

    /// <summary>
    /// Date and time the code was created. UTC
    /// </summary>
    public DateTime Creation { get; set; }

    /// <summary>
    /// Date and time the code expires. UTC
    /// </summary>
    public DateTime Expiration { get; set; }

    /// <summary>
    /// Whether this code is single use.
    /// </summary>
    public bool CloseAfterUse { get; set; }

    /// <summary>
    /// Number of times the code has been used.
    /// </summary>
    public int Uses => Users.Count;

    /// <summary>
    /// Users who used this code.
    /// </summary>
    public ICollection<User> Users { get; set; } = [];

    /// <summary>
    /// Whether to grant admin permissions to users who use this code instead of participant permissions.
    /// </summary>
    public bool Admin { get; set; }

    /// <summary>
    /// Id of the admin user who created the code.
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// Admin user who created the code.
    /// </summary>
    public User Creator { get; set; } = null!;
}
