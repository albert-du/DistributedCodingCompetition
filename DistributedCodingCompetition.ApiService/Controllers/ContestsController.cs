namespace DistributedCodingCompetition.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistributedCodingCompetition.ApiService.Models;

[Route("api/[controller]")]
[ApiController]
public class ContestsController(ContestContext context) : ControllerBase
{
    // GET: api/Contests
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contest>>> GetContests() =>
        await context.Contests.ToListAsync();

    // GET: api/Contests/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Contest>> GetContest(Guid id)
    {
        var contest = await context.Contests.FindAsync(id);

        return contest is null ? NotFound() : contest;
    }

    // GET: api/contests/joincode/{code}
    [HttpGet("joincode/{code}")]
    public async Task<ActionResult<Contest>> GetContestByJoinCode(string code)
    {
        var contest = await context.JoinCodes
            .Where(jc => jc.Code == code)
            .Select(jc => jc.Contest)
            .FirstOrDefaultAsync();

        return contest is null ? NotFound() : contest;
    }

    [HttpGet("{contestId}/admins")]
    public async Task<ActionResult<IReadOnlyList<User>>> GetContestAdmins(Guid contestId)
    {
        var contest = await context.Contests
            .Include(c => c.Administrators)
            .Where(c => c.Id == contestId)
            .FirstOrDefaultAsync();

        var admins = contest?.Administrators;

        return admins is null ? NotFound() : admins.ToList();
    }

    [HttpGet("{contestId}/joincodes")]
    public async Task<ActionResult<IReadOnlyList<JoinCode>>> GetJoinCodes(Guid contestId)
    {
        var contest = await context.Contests
            .Include(c => c.JoinCodes)
            .Where(c => c.Id == contestId)
            .FirstOrDefaultAsync();

        var joinCodes = contest?.JoinCodes;

        return joinCodes is null ? NotFound() : joinCodes.ToList();
    }

    [HttpGet("{contestId}/role/{userId}")]
    public async Task<ActionResult<ContestRole?>> GetUserContestRole(Guid contestId, Guid userId)
    {
        if (await context.Contests.Where(c => c.Id == contestId).SelectMany(c => c.Administrators).AnyAsync(a => a.Id == userId))
            return ContestRole.Admin;

        if (await context.Contests.Where(c => c.Id == contestId).SelectMany(c => c.Participants).AnyAsync(a => a.Id == userId))
            return ContestRole.Participant;

        return NotFound();
    }

    // GET api/contests/{contestId}/banned?count={count}&page={page}
    [HttpGet("{contestId}/banned")]
    public async Task<ActionResult<IReadOnlyList<User>>> GetContestBannedUsers(Guid contestId, int count, int page) =>
        await context.Contests
            .Where(c => c.Id == contestId)
            .SelectMany(c => c.Banned)
            .Skip(count * page)
            .Take(count)
            .ToListAsync();

    // GET api/contests/{contestId}/participants?count={count}&page={page}
    [HttpGet("{contestId}/participants")]
    public async Task<ActionResult<IReadOnlyList<User>>> GetContestParticipants(Guid contestId, int count, int page) =>
        await context.Contests
            .Where(c => c.Id == contestId)
            .SelectMany(c => c.Participants)
            .Skip(count * page)
            .Take(count)
            .ToListAsync();

    // GET api/contests/public?count={count}&page={page}
    [HttpGet("public")]
    public async Task<ActionResult<IReadOnlyList<Contest>>> GetPublicContests(int count, int page) =>
        await context.Contests
            .Where(c => c.Public)
            .OrderByDescending(c => c.StartTime)
            .Skip(count * page)
            .Take(count)
            .ToListAsync();

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
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Contest>> PostContest(Contest contest)
    {
        // fill in owner with the current user based on the id from tracking
        // var owner = await context.Users.FindAsync(contest.OwnerId);
        // if (owner is null)
        //     return BadRequest("Owner not found");
        // contest.Owner = owner;
        context.Contests.Add(contest);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetContest), new { id = contest.Id }, contest);
    }

    // DELETE: api/Contests/5
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
    [HttpGet("{contestId}/submissions")]
    public async Task<ActionResult<IReadOnlyList<Submission>>> GetContestSubmissions(Guid contestId, int count, int page) =>
        await context.Submissions
            .Where(s => s.ContestId == contestId)
            .OrderByDescending(s => s.SubmissionTime)
            .Skip(count * page)
            .Take(count)
            .ToListAsync();

    private bool ContestExists(Guid id) =>
        context.Contests.Any(e => e.Id == id);
}
