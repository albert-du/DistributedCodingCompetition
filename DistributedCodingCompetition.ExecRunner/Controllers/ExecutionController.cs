namespace DistributedCodingCompetition.ExecRunner.Controllers;

using DistributedCodingCompetition.ExecRunner.Services;
using DistributedCodingCompetition.ExecutionShared;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ExecutionController(IExecutionService executionService, IConfiguration configuration) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ExecutionResult>> PostAsync(string key, [FromBody] ExecutionRequest request)
    {
        if (key != configuration["Key"])
            return Unauthorized();

        var result = await executionService.ExecuteCodeAsync(request);
        return Ok(result);
    }
}
