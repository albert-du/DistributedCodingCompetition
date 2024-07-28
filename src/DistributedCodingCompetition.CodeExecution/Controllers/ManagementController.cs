namespace DistributedCodingCompetition.CodeExecution.Controllers;

/// <summary>
/// Controller for code execution management.
/// </summary>
/// <param name="execRunnerRepository"></param>
/// <param name="execRunnerService"></param>
/// <param name="activeRunnersService"></param>
[ApiController]
[Route("[controller]")]
public class ManagementController(IExecRunnerRepository execRunnerRepository, IExecRunnerService execRunnerService, IActiveRunnersService activeRunnersService) : ControllerBase
{
    /// <summary>
    /// List all the runners.
    /// </summary>
    /// <returns></returns>
    [HttpGet("runners")]
    public async Task<IEnumerable<ExecRunnerResponseDTO>> GetRunnersAsync()
    {
        var runners = await execRunnerRepository.GetExecRunnersAsync();
        var tasks = runners.Select(execRunnerService.RefreshExecRunnerAsync);
        var statuses = await Task.WhenAll(tasks);
        return runners.Select((r, i) => new ExecRunnerResponseDTO(r.Id, r.Name, r.Endpoint, r.Enabled, statuses[i]));
    }

    /// <summary>
    /// Register a new runner.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("runners")]
    public async Task<ActionResult<ExecRunnerResponseDTO>> CreateRunnerAsync([FromBody] ExecRunnerRequestDTO request)
    {
        ExecRunner runner = new()
        {
            Name = request.Name,
            Endpoint = request.Endpoint,
            Enabled = request.Enabled,
            Weight = request.Weight,
            Key = request.Key
        };
        await execRunnerRepository.CreateExecRunnerAsync(runner);

        var status = await execRunnerService.RefreshExecRunnerAsync(runner);

        await activeRunnersService.IndexExecRunnersAsync();

        return new ExecRunnerResponseDTO(runner.Id, runner.Name, runner.Endpoint, runner.Enabled, status);
    }

    /// <summary>
    /// Get a runner by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("runners/{id}")]
    public async Task<ActionResult<ExecRunnerResponseDTO>> GetRunnerAsync(Guid id)
    {
        var runner = await execRunnerRepository.ReadExecRunnerAsync(id);
        if (runner is null)
            return NotFound();

        var status = await execRunnerService.RefreshExecRunnerAsync(runner);

        return new ExecRunnerResponseDTO(runner.Id, runner.Name, runner.Endpoint, runner.Enabled, status);
    }

    /// <summary>
    /// Update a runner.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("runners/{id}")]
    public async Task<ActionResult<ExecRunnerResponseDTO>> UpdateRunnerAsync(Guid id, [FromBody] ExecRunnerRequestDTO request)
    {
        var runner = await execRunnerRepository.ReadExecRunnerAsync(id);
        if (runner is null)
            return NotFound();

        runner.Name = request.Name;
        runner.Endpoint = request.Endpoint;
        runner.Enabled = request.Enabled;
        runner.Weight = request.Weight;
        runner.Key = request.Key;

        await execRunnerRepository.UpdateExecRunnerAsync(runner);

        var status = await execRunnerService.RefreshExecRunnerAsync(runner);

        await activeRunnersService.IndexExecRunnersAsync();

        return new ExecRunnerResponseDTO(runner.Id, runner.Name, runner.Endpoint, runner.Enabled, status);
    }

    /// <summary>
    /// Delete a runner.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Set the packages for a runner.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("runners/{id}/packages")]
    public async Task<ActionResult<IReadOnlyList<string>>> SetPackagesAsync(Guid id, [FromBody] IReadOnlyList<string> request)
    {
        var runner = await execRunnerRepository.ReadExecRunnerAsync(id);
        if (runner is null)
            return NotFound();

        await execRunnerService.SetPackagesAsync(runner, request);

        return NoContent();
    }

    /// <summary>
    /// Read the available packages for a runner.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("runners/{id}/packages/available")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetPackagesAsync(Guid id)
    {
        var runner = await execRunnerRepository.ReadExecRunnerAsync(id);
        if (runner is null)
            return NotFound();

        return Ok(await execRunnerService.FetchAvailablePackagesAsync(runner));
    }

    /// <summary>
    /// Read the installed packages for a runner.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("runners/{id}/packages/installed")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetInstalledPackagesAsync(Guid id)
    {
        var runner = await execRunnerRepository.ReadExecRunnerAsync(id);
        if (runner is null)
            return NotFound();
        var status = await execRunnerService.RefreshExecRunnerAsync(runner);
        return Ok(status?.Packages.Split('\n') ?? []);
    }

    /// <summary>
    /// Read the languages available for a runner.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("runners/{id}/languages")]
    public async Task<ActionResult<IEnumerable<string>>> GetLanguagesAsync(Guid id)
    {
        var runner = await execRunnerRepository.ReadExecRunnerAsync(id);
        if (runner is null)
            return NotFound();

        return Ok(await execRunnerService.FetchAvailablePackagesAsync(runner));
    }
}
