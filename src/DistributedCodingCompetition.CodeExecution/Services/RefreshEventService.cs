namespace DistributedCodingCompetition.CodeExecution.Services;

using DistributedCodingCompetition.CodeExecution.Models;

/// <summary>
/// Message broker for when events refresh to update UIs
/// </summary>
public class RefreshEventService : IRefreshEventService
{
    /// <summary>
    /// Send refresh event
    /// </summary>
    /// <param name="sender"></param>
    public void Refresh(object sender, IReadOnlyList<ExecRunner> execRunners) =>
        RefreshEvent?.Invoke(sender, new(execRunners));

    /// <summary>
    /// Sub to react to event
    /// </summary>
    public event EventHandler<RefreshEventArgs>? RefreshEvent;
}
