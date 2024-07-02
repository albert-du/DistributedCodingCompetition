namespace DistributedCodingCompetition.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistributedCodingCompetition.ApiService.Models;

[Route("api/[controller]")]
[ApiController]
public class ProblemsController(ContestContext context) : ControllerBase
{
    // GET: api/Problems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Problem>>> GetProblems() =>
        await context.Problems.ToListAsync();

    // GET: api/Problems/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Problem>> GetProblem(Guid id)
    {
        var problem = await context.Problems.FindAsync(id);

        return problem == null ? NotFound() : problem;
    }

    // PUT: api/Problems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProblem(Guid id, Problem problem)
    {
        if (id != problem.Id)
            return BadRequest();

        context.Entry(problem).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProblemExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Problems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Problem>> PostProblem(Problem problem)
    {
        context.Problems.Add(problem);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProblem), new { id = problem.Id }, problem);
    }

    // DELETE: api/Problems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProblem(Guid id)
    {
        var problem = await context.Problems.FindAsync(id);
        if (problem is null)
            return NotFound();

        context.Problems.Remove(problem);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProblemExists(Guid id) =>
        context.Problems.Any(e => e.Id == id);
}
