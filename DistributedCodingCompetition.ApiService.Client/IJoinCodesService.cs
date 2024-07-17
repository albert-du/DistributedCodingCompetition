namespace DistributedCodingCompetition.ApiService.Client;

/// <summary>
/// Service for interacting with join codes.
/// </summary>
public interface IJoinCodesService
{
    /// <summary>
    /// Reads a paginated list of join codes.
    /// </summary>
    /// <param name="page"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    Task<(bool, PaginateResult<JoinCodeResponseDTO>?)> TryReadJoinCodesAsync(int page = 1, int count = 50);

    /// <summary>
    /// Reads a join code by its id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<(bool, JoinCodeResponseDTO?)> TryReadJoinCodeAsync(Guid id);

    /// <summary>
    /// Reads a join code by its code.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    Task<(bool, JoinCodeResponseDTO?)> TryReadJoinCodeByCodeAsync(string code);

    /// <summary>
    /// Updates a join code.
    /// </summary>
    /// <param name="joinCode"></param>
    /// <returns></returns>
    Task<bool> TryUpdateJoinCodeAsync(JoinCodeRequestDTO joinCode);

    /// <summary>
    /// Creates a join code.
    /// </summary>
    /// <param name="joinCode"></param>
    /// <returns></returns>
    Task<(bool, JoinCodeResponseDTO?)> TryCreateJoinCodeAsync(JoinCodeRequestDTO joinCode);

    /// <summary>
    /// Tries to join a contest with a join code.
    /// </summary>
    /// <param name="joinCodeId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<bool> TryJoinContestAsync(Guid joinCodeId, Guid userId);

    /// <summary>
    /// Deletes a join code.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> TryDeleteJoinCodeAsync(Guid id);
}