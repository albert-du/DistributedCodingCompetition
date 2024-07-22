using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.AspNetCore;
using DistributedCodingCompetition.CodeExecution;
using DistributedCodingCompetition.CodeExecution.Models;
using DistributedCodingCompetition.CodeExecution.Components;
using DistributedCodingCompetition.CodeExecution.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<ExecRunnerContext>("evaluationdb");

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddAntiforgery();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddQuartz(q =>
{
    JobKey jobKey = new("RefreshExecRunnerJob");
    q.AddJob<RefreshExecRunnerJob>(jobKey, j => j.WithDescription("Refreshes the exec runner"));
    q.AddTrigger(t => t
        .WithIdentity("RefreshExecRunnerJobTrigger")
        .ForJob(jobKey)
        .WithCalendarIntervalSchedule(s => s.WithIntervalInSeconds(10))); // every 10 seconds
});

builder.Services.AddQuartzServer(options => options.WaitForJobsToComplete = true);

builder.Services.AddSingleton<IExecRunnerService, ExecRunnerService>();
builder.Services.AddSingleton<IRefreshEventService, RefreshEventService>();
builder.Services.AddScoped<IExecLoadBalancer, ExecLoadBalancer>();


var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // migrate delayed
    _ = Task.Run(async () =>
    {
        await Task.Delay(3000);
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ExecRunnerContext>();
        await context.Database.MigrateAsync();
        // seed
        await Seeding.SeedDataAsync(context);
    });
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    // migrate delayed
    _ = Task.Run(async () =>
    {
        await Task.Delay(5000);
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ExecRunnerContext>();
        await context.Database.MigrateAsync();
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();
app.UseAntiforgery();
app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();