namespace DistributedCodingCompetition.ExecRunner.Services;

using DistributedCodingCompetition.ExecutionShared;
using DistributedCodingCompetition.ExecRunner.Models;

public class PistonExecutionService(HttpClient httpClient, ILogger<PistonExecutionService> logger) : IExecutionService
{
    public async Task<ExecutionResult> ExecuteCodeAsync(ExecutionRequest request)
    {
        var components = request.Language.Split('-');
        if (components.Length != 2)
            throw new ArgumentException("Invalid language format");

        var language = components[0];
        var version = components[1];

        logger.LogInformation("Executing code for {Language} {Version}", language, version);

        PistonRequest pistonRequest = new()
        {
            Language = language,
            Version = version,
            Files = [

                new()
                {
                    Content = request.Code
                }
            ],
            Stdin = request.Input
        };
        var startTime = DateTime.UtcNow;
        var response = await httpClient.PostAsJsonAsync("piston", pistonRequest);
        response.EnsureSuccessStatusCode();
        var pistonResult = await response.Content.ReadFromJsonAsync<PistonResult>() ?? throw new Exception("Failed to parse response");
        return new()
        {
            Id = Guid.NewGuid(),
            RequestId = request.Id,
            TimeStamp = startTime,
            ExecutionTime = DateTime.UtcNow - startTime,
            ExitCode = pistonResult.Run.Code,
            Output = pistonResult.Run.Stdout,
            Error = $"{pistonResult.Compile?.Stdout}\n{pistonResult.Run.Stderr}"
        };
    }
}
