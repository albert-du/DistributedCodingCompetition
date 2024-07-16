namespace DistributedCodingCompetition.ApiService.Models;

public sealed record ContestResponseDTO
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required Guid OwnerId { get; set; }
    public required string OwnerName { get; set; }
    public required string Description { get; set; } = string.Empty;
    public required string RenderedDescription { get; set; } = string.Empty;
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
    public required bool Active { get; set; }
    public required bool Public { get; set; }
    public required bool Open { get; set; }
    public required int MinimumAge { get; set; }
    public required int DefaultPointsForProblem { get; set; }
}