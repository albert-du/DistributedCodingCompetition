namespace DistributedCodingCompetition.ApiService.Client;

/// <inheritdoc/>
public sealed class ContestsService : IContestsService
{
    private readonly ApiClient<ContestsService> _apiClient;

    internal ContestsService(HttpClient httpClient, ILogger<ContestsService> logger) =>
        _apiClient = new ApiClient<ContestsService>(httpClient, logger, "api/contests");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<ContestResponseDTO>?)> TryReadContestsAsync(int page, int count) =>
        _apiClient.GetAsync<PaginateResult<ContestResponseDTO>>($"?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<(bool, ContestResponseDTO?)> TryReadContestAsync(Guid id) =>
        _apiClient.GetAsync<ContestResponseDTO>($"/{id}");

    /// <inheritdoc/>
    public Task<(bool, ContestResponseDTO?)> TryReadContestByJoinCodeAsync(string code) =>
        _apiClient.GetAsync<ContestResponseDTO>($"/joincode/{code}");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadContestAdmins(Guid contestId, int page, int count) =>
        _apiClient.GetAsync<PaginateResult<UserResponseDTO>>($"/{contestId}/admins?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadContestBanned(Guid contestId, int page, int count) =>
        _apiClient.GetAsync<PaginateResult<UserResponseDTO>>($"/{contestId}/banned?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadContestParticipants(Guid contestId, int page, int count) =>
        _apiClient.GetAsync<PaginateResult<UserResponseDTO>>($"/{contestId}/participants?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<(bool, IReadOnlyList<JoinCodeResponseDTO>?)> TryReadContestJoinCodesAsync(Guid contestId) =>
        _apiClient.GetAsync<IReadOnlyList<JoinCodeResponseDTO>>($"/{contestId}/joincodes");

    /// <inheritdoc/>
    public Task<(bool, ContestRole?)> TryReadContestUserRoleAsync(Guid contestId, Guid userId) =>
        _apiClient.GetAsync<ContestRole?>($"/{contestId}/role/{userId}");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<ContestResponseDTO>?)> TryReadPublicContestsAsync(int page, int count) =>
        _apiClient.GetAsync<PaginateResult<ContestResponseDTO>>($"/public?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<(bool, IReadOnlyList<ProblemResponseDTO>?)> TryReadContestProblemsAsync(Guid contestId) =>
        _apiClient.GetAsync<IReadOnlyList<ProblemResponseDTO>>($"/{contestId}/problems");

    /// <inheritdoc/>
    public Task<(bool, IReadOnlyList<ProblemUserSolveStatus>?)> TryReadContestUserSolveStatusAsync(Guid contestId, Guid userId) =>
        _apiClient.GetAsync<IReadOnlyList<ProblemUserSolveStatus>>($"/{contestId}/user/{userId}/solve");

    /// <inheritdoc/>
    public Task<(bool, ProblemUserSolveStatus?)> TryReadContestProblemUserSolveStatusAsync(Guid contestId, Guid problemId, Guid userId) =>
        _apiClient.GetAsync<ProblemUserSolveStatus?>($"/{contestId}/problem/{userId}/solve/{problemId}");

    /// <inheritdoc/>
    public Task<(bool, IReadOnlyList<ProblemPointValueResponseDTO>?)> TryReadContestProblemPointValuesAsync(Guid contestId) =>
        _apiClient.GetAsync<IReadOnlyList<ProblemPointValueResponseDTO>>($"/{contestId}/pointvalues");

    /// <inheritdoc/>
    public Task<(bool, ProblemPointValueResponseDTO?)> TryReadContestProblemPointValueAsync(Guid contestId, Guid problemId) =>
        _apiClient.GetAsync<ProblemPointValueResponseDTO?>($"/{contestId}/pointvalues/{problemId}");

    /// <inheritdoc/>
    public Task<(bool, ProblemPointValueResponseDTO?)> TryUpdateContestProblemPointValueAsync(ProblemPointValueRequestDTO data) =>
        _apiClient.PutAsync<ProblemPointValueRequestDTO, ProblemPointValueResponseDTO>($"/{data.ContestId}/pointvalues/{data.ProblemId}", data);

    /// <inheritdoc/>
    public Task<(bool, ProblemPointValueResponseDTO?)> TryCreateContestProblemPointValueAsync(ProblemPointValueRequestDTO data) =>
        _apiClient.PostAsync<ProblemPointValueRequestDTO, ProblemPointValueResponseDTO>($"/{data.ContestId}/pointvalues/{data.ProblemId}", data);

    /// <inheritdoc/>
    public Task<(bool, Leaderboard?)> TryReadContestLeaderboardAsync(Guid contestId) =>
        _apiClient.GetAsync<Leaderboard>($"/{contestId}/leaderboard");

    /// <inheritdoc/>
    public Task<bool> TryUpdateUserContestRoleAsync(Guid contestId, Guid userId, ContestRole role) =>
        _apiClient.PutAsync($"/{contestId}/role/{userId}", role);

    /// <inheritdoc/>
    public async Task<bool> TryUpdateContestAsync(ContestRequestDTO contest) =>
        await _apiClient.PutAsync($"/{contest.Id}", contest);

    /// <inheritdoc/>
    public async Task<(bool, ContestResponseDTO?)> TryCreateContestAsync(ContestRequestDTO contest) =>
        await _apiClient.PostAsync<ContestRequestDTO, ContestResponseDTO>(data: contest);
}