namespace DistributedCodingCompetition.Web.Models;

public record SavedCode
{
    public required string Code { get; init; } = string.Empty;
    public required string Language { get; init; } = string.Empty;
    public required DateTime SubmissionTime { get; init; } = DateTime.UtcNow;
}
