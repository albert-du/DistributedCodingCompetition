namespace DistributedCodingCompetition.CodeExecution.Client;

using DistributedCodingCompetition.ExecutionShared;

/// <summary>
/// Service for managing exec runners.
/// </summary>
public interface IExecutionManagementService
{
    /// <summary>
    /// Create a new exec runner
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ExecRunnerResponseDTO> CreateExecRunnerAsync(ExecRunnerRequestDTO request);

    /// <summary>
    /// Read an exec runner
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ExecRunnerResponseDTO> ReadExecRunnerAsync(Guid id);

    /// <summary>
    /// Update an exec runner
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    Task UpdateExecRunnerAsync(Guid id, ExecRunnerRequestDTO request);

    /// <summary>
    /// Delete an exec runner
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteExecRunnerAsync(Guid id);

    /// <summary>
    /// List all exec runners
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyList<ExecRunnerResponseDTO>> ListExecRunnersAsync();

    /// <summary>
    /// Set the packages of an exec runner.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="packages"></param>
    /// <returns></returns>
    Task SetPackagesAsync(Guid id, IEnumerable<string> packages);

    /// <summary>
    /// Get the installed packages of an exec runner.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IEnumerable<string>> InstalledPackagesAsync(Guid id);

    /// <summary>
    /// Get the available packages of an exec runner.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IEnumerable<string>> AvailablePackagesAsync(Guid id);

    /// <summary>
    /// Get the installed languages of an exec runner.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<string>> InstalledLanguagesAsync(Guid id);
}
