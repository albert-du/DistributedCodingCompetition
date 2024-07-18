namespace DistributedCodingCompetition.ApiService.Client;

/// <inheritdoc/>
public sealed class JoinCodesService(HttpClient httpClient, ILogger<JoinCodesService> logger) : IJoinCodesService
{
    private readonly ApiClient<JoinCodesService> apiClient = new(httpClient, logger, "api/joincodes");

    /// <inheritdoc/>
    public Task<(bool, JoinCodeResponseDTO?)> TryCreateJoinCodeAsync(JoinCodeRequestDTO joinCode) =>
        apiClient.PostAsync<JoinCodeRequestDTO, JoinCodeResponseDTO>(data: joinCode);

    /// <inheritdoc/>
    public Task<bool> TryDeleteJoinCodeAsync(Guid id) =>
        apiClient.DeleteAsync($"/{id}");

    /// <inheritdoc/>
    public Task<bool> TryJoinContestAsync(Guid joinCodeId, Guid userId) =>
        apiClient.PostAsync($"/{joinCodeId}/join/{userId}");

    /// <inheritdoc/>
    public Task<(bool, JoinCodeResponseDTO?)> TryReadJoinCodeAsync(Guid id) =>
        apiClient.GetAsync<JoinCodeResponseDTO>($"/{id}");

    /// <inheritdoc/>
    public Task<(bool, JoinCodeResponseDTO?)> TryReadJoinCodeByCodeAsync(string code) =>
        apiClient.GetAsync<JoinCodeResponseDTO>($"/code/{code}");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<JoinCodeResponseDTO>?)> TryReadJoinCodesAsync(int page = 1, int count = 50) =>
        apiClient.GetAsync<PaginateResult<JoinCodeResponseDTO>>($"?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<bool> TryUpdateJoinCodeAsync(JoinCodeRequestDTO joinCode) =>
        apiClient.PutAsync($"?id={joinCode.Id}", joinCode);
}