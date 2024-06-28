using Microsoft.EntityFrameworkCore;
using DistributedCodingCompetition.ApiService;
using DistributedCodingCompetition.ApiService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<ContestContext>("contestdb");

// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    // migrate delayed
    _ = Task.Run(async () =>
    {
        await Task.Delay(5000);
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ContestContext>();
        await context.Database.MigrateAsync();
        await Seeding.SeedDataAsync(context);
    });
}

app.Run();