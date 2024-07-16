namespace DistributedCodingCompetition.ApiService.Models;

public sealed record JoinCodeResponseDTO
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required Guid ContestId { get; init; }
    public required string ContestName { get; init; }
    public required Guid CreatorId { get; init; }
    public required string CreatorName { get; init; }
    public required string Code { get; init; }
    public required bool Admin { get; init; }
    public required bool Active { get; init; }
    public required bool CloseAfterUse { get; init; }
    public required int Uses { get; init; }
    public required DateTime CreatedAt { get; init; }
}