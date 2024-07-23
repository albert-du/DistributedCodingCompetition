namespace DistributedCodingCompetition.CodeExecution.Services;

using DistributedCodingCompetition.CodeExecution.Models;

/// <summary>
/// Manages an active cache of exec runners.
/// </summary>
public interface IActiveRunnersService
{
    /// <summary>
    /// Find an exec runner by language.
    /// </summary>
    /// <param name="language"></param>
    /// <returns></returns>
    Task<ExecRunner?> FindExecRunnerAsync(string language);
}