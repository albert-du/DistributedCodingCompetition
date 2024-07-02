namespace DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// Contest model
/// </summary>
public class Contest
{
    /// <summary>
    /// Id of the contest
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    /// <summary>
    /// Name of the contest
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Description of the contest in markdown
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Rendered description of the contest in HTML
    /// </summary>
    public string RenderedDescription { get; set; } = string.Empty;

    /// <summary>
    /// UTC time
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// UTC time
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Active status of the contest, set to false to "archive"
    /// </summary>
    public bool Active { get; set; } = true;

    /// <summary>
    /// Owner of the contest
    /// </summary>
    public User Owner { get; set; } = default!;

    public Guid OwnerId { get; set; }

    /// <summary>
    /// Problems in the contest
    /// </summary>
    public ICollection<Problem> Problems { get; set; } = [];
    
    /// <summary>
    /// Problem count
    /// </summary>
    public int ProblemsCount => Problems.Count;
    
    /// <summary>
    /// Participants in the contest
    /// </summary>
    public ICollection<User> Participants { get; set; } = [];
    
    /// <summary>
    /// Number of participants
    /// </summary>
    public int ParticipantsCount => Participants.Count;

    /// <summary>
    /// Administrators of the contest
    /// </summary>
    public ICollection<User> Administrators { get; set; } = [];
    
    /// <summary>
    /// Number of administrators
    /// </summary>
    public int AdministratorsCount => Administrators.Count;

    /// <summary>
    /// Banned users from the contest
    /// </summary>
    public ICollection<User> Banned { get; set; } = [];
    
    /// <summary>
    /// Number of banned users
    /// </summary>
    public int BannedCount => Banned.Count;

    /// <summary>
    /// Submissions in the contest
    /// </summary>
    public ICollection<Submission> Submissions { get; set; } = [];
    
    /// <summary>
    /// Number of submissions
    /// </summary>
    public int SubmissionsCount => Submissions.Count;

    /// <summary>
    /// Join codes for the contest
    /// </summary>
    public ICollection<JoinCode> JoinCodes { get; set; } = [];
    
    /// <summary>
    /// Number of join codes
    /// </summary>
    public int JoinCodesCount => JoinCodes.Count;

    /// <summary>
    /// Public status of the contest, whether to list on the homepage/not require a join code
    /// </summary>
    public bool Public { get; set; }

    /// <summary>
    /// Open status of the contest, whether to allow new participants
    /// </summary>
    public bool Open { get; set; }

    /// <summary>
    /// Minimum age to participate in the contest Age <= MinimumAge
    /// </summary>
    public int MinimumAge { get; set; }
}
