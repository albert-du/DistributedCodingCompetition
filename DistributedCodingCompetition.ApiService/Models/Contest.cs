namespace DistributedCodingCompetition.ApiService.Models;

public class Contest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool Active { get; set; } = true;
    public ICollection<Problem> Problems { get; set; } = [];
    // public ICollection<User> Participants { get; set; } = [];
    // public ICollection<User> Administrators { get; set; } = [];
    public ICollection<Submission> Submissions { get; set; } = [];
    public bool Public { get; set; }
    public ICollection<JoinCode> JoinCodes { get; set; } = [];
}
