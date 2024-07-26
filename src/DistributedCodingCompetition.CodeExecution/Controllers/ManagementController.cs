﻿namespace DistributedCodingCompetition.CodeExecution.Controllers;

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

    [HttpGet("runners/{id}")]
    public async Task<ActionResult<ExecRunnerResponseDTO>> GetRunnerAsync(Guid id)
    {
        var runner = await execRunnerRepository.ReadExecRunnerAsync(id);
        if (runner is null)
            return NotFound();

        var status = await execRunnerService.RefreshExecRunnerAsync(runner);

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

    [HttpPost("runners{id}/packages")]
    public async Task<ActionResult<IReadOnlyList<string>>> SetPackagesAsync(Guid id, [FromBody] IReadOnlyList<string> request)
    {
        var runner = await execRunnerRepository.ReadExecRunnerAsync(id);
        if (runner is null)
            return NotFound();

        await execRunnerService.SetPackagesAsync(runner, request);

        return NoContent();
    }

    [HttpGet("runners/{id}/packages/available")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetPackagesAsync(Guid id)
    {
        var runner = await execRunnerRepository.ReadExecRunnerAsync(id);
        if (runner is null)
            return NotFound();

        return Ok(await execRunnerService.FetchAvailablePackagesAsync(runner));
    }

    [HttpGet("runners/{id}/packages/installed")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetInstalledPackagesAsync(Guid id)
    {
        var runner = await execRunnerRepository.ReadExecRunnerAsync(id);
        if (runner is null)
            return NotFound();
        var status = await execRunnerService.RefreshExecRunnerAsync(runner);
        return Ok(status?.Packages.Split('\n') ?? []);
    }


    [HttpGet("runners/{id}/languages")]
    public async Task<ActionResult<IEnumerable<string>>> GetLanguagesAsync(Guid id)
    {
        var runner = await execRunnerRepository.ReadExecRunnerAsync(id);
        if (runner is null)
            return NotFound();

        return Ok(await execRunnerService.FetchAvailablePackagesAsync(runner));
    }
}
