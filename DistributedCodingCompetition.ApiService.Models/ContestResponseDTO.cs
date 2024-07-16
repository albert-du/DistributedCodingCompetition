namespace DistributedCodingCompetition.ApiService.Models;

public sealed record ContestResponseDTO
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; } = string.Empty;
    public required string RenderedDescription { get; set; } = string.Empty;
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
}