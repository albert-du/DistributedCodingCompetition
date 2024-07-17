namespace DistributedCodingCompetition.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistributedCodingCompetition.ApiService.Models;
using static DistributedCodingCompetition.ApiService.QueryExtensions;

[Route("api/[controller]")]
[ApiController]
public sealed class ContestsController(ContestContext context) : ControllerBase
{
    // GET: api/Contests
    /// <summary>
    /// Get all contests
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<PaginateResult<ContestResponseDTO>> GetContestsAsync(int page, int count) =>
        await context.Contests.AsNoTracking().PaginateAsync(page, count, q => q.ReadContestsAsync());

    // GET: api/Contests/5
    /// <summary>
    /// Get a contest by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ContestResponseDTO>> GetContestAsync(Guid id)
    {
        var contests = await context.Contests.AsNoTracking().Where(c => c.Id == id).ReadContestsAsync();

        return contests.Count == 0 ? NotFound() : contests[0];
    }

    // GET: api/contests/joincode/{code}
    /// <summary>
    /// Get a contest by join code
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [HttpGet("joincode/{code}")]
    public async Task<ActionResult<ContestResponseDTO>> GetContestByJoinCodeAsync(string code)
    {
        var contest = await context.JoinCodes
            .AsNoTracking()
            .Where(jc => jc.Code == code)
            .Select(jc => jc.Contest)
            .ReadContestsAsync();

        return contest.Count == 0 ? NotFound() : contest[1];
    }

    // GET: api/contests/{contestId}/admins
    /// <summary>
    /// Get all admins for a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/admins")]
    public Task<PaginateResult<UserResponseDTO>> GetContestAdminsAsync(Guid contestId, int page, int count) =>
        context.Contests
               .AsNoTracking()
               .Where(c => c.Id == contestId)
               .SelectMany(c => c.Administrators)
               .PaginateAsync(page, count, q => q.ReadUsersAsync());

    // GET: api/contests/{contestId}/joincodes
    /// <summary>
    /// Get all join codes for a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/joincodes")]
    public Task<IReadOnlyList<JoinCodeResponseDTO>> GetJoinCodesAsync(Guid contestId) =>
        context.JoinCodes
               .AsNoTracking()
               .Where(jc => jc.ContestId == contestId)
               .ReadJoinCodesAsync();

    // GET: api/contests/{contestId}/role/{userId}
    /// <summary>
    /// Get the role of a user in a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/role/{userId}")]
    public async Task<ActionResult<ContestRole?>> GetUserContestRoleAsync(Guid contestId, Guid userId)
    {
        // Check if the user is an admin
        if (await context.Contests.Where(c => c.Id == contestId).SelectMany(c => c.Administrators).AnyAsync(a => a.Id == userId))
            return ContestRole.Admin;

        // Check if the user is a participant
        if (await context.Contests.Where(c => c.Id == contestId).SelectMany(c => c.Participants).AnyAsync(a => a.Id == userId))
            return ContestRole.Participant;

        return NotFound();
    }

    // GET api/contests/{contestId}/banned?count={count}&page={page}
    /// <summary>
    /// Get banned users for a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/banned")]
    public Task<PaginateResult<UserResponseDTO>> GetContestBannedUsersAsync(Guid contestId, int count, int page) =>
        context.Contests
               .AsNoTracking()
               .Where(c => c.Id == contestId)
               .SelectMany(c => c.Banned)
               .PaginateAsync(page, count, q => q.ReadUsersAsync());

    // GET api/contests/{contestId}/participants?count={count}&page={page}
    /// <summary>
    /// Get participants for a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/participants")]
    public async Task<PaginateResult<UserResponseDTO>> GetContestParticipantsAsync(Guid contestId, int count, int page) =>
        await context.Contests
            .AsNoTracking()
            .Where(c => c.Id == contestId)
            .SelectMany(c => c.Participants)
            .PaginateAsync(page, count, q => q.ReadUsersAsync());

