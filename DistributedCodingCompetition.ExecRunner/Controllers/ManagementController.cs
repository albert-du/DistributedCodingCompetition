namespace DistributedCodingCompetition.ExecRunner.Controllers;

using Microsoft.AspNetCore.Mvc;
using DistributedCodingCompetition.ExecutionShared;


[Route("api/[controller]")]
[ApiController]
public class ManagementController : ControllerBase
{
    [HttpGet]
    public RunnerStatus Get()
    {
        return new()
        {
            TimeStamp = DateTime.UtcNow,
            Status = "Running",
            Version = "1.0.0",
            LastExecution = DateTime.UtcNow,
            LastExecutionDuration = TimeSpan.FromSeconds(1),,
        };
    }
}
