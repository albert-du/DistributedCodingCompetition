namespace DistributedCodingCompetition.ExecRunner.Models;

/// <summary>
/// Piston request
/// </summary>
internal record PistonRequest
{
    /// <summary>
    /// Files
    /// </summary>
    internal record File
    {
        public required string Content { get; init; }
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
    /// Files to run
    /// </summary>
    public required IReadOnlyList<File> Files { get; init; }
    
    /// <summary>
    /// Input to provide
    /// </summary>
    public required string Stdin { get; init; }
}