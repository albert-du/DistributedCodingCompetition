
namespace DistributedCodingCompetition.ApiService.Client;

/// <inheritdoc/>
public sealed class UsersService : IUsersService
{
    private readonly ApiClient<UsersService> apiClient;

    internal UsersService(HttpClient httpClient, ILogger<UsersService> logger) =>
        apiClient = new(httpClient, logger, "api/users");

    /// <inheritdoc/>
    public Task<(bool, UserResponseDTO?)> TryCreateUserAsync(UserRequestDTO user) =>
        apiClient.PostAsync<UserRequestDTO, UserResponseDTO>(data: user);

    /// <inheritdoc/>
    public Task<bool> TryDeleteUserAsync(Guid id) =>
        apiClient.DeleteAsync($"/{id}");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<ContestResponseDTO>?)> TryReadAdministeredContestsAsync(Guid userId, int page = 1, int count = 50) =>
        apiClient.GetAsync<PaginateResult<ContestResponseDTO>>($"/{userId}/administered?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<ContestResponseDTO>?)> TryReadBannedContestsAsync(Guid userId, int page = 1, int count = 50) =>
        apiClient.GetAsync<PaginateResult<ContestResponseDTO>>($"/{userId}/banned?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadBannedUsersAsync(int page = 1, int count = 50) =>
        apiClient.GetAsync<PaginateResult<UserResponseDTO>>($"/banned?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<ContestResponseDTO>?)> TryReadEnteredContestsAsync(Guid userId, int page = 1, int count = 50) =>
        apiClient.GetAsync<PaginateResult<ContestResponseDTO>>($"/{userId}/entered?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<ContestResponseDTO>?)> TryReadOwnedContestsAsync(Guid userId, int page = 1, int count = 50) =>
        apiClient.GetAsync<PaginateResult<ContestResponseDTO>>($"/{userId}/owned?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<(bool, UserResponseDTO?)> TryReadUserAsync(Guid id) =>
        apiClient.GetAsync<UserResponseDTO>($"/{id}");

    /// <inheritdoc/>
    public Task<(bool, UserResponseDTO?)> TryReadUserByEmailAsync(string email) =>
        apiClient.GetAsync<UserResponseDTO>($"/email/{email}");

    /// <inheritdoc/>
    public Task<(bool, UserResponseDTO?)> TryReadUserByUsernameAsync(string username) =>
        apiClient.GetAsync<UserResponseDTO>($"/username/{username}");

    /// <inheritdoc/>
    public Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadUsersAsync(int page = 1, int count = 50) =>
        apiClient.GetAsync<PaginateResult<UserResponseDTO>>($"?page={page}&count={count}");

    /// <inheritdoc/>
    public Task<bool> TryUpdateUserAsync(UserRequestDTO user) =>
        apiClient.PutAsync($"/{user.Id}", user);
}