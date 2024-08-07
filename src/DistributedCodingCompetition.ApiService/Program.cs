global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using DistributedCodingCompetition.ApiService;
global using DistributedCodingCompetition.ApiService.Models;
global using DistributedCodingCompetition.ApiService.Data;
global using DistributedCodingCompetition.ApiService.Data.Models;
global using DistributedCodingCompetition.ApiService.Data.Contexts;

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
}

app.Run();