namespace DistributedCodingCompetition.ExecutionShared;

/// <summary>
/// The status of a code runner
/// </summary>
public record RunnerStatus
{
    /// <summary>
    /// The time this status was generated.
    /// </summary>
    public required DateTime TimeStamp { get; init; } = DateTime.UtcNow;
    
    /// <summary>
    /// The version of the code runner.
    /// </summary>
    public required string Version { get; init; }

    /// <summary>
    /// The uptime of the code runner.
    /// </summary>
    public required TimeSpan Uptime { get; init; }

    /// <summary>
    /// If this code runner is ready.
    /// </summary>
    public required bool Ready { get; init; }

    /// <summary>
    /// Message from the runner.
    /// </summary>
    public required string Message { get; init; }

    /// <summary>
    /// Self declared name of the runner
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Languages installed
    /// </summary>
    public required string Languages { get; init; }

    /// <summary>
    /// Packages installed
    /// </summary>
    public required string Packages { get; init; }

    /// <summary>
    /// Hardware info
    /// </summary>
    public required string SystemInfo { get; init; }

    /// <summary>
    /// Execution requests since application startup
    /// </summary>
    public required int ExecutionCount { get; init; }
}
