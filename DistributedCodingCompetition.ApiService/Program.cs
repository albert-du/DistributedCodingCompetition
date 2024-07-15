global using DistributedCodingCompetition.Models;
global using DistributedCodingCompetition.ApiService.Models;

using DistributedCodingCompetition.ApiService;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<ContestContext>("contestdb");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapDefaultEndpoints();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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
else
{
    _ = Task.Run(async () =>
    {
        await Task.Delay(5000);
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ContestContext>();
        await context.Database.MigrateAsync();
    });
}

app.Run();