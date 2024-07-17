namespace DistributedCodingCompetition.ApiService.Models;

public sealed record JoinCodeRequestDTO
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string? Name { get; init; }
    public Guid? ContestId { get; init; }
    public Guid? CreatorId { get; init; }
    public string? Code { get; init; }
    public bool? Admin { get; init; }
    public bool? Active { get; init; }
    public bool? CloseAfterUse { get; init; }
}