using DistributedCodingCompetition.AuthService.MigrationService;
using DistributedCodingCompetition.AuthService.Data.Contexts;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddNpgsqlDbContext<AuthenticationDbContext>("authdb");

var host = builder.Build();
host.Run();
