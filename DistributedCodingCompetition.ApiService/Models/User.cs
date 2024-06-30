namespace DistributedCodingCompetition.ApiService.Models;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime Birthday { get; set; }
    public DateTime Creation { get; set; } = DateTime.UtcNow;
    public string Email { get; set; } = string.Empty;
    public ICollection<Submission> Submissions { get; set; } = [];
    public ICollection<Contest> EnteredContests { get; set; } = [];
    public ICollection<Contest> OwnedContests { get; set; } = [];
    public ICollection<Contest> AdministeredContests { get; set; } = [];
    public ICollection<Problem> Problems { get; set; } = [];
}
