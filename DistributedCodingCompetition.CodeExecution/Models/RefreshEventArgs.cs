namespace DistributedCodingCompetition.CodeExecution.Models;

public class RefreshEventArgs(IReadOnlyList<ExecRunner> runners) : EventArgs
{
    public IReadOnlyList<ExecRunner> Runners { get; } = runners;
}