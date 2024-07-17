namespace DistributedCodingCompetition.ApiService.Client;

/// <summary>
/// Service for interacting with users.
/// </summary>
public interface IUsersService
{
    /// <summary>
    /// Reads a paginated list of users.
    /// </summary>
    /// <param name="page"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadUsersAsync(int page = 1, int count = 50);

    /// <summary>
    /// Reads a user by its id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<(bool, UserResponseDTO?)> TryReadUserAsync(Guid id);

    /// <summary>
    /// Reads a user by its email.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<(bool, UserResponseDTO?)> TryReadUserByEmailAsync(string email);

    /// <summary>
    /// Reads a user by its username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<(bool, UserResponseDTO?)> TryReadUserByUsernameAsync(string username);

    /// <summary>
    /// Reads a paginated list of contests a user administers.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="page"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    Task<(bool, PaginateResult<ContestResponseDTO>?)> TryReadAdministeredContestsAsync(Guid userId, int page = 1, int count = 50);

    /// <summary>
    /// Reads a paginated list of contests a user owns.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="page"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    Task<(bool, PaginateResult<ContestResponseDTO>?)> TryReadOwnedContestsAsync(Guid userId, int page = 1, int count = 50);

    /// <summary>
    /// Reads a paginated list of contests a user has been banned from.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="page"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    Task<(bool, PaginateResult<ContestResponseDTO>?)> TryReadBannedContestsAsync(Guid userId, int page = 1, int count = 50);

    /// <summary>
    /// Reads a paginated list of contests a user has entered.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="page"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    Task<(bool, PaginateResult<ContestResponseDTO>?)> TryReadEnteredContestsAsync(Guid userId, int page = 1, int count = 50);

    /// <summary>
    /// Reads a paginated list of contests a user has been banned from.
    /// </summary>
    /// <param name="page"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadBannedUsersAsync(int page = 1, int count = 50);

    /// <summary>
    /// Updates a user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<bool> TryUpdateUserAsync(UserRequestDTO user);

    /// <summary>
    /// Create a user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<(bool, UserResponseDTO?)> TryCreateUserAsync(UserRequestDTO user);

    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> TryDeleteUserAsync(Guid id);
}