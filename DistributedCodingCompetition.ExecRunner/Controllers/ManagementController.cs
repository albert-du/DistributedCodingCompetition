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
    static bool installing = false;
    static bool selfCheck = false;
    static bool Available => !installing && selfCheck;

    [HttpGet]
    public async Task<ActionResult<RunnerStatus>> GetAsync(string key)
    {
        if (key != configuration["Key"])
            return Unauthorized();
        string? languages = null;
        string packages = string.Empty;
        selfCheck = true;
        try
        {
            languages = await GetLanguages();
            packages = await GetPackages();
        }
        catch
        {
            selfCheck = false;
        }

        return new RunnerStatus
        {
            TimeStamp = DateTime.UtcNow,
            Version = "1.0.0",
            Ready = Available,
            Message = installing ? "Installation in progress" : !selfCheck ? "Self Check Failed" : "Ready",
            Name = configuration["Name"] ?? "EXEC",
            Languages = languages ?? string.Empty,
            Packages = packages,
            SystemInfo = SystemInfo()
        };
    }

    [HttpGet("packages")]
    public async Task<IActionResult> GetPackagesAsync(string key)
    {
        if (key != configuration["Key"])
            return Unauthorized();

        var packages = await httpClient.GetFromJsonAsync<IReadOnlyList<Package>>(configuration["Piston"] + "api/v2/packages") ?? [];

        return Ok(packages.Where(x => x.Installed).Select(x => $"{x.Name}={x.Version}").Where(x => !string.IsNullOrWhiteSpace(x)));
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailablePackagesAsync(string key)
    {
        if (key != configuration["Key"])
            return Unauthorized();

        var packages = await httpClient.GetFromJsonAsync<IReadOnlyList<Package>>(configuration["Piston"] + "api/v2/packages") ?? [];

        return Ok(packages.Select(x => $"{x.Name}={x.Version}").Where(x => !string.IsNullOrWhiteSpace(x)));
    }

    [HttpPost("packages")]
    public async Task<IActionResult> InstallPackagesAsync(string key, [FromBody] IReadOnlyList<string> spec)
    {
        if (key != configuration["Key"])
            return Unauthorized();
        var lines = spec.Where(x => !string.IsNullOrWhiteSpace(x));
        if (!lines.All(x => x.Split('=').Length == 2))
            return BadRequest("Bad Spec");
        if (installing)
            return BadRequest("Already installing");
        installing = true;
        try
        {
            var oldSpec = (await httpClient.GetFromJsonAsync<IReadOnlyList<Package>>(configuration["Piston"] + "api/v2/packages") ?? []).Where(x => x.Installed).Select(x => $"{x.Name}={x.Version}").Where(x => !string.IsNullOrWhiteSpace(x));
            HashSet<string> removed = new(oldSpec);
            removed.ExceptWith(lines);
            List<string> response = [];
            // Uninstall removed packages
            foreach (var package in removed)
            {
                var tokens = package.Split('=');
                var name = tokens[0];
                var version = tokens[1];
                using HttpRequestMessage message = new()
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new(configuration["Piston"] + "api/v2/packages"),
                    Content = new StringContent($$"""{ "language": "{{name}}", "version": "{{version}}"}""", Encoding.UTF8, "application/json")
                };
                var resp = await httpClient.SendAsync(message);
                if (resp.IsSuccessStatusCode)
                    response.Add($"Removed {name}={version}");
                else
                    response.Add($"ERROR: Could not remove {name}={version}; {await resp.Content.ReadAsStringAsync()}");
            }

            HashSet<string> installed = new(lines);
            installed.ExceptWith(oldSpec);
            // Install new packages
            foreach (var package in installed)
            {
                var tokens = package.Split('=');
                var name = tokens[0];
                var version = tokens[1];
                using HttpRequestMessage message = new()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new(configuration["Piston"] + "api/v2/packages"),
                    Content = new StringContent($$"""{ "language": "{{name}}", "version": "{{version}}"}""", Encoding.UTF8, "application/json")

                };
                var resp = await httpClient.SendAsync(message);
                if (resp.IsSuccessStatusCode)
                    response.Add($"Installed {name}={version}");
                else
                    response.Add($"ERROR: Could not install {name}={version}; {await resp.Content.ReadAsStringAsync()}");
            }
            return Ok(response.Count is 0 ? "Already to spec" : string.Join('\n', response));
        }
        finally
        {
            installing = false;
        }
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
        var s = configuration["Piston"] + "api/v2/runtimes";
        var languages = await httpClient.GetFromJsonAsync<IReadOnlyList<Language>>(s);
        if (languages == null)
            return string.Empty;
        return string.Join('\n', languages.Select(l => l.Name + " " + l.Version + " " + l.Runtime));
    }

    private async Task<string> GetPackages()
    {
        var packages = await httpClient.GetFromJsonAsync<IReadOnlyList<Package>>(configuration["Piston"] + "api/v2/packages");
        if (packages == null)
            return string.Empty;
        return string.Join('\n', packages.Select(p => p.Name + " " + p.Version + " " + p.Installed));
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