    // GET api/contests/public?count={count}&page={page}
    /// <summary>
    /// Get public contests
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    [HttpGet("public")]
    public async Task<PaginateResult<ContestResponseDTO>> GetPublicContestsAsync(int count, int page) =>
        await context.Contests
            .AsNoTracking()
            .Where(c => c.Public)
            .OrderByDescending(c => c.StartTime)
            .PaginateAsync(page, count, q => q.ReadContestsAsync());

    // GET api/contests/{contestId}/problems
    /// <summary>
    /// Get problems for a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/problems")]
    public Task<IReadOnlyList<ProblemResponseDTO>> GetContestProblemsAsync(Guid contestId) =>
        context.Contests
            .AsNoTracking()
            .Where(c => c.Id == contestId)
            .SelectMany(c => c.Problems)
            .ReadProblemsAsync();

    // GET api/contests/{contestId}/user/{userId}/solve
    /// <summary>
    /// Get user solve status for a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/user/{userId}/solve")]
    public async Task<IEnumerable<ProblemUserSolveStatus>> GetUserSolveStatusForContestAsync(Guid contestId, Guid userId)
    {
        var submissions = await context.Submissions
            .AsNoTracking()
            .Where(c => c.ContestId == contestId && c.SubmitterId == userId)
            .Select(s => new { s.ProblemId, s.Points, s.Score, s.MaxPossibleScore, s.TotalTestCases, s.PassedTestCases }).ToListAsync();

        return submissions.OrderByDescending(s => s.Score)
                          .DistinctBy(s => s.ProblemId)
                          .Select(s => new ProblemUserSolveStatus(s.ProblemId, s.Points, s.Score, s.MaxPossibleScore, s.PassedTestCases, s.TotalTestCases));
    }

    // GET api/contests/{contestId}/user/{userId}/solve/{problemId}
    [HttpGet("{contestId}/user/{userId}/solve/{problemId}")]
    public async Task<ActionResult<ProblemUserSolveStatus>> GetUserSolveStatusForProblemAsync(Guid contestId, Guid userId, Guid problemId)
    {
        var submission = await context.Submissions
            .AsNoTracking()
            .Where(c => c.ContestId == contestId && c.SubmitterId == userId && c.ProblemId == problemId)
            .OrderByDescending(s => s.Score)
            .FirstOrDefaultAsync();

        if (submission is null)
            return NotFound();

        return new ProblemUserSolveStatus(submission.ProblemId, submission.Points, submission.Score, submission.MaxPossibleScore, submission.PassedTestCases, submission.TotalTestCases);
    }

    /// <summary>
    /// Get point values for a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/pointvalues")]
    public async Task<IEnumerable<ProblemPointValueResponseDTO>> GetProblemPointValuesAsync(Guid contestId) =>
        await context.ProblemPointValues
            .AsNoTracking()
            .Where(ppv => ppv.ContestId == contestId)
            .ReadProblemPointValuesAsync();

    /// <summary>
    /// Get point value for a problem in a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="problemId"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/pointvalues/{problemId}")]
    public async Task<ActionResult<ProblemPointValueResponseDTO>> GetProblemPointValueAsync(Guid contestId, Guid problemId, bool generateIfNotExist = true)
    {
        var ppv = await context.ProblemPointValues.AsNoTracking().Where(ppv => ppv.ContestId == contestId && ppv.ProblemId == problemId).ReadProblemPointValuesAsync();
        if (ppv.Count > 0)
            return ppv[0];

        // read the contest max
        var contest = await context.Contests.FindAsync(contestId);
        if (contest is null)
            return NotFound();

        if (!generateIfNotExist)
            return NotFound();

        return new ProblemPointValueResponseDTO() { Id = Guid.Empty, ContestId = contestId, ProblemId = problemId, Points = contest.DefaultPointsForProblem };
    }

