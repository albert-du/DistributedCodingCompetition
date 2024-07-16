namespace DistributedCodingCompetition.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistributedCodingCompetition.ApiService.Models;
using static DistributedCodingCompetition.ApiService.QueryExtensions;

[Route("api/[controller]")]
[ApiController]
public class ContestsController(ContestContext context) : ControllerBase
{
    // GET: api/Contests
    /// <summary>
    /// Get all contests
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<PaginateResult<ContestResponseDTO>> GetContests(int page, int count) =>
        await context.Contests.AsNoTracking().PaginateAsync(page, count, q => q.ReadContestsAsync());

    // GET: api/Contests/5
    /// <summary>
    /// Get a contest by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ContestResponseDTO>> GetContest(Guid id)
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
    public async Task<ActionResult<ContestResponseDTO>> GetContestByJoinCode(string code)
    {
        var contest = await context.JoinCodes
            .Where(jc => jc.Code == code)
            .Select(jc => jc.Contest)
            .ReadContestsAsync();

        return contest is null ? NotFound() : contest.Serialize();
    }

    // GET: api/contests/{contestId}/admins
    /// <summary>
    /// Get all admins for a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/admins")]
    public async Task<PaginateResult<UserResponseDTO>> GetContestAdmins(Guid contestId, int page, int count)
    {
        var resp =
            await context.Contests
                .Where(c => c.Id == contestId)
                .SelectMany(c => c.Administrators)
                .Skip(count * (page - 1))
                .Take(count)
                .ToListAsync();

        var total = await context.Contests.Where(c => c.Id == contestId).SelectMany(c => c.Administrators).CountAsync();

        return new()
        {
            Page = page,
            PageSize = count,
            TotalCount = total,
            TotalPages = (int)Math.Ceiling(total / (double)count),
            Items = resp.Select(u => u.Serialize()).ToArray()
        };
    }

