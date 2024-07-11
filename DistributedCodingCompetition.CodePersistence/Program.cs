using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddMongoDBClient("codePersistenceDB");

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
    var container = GetCollection(mongoClient);
    var idString = PersistenceRecord.IdString(contest, problem, user);
    var record = await container.Find(c => c.Id == idString).FirstOrDefaultAsync();

    if (record is null)
    {
        app.Logger.LogInformation("Code not found for {contest}/{problem}/{user}", contest, problem, user);
        return Results.NotFound();
    }
    app.Logger.LogInformation("Code found for {contest}/{problem}/{user}", contest, problem, user);

    return Results.Ok(new SavedCodeDTO
    {
        Code = record.Code,
        Language = record.Language,
        SubmissionTime = record.SubmissionTime
    });
})
.WithName("ReadCode")
.WithOpenApi();

app.MapPost("/{contest}/{problem}/{user}", async (Guid contest, Guid problem, Guid user, IMongoClient mongoClient, SavedCodeDTO code) =>
{
    var container = GetCollection(mongoClient);
    var idString = PersistenceRecord.IdString(contest, problem, user);
    var existingRecord = await container.Find(c => c.Id == idString).FirstOrDefaultAsync();
    if (existingRecord is not null && existingRecord.SubmissionTime > code.SubmissionTime)
    {
        app.Logger.LogInformation("Code is older than existing code for {contest}/{problem}/{user}", contest, problem, user);
        return Results.BadRequest("Code is older than existing code");
    }

    app.Logger.LogInformation("Saving code for {contest}/{problem}/{user}", contest, problem, user);

    var record = new PersistenceRecord
    {
        Id = idString,
        Contest = contest,
        Problem = problem,
        User = user,
        Code = code.Code,
        Language = code.Language,
        SubmissionTime = code.SubmissionTime
    };


    if (existingRecord is null)
        await container.InsertOneAsync(record);
    else
        await container.ReplaceOneAsync(c => c.Id == idString, record);

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

static IMongoCollection<PersistenceRecord> GetCollection(IMongoClient client) =>
    client.GetDatabase("codePersistenceDB").GetCollection<PersistenceRecord>("user-code");

record PersistenceRecord
{
    public required string Id { get; init; }
    public Guid Contest { get; init; }
    public Guid Problem { get; init; }
    public Guid User { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Language { get; init; } = string.Empty;
    public DateTime SubmissionTime { get; init; } = DateTime.UtcNow;

    public static string IdString(Guid contest, Guid problem, Guid user) => $"{contest}-{problem}-{user}";
}

record SavedCodeDTO
{
    public string Code { get; init; } = string.Empty;
    public string Language { get; init; } = string.Empty;
    public DateTime SubmissionTime { get; init; } = DateTime.UtcNow;
}