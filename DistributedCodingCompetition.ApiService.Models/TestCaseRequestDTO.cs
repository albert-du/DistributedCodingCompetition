namespace DistributedCodingCompetition.ApiService.Models;

public sealed record TestCaseRequestDTO
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid? ProblemId { get; init; }
    public string? Input { get; init; }
    public string? Output { get; init; }
    public string? Description { get; init; }
    public bool? Sample { get; init; }
    public bool? Active { get; init; }
    public int? Weight { get; init; }
}