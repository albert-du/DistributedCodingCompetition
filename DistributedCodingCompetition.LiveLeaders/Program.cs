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

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/refresh/{contestId}", async (Guid contestId, IReadOnlyList<(Guid, int)> Leaders, ILeadersService leadersService) =>
{
    await leadersService.RefreshLeaderboardAsync(contestId, Leaders, DateTime.UtcNow);
    return Results.Ok();
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/leaders/{contestId}", async (Guid contestId, ILeadersService leadersService) =>
{
    var leaders = await leadersService.GetLeadersAsync(contestId);
    return Results.Ok(leaders);
})

app.Run();

