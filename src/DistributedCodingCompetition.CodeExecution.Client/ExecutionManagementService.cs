namespace DistributedCodingCompetition.CodeExecution.Client;

public class ExecutionManagementService(HttpClient httpClient, ILogger<ExecutionManagementService> logger) : IExecutionManagementService
{
    public async Task<ExecRunnerResponseDTO> CreateExecRunnerAsync(ExecRunnerRequestDTO request)
    {

    }

    public async Task<ExecRunnerResponseDTO> ReadExecRunnerAsync(Guid id);

    public async Task UpdateExecRunnerAsync(Guid id, ExecRunnerRequestDTO request);

    public async Task DeleteExecRunnerAsync(Guid id);

    public async Task<IReadOnlyList<ExecRunnerResponseDTO>> ListExecRunnersAsync();

    public async Task SetPackagesAsync(Guid id, IEnumerable<string> packages);

    public async Task<IEnumerable<string>> InstalledPackagesAsync(Guid id);

    public async Task<IEnumerable<string>> AvailablePackagesAsync(Guid id);

    public async Task<IEnumerable<string>> InstalledLanguagesAsync(Guid id);
}
