namespace DistributedCodingCompetition.ApiService.Models;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public ICollection<Submission> Submissions { get; set; } = [];
    public ICollection<Contest> EnteredContests { get; set; } = [];
    public ICollection<Contest> AdministeredContests { get; set; } = [];
    public ICollection<Problem> Problems { get; set; } = [];
}
