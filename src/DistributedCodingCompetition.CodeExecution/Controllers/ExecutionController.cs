namespace DistributedCodingCompetition.CodeExecution.Controllers;

using Microsoft.AspNetCore.Mvc;
using DistributedCodingCompetition.CodeExecution.Services;
using DistributedCodingCompetition.ExecutionShared;

/// <summary>
/// Controller for code execution.
/// </summary>
/// <param name="logger"></param>
/// <param name="execRunnerService"></param>
/// <param name="activeRunnersService"></param>
[ApiController]
[Route("[controller]")]
public class ExecutionController(ILogger<ExecutionController> logger, IActiveRunnersService activeRunnersService, IExecRunnerService execRunnerService) : ControllerBase
{
    /// <summary>
    /// Executes code.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<ExecutionResult>> PostAsync([FromBody] ExecutionRequest request)
    {
        var execRunner = await activeRunnersService.FindExecRunnerAsync(request.Language);
        if (execRunner == null)
        {
            logger.LogWarning("No available runners for requested language: \"{Language}\"", request.Language);
            return StatusCode(503, $"No available runners for language: \"{request.Language}\"");
        }
        return await execRunnerService.ExecuteCodeAsync(execRunner, request);
    }

    /// <summary>
    /// Batch executes code.
    /// </summary>
    /// <param name="requests"></param>
    /// <returns></returns>
    [HttpPost("batch")]
    public async Task<ActionResult<IReadOnlyList<ExecutionResult>>> PostBatchAsync([FromBody] IReadOnlyCollection<ExecutionRequest> requests)
    {
        var execRunnerRequests = await activeRunnersService.BalanceRequestsAsync(requests);
        List<Task<ExecutionResult>> tasks = [];

        foreach (var (request, execRunner) in execRunnerRequests)
        {
            if (execRunner is null)
            {
                Console.WriteLine(request.Language);
                logger.LogWarning("No available runners for requested language: \"{Language}\"", request.Language);
                return StatusCode(503, $"No available runners for language: \"{request.Language}\"");
            }
            else
                tasks.Add(execRunnerService.ExecuteCodeAsync(execRunner, request));
        }
        List<ExecutionResult> results = new(tasks.Count);
        foreach (var task in tasks) results.Add(await task);
        logger.LogInformation("Batch execution completed with {Count} results", results.Count);

        return results;
    }

    /// <summary>
    /// All available languages.
    /// </summary>
    /// <returns></returns>
    [HttpGet("languages")]
    public Task<IEnumerable<string>> GetLanguages() =>
        activeRunnersService.GetLanguagesAsync();
}