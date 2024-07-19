namespace DistributedCodingCompetition.ExecRunner.Models;

/// <summary>
/// Piston result
/// </summary>
internal record PistonResult
{
    /// <summary>
    /// Stage result
    /// </summary>
    internal record StageResult
    {
        public required string Stdout { get; init; }
        public required string Stderr { get; init; }
        public required int Code { get; init; }
        public required string Output { get; init; }
    }

    /// <summary>
    /// Language of the code
    /// </summary>

    public required string Language { get; init; }

    /// <summary>
    /// Version of the code
    /// </summary>
    public required string Version { get; init; }

    /// <summary>
    /// Run result
    /// </summary>
    public required StageResult Run { get; init; }

    /// <summary>
    /// Compile Result 
    /// </summary>
    public StageResult? Compile { get; init; }
}