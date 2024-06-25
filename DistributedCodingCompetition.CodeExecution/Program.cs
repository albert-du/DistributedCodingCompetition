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
    var jobKey = new JobKey("RefreshExecRunnerJob");
    q.AddJob<RefreshExecRunnerJob>(jobKey, j => j.WithDescription("Refreshes the exec runner"));
    q.AddTrigger(t => t
        .WithIdentity("RefreshExecRunnerJobTrigger")
        .ForJob(jobKey)
        .WithCronSchedule("0 * * ? * *")); // every minute
});

builder.Services.AddQuartzServer(options => options.WaitForJobsToComplete = true);

builder.Services.AddSingleton<IExecRunnerService, ExecRunnerService>();
builder.Services.AddSingleton<IRefreshEventService, RefreshEventService>();


var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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

// migrate
using (var scope = app.Services.CreateScope())
    scope.ServiceProvider.GetRequiredService<ExecRunnerContext>().Database.MigrateAsync().Wait();


app.Run();

