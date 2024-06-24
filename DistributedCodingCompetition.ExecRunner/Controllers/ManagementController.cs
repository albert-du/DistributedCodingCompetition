namespace DistributedCodingCompetition.ExecRunner.Controllers;

using Microsoft.AspNetCore.Mvc;
using DistributedCodingCompetition.ExecutionShared;
using System.Text;
using System.Net.Http;
using System.Text.Json.Serialization;

[Route("api/[controller]")]
[ApiController]
public class ManagementController(IConfiguration configuration, HttpClient httpClient) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<RunnerStatus>> GetAsync(string key)
    {
        if (key != configuration["Key"])
            return Unauthorized();
        var languages = await GetLanguages();
        return new RunnerStatus()
        {
            TimeStamp = DateTime.UtcNow,
            Version = "1.0.0",
            Healthy = true,
            Message = "Ready",
            Name = configuration["Name"] ?? "EXEC",
            Languages = languages,
            SystemInfo = SystemInfo()
        };
    }

    private static string SystemInfo()
    {
        StringBuilder sb = new();
        sb.AppendLine("OS: " + System.Runtime.InteropServices.RuntimeInformation.OSDescription);
        sb.AppendLine("Framework: " + System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);
        sb.AppendLine("Processors: " + Environment.ProcessorCount);
        sb.AppendLine("Runtime: " + System.Runtime.InteropServices.RuntimeInformation.RuntimeIdentifier);
        sb.AppendLine("Memory: " + (Environment.WorkingSet / 1024 / 1024) + "MB");
        return sb.ToString();
    }

    private async Task<string> GetLanguages()
    {
        StringBuilder sb = new();
        var languages = await httpClient.GetFromJsonAsync<IReadOnlyList<Language>>(configuration["Piston"] + "/api/v2/runtimes");
        if (languages == null)
            return string.Empty;
        return string.Join('\n', languages.Select(l => l.Name + " " + l.Version + " " + l.Runtime));
    }
}
internal record Language
{
    [JsonPropertyName("language")]
    public required string Name { get; init; }

    [JsonPropertyName("version")]
    public required string Version { get; init; }

    [JsonPropertyName("runtime")]
    public string? Runtime { get; init; }
}

internal record Package
{
    [JsonPropertyName("language")]
    public required string Name { get; init; }

    [JsonPropertyName("language_version")]
    public required string Version { get; init; }

    [JsonPropertyName("installed")]
    public required bool Installed { get; init; }
}