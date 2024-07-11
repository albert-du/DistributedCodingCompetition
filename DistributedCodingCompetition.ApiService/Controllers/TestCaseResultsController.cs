namespace DistributedCodingCompetition.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// Api controller for TestCaseResults
/// </summary>
/// <param name="context"></param>
[Route("api/[controller]")]
[ApiController]
public class TestCaseResultsController(ContestContext context) : ControllerBase
{
    // GET: api/TestCaseResults
    /// <summary>
    /// Gets all test case results
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TestCaseResult>>> GetTestCaseResults(Guid submissionId) =>
         await context.TestCaseResults.Where(t => t.SubmissionId == submissionId).ToListAsync();

    // GET: api/TestCaseResults/5
    /// <summary>
    /// Gets a test case result by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TestCaseResult>> GetTestCaseResult(Guid id)
    {
        var testCaseResult = await context.TestCaseResults.FindAsync(id);

        if (testCaseResult is null)
            return NotFound();

        return testCaseResult;
    }

    // PUT: api/TestCaseResults/5
    /// <summary>
    /// Updates a test case result
    /// </summary>
    /// <param name="id"></param>
    /// <param name="testCaseResult"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTestCaseResult(Guid id, TestCaseResult testCaseResult)
    {
        if (id != testCaseResult.Id)
            return BadRequest();

        context.Entry(testCaseResult).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TestCaseResultExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/TestCaseResults
    /// <summary>
    /// Creates a test case result
    /// </summary>
    /// <param name="testCaseResult"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<TestCaseResult>> PostTestCaseResult(TestCaseResult testCaseResult)
    {
        context.TestCaseResults.Add(testCaseResult);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetTestCaseResult", new { id = testCaseResult.Id }, testCaseResult);
    }

    // DELETE: api/TestCaseResults/5
    /// <summary>
    /// Deletes a test case result by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTestCaseResult(Guid id)
    {
        var testCaseResult = await context.TestCaseResults.FindAsync(id);
        if (testCaseResult is null)
            return NotFound();

        context.TestCaseResults.Remove(testCaseResult);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool TestCaseResultExists(Guid id) =>
        context.TestCaseResults.Any(e => e.Id == id);
}
