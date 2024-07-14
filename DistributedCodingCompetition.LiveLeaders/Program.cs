using DistributedCodingCompetition.LiveLeaders.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("cache");
    options.InstanceName = "LiveLeaders";
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ILeadersService, LeadersService>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/refresh/{contestId}", async (Guid contestId, DateTime sync, [FromBody] string bodyStr, ILeadersService leadersService) =>
{
    var leaders = bodyStr.Split(';').Select(x =>
    {
        var parts = x.Split(',');
        return (Guid.Parse(parts[0]), int.Parse(parts[1]));
    }).ToList();
    await leadersService.RefreshLeaderboardAsync(contestId, leaders, sync);
    return Results.Ok();
})
.WithName("Refresh")
.WithOpenApi();

app.MapPost("/report/{contestId}/{userId}", async (Guid contestId, Guid userId, int points, DateTime sync, ILeadersService leadersService) =>
{
    await leadersService.ReportJudgingAsync(contestId, userId, points, sync);
    return Results.Ok();
})
.WithName("Report")
.WithOpenApi();

app.MapGet("/leaders/{contestId}", async (Guid contestId, ILeadersService leadersService) =>
{
    var leaders = await leadersService.GetLeadersAsync(contestId, 100);
    return Results.Ok(string.Join(';', leaders.Select(x => $"{x.Item1},{x.Item2}")));
})
.WithName("GetLeaders")
.WithOpenApi();

app.Run();

