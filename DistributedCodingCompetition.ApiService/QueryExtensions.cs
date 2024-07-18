namespace DistributedCodingCompetition.ApiService;

/// <summary>
/// Extension methods for querying the database.
/// </summary>
internal static class QueryExtensions
{
    /// <summary>
    /// Paginate and transform a query.
    /// </summary>
    /// <typeparam name="T">Source type</typeparam>
    /// <typeparam name="TR">Return type</typeparam>
    /// <param name="query">Source query</param>
    /// <param name="page">page count starting at 1</param>
    /// <param name="count">count</param>
    /// <param name="transform">method to apply after</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    internal static Task<PaginateResult<TR>> PaginateAsync<T, TR>(this IQueryable<T> query, int page, int count, Func<T, TR> transform)
    {
        async Task<IReadOnlyList<TR>> Transform(IQueryable<T> querySection) =>
            await querySection.AsAsyncEnumerable().Select(transform).ToArrayAsync();

        return PaginateAsync(query, page, count, Transform);
    }

    /// <summary>
    /// Paginate and transform a query.
    /// </summary>
    /// <typeparam name="T">source type</typeparam>
    /// <typeparam name="TR">return type</typeparam>
    /// <param name="query">source query</param>
    /// <param name="page">page number starting at 1</param>
    /// <param name="count">number of returned</param>
    /// <param name="transform">actually perform and transform the query</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    internal static async Task<PaginateResult<TR>> PaginateAsync<T, TR>(this IQueryable<T> query, int page, int count, Func<IQueryable<T>, Task<IReadOnlyList<TR>>> transform)
    {
        if (page < 1)
            throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than 0.");

        if (count < 1)
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than 0.");

        var querySection =
            query.Skip((page - 1) * count)
                 .Take(count);

        var length = await query.CountAsync();

        var items = await transform(querySection);

        return new PaginateResult<TR>
        {
            Page = page,
            PageSize = count,
            TotalCount = length,
            TotalPages = (int)Math.Ceiling((double)length / count),
            Items = items
        };
    }

    internal static async Task<IReadOnlyList<ContestResponseDTO>> ReadContestsAsync(this IQueryable<Contest> contests) =>
        await contests
            .Select(contest => new
            {
                contest.Id,
                contest.Name,
                contest.OwnerId,
                OwnerName = contest.Owner.Username,
                contest.Description,
                contest.RenderedDescription,
                contest.StartTime,
                contest.EndTime,
                contest.Active,
                contest.Public,
                contest.Open,
                contest.MinimumAge,
                contest.DefaultPointsForProblem,
                TotalSubmissions = contest.Submissions.Count,
                TotalProblems = contest.Problems.Count,
                TotalParticipants = contest.Participants.Count,
                TotalBanned = contest.Banned.Count,
                TotalJoinCodes = contest.JoinCodes.Count,
                TotalAdmins = contest.Administrators.Count
            })
            .ToAsyncEnumerable()
            .Select(contest => new ContestResponseDTO
            {
                Id = contest.Id,
                Name = contest.Name,
                OwnerId = contest.OwnerId,
                OwnerName = contest.OwnerName,
                Description = contest.Description,
                RenderedDescription = contest.RenderedDescription,
                StartTime = contest.StartTime,
                EndTime = contest.EndTime,
                Active = contest.Active,
                Public = contest.Public,
                Open = contest.Open,
                MinimumAge = contest.MinimumAge,
                DefaultPointsForProblem = contest.DefaultPointsForProblem,
                TotalSubmissions = contest.TotalSubmissions,
                TotalProblems = contest.TotalProblems,
                TotalParticipants = contest.TotalParticipants,
                TotalBanned = contest.TotalBanned,
                TotalJoinCodes = contest.TotalJoinCodes,
                TotalAdmins = contest.TotalAdmins
            })
            .ToArrayAsync();

    internal static async Task<IReadOnlyList<JoinCodeResponseDTO>> ReadJoinCodesAsync(this IQueryable<JoinCode> joinCodes) =>
        await joinCodes
            .Select(jc => new
            {
                jc.Id,
                jc.Name,
                jc.ContestId,
                ContestName = jc.Contest.Name,
                jc.CreatorId,
                CreatorName = jc.Creator.Username,
                jc.Code,
                jc.Admin,
                jc.Active,
                jc.CloseAfterUse,
                jc.Uses,
                CreatedAt = jc.Creation,
                jc.Expiration
            })
            .ToAsyncEnumerable()
            .Select(jc => new JoinCodeResponseDTO
            {
                Id = jc.Id,
                Name = jc.Name,
                ContestId = jc.ContestId,
                ContestName = jc.ContestName,
                CreatorId = jc.CreatorId,
                CreatorName = jc.CreatorName,
                Code = jc.Code,
                Admin = jc.Admin,
                Active = jc.Active,
                CloseAfterUse = jc.CloseAfterUse,
                Uses = jc.Uses,
                CreatedAt = jc.CreatedAt,
                Expiration = jc.Expiration
            })
            .ToArrayAsync();

