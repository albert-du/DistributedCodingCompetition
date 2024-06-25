namespace DistributedCodingCompetition.CodeExecution.Services;

/// <summary>
/// Message broker for when events refresh to update UIs
/// </summary>
public class RefreshEventService : IRefreshEventService
{
    /// <summary>
    /// Send refresh event
    /// </summary>
    /// <param name="sender"></param>
    public void Refresh(object sender) =>
        RefreshEvent?.Invoke(sender, EventArgs.Empty);

    /// <summary>
    /// Sub to react to event
    /// </summary>
    public event EventHandler? RefreshEvent;
}
