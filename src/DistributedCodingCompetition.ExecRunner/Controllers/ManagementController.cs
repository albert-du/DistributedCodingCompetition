namespace DistributedCodingCompetition.ExecRunner.Controllers;

using Microsoft.AspNetCore.Mvc;
using DistributedCodingCompetition.ExecutionShared;
using System.Text;
using System.Net.Http;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Management Controller for ExecRunner
/// </summary>
/// <param name="configuration"></param>
/// <param name="httpClient"></param>
/// <param name="logger"></param>
/// <param name="serviceScopeFactory"></param>
[Route("api/[controller]")]
[ApiController]
public class ManagementController(IConfiguration configuration, HttpClient httpClient, ILogger<ManagementController> logger, IServiceScopeFactory serviceScopeFactory) : ControllerBase
{
    private readonly static DateTime startTime = DateTime.UtcNow;
    static bool installing = false;
    static bool selfCheck = false;

    static bool Available => !installing && selfCheck;

    static string? installationResult;

    /// <summary>
    /// Get the status of the runner
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<RunnerStatus>> GetAsync(string key)
    {
        if (key != configuration["Key"])
            return Unauthorized();

        if (installing)
        {
            logger.LogInformation("Installation in progress");
            return new RunnerStatus
            {
                Languages = string.Empty,
                Packages = string.Empty,
                TimeStamp = DateTime.UtcNow,
                SystemInfo = SystemInfo(),
                Version = "1.0.0",
                Ready = false,
                Message = "Installation in progress",
                Name = configuration["Name"] ?? "EXEC",
                Uptime = DateTime.UtcNow - startTime,
                ExecutionCount = ExecutionController.ExecutionCount,
            };
        }

        string? languages = null;
        string packages = string.Empty;
        selfCheck = true;
        try
        {
            languages = await GetLanguages();
            packages = await GetInstalledPackages();
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
            Message = installationResult ?? (installing ? "Installation in progress" : !selfCheck ? "Self Check Failed" : "Ready"),
            Name = configuration["Name"] ?? "EXEC",
            Uptime = DateTime.UtcNow - startTime,
            Languages = languages ?? string.Empty,
            Packages = packages,
            SystemInfo = SystemInfo(),
            ExecutionCount = ExecutionController.ExecutionCount,
        };
    }

    /// <summary>
    /// Get installed packages
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [HttpGet("packages")]
    public async Task<IActionResult> GetPackagesAsync(string key)
    {
        if (key != configuration["Key"])
            return Unauthorized();

        if (installing)
            return Ok(new { });

        var packages = await httpClient.GetFromJsonAsync<IReadOnlyList<Package>>(configuration["Piston"] + "api/v2/packages") ?? [];

        return Ok(packages.Where(x => x.Installed).Select(x => $"{x.Name}={x.Version}").Where(x => !string.IsNullOrWhiteSpace(x)));
    }

    /// <summary>
    /// Get the available packages
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailablePackagesAsync(string key)
    {
        if (key != configuration["Key"])
            return Unauthorized();

        var packages = await httpClient.GetFromJsonAsync<IReadOnlyList<Package>>(configuration["Piston"] + "api/v2/packages") ?? [];

        return Ok(packages.Select(x => $"{x.Name}={x.Version}").Where(x => !string.IsNullOrWhiteSpace(x)));
    }

    /// <summary>
    /// Install packages according to a specification
    /// </summary>
    /// <param name="key"></param>
    /// <param name="spec"></param>
    /// <returns></returns>
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
        var oldSpec = (await httpClient.GetFromJsonAsync<IReadOnlyList<Package>>(configuration["Piston"] + "api/v2/packages") ?? []).Where(x => x.Installed).Select(x => $"{x.Name}={x.Version}").Where(x => !string.IsNullOrWhiteSpace(x));

        HashSet<string> removed = new(oldSpec);
        removed.ExceptWith(lines);

        HashSet<string> installed = new(lines);
        installed.ExceptWith(oldSpec);

        if (installed.Count == 0 && removed.Count == 0)
            return Ok("No changes");

        // fire and forget
        _ = Task.Run(async () =>
        {
            using var scope = serviceScopeFactory.CreateScope();
            var httpClient = scope.ServiceProvider.GetRequiredService<HttpClient>();
            await InstallAsync(installed, removed, httpClient);
        });

        return Ok("Installation started");
    }

    private async Task InstallAsync(IEnumerable<string> install, IEnumerable<string> remove, HttpClient httpClient)
    {
        Console.WriteLine("Starting installation");
        installing = true;
        try
        {
            List<string> response = [];
            // Uninstall removed packages
            foreach (var package in remove)
            {
                Console.WriteLine($"Removing {package}");
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

            // Install new packages
            foreach (var package in install)
            {
                Console.WriteLine($"Installing {package}");
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
            installationResult = string.Join('\n', response);
        }
        finally
        {
            installing = false;
        }
        Console.WriteLine("Installation complete");
        await Task.Delay(30_000);
        installationResult = null;
    }

    /// <summary>
    /// Return system information
    /// </summary>
    /// <returns></returns>
    private static string SystemInfo() =>
        $"OS: {System.Runtime.InteropServices.RuntimeInformation.OSDescription}\n" +
        $"Framework: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}\n" +
        $"Processors: {Environment.ProcessorCount}\n" +
        $"Runtime: {System.Runtime.InteropServices.RuntimeInformation.RuntimeIdentifier}\n" +
        $"Memory: {Environment.WorkingSet / 1024 / 1024}MB";

    /// <summary>
    /// Get the installed languages
    /// </summary>
    /// <returns></returns>
    private async Task<string> GetLanguages()
    {
        StringBuilder sb = new();
        var s = configuration["Piston"] + "api/v2/runtimes";
        var languages = await httpClient.GetFromJsonAsync<IReadOnlyList<Language>>(s);
        if (languages == null)
            return string.Empty;
        return string.Join('\n', languages.Select(l => $"{l.Name}={l.Version}"));
    }

    /// <summary>
    /// Get the installed packages
    /// </summary>
    /// <returns></returns>
    private async Task<string> GetInstalledPackages()
    {
        if (installing)
            return string.Empty;
        var packages = await httpClient.GetFromJsonAsync<IReadOnlyList<Package>>(configuration["Piston"] + "api/v2/packages");
        if (packages == null)
            return string.Empty;
        return string.Join('\n', packages.Where(p => p.Installed).Select(p => $"{p.Name}={p.Version}"));
    }
}

/// <summary>
/// Structure to work with the piston instance directly
/// </summary>
internal record Language
{
    [JsonPropertyName("language")]
    public required string Name { get; init; }

    [JsonPropertyName("version")]
    public required string Version { get; init; }

    [JsonPropertyName("runtime")]
    public string? Runtime { get; init; }
}

/// <summary>
/// Piston package
/// </summary>
internal record Package
{
    [JsonPropertyName("language")]
    public required string Name { get; init; }

    [JsonPropertyName("language_version")]
    public required string Version { get; init; }

    [JsonPropertyName("installed")]
    public required bool Installed { get; init; }
}