    [HttpPost("{contestId}/pointvalues/{problemId}")]
    public async Task<ActionResult<ProblemPointValue>> PostProblemPointValueAsync(Guid contestId, Guid problemId, ProblemPointValue ppv)
    {
        ppv.ContestId = contestId;
        ppv.ProblemId = problemId;

        context.ProblemPointValues.Add(ppv);
        await context.SaveChangesAsync();

        ProblemPointValueResponseDTO response = new() { Id = ppv.Id, ContestId = ppv.ContestId, ProblemId = ppv.ProblemId, Points = ppv.Points };

        return CreatedAtAction(nameof(GetProblemPointValueAsync), new { contestId, problemId }, response);
    }

    [HttpPut("{contestId}/pointvalues/{problemId}")]
    public async Task<IActionResult> PutProblemPointValueAsync(Guid contestId, Guid problemId, ProblemPointValue ppv)
    {
        if (contestId != ppv.ContestId || problemId != ppv.ProblemId)
            return BadRequest();

        context.Entry(ppv).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProblemPointValueExists(contestId, problemId))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    bool ProblemPointValueExists(Guid contestId, Guid problemId) =>
        context.ProblemPointValues.Any(e => e.ContestId == contestId && e.ProblemId == problemId);

    /// <summary>
    /// Calculate the leaderboard for a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/leaderboard")]
    public async Task<ActionResult<Leaderboard>> GetLeaderboardAsync(Guid contestId)
    {
        var contest = await context.Contests.FindAsync(contestId);
        if (contest is null)
            return NotFound();

        var defaultPoints = contest.DefaultPointsForProblem;

        const int SCALER = 10000;

        var entries =
            await context.Submissions
                         .AsNoTracking()
                         .Where(s => s.ContestId == contestId && !s.Invalidated && s.Score > 0)
                         .GroupBy(s => s.Submitter)
                         .Select(selector => new
                         {
                             SubmitterId = selector.Key!.Id,
                             SubmitterName = $"{selector.Key.FullName} @{selector.Key.Username}",
                             Score = selector.GroupBy(sub => sub.ProblemId)
                                                .Select(subs => subs.Select(sub => SCALER * sub.Score / sub.MaxPossibleScore)
                                                                    .Max() *
                                                                (context.ProblemPointValues
                                                                        .Any(p => p.ProblemId == subs.Key)
                                                                 ? context.ProblemPointValues
                                                                          .Where(p => p.ProblemId == subs.Key)
                                                                          .First()
                                                                          .Points
                                                                 : defaultPoints) / SCALER)
                                                .Sum()
                         })
                         .OrderByDescending(x => x.Score)
                         .Take(1000)
                         .AsAsyncEnumerable()
                         .Select((result, i) => new LeaderboardEntry(result.SubmitterId, result.SubmitterName, result.Score, i + 1))
                         .ToListAsync();

        return new Leaderboard()
        {
            ContestId = contestId,
            ContestName = contest.Name,
            Count = entries.Count,
            Entries = entries
        };
    }

    // PUT api/contests/{contestId}/role/{userId}
    /// <summary>
    /// Update the role of a user in a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="userId"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    [HttpPut("{contestId}/role/{userId}")]
    public async Task<IActionResult> UpdateUserContestRoleAsync(Guid contestId, Guid userId, ContestRole role)
    {
        var contest = await context.Contests.FindAsync(contestId);
        if (contest is null)
            return NotFound();

        var user = await context.Users.FindAsync(userId);
        if (user is null)
            return NotFound();

        switch (role)
        {
            case ContestRole.Admin:
                contest.Administrators.Add(user);
                contest.Participants.Remove(user);
                break;
            case ContestRole.Participant:
                contest.Participants.Add(user);
                contest.Administrators.Remove(user);
                break;
            default:
                return BadRequest();
        }

        await context.SaveChangesAsync();

        return NoContent();
    }

