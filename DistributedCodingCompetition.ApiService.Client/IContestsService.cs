namespace DistributedCodingCompetition.ApiService.Client;

public interface IContestsService
{
    Task<(bool, PaginateResult<ContestResponseDTO>?)> TryReadContestsAsync(int page, int count);

    Task<(bool, ContestResponseDTO?)> TryReadContestAsync(Guid id);

    Task<(bool, ContestResponseDTO?)> TryReadContestByJoinCodeAsync(string code);

    Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadContestAdmins(Guid contestId, int page, int count);

    Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadContestBanned(Guid contestId, int page, int count);

    Task<(bool, PaginateResult<UserResponseDTO>?)> TryReadContestParticipants(Guid contestId, int page, int count);

    Task<(bool, IReadOnlyList<JoinCodeResponseDTO>?)> TryReadContestJoinCodesAsync(Guid contestId);

    Task<(bool, ContestRole?)> TryReadContestUserRoleAsync(Guid contestId, Guid userId);

    Task<(bool, PaginateResult<ContestResponseDTO>?)> TryReadPublicContestsAsync(int page, int count);

    Task<(bool, IReadOnlyList<ProblemResponseDTO>?)> TryReadContestProblemsAsync(Guid contestId);

    Task<(bool, IReadOnlyList<ProblemUserSolveStatus>?)> TryReadContestUserSolveStatusAsync(Guid contestId, Guid userId);

    Task<(bool, ProblemUserSolveStatus?)> TryReadContestProblemUserSolveStatusAsync(Guid contestId, Guid problemId, Guid userId);

    Task<(bool, IReadOnlyList<ProblemPointValueResponseDTO>?)> TryReadContestProblemPointValuesAsync(Guid contestId);

    Task<(bool, ProblemPointValueResponseDTO?)> TryReadContestProblemPointValueAsync(Guid contestId, Guid problemId);

    Task<(bool, ProblemPointValueResponseDTO?)> TryUpdateContestProblemPointValueAsync(ProblemPointValueRequestDTO data);
}