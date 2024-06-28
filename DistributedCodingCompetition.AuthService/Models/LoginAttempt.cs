namespace DistributedCodingCompetition.AuthService.Models;

public record LoginAttempt
{
    public required string IP { get; init; } = string.Empty;
    public required string UserAgent { get; init; } = string.Empty;
    public required DateTime Time { get; init; }
    public required bool Success { get; init; }
    public string? Error { get; init; }
}
