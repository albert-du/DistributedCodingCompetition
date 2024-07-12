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

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/leaders/{contestId}/{page}", (Guid contestId, int page, ILeaderboardService leaderboardService) =>
{
    // check if page is valid
    if (page < 1)
        return Results.BadRequest("Page must be greater than 0");

    // check the redis cache for the leaderboard
    return Results.Ok(leaderboardService.GetLeaderboardAsync(contestId, page));

})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

