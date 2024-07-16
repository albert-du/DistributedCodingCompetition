namespace DistributedCodingCompetition.ApiService.Models;

public sealed record ContestRequestDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? RenderedDescription { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public bool? Active { get; set; }
    public Guid? OwnerId { get; set; }
    public bool? Public { get; set; }
    public bool? Open { get; set; }
    public int? MinimumAge { get; set; }
    public int? DefaultPointsForProblem { get; set; }
}
