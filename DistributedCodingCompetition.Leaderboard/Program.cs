using DistributedCodingCompetition.ApiService.Models;
using DistributedCodingCompetition.Leaderboard.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("cache");
    options.InstanceName = "RateLimit";
});

builder.Services.AddSingleton<ILeaderboardService, LeaderboardService>();
builder.Services.AddHttpClient<ILeaderboardService, LeaderboardService>(client => client.BaseAddress = new("https+http://apiservice"));
builder.Services.AddSingleton<ILiveReportingService, LiveReportingService>();
builder.Services.AddHttpClient<ILiveReportingService, LiveReportingService>(client => client.BaseAddress = new("https+http://liveleaders"));

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/leaderboard/{contestId}/{page}", async (Guid contestId, int page, ILeaderboardService leaderboardService) =>
{
    // check if page is valid
    if (page < 1)
        return Results.BadRequest("Page must be greater than 0");

    // check the redis cache for the leaderboard
    return Results.Ok(await leaderboardService.GetLeaderboardAsync(contestId, page) ?? new Leaderboard() { ContestId = contestId, ContestName = "No leaderboard", Count = 0, Entries = [], Creation = DateTime.UtcNow });
})
.WithName("Leaderboard")
.WithOpenApi();

app.MapGet("/live/{contestId}", async (Guid contestId, ILeaderboardService leaderboardService, ILiveReportingService liveReportingService) =>
{
    // get the first 200 leaders from last leaderboard
    List<Task<Leaderboard?>> tasks = [];

    for (var i = 1; i <= 4; i++)
        tasks.Add(leaderboardService.GetLeaderboardAsync(contestId, i));

    Dictionary<Guid, string> leaderboardEntries = [];
    var contestName = "";

    foreach (var task in tasks)
    {
        var leaderboard = await task;
        if (leaderboard is not null)
        {
            contestName = leaderboard.ContestName;
            foreach (var entry in leaderboard.Entries)
                leaderboardEntries[entry.UserId] = entry.Username;
        }
    }

    var adjusted = await liveReportingService.GetLeadersAsync(contestId) ?? [];

    var liveEntries = adjusted.Select((x, i) => new LeaderboardEntry(x.Item1, leaderboardEntries[x.Item1], x.Item2, i + 1)).ToList();

    return Results.Ok(new Leaderboard
    {
        ContestName = $"{contestName} LIVE",
        Count = liveEntries.Count,
        ContestId = contestId,
        Entries = liveEntries
    });
})
.WithName("LiveLeaderboard")
.WithOpenApi();

app.Run();

