namespace DistributedCodingCompetition.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// Api controller for TestCases
/// </summary>
/// <param name="context"></param>
[Route("api/[controller]")]
[ApiController]
public class TestCasesController(ContestContext context) : ControllerBase
{
    // GET: api/TestCases
    /// <summary>
    /// Gets all test cases
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TestCase>>> GetTestCases() =>
        await context.TestCases.ToListAsync();

    // GET: api/TestCases/5
    /// <summary>
    /// Gets a test case by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TestCase>> GetTestCase(Guid id)
    {
        var testCase = await context.TestCases.FindAsync(id);

        if (testCase is null)
            return NotFound();

        return testCase;
    }

    // GET: api/TestCases/{id}/results
    /// <summary>
    /// Gets all test case results for a test case
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ActionResult<IEnumerable<TestCaseResult>>> GetTestCaseResults(Guid id) =>
        await context.TestCaseResults.Where(r => r.TestCaseId == id).ToListAsync();

    // PUT: api/TestCases/5
    /// <summary>
    /// Updates a test case
    /// </summary>
    /// <param name="id"></param>
    /// <param name="testCase"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTestCase(Guid id, TestCase testCase)
    {
        if (id != testCase.Id)
            return BadRequest();

        context.Entry(testCase).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TestCaseExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/TestCases
    /// <summary>
    /// Creates a test case
    /// </summary>
    /// <param name="testCase"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<TestCase>> PostTestCase(TestCase testCase)
    {

        // add testcase to problem
        var problem = await context.Problems.FindAsync(testCase.ProblemId);
        if (problem is null)
            return NotFound();

        context.TestCases.Add(testCase);

        testCase.Problem = problem;


        //problem.TestCases.Add(testCase);

        await context.SaveChangesAsync();
        testCase.Problem = null;
        return CreatedAtAction(nameof(GetTestCase), new { id = testCase.Id }, testCase);
    }

    // DELETE: api/TestCases/5
    /// <summary>
    /// Deletes a test case by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTestCase(Guid id)
    {
        var testCase = await context.TestCases.FindAsync(id);
        if (testCase is null)
            return NotFound();

        context.TestCases.Remove(testCase);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool TestCaseExists(Guid id) =>
        context.TestCases.Any(e => e.Id == id);
}
