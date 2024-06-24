namespace DistributedCodingCompetition.ExecRunner;

using DistributedCodingCompetition.ExecRunner.Services;
using DistributedCodingCompetition.ExecutionShared;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ExecutionController(IExecutionService executionService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ExecutionResult>> PostAsync([FromBody] ExecutionRequest request)
    {
        var result = await executionService.ExecuteCodeAsync(request);
        return Ok(result);
    }
}
