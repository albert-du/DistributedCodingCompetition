namespace DistributedCodingCompetition.ExecutionShared;

/// <summary>
/// The result of a code execution
/// </summary>
public record ExecutionResult
{
    /// <summary>
    /// Id of this result.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The start time of the execution.
    /// </summary>
    public DateTime TimeStamp { get; init; }

    /// <summary>
    /// The execution time of the code.
    /// </summary>
    public TimeSpan ExecutionTime { get; init; }

    /// <summary>
    /// The exit code of the execution.
    /// </summary>
    public int ExitCode { get; init; }

    /// <summary>
    /// The stdout output of the execution.
    /// </summary>
    public string Output { get; init; } = string.Empty;

    /// <summary>
    /// The stderr output of the execution.
    /// </summary>
    public string Error { get; init; } = string.Empty;
}
