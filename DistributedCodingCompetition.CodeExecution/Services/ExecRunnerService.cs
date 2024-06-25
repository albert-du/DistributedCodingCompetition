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
        runner.Available = status.Ready;
        runner.Languages = status.Languages.Split('\n').ToList();
    }
}
