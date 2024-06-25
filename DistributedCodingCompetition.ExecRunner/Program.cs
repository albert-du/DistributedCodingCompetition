using DistributedCodingCompetition.ExecRunner.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IExecutionService, PistonExecutionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler("/Error");

app.UseHttpsRedirection();

app.UseStaticFiles();
app.MapControllers();

app.Run();
