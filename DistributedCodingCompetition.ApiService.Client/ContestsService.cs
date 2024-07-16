namespace DistributedCodingCompetition.ApiService.Client;

public sealed class ContestsService : IContestsService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ContestsService> _logger;
    private readonly ApiClient<ContestsService> _apiClient;
    internal ContestsService(HttpClient httpClient, ILogger<ContestsService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _apiClient = new ApiClient<ContestsService>(_httpClient, _logger, "api/contests");
    }

    public Task<(bool, PaginateResult<ContestResponseDTO>?)> TryReadContestsAsync(int page, int count) =>
        _apiClient.GetAsync<PaginateResult<ContestResponseDTO>>($"?page={page}&count={count}");

    public Task<(bool, ContestResponseDTO?)> TryReadContestAsync(Guid id) =>
        _apiClient.GetAsync<ContestResponseDTO>($"/{id}");

    public Task<(bool, ContestResponseDTO?)> TryReadContestByJoinCodeAsync(string code) =>
        _apiClient.GetAsync<ContestResponseDTO>($"/joincode/{code}");

    public Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadContestAdmins(Guid contestId, int page, int count) =>
        _apiClient.GetAsync<PaginateResult<UserResponseDTO>>($"/{contestId}/admins?page={page}&count={count}");

    public Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadContestBanned(Guid contestId, int page, int count) =>
        _apiClient.GetAsync<PaginateResult<UserResponseDTO>>($"/{contestId}/banned?page={page}&count={count}");

    public Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadContestParticipants(Guid contestId, int page, int count) =>
        _apiClient.GetAsync<PaginateResult<UserResponseDTO>>($"/{contestId}/participants?page={page}&count={count}");

    public Task<(bool, IReadOnlyList<JoinCodeResponseDTO>?)> TryReadContestJoinCodesAsync(Guid contestId) =>
        _apiClient.GetAsync<IReadOnlyList<JoinCodeResponseDTO>>($"/{contestId}/joincodes");

    public Task<(bool, ContestRole?)> TryReadContestUserRoleAsync(Guid contestId, Guid userId) =>
        _apiClient.GetAsync<ContestRole?>($"/{contestId}/role/{userId}");

    public Task<(bool, PaginateResult<ContestResponseDTO>?)> TryReadPublicContestsAsync(int page, int count) =>
        _apiClient.GetAsync<PaginateResult<ContestResponseDTO>>($"/public?page={page}&count={count}");

    public Task<(bool, IReadOnlyList<ProblemResponseDTO>?)> TryReadContestProblemsAsync(Guid contestId) =>
        _apiClient.GetAsync<IReadOnlyList<ProblemResponseDTO>>($"/{contestId}/problems");

    public Task<(bool, IReadOnlyList<ProblemUserSolveStatus>?)> TryReadContestUserSolveStatusAsync(Guid contestId, Guid userId) =>
        _apiClient.GetAsync<IReadOnlyList<ProblemUserSolveStatus>>($"/{contestId}/user/{userId}/solve");

    public Task<(bool, ProblemUserSolveStatus?)> TryReadContestProblemUserSolveStatusAsync(Guid contestId, Guid problemId, Guid userId) =>
        _apiClient.GetAsync<ProblemUserSolveStatus?>($"/{contestId}/problem/{userId}/solve/{problemId}");

    public Task<(bool, IReadOnlyList<ProblemPointValueResponseDTO>?)> TryReadContestProblemPointValuesAsync(Guid contestId) =>
        _apiClient.GetAsync<IReadOnlyList<ProblemPointValueResponseDTO>>($"/{contestId}/pointvalues");

    public Task<(bool, ProblemPointValueResponseDTO?)> TryReadContestProblemPointValueAsync(Guid contestId, Guid problemId) =>
        _apiClient.GetAsync<ProblemPointValueResponseDTO?>($"/{contestId}/pointvalues/{problemId}");

    public Task<(bool, ProblemPointValueResponseDTO?)> TryUpdateContestProblemPointValueAsync(ProblemPointValueRequestDTO data) =>
        _apiClient.PutAsync<ProblemPointValueRequestDTO, ProblemPointValueResponseDTO>($"/{data.ContestId}/pointvalues/{data.ProblemId}", data);

    public Task<(bool, ProblemPointValueResponseDTO?)> TryCreateContestProblemPointValueAsync(ProblemPointValueRequestDTO data) =>
        _apiClient.PostAsync<ProblemPointValueRequestDTO, ProblemPointValueResponseDTO>($"/{data.ContestId}/pointvalues/{data.ProblemId}", data);

    public Task<(bool, Leaderboard?)> TryReadContestLeaderboardAsync(Guid contestId) =>
        _apiClient.GetAsync<Leaderboard>($"/{contestId}/leaderboard");
    
    public Task<bool> TryUpdateUserContestRoleAsync(Guid contestId, Guid userId, ContestRole role) =>
        _apiClient.PutAsync($"/{contestId}/role/{userId}", role);

}