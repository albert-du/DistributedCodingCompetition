namespace DistributedCodingCompetition.CodeExecution.Services;

using System.Net;
using DistributedCodingCompetition.CodeExecution.Models;
using DistributedCodingCompetition.ExecutionShared;

public class ExecRunnerService(HttpClient httpClient) : IExecRunnerService
{
    public async Task RefreshExecRunnerAsync(ExecRunner runner)
    {
        var result = await httpClient.GetAsync($"{runner.Endpoint}{(runner.Endpoint.EndsWith('/') ? "" : "/")}api/management?key={runner.Key}");
        if (result.StatusCode is HttpStatusCode.Unauthorized)
        {
            runner.Authenticated = false;
            runner.Available = false;
            return;
        }
        if (result.StatusCode is not HttpStatusCode.OK)
        {
            runner.Available = false;
            return;
        }
        var status = await result.Content.ReadFromJsonAsync<RunnerStatus>() ?? throw new Exception("execrunner status empty");
        runner.Authenticated = true;
        runner.Live = true;
        runner.Available = status.Ready;
        runner.Languages = [..status.Languages.Split('\n')];
        runner.Packages = [..status.Packages.Split('\n')];
        runner.SystemInfo = status.SystemInfo;
        runner.Status = status.Message;
        runner.Name = status.Name;
    }

    public async Task<IReadOnlyList<string>> FetchAvailablePackagesAsync(ExecRunner runner)
    {
        var result = await httpClient.GetAsync($"{runner.Endpoint}{(runner.Endpoint.EndsWith('/') ? "" : "/")}api/management/available?key={runner.Key}");
        if (result.StatusCode is HttpStatusCode.Unauthorized)
            throw new Exception("Unauthorized");
        if (result.StatusCode is not HttpStatusCode.OK)
            throw new Exception("ExecRunner failed to fetch available packages");
        return await result.Content.ReadFromJsonAsync<IReadOnlyList<string>>() ?? throw new Exception("execrunner packages empty");
    }

    public async Task<ExecutionResult> ExecuteCodeAsync(ExecRunner runner, ExecutionRequest request)
    {
        if (!runner.Available)
            throw new Exception("ExecRunner not available");
        if (!runner.Authenticated)
            throw new Exception("ExecRunner not authenticated");
        var result = await httpClient.PostAsJsonAsync($"{runner.Endpoint}{(runner.Endpoint.EndsWith('/') ? "" : "/")}api/execution?key={runner.Key}", request);
        if (result.StatusCode is HttpStatusCode.Unauthorized)
            throw new Exception("Unauthorized");
        if (result.StatusCode is not HttpStatusCode.OK)
            throw new Exception("ExecRunner failed to execute code");
        return await result.Content.ReadFromJsonAsync<ExecutionResult>() ?? throw new Exception("execrunner result empty");
    }
}
