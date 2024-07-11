namespace DistributedCodingCompetition.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// Api controller for Problems
/// </summary>
/// <param name="context"></param>
[Route("api/[controller]")]
[ApiController]
public class ProblemsController(ContestContext context) : ControllerBase
{
    // GET: api/Problems
    /// <summary>
    /// Gets all problems
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Problem>>> GetProblems() =>
        await context.Problems.ToListAsync();

    // GET: api/Problems/5
    /// <summary>
    /// Gets a problem by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Problem>> GetProblem(Guid id)
    {
        var problem = await context.Problems.FindAsync(id);

        return problem == null ? NotFound() : problem;
    }


    [HttpGet("{id}/submissions")]
    public async Task<ActionResult<IEnumerable<Submission>>> GetSubmissionsForProblem(Guid id) =>
        await context.Submissions.Where(s => s.ProblemId == id).ToListAsync();

    // PUT: api/Problems/5
    /// <summary>
    /// Updates a problem
    /// </summary>
    /// <param name="id"></param>
    /// <param name="problem"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Creates a problem in the database
    /// </summary>
    /// <param name="problem"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Problem>> PostProblem(Problem problem)
    {
        context.Problems.Add(problem);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProblem), new { id = problem.Id }, problem);
    }

    // DELETE: api/Problems/5
    /// <summary>
    /// Deletes a problem by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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


    // GET: api/problems/{problemId}/testcases
    /// <summary>
    /// Gets all test cases for a problem
    /// </summary>
    /// <param name="problemId"></param>
    /// <returns></returns>
    [HttpGet("{problemId}/testcases")]
    public async Task<ActionResult<IReadOnlyList<TestCase>>> GetTestCasesForProblem(Guid problemId)
    {
        try
        {
            var problem = await context.Problems.Include(p => p.TestCases).FirstOrDefaultAsync(p => p.Id == problemId);
            if (problem is null)
                return NotFound();
            return problem.TestCases.Select(x =>
            {
                x.Problem = null;
                return x;
            }).ToList();
        }
        catch
        {
            throw;
        }
    }


    // POST: api/problems/{problemId}/testcases
    /// <summary>
    /// Adds a test case to a problem
    /// </summary>
    /// <param name="problemId"></param>
    /// <param name="testCase"></param>
    /// <returns></returns>
    [HttpPost("{problemId}/testcases")]
    public async Task<IActionResult> AddTestCaseToProblem(Guid problemId, TestCase testCase)
    {
        var problem = await context.Problems.FindAsync(problemId);
        if (problem is null)
            return NotFound();
        problem.TestCases.Add(testCase);

        var entry = context.Entry(problem);
        entry.State = EntityState.Modified;

        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProblemExists(Guid id) =>
        context.Problems.Any(e => e.Id == id);
}
