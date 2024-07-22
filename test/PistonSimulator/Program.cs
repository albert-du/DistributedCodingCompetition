using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/v2/execute", (PistonRequest request) =>

    request switch
    {
        { Files: [{ Content: "test1" }] } => new PistonResult
        {
            Language = request.Language,
            Version = request.Version,
            Run = new PistonResult.StageResult
            {
                Code = 0,
                Stdout = request.Stdin,
                Stderr = "",
                Output = ""
            },
        },
        _ => new PistonResult
        {
            Language = request.Language,
            Version = request.Version,
            Run = new PistonResult.StageResult
            {
                Code = 1,
                Stdout = "",
                Stderr = "",
                Output = ""
            },
        }
    }
)
.WithName("Execute")
.WithOpenApi();

app.MapGet("/api/v2/packages", () =>
    new[]
    {
        new Package
        {
            Name = "test",
            Version = "1.0",
            Installed = true
        }
    }
)
.WithName("Packages")
.WithOpenApi();

app.Run();


internal record Package
{
    [JsonPropertyName("language")]
    public required string Name { get; init; }

    [JsonPropertyName("language_version")]
    public required string Version { get; init; }

    [JsonPropertyName("installed")]
    public required bool Installed { get; init; }
}

internal record PistonResult
{
    internal record StageResult
    {
        public required string Stdout { get; init; }
        public required string Stderr { get; init; }
        public required int Code { get; init; }
        public required string Output { get; init; }
    }

    public required string Language { get; init; }

    public required string Version { get; init; }

    public required StageResult Run { get; init; }

    public StageResult? Compile { get; init; }
}

internal record PistonRequest
{
    internal record File
    {
        public required string Content { get; init; }
    }

    public required string Language { get; init; }

    public required string Version { get; init; }

    public required IReadOnlyList<File> Files { get; init; }

    public required string Stdin { get; init; }
}