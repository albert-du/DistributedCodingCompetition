namespace DistributedCodingCompetition.ExecutionShared;
public record RunnerStatus
{
    public required DateTime TimeStamp { get; init; }
    public required string Version { get; init; }
    public TimeSpan Uptime { get; init; }
    public required bool Ready { get; init; }
    public required string Message { get; init; }
    public required string Name { get; init; }
    public required string Languages { get; init; }
    public required string Packages { get; init; }
    public required string SystemInfo { get; init; }
}
