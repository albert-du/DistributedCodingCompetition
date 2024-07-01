namespace DistributedCodingCompetition.ApiService.Models;

public class JoinCode
{
    public Guid Id { get; set; }
    public Guid ContestId { get; set; }
    public Contest Contest { get; set; } = null!;
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
    public DateTime Creation { get; set; }
    public DateTime Expiration { get; set; }
    public bool CloseAfterUse { get; set; }
    public int Uses => Users.Count;
    public ICollection<User> Users { get; set; } = [];
    public bool Admin { get; set; }
    public User? Creator { get; set; }
}
