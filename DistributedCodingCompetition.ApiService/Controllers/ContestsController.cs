namespace DistributedCodingCompetition.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistributedCodingCompetition.ApiService.Models;

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
    public async Task<ActionResult<IEnumerable<Contest>>> GetContests() =>
        await context.Contests.ToListAsync();

    // GET: api/Contests/5
    /// <summary>
    /// Get a contest by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Contest>> GetContest(Guid id)
    {
        var contest = await context.Contests.FindAsync(id);

        return contest is null ? NotFound() : contest;
    }

    // GET: api/contests/joincode/{code}
    /// <summary>
    /// Get a contest by join code
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [HttpGet("joincode/{code}")]
    public async Task<ActionResult<Contest>> GetContestByJoinCode(string code)
    {
        var contest = await context.JoinCodes
            .Where(jc => jc.Code == code)
            .Select(jc => jc.Contest)
            .FirstOrDefaultAsync();

        return contest is null ? NotFound() : contest;
    }

    // GET: api/contests/{contestId}/admins
    /// <summary>
    /// Get all admins for a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/admins")]
    public async Task<ActionResult<IReadOnlyList<User>>> GetContestAdmins(Guid contestId) =>
        await context.Contests
            .Where(c => c.Id == contestId)
            .SelectMany(c => c.Administrators)
            .ToListAsync();

    // GET: api/contests/{contestId}/joincodes
    /// <summary>
    /// Get all join codes for a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/joincodes")]
    public async Task<ActionResult<IReadOnlyList<JoinCode>>> GetJoinCodes(Guid contestId) =>
        await context.Contests
            .Where(c => c.Id == contestId)
            .SelectMany(c => c.JoinCodes)
            .ToListAsync();

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
    public async Task<ActionResult<IReadOnlyList<User>>> GetContestBannedUsers(Guid contestId, int count, int page) =>
        await context.Contests
            .Where(c => c.Id == contestId)
            .SelectMany(c => c.Banned)
            .Skip(count * page)
            .Take(count)
            .ToListAsync();

    // GET api/contests/{contestId}/participants?count={count}&page={page}
    /// <summary>
    /// Get participants for a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/participants")]
    public async Task<ActionResult<IReadOnlyList<User>>> GetContestParticipants(Guid contestId, int count, int page) =>
        await context.Contests
            .Where(c => c.Id == contestId)
            .SelectMany(c => c.Participants)
            .Skip(count * page)
            .Take(count)
            .ToListAsync();

    // GET api/contests/public?count={count}&page={page}
    /// <summary>
    /// Get public contests
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    [HttpGet("public")]
    public async Task<ActionResult<IReadOnlyList<Contest>>> GetPublicContests(int count, int page) =>
        await context.Contests
            .Where(c => c.Public)
            .OrderByDescending(c => c.StartTime)
            .Skip(count * page)
            .Take(count)
            .ToListAsync();

    // GET api/contests/{contestId}/problems
    /// <summary>
    /// Get problems for a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/problems")]
    public async Task<ActionResult<IReadOnlyList<Problem>>> GetContestProblems(Guid contestId) =>
        await context.Contests
            .Where(c => c.Id == contestId)
            .SelectMany(c => c.Problems)
            .OrderBy(c => c.Name)
            .ToListAsync();

    // GET api/contests/{contestId}/user/{userId}/solve
    /// <summary>
    /// Get user solve status for a contest
    /// </summary>
    /// <param name="contestId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{contestId}/user/{userId}/solve")]
    public async Task<ActionResult<IReadOnlyList<ProblemUserSolveStatus>>> GetUserSolveStatusForContest(Guid contestId, Guid userId)
    {
        var submissions = await context.Submissions
            .Where(c => c.ContestId == contestId && c.SubmitterId == userId)
            .Select(s => new { s.ProblemId, s.Points, s.Score, s.MaxPossibleScore }).ToListAsync();
        return submissions.Select(s => new ProblemUserSolveStatus(s.ProblemId, s.Points, s.Score, s.MaxPossibleScore)).ToList();
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
    public async Task<IActionResult> PutContest(Guid id, Contest contest)
    {
        if (id != contest.Id)
            return BadRequest();

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
    public async Task<ActionResult<IReadOnlyList<Submission>>> GetContestSubmissions(Guid contestId, int count, int page) =>
        await context.Submissions
            .Where(s => s.ContestId == contestId)
            .OrderByDescending(s => s.SubmissionTime)
            .Skip(count * page)
            .Take(count)
            .ToListAsync();

    /// <summary>
    /// Check if a contest exists
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private bool ContestExists(Guid id) =>
        context.Contests.Any(e => e.Id == id);

}
