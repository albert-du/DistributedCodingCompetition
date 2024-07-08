namespace DistributedCodingCompetition.Web.Models;

/// <summary>
/// Saved code
/// </summary>
public sealed record SavedCode
{
    /// <summary>
    /// Source Code
    /// </summary>
    public required string Code { get; init; } = string.Empty;

    /// <summary>
    /// Language of the code "lang=version"
    /// </summary>
    public required string Language { get; init; } = string.Empty;

    /// <summary>
    /// Time of save
    /// </summary>
    public required DateTime SubmissionTime { get; init; } = DateTime.UtcNow;
}