    // PUT: api/Contests/5
    /// <summary>
    /// Update a contest
    /// </summary>
    /// <param name="id"></param>
    /// <param name="contest"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutContestAsync(Guid id, ContestRequestDTO contestDTO)
    {
        if (id != contestDTO.Id)
            return BadRequest();
        // read the contest
        var contest = await context.Contests.FindAsync(id);

        if (contest is null)
            return NotFound();

        contest.Name = contestDTO.Name ?? contest.Name;
        contest.Description = contestDTO.Description ?? contest.Description;
        contest.RenderedDescription = contestDTO.RenderedDescription ?? contest.RenderedDescription;
        contest.StartTime = contestDTO.StartTime ?? contest.StartTime;
        contest.EndTime = contestDTO.EndTime ?? contest.EndTime;
        contest.Active = contestDTO.Active ?? contest.Active;
        contest.Public = contestDTO.Public ?? contest.Public;
        contest.Open = contestDTO.Open ?? contest.Open;
        contest.MinimumAge = contestDTO.MinimumAge ?? contest.MinimumAge;
        contest.DefaultPointsForProblem = contestDTO.DefaultPointsForProblem ?? contest.DefaultPointsForProblem;
        contest.OwnerId = contestDTO.OwnerId ?? contest.OwnerId;

        context.Entry(contest).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ContestExists(id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }


    // POST: api/Contests
    /// <summary>
    /// Create a contest
    /// </summary>
    /// <param name="contest"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<ContestResponseDTO>> PostContestAsync(ContestRequestDTO dto)
    {
        if (!dto.OwnerId.HasValue)
            return BadRequest("Owner ID is required");

        Contest contest = new()
        {
            Id = dto.Id,
            Name = dto.Name ?? string.Empty,
            Description = dto.Description ?? string.Empty,
            RenderedDescription = dto.RenderedDescription ?? string.Empty,
            StartTime = dto.StartTime ?? DateTime.UtcNow,
            EndTime = dto.EndTime ?? DateTime.UtcNow + TimeSpan.FromDays(1),
            Active = dto.Active ?? true,
            OwnerId = dto.OwnerId.Value,
            Public = dto.Public ?? false,
            Open = dto.Open ?? true,
            MinimumAge = dto.MinimumAge ?? 0,
            DefaultPointsForProblem = dto.DefaultPointsForProblem ?? 100
        };

        context.Contests.Add(contest);
        await context.SaveChangesAsync();

        // just read it back
        var responseDTOs = await context.Contests.Where(c => c.Id == contest.Id).ReadContestsAsync();

        return CreatedAtAction(nameof(GetContestAsync), new { id = contest.Id }, responseDTOs.Count > 0 ? responseDTOs[0] : null);
    }

    // POST: api/contests/{contestId}/problems
    /// <summary>
    /// Add a problem to a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="problem"></param>
    /// <returns></returns>
    [HttpPost("{contestId}/problems/{problemId}")]
    public async Task<IActionResult> AddProblemToContestAsync(Guid contestId, Guid problemId)
    {
        var contest = await context.Contests.FindAsync(contestId);
        if (contest is null)
            return NotFound();

        var problem = await context.Problems.FindAsync(problemId);
        if (problem is null)
            return NotFound();

        contest.Problems.Add(problem);
        await context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Contests/5
    /// <summary>
    /// Delete a contest
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContest(Guid id)
    {
        var contest = await context.Contests.FindAsync(id);
        if (contest is null)
            return NotFound();

        context.Contests.Remove(contest);
        await context.SaveChangesAsync();

        return NoContent();
    }

    // GET api/contests/{contestId}/submissions
    /// <summary>
    /// Get submissions for a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/submissions")]
    public Task<PaginateResult<SubmissionResponseDTO>> GetContestSubmissionsAsync(Guid contestId, int count, int page) =>
        context.Submissions
            .AsNoTracking()
            .Where(s => s.ContestId == contestId)
            .OrderByDescending(s => s.SubmissionTime)
            .PaginateAsync(page, count, q => q.ReadSubmissionsAsync());

    /// <summary>
    /// Check if a contest exists
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private bool ContestExists(Guid id) =>
        context.Contests.Any(e => e.Id == id);
}
