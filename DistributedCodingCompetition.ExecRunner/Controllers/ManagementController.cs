namespace DistributedCodingCompetition.ExecRunner.Controllers;

using Microsoft.AspNetCore.Mvc;
using DistributedCodingCompetition.ExecutionShared;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class ManagementController(IConfiguration configuration) : ControllerBase
{
    [HttpGet]
    public RunnerStatus Get()
    {
        return new()
        {
            TimeStamp = DateTime.UtcNow,
            Version = "1.0.0",
            Healthy = true,
            Message = "Ready",
            Name = configuration["Name"] ?? "EXEC",
            Languages = "",
            SystemInfo = SystemInfo()
        };
    }

    private static string SystemInfo()
    {
        StringBuilder sb = new();
        sb.AppendLine("OS: " + System.Runtime.InteropServices.RuntimeInformation.OSDescription);
        sb.AppendLine("Framework: " + System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);
        sb.AppendLine("Processors: " + Environment.ProcessorCount);
        sb.AppendLine("Runtime: " + System.Runtime.InteropServices.RuntimeInformation.RuntimeIdentifier);
        sb.AppendLine("Memory: " + (Environment.WorkingSet / 1024 / 1024) + "MB");
        return sb.ToString();
    }
}