    // GET: api/contests/{contestId}/joincodes
    /// <summary>
    /// Get all join codes for a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/joincodes")]
    public async Task<IEnumerable<JoinCodeResponseDTO>> GetJoinCodes(Guid contestId)
    {
        var resp =
            await context.Contests
                .Where(c => c.Id == contestId)
                .SelectMany(c => c.JoinCodes)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.ContestId,
                    ContestName = c.Contest.Name,
                    c.CreatorId,
                    CreatorName = c.Creator.Username,
                    c.Code,
                    c.Admin,
                    c.Active,
                    c.CloseAfterUse,
                    Uses = c.Users.Count,
                    CreatedAt = c.Creation
                })
                .ToListAsync();
        return resp.Select(jc => new JoinCodeResponseDTO()
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
        }).ToArray();

    }

    // GET: api/contests/{contestId}/role/{userId}
    /// <summary>
    /// Get the role of a user in a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/role/{userId}")]
    public async Task<ActionResult<ContestRole?>> GetUserContestRole(Guid contestId, Guid userId)
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
    public async Task<PaginateResult<UserResponseDTO>> GetContestBannedUsers(Guid contestId, int count, int page)
    {

        var resp =
            await context.Contests
                .Where(c => c.Id == contestId)
                .SelectMany(c => c.Banned)
                .Skip(count * (page - 1))
                .Take(count)
                .ToListAsync();

        var total = await context.Contests.Where(c => c.Id == contestId).SelectMany(c => c.Banned).CountAsync();

        return new()
        {
            Page = page,
            PageSize = count,
            TotalCount = total,
            TotalPages = (int)Math.Ceiling(total / (double)count),
            Items = resp.Select(u => u.Serialize()).ToArray()
        };
    }

    // GET api/contests/{contestId}/participants?count={count}&page={page}
    /// <summary>
    /// Get participants for a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/participants")]
    public async Task<PaginateResult<UserResponseDTO>> GetContestParticipants(Guid contestId, int count, int page)
    {

        var resp =
            await context.Contests
                .Where(c => c.Id == contestId)
                .SelectMany(c => c.Participants)
                .Skip(count * (page - 1))
                .Take(count)
                .ToListAsync();

        var total = await context.Contests.Where(c => c.Id == contestId).SelectMany(c => c.Participants).CountAsync();

        return new()
        {
            Page = page,
            PageSize = count,
            TotalCount = total,
            TotalPages = (int)Math.Ceiling(total / (double)count),
            Items = resp.Select(u => u.Serialize()).ToArray()
        };
    }
    // GET api/contests/public?count={count}&page={page}
    /// <summary>
    /// Get public contests
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    [HttpGet("public")]
    public async Task<PaginateResult<ContestResponseDTO>> GetPublicContests(int count, int page)
    {
        var contests =
            await context.Contests
                .Where(c => c.Public)
                .OrderByDescending(c => c.StartTime)
                .Skip(count * (page - 1))
                .Take(count)
                .ReadContestsAsync();

        var total = await context.Contests.Where(c => c.Public).CountAsync();

        return new()
        {
            Page = page,
            PageSize = count,
            TotalCount = total,
            TotalPages = (int)Math.Ceiling(total / (double)count),
            Items = contests
        };
    }

    // GET api/contests/{contestId}/problems
    /// <summary>
    /// Get problems for a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/problems")]
    public async Task<IEnumerable<ProblemResponseDTO>> GetContestProblems(Guid contestId)
    {
        var resp =
            await context.Contests
                .Where(c => c.Id == contestId)
                .SelectMany(c => c.Problems)
                .OrderBy(c => c.Name)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    OwnerId = p.Owner!.Id,
                    OwnerName = p.Owner.Username,
                    p.TagLine,
                    p.Description,
                    p.RenderedDescription,
                    p.Difficulty,
                    TestCaseCount = p.TestCases.Count
                })
                .ToListAsync();
        return resp.Select(p => new ProblemResponseDTO()
        {
            Id = p.Id,
            Name = p.Name,
            OwnerId = p.OwnerId,
            OwnerName = p.OwnerName,
            TagLine = p.TagLine,
            Description = p.Description,
            RenderedDescription = p.RenderedDescription,
            Difficulty = p.Difficulty,
            TestCaseCount = p.TestCaseCount
        });
    }

    // GET api/contests/{contestId}/user/{userId}/solve
    /// <summary>
    /// Get user solve status for a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/user/{userId}/solve")]
    public async Task<IEnumerable<ProblemUserSolveStatus>> GetUserSolveStatusForContest(Guid contestId, Guid userId)
    {
        var submissions = await context.Submissions
            .Where(c => c.ContestId == contestId && c.SubmitterId == userId)
            .Select(s => new { s.ProblemId, s.Points, s.Score, s.MaxPossibleScore, s.TotalTestCases, s.PassedTestCases }).ToListAsync();

        return submissions.OrderByDescending(s => s.Score)
                          .DistinctBy(s => s.ProblemId)
                          .Select(s => new ProblemUserSolveStatus(s.ProblemId, s.Points, s.Score, s.MaxPossibleScore, s.PassedTestCases, s.TotalTestCases));
    }

    // GET api/contests/{contestId}/user/{userId}/solve/{problemId}
    [HttpGet("{contestId}/user/{userId}/solve/{problemId}")]
    public async Task<ActionResult<ProblemUserSolveStatus>> GetUserSolveStatusForProblem(Guid contestId, Guid userId, Guid problemId)
    {
        var submission = await context.Submissions
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
    public async Task<ActionResult<IEnumerable<ProblemPointValue>>> GetProblemPointValues(Guid contestId) =>
        await context.ProblemPointValues.Where(ppv => ppv.ContestId == contestId).ToListAsync();

    /// <summary>
    /// Get point value for a problem in a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="problemId"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/pointvalues/{problemId}")]
    public async Task<ActionResult<ProblemPointValue>> GetProblemPointValue(Guid contestId, Guid problemId, bool generateIfNotExist = true)
    {
        var ppv = await context.ProblemPointValues.Where(ppv => ppv.ContestId == contestId && ppv.ProblemId == problemId).FirstOrDefaultAsync();
        if (ppv is not null)
            return ppv;

        // read the contest max
        var contest = await context.Contests.FindAsync(contestId);
        if (contest is null)
            return NotFound();

        if (!generateIfNotExist)
            return NotFound();

        return new ProblemPointValue() { Id = Guid.Empty, ContestId = contestId, ProblemId = problemId, Points = contest.DefaultPointsForProblem };
    }

    [HttpPost("{contestId}/pointvalues/{problemId}")]
    public async Task<ActionResult<ProblemPointValue>> PostProblemPointValue(Guid contestId, Guid problemId, ProblemPointValue ppv)
    {
        ppv.ContestId = contestId;
        ppv.ProblemId = problemId;

        context.ProblemPointValues.Add(ppv);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProblemPointValue), new { contestId, problemId }, ppv);
    }

    [HttpPut("{contestId}/pointvalues/{problemId}")]
    public async Task<IActionResult> PutProblemPointValue(Guid contestId, Guid problemId, ProblemPointValue ppv)
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
    public async Task<ActionResult<Leaderboard>> GetLeaderboard(Guid contestId)
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
    public async Task<IActionResult> UpdateUserContestRole(Guid contestId, Guid userId, ContestRole role)
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
    public async Task<IActionResult> PutContest(Guid id, ContestRequestDTO contestDTO)
    {
        if (id != contestDTO.Id)
            return BadRequest();
        // read the contest
        var contest = await context.Contests.FindAsync(id);

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
    public async Task<ActionResult<Contest>> PostContest(Contest contest)
    {
        context.Contests.Add(contest);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetContest), new { id = contest.Id }, contest);
    }

    // POST: api/contests/{contestId}/problems
    /// <summary>
    /// Add a problem to a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="problem"></param>
    /// <returns></returns>
    [HttpPost("{contestId}/problems")]
    public async Task<IActionResult> AddProblemToContest(Guid contestId, Problem problem)
    {
        var contest = await context.Contests.FindAsync(contestId);
        if (contest is null)
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
    public async Task<ActionResult<IReadOnlyList<Submission>>> GetContestSubmissions(Guid contestId, int count, int page)
    {
        var rv = await context.Submissions
            .Include(s => s.Problem)
            .Include(s => s.Submitter)
            .Where(s => s.ContestId == contestId)
            .OrderByDescending(s => s.SubmissionTime)
            .Skip(count * page)
            .Take(count)
            .ToListAsync();
        foreach (var s in rv)
        {
            if (s.Submitter?.Submissions is not null)
                s.Submitter.Submissions = [];
        }
        return rv;
    }

    /// <summary>
    /// Check if a contest exists
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private bool ContestExists(Guid id) =>
        context.Contests.Any(e => e.Id == id);
}
