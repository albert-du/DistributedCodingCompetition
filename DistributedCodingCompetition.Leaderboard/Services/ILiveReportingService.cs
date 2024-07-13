﻿namespace DistributedCodingCompetition.Leaderboard.Services;

using DistributedCodingCompetition.ApiService.Models;

public interface ILiveReportingService
{
    Task RefreshAsync(Leaderboard leaderboard);
}