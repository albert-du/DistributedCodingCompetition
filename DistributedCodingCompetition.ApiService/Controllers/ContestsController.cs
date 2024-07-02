﻿namespace DistributedCodingCompetition.ApiService.Controllers;

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
        context.Contests.Add(contest);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetContest", new { id = contest.Id }, contest);
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
