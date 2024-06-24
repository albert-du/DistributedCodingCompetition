using DistributedCodingCompetition.ExecRunner.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IExecutionService, PistonExecutionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler("/Error");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.Run();
