namespace DistributedCodingCompetition.CodeExecution.Models;

/// <summary>
/// Event Args for Refreshing the Exec Runners
/// </summary>
/// <param name="runners"></param>
public class RefreshEventArgs(IReadOnlyList<ExecRunner> runners) : EventArgs
{
    /// <summary>
    /// List of Exec Runners
    /// </summary>
    public IReadOnlyList<ExecRunner> Runners { get; } = runners;
}