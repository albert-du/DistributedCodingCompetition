namespace DistributedCodingCompetition.AuthService.Models;

public record LoginResult
{
    public required string Token { get; set; }
    public required bool Admin { get; set; }
}