    internal static async Task<IReadOnlyList<ProblemResponseDTO>> ReadProblemsAsync(this IQueryable<Problem> problems) =>
        await problems
            .Select(problem => new
            {
                problem.Id,
                problem.Name,
                problem.OwnerId,
                OwnerName = problem.Owner.Username,
                problem.TagLine,
                problem.Description,
                problem.RenderedDescription,
                problem.Difficulty,
                TestCaseCount = problem.TestCases.Count
            })
            .ToAsyncEnumerable()
            .Select(problem => new ProblemResponseDTO
            {
                Id = problem.Id,
                Name = problem.Name,
                OwnerId = problem.OwnerId,
                OwnerName = problem.OwnerName,
                TagLine = problem.TagLine,
                Description = problem.Description,
                RenderedDescription = problem.RenderedDescription,
                Difficulty = problem.Difficulty,
                TestCaseCount = problem.TestCaseCount
            })
            .ToArrayAsync();

    internal static async Task<IReadOnlyList<SubmissionResponseDTO>> ReadSubmissionsAsync(this IQueryable<Submission> submissions) =>
        await submissions
            .Select(submission => new
            {
                submission.Id,
                submission.ContestId,
                ContestName = submission.Contest == null ? "" : submission.Contest.Name,
                submission.ProblemId,
                ProblemName = submission.Problem.Name,
                UserId = submission.SubmitterId,
                UserName = submission.Submitter.Username,
                submission.Language,
                submission.Code,
                submission.Points,
                CreatedAt = submission.SubmissionTime,
                submission.EvaluationTime,
                submission.Score,
                submission.MaxPossibleScore,
                submission.Invalidated,
                submission.PassedTestCases,
                submission.TotalTestCases,
            })
            .ToAsyncEnumerable()
            .Select(submission => new SubmissionResponseDTO
            {
                Id = submission.Id,
                ContestId = submission.ContestId,
                ContestName = submission.ContestName,
                ProblemId = submission.ProblemId,
                ProblemName = submission.ProblemName,
                UserId = submission.UserId,
                UserName = submission.UserName,
                Language = submission.Language,
                Code = submission.Code,
                CreatedAt = submission.CreatedAt,
                JudgedAt = submission.EvaluationTime,
                Score = submission.Score,
                MaxPossibleScore = submission.MaxPossibleScore,
                Points = submission.Points,
                Invalidated = submission.Invalidated,
                TestCasesPassed = submission.PassedTestCases,
                TestCasesTotal = submission.TotalTestCases
            })
            .ToArrayAsync();

    internal static async Task<IReadOnlyList<TestCaseResponseDTO>> ReadTestCasesAsync(this IQueryable<TestCase> testCases) =>
        await testCases
            .Select(testCase => new
            {
                testCase.Id,
                testCase.ProblemId,
                ProblemName = testCase.Problem.Name,
                testCase.Input,
                testCase.Output,
                testCase.Description,
                testCase.Sample,
                testCase.Active,
                testCase.Weight
            })
            .ToAsyncEnumerable()
            .Select(testCase => new TestCaseResponseDTO
            {
                Id = testCase.Id,
                ProblemId = testCase.ProblemId,
                ProblemName = testCase.ProblemName,
                Input = testCase.Input,
                Output = testCase.Output,
                Description = testCase.Description,
                Sample = testCase.Sample,
                Active = testCase.Active,
                Weight = testCase.Weight
            })
            .ToArrayAsync();

    internal static async Task<IReadOnlyList<TestCaseResultDTO>> ReadTestCaseResultsAsync(this IQueryable<TestCaseResult> result) =>
        await result
            .Select(r => new
            {
                r.SubmissionId,
                r.TestCaseId,
                r.Passed,
                r.Output,
                r.ExecutionTime,
                r.Error
            })
            .ToAsyncEnumerable()
            .Select(r => new TestCaseResultDTO
            {
                SubmissionId = r.SubmissionId,
                TestCaseId = r.TestCaseId,
                Passed = r.Passed,
                Output = r.Output,
                ExecutionTime = r.ExecutionTime,
                Error = r.Error
            })
            .ToArrayAsync();

    internal static async Task<IReadOnlyList<ProblemPointValueResponseDTO>> ReadProblemPointValuesAsync(this IQueryable<ProblemPointValue> pointValues) =>
        await pointValues
            .Select(pv => new
            {
                pv.Id,
                pv.ContestId,
                pv.ProblemId,
                pv.Points
            })
            .ToAsyncEnumerable()
            .Select(pv => new ProblemPointValueResponseDTO
            {
                Id = pv.Id,
                ContestId = pv.ContestId,
                ProblemId = pv.ProblemId,
                Points = pv.Points
            })
            .ToArrayAsync();

    internal static async Task<IReadOnlyList<UserResponseDTO>> ReadUsersAsync(this IQueryable<User> users) =>
        await users
            .Select(user => new
            {
                user.Id,
                user.Username,
                user.Email,
                user.FullName,
                user.Birthday,
                Administered = user.AdministeredContests.Count,
                Owned = user.OwnedContests.Count,
                Entered = user.EnteredContests.Count,
                Banned = user.BanId != null,
                BannedContests = user.BannedContests.Count,
                CreatedAt = user.Creation,
            })
            .ToAsyncEnumerable()
            .Select(user => new UserResponseDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                ParticipatedContests = user.Entered,
                OwnedContests = user.Owned,
                AdministeredContests = user.Administered,
                FullName = user.FullName,
                CreatedAt = user.CreatedAt,
                Banned = user.Banned,
                BannedContests = user.BannedContests,
                Birthday = user.Birthday
            })
            .ToArrayAsync();
}