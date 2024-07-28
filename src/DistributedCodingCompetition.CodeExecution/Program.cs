global using System.Text.Json;
global using System.Net;
global using System.Threading.Tasks;
global using Microsoft.Extensions.Caching.Distributed;
global using MongoDB.Driver;
global using DistributedCodingCompetition.CodeExecution;
global using DistributedCodingCompetition.CodeExecution.Components;
global using DistributedCodingCompetition.CodeExecution.Services;
global using DistributedCodingCompetition.CodeExecution.Models;
global using DistributedCodingCompetition.ExecutionShared;
global using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddMongoDBClient("evaluationdb");

builder.Services.AddControllers();
builder.Services.AddAntiforgery();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("cache");
    options.InstanceName = "CodeExecution";
});

builder.Services.AddSingleton<IExecRunnerService, ExecRunnerService>();
builder.Services.AddSingleton<IActiveRunnersService, ActiveRunnersService>();
builder.Services.AddSingleton<IExecRunnerRepository, ExecRunnerRepository>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // seed the database with a default exec runner
    await Seeding.SeedDataAsync(app.Services.GetRequiredService<IExecRunnerRepository>());
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();
app.UseAntiforgery();
app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();