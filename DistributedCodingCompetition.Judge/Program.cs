
global using DistributedCodingCompetition.ApiService.Client;
global using DistributedCodingCompetition.CodeExecution.Client;
global using DistributedCodingCompetition.LiveLeaders.Client;
global using DistributedCodingCompetition.Judge.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddDistributedCodingCompetitionAPI("https+http://apiservice");
builder.AddDistributedCodingCompetitionCodeExecution("https+http://codeexecution");
builder.AddDistributedCodingCompetitionLiveLeaders("https+http://liveleaders");

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
