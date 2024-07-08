namespace DistributedCodingCompetition.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// Api controller for JoinCodes
/// </summary>
/// <param name="context"></param>
[Route("api/[controller]")]
[ApiController]
public class JoinCodesController(ContestContext context) : ControllerBase
{
    // GET: api/JoinCodes
    /// <summary>
    /// Get all join codes
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JoinCode>>> GetJoinCodes() =>
        await context.JoinCodes.ToListAsync();

    // GET: api/JoinCodes/Code/5
    /// <summary>
    /// Get join code by code
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [HttpGet("Code/{code}")]
    public async Task<ActionResult<JoinCode>> GetJoinCode(string code)
    {
        var joinCode = await context.JoinCodes.FirstOrDefaultAsync(j => j.Code == code);

        return joinCode is null ? NotFound() : joinCode;
    }

    // GET: api/JoinCodes/5
    /// <summary>
    /// Gets a join code by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<JoinCode>> GetJoinCode(Guid id)
    {
        var joinCode = await context.JoinCodes.FindAsync(id);

        return joinCode is null ? NotFound() : joinCode;
    }

    // PUT: api/JoinCodes/5
    /// <summary>
    /// Updates a join code
    /// </summary>
    /// <param name="id"></param>
    /// <param name="joinCode"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutJoinCode(Guid id, JoinCode joinCode)
    {
        if (id != joinCode.Id)
            return BadRequest();

        context.Entry(joinCode).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!JoinCodeExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/JoinCodes
    /// <summary>
    /// Creates a new join code
    /// </summary>
    /// <param name="joinCode"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<JoinCode>> PostJoinCode(JoinCode joinCode)
    {
        context.JoinCodes.Add(joinCode);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetJoinCode", new { id = joinCode.Id }, joinCode);
    }

    // POST: api/joincodes/{joinCodeId}/join/{userId}
    /// <summary>
    /// Join a contest using a join code
    /// </summary>
    /// <param name="joinCodeId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpPost("{joinCodeId}/join/{userId}")]
    public async Task<ActionResult<JoinCode>> JoinContest(Guid joinCodeId, Guid userId)
    {
        var joinCode = await context.JoinCodes.FindAsync(joinCodeId);
        if (joinCode is null)
            return NotFound();

        if (!joinCode.Active)
            return BadRequest("Join code is not active.");

        var user = await context.Users.FindAsync(userId);
        if (user is null)
            return NotFound();

        var contest = await context.Contests.FindAsync(joinCode.ContestId);
        if (contest is null)
            return NotFound();

        // check if user is already an admin
        if (await context.Contests.Where(c => c.Id == contest.Id).AnyAsync(c => c.Administrators.Contains(user)))
            return BadRequest("User is already an admin.");

        if (!joinCode.Admin && await context.Contests.Where(c => c.Id == contest.Id).AnyAsync(c => c.Participants.Contains(user)))
            return BadRequest("User is already a participant.");

        joinCode.Users.Add(user);
        if (joinCode.CloseAfterUse)
            joinCode.Active = false;

        // add user to contest
        if (joinCode.Admin)
            contest.Administrators.Add(user);
        else
            contest.Participants.Add(user);

        await context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/JoinCodes/5
    /// <summary>
    /// Deletes a join code
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJoinCode(Guid id)
    {
        var joinCode = await context.JoinCodes.FindAsync(id);
        if (joinCode == null)
            return NotFound();

        context.JoinCodes.Remove(joinCode);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool JoinCodeExists(Guid id) =>
        context.JoinCodes.Any(e => e.Id == id);
}
