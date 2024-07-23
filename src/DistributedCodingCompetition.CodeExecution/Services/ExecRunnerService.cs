namespace DistributedCodingCompetition.CodeExecution.Services;

using System.Net;
using DistributedCodingCompetition.CodeExecution.Models;
using DistributedCodingCompetition.ExecutionShared;

/// <inheritdoc />
public class ExecRunnerService(HttpClient httpClient) : IExecRunnerService
{
    /// <inheritdoc />
    public async Task<RunnerStatus> RefreshExecRunnerAsync(ExecRunner runner)
    {
        HttpResponseMessage result;
        try
        {
            result = await httpClient.GetAsync($"{runner.Endpoint}{(runner.Endpoint.EndsWith('/') ? "" : "/")}api/management?key={runner.Key}");

        }
        catch (HttpRequestException)
        {
            return new()
            {
                Name = runner.Name,
                Version = "Unknown",
                Uptime = TimeSpan.Zero,
                Ready = false,
                TimeStamp = DateTime.UtcNow,
                Message = "Failed to connect to this execution runner",
                Languages = string.Empty,
                Packages = string.Empty,
                SystemInfo = string.Empty,
                ExecutionCount = 0
            };
        }

        if (result.StatusCode is HttpStatusCode.Unauthorized)
            return new()
            {
                Name = runner.Name,
                Version = "Unknown",
                Uptime = TimeSpan.Zero,
                Ready = false,
                TimeStamp = DateTime.UtcNow,
                Message = "Failed to authenticate to this execution runner",
                Languages = string.Empty,
                Packages = string.Empty,
                SystemInfo = string.Empty,
                ExecutionCount = 0
            };
        if (result.StatusCode is not HttpStatusCode.OK)
            return new()
            {
                Name = runner.Name,
                Version = "Unknown",
                Uptime = TimeSpan.Zero,
                Ready = false,
                TimeStamp = DateTime.UtcNow,
                Message = $"Failed to connect to this execution runner {result.StatusCode}",
                Languages = string.Empty,
                Packages = string.Empty,
                SystemInfo = string.Empty,
                ExecutionCount = 0
            };
        return await result.Content.ReadFromJsonAsync<RunnerStatus>() ?? throw new Exception("execrunner status empty");
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<string>> FetchAvailablePackagesAsync(ExecRunner runner)
    {
        var result = await httpClient.GetAsync($"{runner.Endpoint}{(runner.Endpoint.EndsWith('/') ? "" : "/")}api/management/available?key={runner.Key}");
        if (result.StatusCode is HttpStatusCode.Unauthorized)
            throw new Exception("Unauthorized");
        if (result.StatusCode is not HttpStatusCode.OK)
            throw new Exception("ExecRunner failed to fetch available packages");
        return await result.Content.ReadFromJsonAsync<IReadOnlyList<string>>() ?? throw new Exception("execrunner packages empty");
    }

    /// <inheritdoc />
    public async Task SetPackagesAsync(ExecRunner execRunner, IEnumerable<string> packages)
    {
        var result = await httpClient.PostAsJsonAsync($"{execRunner.Endpoint}{(execRunner.Endpoint.EndsWith('/') ? "" : "/")}api/management/packages?key={execRunner.Key}", packages);
        if (result.StatusCode is HttpStatusCode.Unauthorized)
            throw new Exception("Unauthorized");
        if (result.StatusCode is not HttpStatusCode.OK)
            throw new Exception("ExecRunner failed to set packages");
    }

    /// <inheritdoc />
    public async Task<ExecutionResult> ExecuteCodeAsync(ExecRunner runner, ExecutionRequest request)
    {
        var result = await httpClient.PostAsJsonAsync($"{runner.Endpoint}{(runner.Endpoint.EndsWith('/') ? "" : "/")}api/execution?key={runner.Key}", request);
        if (result.StatusCode is HttpStatusCode.Unauthorized)
            throw new Exception("Unauthorized");
        if (result.StatusCode is not HttpStatusCode.OK)
            throw new Exception("ExecRunner failed to execute code");
        return await result.Content.ReadFromJsonAsync<ExecutionResult>() ?? throw new Exception("execrunner result empty");
    }
}
