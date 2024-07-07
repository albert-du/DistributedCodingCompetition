using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddMongoDBClient("codepersistence");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/{contest}/{problem}/{user}", async (Guid contest, Guid problem, Guid user, IMongoClient mongoClient) =>
{
    var database = mongoClient.GetDatabase("codepersistence");
    var container = database.GetCollection<PersistenceRecord>("user-code");
    var record = await container.Find($"{contest}-{problem}-{user}").FirstOrDefaultAsync();

    if (record is null)
        return Results.NotFound();

    return Results.Ok(new SavedCodeDTO
    {
        Code = record.Code,
        Language = record.Language,
        SubmissionTime = record.SubmissionTime
    });
})
.WithName("ReadCode")
.WithOpenApi();

app.MapPost("/{contest}/{problem}/{user}", async (Guid contest, Guid problem, Guid user, IMongoClient mongoClient, [FromBody] SavedCodeDTO code) =>
{
    var database = mongoClient.GetDatabase("codepersistence");
    var container = database.GetCollection<PersistenceRecord>("user-code");
    var record = new PersistenceRecord
    {
        Contest = contest,
        Problem = problem,
        User = user,
        Code = code.Code,
        Language = code.Language,
        SubmissionTime = code.SubmissionTime
    };

    await container.InsertOneAsync(record);

    return Results.Created($"/{contest}/{problem}/{user}", new SavedCodeDTO
    {
        Code = record.Code,
        Language = record.Language,
        SubmissionTime = record.SubmissionTime
    });
})
.WithName("SaveCode")
.WithOpenApi();

app.Run();

record PersistenceRecord
{
    public string Id => $"{Contest}-{Problem}-{User}";
    public Guid Contest { get; init; }
    public Guid Problem { get; init; }
    public Guid User { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Language { get; init; } = string.Empty;
    public DateTime SubmissionTime { get; init; } = DateTime.UtcNow;
}

record SavedCodeDTO
{
    public string Code { get; init; } = string.Empty;
    public string Language { get; init; } = string.Empty;
    public DateTime SubmissionTime { get; init; } = DateTime.UtcNow;
}