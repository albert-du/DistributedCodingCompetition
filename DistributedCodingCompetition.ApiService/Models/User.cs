namespace DistributedCodingCompetition.ApiService.Models;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    
    public ICollection<Submission> Submissions { get; set; } = [];
    public ICollection<Contest> Contests { get; set; } = [];
    public ICollection<Contest> AdministeredContests { get; set; } = [];
    public ICollection<Problem> Problems { get; set; } = [];
}
