namespace DistributedCodingCompetition.ExecRunner.Controllers;

using DistributedCodingCompetition.ExecRunner.Services;
using DistributedCodingCompetition.ExecutionShared;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Api Controller for ExecRunner
/// </summary>
/// <param name="executionService"></param>
/// <param name="configuration"></param>
[Route("api/[controller]")]
[ApiController]
public class ExecutionController(IExecutionService executionService, IConfiguration configuration) : ControllerBase
{
    private static int execCount = 0;
    public static int ExecutionCount => execCount;
    /// <summary>
    /// Execute code
    /// </summary>
    /// <param name="key"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<ExecutionResult>> PostAsync(string key, [FromBody] ExecutionRequest request)
    {
        if (key != configuration["Key"])
            return Unauthorized();

        var result = await executionService.ExecuteCodeAsync(request);
        
        // interlock increment the execution count
        Interlocked.Increment(ref execCount);
        
        return Ok(result);
    }
}
