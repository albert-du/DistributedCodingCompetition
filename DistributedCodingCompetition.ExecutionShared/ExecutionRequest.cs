namespace DistributedCodingCompetition.ExecutionShared;

/// <summary>
/// One execution on a code runner.
/// </summary>
public record ExecutionRequest
{
    /// <summary>
    /// Identification of the execution.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The source code to execute.
    /// </summary>
    public string Code { get; init; } = string.Empty;

    /// <summary>
    /// The language of the source code.
    /// </summary>
    public string Language { get; init; } = string.Empty;

    /// <summary>
    /// The stdin input for the code.
    /// Split on newlines.
    /// </summary>
    public string Input { get; init; } = string.Empty;
}
