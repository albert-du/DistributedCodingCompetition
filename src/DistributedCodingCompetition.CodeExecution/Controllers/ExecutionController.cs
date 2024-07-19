namespace DistributedCodingCompetition.CodeExecution.Controllers;

using Microsoft.AspNetCore.Mvc;
using DistributedCodingCompetition.CodeExecution.Models;
using DistributedCodingCompetition.CodeExecution.Services;
using DistributedCodingCompetition.ExecutionShared;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Controller for code execution.
/// </summary>
/// <param name="logger"></param>
/// <param name="execLoadBalancer"></param>
/// <param name="execRunnerService"></param>
/// <param name="execRunnerContext"></param>
[ApiController]
[Route("[controller]")]
public class ExecutionController(ILogger<ExecutionController> logger, IExecLoadBalancer execLoadBalancer, IExecRunnerService execRunnerService, ExecRunnerContext execRunnerContext) : ControllerBase
{
    /// <summary>
    /// Executes code.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<ExecutionResult>> PostAsync([FromBody] ExecutionRequest request)
    {
        var execRunner = execLoadBalancer.SelectRunner(request);
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
        var execRunnerRequests = execLoadBalancer.BalanceRequests(requests);
        List<Task<ExecutionResult>> tasks = [];

        foreach (var (request, execRunner) in execRunnerRequests)
        {
            if (execRunner is null)
            {
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
    public IEnumerable<string> GetLanguages() =>
        execRunnerContext.ExecRunners.AsNoTracking().ToArray().SelectMany(x => x.Languages).Distinct();
}