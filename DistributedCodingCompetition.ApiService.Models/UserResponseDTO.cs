namespace DistributedCodingCompetition.ApiService.Models;

public sealed record UserResponseDTO
{
    public required Guid Id { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string FullName { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required bool Banned { get; init; }
}