namespace DistributedCodingCompetition.CodeExecution.Client;

/// <inheritdoc/>
public class ExecutionManagementService(HttpClient httpClient, ILogger<ExecutionManagementService> logger) : IExecutionManagementService
{
    /// <inheritdoc/>
    public async Task<ExecRunnerResponseDTO> CreateExecRunnerAsync(ExecRunnerRequestDTO request)
    {
        logger.LogInformation("Creating ExecRunner {@ExecRunner}", request);
        var response = await httpClient.PostAsJsonAsync("management/runners", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ExecRunnerResponseDTO>() ?? throw new Exception("Failed to parse response");
    }

    /// <inheritdoc/>
    public async Task<ExecRunnerResponseDTO> ReadExecRunnerAsync(Guid id)
    {
        logger.LogInformation("Reading ExecRunner {@Id}", id);
        var response = await httpClient.GetAsync($"management/runners/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ExecRunnerResponseDTO>() ?? throw new Exception("Failed to parse response");
    }

    /// <inheritdoc/>
    public async Task UpdateExecRunnerAsync(Guid id, ExecRunnerRequestDTO request)
    {
        logger.LogInformation("Updating ExecRunner {@Id} with {@ExecRunner}", id, request);
        var response = await httpClient.PutAsJsonAsync($"management/runners/{id}", request);
        response.EnsureSuccessStatusCode();
    }

    /// <inheritdoc/>
    public async Task DeleteExecRunnerAsync(Guid id)
    {
        logger.LogInformation("Deleting ExecRunner {@Id}", id);
        var response = await httpClient.DeleteAsync($"management/runners/{id}");
        response.EnsureSuccessStatusCode();
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<ExecRunnerResponseDTO>> ListExecRunnersAsync()
    {
        logger.LogInformation("Listing ExecRunners");
        var response = await httpClient.GetAsync("management/runners");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IReadOnlyList<ExecRunnerResponseDTO>>() ?? throw new Exception("Failed to parse response");
    }

    /// <inheritdoc/>
    public async Task SetPackagesAsync(Guid id, IEnumerable<string> packages)
    {
        logger.LogInformation("Setting packages for ExecRunner {@Id} with {@Packages}", id, packages);
        var response = await httpClient.PostAsJsonAsync($"management/runners/{id}/packages", packages);
        response.EnsureSuccessStatusCode();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> InstalledPackagesAsync(Guid id)
    {
        logger.LogInformation("Getting installed packages for ExecRunner {@Id}", id);
        var response = await httpClient.GetAsync($"management/runners/{id}/packages/installed");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<string>>() ?? throw new Exception("Failed to parse response");
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> AvailablePackagesAsync(Guid id)
    {
        logger.LogInformation("Getting available packages for ExecRunner {@Id}", id);
        var response = await httpClient.GetAsync($"management/runners/{id}/packages/available");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<string>>() ?? throw new Exception("Failed to parse response");
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> InstalledLanguagesAsync(Guid id)
    {
        logger.LogInformation("Getting installed languages for ExecRunner {@Id}", id);
        var response = await httpClient.GetAsync($"management/runners/{id}/languages");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<string>>() ?? throw new Exception("Failed to parse response");
    }
}
