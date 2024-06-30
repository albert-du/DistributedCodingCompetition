namespace DistributedCodingCompetition.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistributedCodingCompetition.ApiService.Models;

[Route("api/[controller]")]
[ApiController]
public class JoinCodesController(ContestContext context) : ControllerBase
{
    // GET: api/JoinCodes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JoinCode>>> GetJoinCodes() =>
        await context.JoinCodes.ToListAsync();

    // GET: api/JoinCodes/Code/5
    [HttpGet("Code/{code}")]
    public async Task<ActionResult<JoinCode>> GetJoinCode(string code)
    {
        var joinCode = await context.JoinCodes.FirstOrDefaultAsync(j => j.Code == code);

        return joinCode is null ? NotFound() : joinCode;
    }

    // GET: api/JoinCodes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<JoinCode>> GetJoinCode(Guid id)
    {
        var joinCode = await context.JoinCodes.FindAsync(id);

        return joinCode is null ? NotFound() : joinCode;
    }

    // PUT: api/JoinCodes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
            else
                throw;
        }

        return NoContent();
    }

    // POST: api/JoinCodes
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<JoinCode>> PostJoinCode(JoinCode joinCode)
    {
        context.JoinCodes.Add(joinCode);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetJoinCode", new { id = joinCode.Id }, joinCode);
    }

    // DELETE: api/JoinCodes/5
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
