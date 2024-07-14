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

app.MapPost("/refresh/{contestId}", async (Guid contestId, DateTime sync, IReadOnlyList<(Guid, int)> Leaders, ILeadersService leadersService) =>
{
    await leadersService.RefreshLeaderboardAsync(contestId, Leaders, sync);
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
    return Results.Ok(leaders);
})
.WithName("GetLeaders")
.WithOpenApi();

app.Run();

