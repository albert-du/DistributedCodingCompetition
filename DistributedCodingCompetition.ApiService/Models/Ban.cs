namespace DistributedCodingCompetition.ApiService.Models;

public class Ban
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Reason { get; set; } = string.Empty;
    public User? Issuer { get; set; }
    public bool Active { get; set; } = true;
}