﻿namespace DistributedCodingCompetition.CodeExecution.Services;

using DistributedCodingCompetition.CodeExecution.Models;

/// <summary>
/// Message broker for when events refresh to update UIs
/// </summary>
public interface IRefreshEventService
{
    /// <summary>
    /// Send refresh event
    /// </summary>
    /// <param name="sender"></param>
    void Refresh(object sender, IReadOnlyList<ExecRunner> execRunners);

    /// <summary>
    /// Sub to react to event
    /// </summary>
    event EventHandler<RefreshEventArgs>? RefreshEvent;
}
