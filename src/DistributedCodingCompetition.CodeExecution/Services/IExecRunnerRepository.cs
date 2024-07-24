namespace DistributedCodingCompetition.CodeExecution.Services;

using DistributedCodingCompetition.CodeExecution.Models;

/// <summary>
/// Manages the exec runners.
/// </summary>
public interface IExecRunnerRepository
{
    /// <summary>
    /// Get all exec runners.
    /// </summary>
    /// <param name="enabled">whether to include just the enabled execution runners</param>
    /// <returns></returns>
    Task<IReadOnlyList<ExecRunner>> GetExecRunnersAsync(bool enabled = false);

    /// <summary>
    /// Read an exec runner by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ExecRunner?> ReadExecRunnerAsync(Guid id);

    /// <summary>
    /// Create an exec runner.
    /// </summary>
    /// <param name="execRunner"></param>
    /// <returns></returns>
    Task CreateExecRunnerAsync(ExecRunner execRunner);

    /// <summary>
    /// Update an exec runner.
    /// </summary>
    /// <param name="execRunner"></param>
    /// <returns></returns>
    Task UpdateExecRunnerAsync(ExecRunner execRunner);

    /// <summary>
    /// Delete an exec runner.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteExecRunnerAsync(Guid id);
}