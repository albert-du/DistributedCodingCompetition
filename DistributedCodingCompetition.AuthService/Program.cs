using DistributedCodingCompetition.AuthService.Models;
using DistributedCodingCompetition.AuthService.Services;
using DistributedCodingCompetition.AuthService;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddMongoDBClient("authdb");

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IPasswordService, Argon2Service>();
builder.Services.AddTransient<ITokenService, JWTTokenService>();
builder.Services.Configure<ArgonOptions>(builder.Configuration.GetSection(nameof(ArgonOptions)));

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

// seed
_ = Task.Run(async () =>
{
    await Task.Delay(3000);
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<MongoClient>();
    var db = context.GetDatabase("authdb");
    var collection = db.GetCollection<UserAuth>(nameof(UserAuth));
    var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();
    await Seeding.SeedDataAsync(collection, passwordService);
});

app.Run();
