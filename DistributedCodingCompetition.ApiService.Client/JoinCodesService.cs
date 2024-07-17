namespace DistributedCodingCompetition.ApiService.Client;

/// <inheritdoc/>
public sealed class JoinCodesService : IJoinCodesService
{
    private readonly ApiClient<JoinCodesService> _apiClient;

    internal JoinCodesService(HttpClient httpClient, ILogger<JoinCodesService> logger) =>
        _apiClient = new(httpClient, logger, "api/joincodes");

    /// <inheritdoc/>
    public Task<(bool, JoinCodeResponseDTO?)> TryCreateJoinCodeAsync(JoinCodeRequestDTO joinCode) =>
        _apiClient.PostAsync<JoinCodeRequestDTO, JoinCodeResponseDTO>(data: joinCode);

    /// <inheritdoc/>
    public Task<bool> TryDeleteJoinCodeAsync(Guid id) =>
        _apiClient.DeleteAsync($"/{id}");

    /// <inheritdoc/>
    public Task<bool> TryJoinContestAsync(Guid joinCodeId, Guid userId) =>
        _apiClient.PostAsync($"/{joinCodeId}/join/{userId}");

    /// <inheritdoc/>
    public Task<(bool, JoinCodeResponseDTO?)> TryReadJoinCodeAsync(Guid id) =>
        _apiClient.GetAsync<JoinCodeResponseDTO>($"/{id}");

    /// <inheritdoc/>
    public Task<(bool, JoinCodeResponseDTO?)> TryReadJoinCodeByCodeAsync(string code) =>
        _apiClient.GetAsync<JoinCodeResponseDTO>($"/code/{code}");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<JoinCodeResponseDTO>?)> TryReadJoinCodesAsync(int page = 1, int count = 50) =>
        _apiClient.GetAsync<PaginateResult<JoinCodeResponseDTO>>($"?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<bool> TryUpdateJoinCodeAsync(JoinCodeRequestDTO joinCode) =>
        _apiClient.PutAsync($"?id={joinCode.Id}", joinCode);
}