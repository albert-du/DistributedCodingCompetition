namespace DistributedCodingCompetition.ApiService.Models;

public class TestCase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProblemId { get; set; }
    public Problem? Problem { get; set; } = null!;
    public string Input { get; set; } = string.Empty;
    public string Output { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool Sample { get; set; }
    public bool Active { get; set; } = true;
    public int Weight { get; set; } = 100;
}
