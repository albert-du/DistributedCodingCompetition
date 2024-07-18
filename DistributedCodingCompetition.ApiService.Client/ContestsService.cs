namespace DistributedCodingCompetition.ApiService.Client;

/// <inheritdoc/>
public sealed class ContestsService(HttpClient httpClient, ILogger<ContestsService> logger) : IContestsService
{
    private readonly ApiClient<ContestsService> apiClient = new(httpClient, logger, "api/contests");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<ContestResponseDTO>?)> TryReadContestsAsync(int page, int count) =>
        apiClient.GetAsync<PaginateResult<ContestResponseDTO>>($"?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<(bool, ContestResponseDTO?)> TryReadContestAsync(Guid id) =>
        apiClient.GetAsync<ContestResponseDTO>($"/{id}");

    /// <inheritdoc/>
    public Task<(bool, ContestResponseDTO?)> TryReadContestByJoinCodeAsync(string code) =>
        apiClient.GetAsync<ContestResponseDTO>($"/joincode/{code}");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadContestAdminsAsync(Guid contestId, int page, int count) =>
        apiClient.GetAsync<PaginateResult<UserResponseDTO>>($"/{contestId}/admins?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadContestBannedAsync(Guid contestId, int page, int count) =>
        apiClient.GetAsync<PaginateResult<UserResponseDTO>>($"/{contestId}/banned?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadContestParticipantsAsync(Guid contestId, int page, int count) =>
        apiClient.GetAsync<PaginateResult<UserResponseDTO>>($"/{contestId}/participants?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<(bool, IReadOnlyList<JoinCodeResponseDTO>?)> TryReadContestJoinCodesAsync(Guid contestId) =>
        apiClient.GetAsync<IReadOnlyList<JoinCodeResponseDTO>>($"/{contestId}/joincodes");

    /// <inheritdoc/>
    public Task<(bool, ContestRole?)> TryReadContestUserRoleAsync(Guid contestId, Guid userId) =>
        apiClient.GetAsync<ContestRole?>($"/{contestId}/role/{userId}");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<ContestResponseDTO>?)> TryReadPublicContestsAsync(int page, int count) =>
        apiClient.GetAsync<PaginateResult<ContestResponseDTO>>($"/public?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<(bool, IReadOnlyList<ProblemResponseDTO>?)> TryReadContestProblemsAsync(Guid contestId) =>
        apiClient.GetAsync<IReadOnlyList<ProblemResponseDTO>>($"/{contestId}/problems");

    /// <inheritdoc/>
    public Task<(bool, IReadOnlyList<ProblemUserSolveStatus>?)> TryReadContestUserSolveStatusAsync(Guid contestId, Guid userId) =>
        apiClient.GetAsync<IReadOnlyList<ProblemUserSolveStatus>>($"/{contestId}/user/{userId}/solve");

    /// <inheritdoc/>
    public Task<(bool, ProblemUserSolveStatus?)> TryReadContestProblemUserSolveStatusAsync(Guid contestId, Guid problemId, Guid userId) =>
        apiClient.GetAsync<ProblemUserSolveStatus?>($"/{contestId}/problem/{userId}/solve/{problemId}");

    /// <inheritdoc/>
    public Task<(bool, IReadOnlyList<ProblemPointValueResponseDTO>?)> TryReadContestProblemPointValuesAsync(Guid contestId) =>
        apiClient.GetAsync<IReadOnlyList<ProblemPointValueResponseDTO>>($"/{contestId}/pointvalues");

    /// <inheritdoc/>
    public Task<(bool, ProblemPointValueResponseDTO?)> TryReadContestProblemPointValueAsync(Guid contestId, Guid problemId, bool autoGenerate) =>
        apiClient.GetAsync<ProblemPointValueResponseDTO?>($"/{contestId}/pointvalues/{problemId}");

    /// <inheritdoc/>
    public Task<(bool, ProblemPointValueResponseDTO?)> TryUpdateContestProblemPointValueAsync(ProblemPointValueRequestDTO data) =>
        apiClient.PutAsync<ProblemPointValueRequestDTO, ProblemPointValueResponseDTO>($"/{data.ContestId}/pointvalues/{data.ProblemId}", data);

    /// <inheritdoc/>
    public Task<(bool, ProblemPointValueResponseDTO?)> TryCreateContestProblemPointValueAsync(ProblemPointValueRequestDTO data) =>
        apiClient.PostAsync<ProblemPointValueRequestDTO, ProblemPointValueResponseDTO>($"/{data.ContestId}/pointvalues/{data.ProblemId}", data);

    /// <inheritdoc/>
    public Task<(bool, Leaderboard?)> TryReadContestLeaderboardAsync(Guid contestId) =>
        apiClient.GetAsync<Leaderboard>($"/{contestId}/leaderboard");

    /// <inheritdoc/>
    public Task<bool> TryUpdateUserContestRoleAsync(Guid contestId, Guid userId, ContestRole role) =>
        apiClient.PutAsync($"/{contestId}/role/{userId}", role);

    /// <inheritdoc/>
    public Task<bool> TryUpdateContestAsync(ContestRequestDTO contest) =>
         apiClient.PutAsync($"/{contest.Id}", contest);

    /// <inheritdoc/>
    public Task<(bool, ContestResponseDTO?)> TryCreateContestAsync(ContestRequestDTO contest) =>
         apiClient.PostAsync<ContestRequestDTO, ContestResponseDTO>(data: contest);

    /// <inheritdoc/>
    public Task<bool> TryAddProblemToContest(Guid contestId, Guid problemId) =>
         apiClient.PostAsync($"/{contestId}/problems/{problemId}");
}