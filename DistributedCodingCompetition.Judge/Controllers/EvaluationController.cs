using DistributedCodingCompetition.Judge.Services;
namespace DistributedCodingCompetition.Judge.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class EvaluationController(ILogger<EvaluationController> logger, ICodeExecutionService codeExecutionService, IRateLimitService rateLimitService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PostAsync()
    {

    }
}
