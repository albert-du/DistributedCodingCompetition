namespace DistributedCodingCompetition.CodeExecution.Controllers;

using DistributedCodingCompetition.CodeExecution.Services;
using DistributedCodingCompetition.CodeExecution.Models;
using DistributedCodingCompetition.ExecutionShared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

[ApiController]
[Route("[controller]")]
public class ManagementController(IExecRunnerRepository execRunnerRepository, IExecRunnerService execRunnerService, IActiveRunnersService activeRunnersService) : ControllerBase
{
    [HttpGet("runners")]
    public async Task<IEnumerable<ExecRunnerResponseDTO>> GetRunnersAsync()
    {
        var runners = await execRunnerRepository.GetExecRunnersAsync();
        var tasks = runners.Select(execRunnerService.RefreshExecRunnerAsync);
        var statuses = await Task.WhenAll(tasks);
        return runners.Select((r, i) => new ExecRunnerResponseDTO(r.Id, r.Name, r.Endpoint, r.Enabled, statuses[i]));
    }

    [HttpPost("runners")]
    public async Task<ActionResult<ExecRunnerResponseDTO>> CreateRunnerAsync([FromBody] ExecRunnerRequestDTO request)
    {
        ExecRunner runner = new()
        {
            Name = request.Name,
            Endpoint = request.Endpoint,
            Enabled = request.Enabled
        };
        await execRunnerRepository.CreateExecRunnerAsync(runner);

        var status = await execRunnerService.RefreshExecRunnerAsync(runner);

        _ = activeRunnersService.IndexExecRunnersAsync();

        return new ExecRunnerResponseDTO(runner.Id, runner.Name, runner.Endpoint, runner.Enabled, status);
    }

    [HttpPut("runners/{id}")]
    public async Task<ActionResult<ExecRunnerResponseDTO>> UpdateRunnerAsync(Guid id, [FromBody] ExecRunnerRequestDTO request)
    {
        var runner = await execRunnerRepository.ReadExecRunnerAsync(id);
        if (runner is null)
            return NotFound();

        runner.Name = request.Name;
        runner.Endpoint = request.Endpoint;
        runner.Enabled = request.Enabled;

        await execRunnerRepository.UpdateExecRunnerAsync(runner);

        var status = await execRunnerService.RefreshExecRunnerAsync(runner);

        _ = activeRunnersService.IndexExecRunnersAsync();

        return new ExecRunnerResponseDTO(runner.Id, runner.Name, runner.Endpoint, runner.Enabled, status);
    }

    [HttpDelete("runners/{id}")]
    public async Task<ActionResult> DeleteRunnerAsync(Guid id)
    {
        var runner = await execRunnerRepository.ReadExecRunnerAsync(id);
        if (runner is null)
            return NotFound();

        await execRunnerRepository.DeleteExecRunnerAsync(id);

        _ = activeRunnersService.IndexExecRunnersAsync();

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<IReadOnlyList<string>>> SetPackagesAsync([FromBody] IReadOnlyList<string> request)
    {
        var runner = await execRunnerRepository.ReadExecRunnerAsync(request.RunnerId);
        if (runner is null)
            return NotFound();

        await execRunnerService.SetPackagesAsync(runner, request.Packages);

        return await execRunnerService.FetchAvailablePackagesAsync(runner);
    }
}
