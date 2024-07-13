using DistributedCodingCompetition.Judge.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("cache");
    options.InstanceName = "RateLimit";
});

builder.Services.AddSingleton<ICodeExecutionService, CodeExecutionService>();
builder.Services.AddHttpClient<ICodeExecutionService, CodeExecutionService>(client => client.BaseAddress = new("https+http://codeexecution"));
builder.Services.AddSingleton<ISubmissionService, SubmissionService>();
builder.Services.AddHttpClient<ISubmissionService, SubmissionService>(client => client.BaseAddress = new("https+http://apiservice"));
builder.Services.AddSingleton<IProblemService, ProblemService>();
builder.Services.AddHttpClient<IProblemService, ProblemService>(client => client.BaseAddress = new("https+http://apiservice"));
builder.Services.AddSingleton<IProblemPointValueService, ProblemPointValueService>();
builder.Services.AddHttpClient<IProblemPointValueService, ProblemPointValueService>(client => client.BaseAddress = new("https+http://apiservice"));
builder.Services.AddSingleton<ILiveReportingService, LiveReportingService>();
builder.Services.AddHttpClient<ILiveReportingService, LiveReportingService>(client => client.BaseAddress = new("https+http://liveleaders"));

builder.Services.AddSingleton<IRateLimitService, RateLimitService>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
