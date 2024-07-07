namespace DistributedCodingCompetition.ApiService.Models;

public class ProblemPointValue
{
    public Guid Id { get; set; }
    public Guid ProblemId { get; set; }
    public Guid ContestId { get; set; }
    public int Points { get; set; } = 100;
}
