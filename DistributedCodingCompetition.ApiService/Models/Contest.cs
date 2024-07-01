namespace DistributedCodingCompetition.ApiService.Models;

public class Contest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string RenderedDescription { get; set; } = string.Empty;

    /// <summary>
    /// UTC time
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// UTC time
    /// </summary>
    public DateTime EndTime { get; set; }
    public bool Active { get; set; } = true;
    public User Owner { get; set; } = null!;
    public ICollection<Problem> Problems { get; set; } = [];
    public int ProblemsCount => Problems.Count;
    public ICollection<User> Participants { get; set; } = [];
    public int ParticipantsCount => Participants.Count;
    public ICollection<User> Administrators { get; set; } = [];
    public int AdministratorsCount => Administrators.Count;
    public ICollection<User> Banned { get; set; } = [];
    public int BannedCount => Banned.Count;
    public ICollection<Submission> Submissions { get; set; } = [];
    public int SubmissionsCount => Submissions.Count;
    public ICollection<JoinCode> JoinCodes { get; set; } = [];
    public int JoinCodesCount => JoinCodes.Count;
    public bool Public { get; set; }
    public bool Open { get; set; }
    public int MinimumAge { get; set; }
}
