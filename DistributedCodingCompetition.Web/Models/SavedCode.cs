namespace DistributedCodingCompetition.Web.Models;

public record SavedCode
{
    public string Code { get; init; } = string.Empty;
    public string Language { get; init; } = string.Empty;
    public DateTime SubmissionTime { get; init; } = DateTime.UtcNow;
}
