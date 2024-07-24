using DistributedCodingCompetition.CodeExecution;
using DistributedCodingCompetition.CodeExecution.Models;
using DistributedCodingCompetition.CodeExecution.Components;
using DistributedCodingCompetition.CodeExecution.Services;

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
builder.Services.AddScoped<IExecRunnerRepository, ExecRunnerRepository>();

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

app.Run();