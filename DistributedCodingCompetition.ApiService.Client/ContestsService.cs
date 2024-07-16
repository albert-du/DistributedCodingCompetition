namespace DistributedCodingCompetition.ApiService.Client;

public class ContestsService : IContestsService
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
        _apiClient.Get<PaginateResult<ContestResponseDTO>>($"?page={page}&count={count}");

    public Task<(bool, ContestResponseDTO?)> TryReadContestAsync(Guid id) =>
        _apiClient.Get<ContestResponseDTO>($"/{id}");

    public Task<(bool, ContestResponseDTO?)> TryReadContestByJoinCodeAsync(string code) =>
        _apiClient.Get<ContestResponseDTO>($"/joincode/{code}");

    public Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadContestAdmins(Guid contestId, int page, int count) =>
        _apiClient.Get<PaginateResult<UserResponseDTO>>($"/{contestId}/admins?page={page}&count={count}");

    public Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadContestBanned(Guid contestId, int page, int count) =>
        _apiClient.Get<PaginateResult<UserResponseDTO>>($"/{contestId}/banned?page={page}&count={count}");


    public Task<(bool, IReadOnlyList<JoinCodeResponseDTO>?)> TryReadContestJoinCodesAsync(Guid contestId) =>
        _apiClient.Get<IReadOnlyList<JoinCodeResponseDTO>>($"/{contestId}/joincodes");

    public Task<(bool, ContestRole?)> TryReadContestUserRoleAsync(Guid contestId, Guid userId) =>
        _apiClient.Get<ContestRole?>($"/{contestId}/role/{userId}");

}