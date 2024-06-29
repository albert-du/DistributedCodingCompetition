namespace DistributedCodingCompetition.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistributedCodingCompetition.ApiService.Models;

[Route("api/[controller]")]
[ApiController]
internal class TestCaseResultsController(ContestContext context) : ControllerBase
{
    // GET: api/TestCaseResults
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TestCaseResult>>> GetTestCaseResults() =>
         await context.TestCaseResults.ToListAsync();

    // GET: api/TestCaseResults/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TestCaseResult>> GetTestCaseResult(Guid id)
    {
        var testCaseResult = await context.TestCaseResults.FindAsync(id);

        if (testCaseResult is null)
            return NotFound();

        return testCaseResult;
    }

    // PUT: api/TestCaseResults/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TestCaseResult>> PostTestCaseResult(TestCaseResult testCaseResult)
    {
        context.TestCaseResults.Add(testCaseResult);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetTestCaseResult", new { id = testCaseResult.Id }, testCaseResult);
    }

    // DELETE: api/TestCaseResults/5
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
