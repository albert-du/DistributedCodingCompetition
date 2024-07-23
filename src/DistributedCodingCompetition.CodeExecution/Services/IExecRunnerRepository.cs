namespace DistributedCodingCompetition.CodeExecution.Services;

using DistributedCodingCompetition.CodeExecution.Models;

public interface IExecRunnerRepository
{
    Task<IReadOnlyList<ExecRunner>> GetExecRunnersAsync(bool enabled = false);

    Task<ExecRunner?> ReadExecRunnerAsync(Guid id);

    Task CreateExecRunnerAsync(ExecRunner execRunner);

    Task UpdateExecRunnerAsync(ExecRunner execRunner);

    Task DeleteExecRunnerAsync(Guid id);
}