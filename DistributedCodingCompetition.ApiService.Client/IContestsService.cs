namespace DistributedCodingCompetition.ApiService.Client;

public interface IContestsService
{
    Task<(bool, PaginateResult<ContestResponseDTO>?)> TryReadContestsAsync(int page, int count);

    Task<(bool, ContestResponseDTO?)> TryReadContestAsync(Guid id);

    Task<(bool, ContestResponseDTO?)> TryReadContestByJoinCodeAsync(string code);

    Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadContestAdmins(Guid contestId, int page, int count);
    Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadContestBanned(Guid contestId, int page, int count);

    Task<(bool, IReadOnlyList<JoinCodeResponseDTO>?)> TryReadContestJoinCodesAsync(Guid contestId);

    Task<(bool, ContestRole?)> TryReadContestUserRoleAsync(Guid contestId, Guid userId);

}