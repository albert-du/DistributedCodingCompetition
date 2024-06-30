namespace DistributedCodingCompetition.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistributedCodingCompetition.ApiService.Models;

[Route("api/[controller]")]
[ApiController]
public class TestCasesController(ContestContext context) : ControllerBase
{
    // GET: api/TestCases
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TestCase>>> GetTestCases() =>
        await context.TestCases.ToListAsync();

    // GET: api/TestCases/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TestCase>> GetTestCase(Guid id)
    {
        var testCase = await context.TestCases.FindAsync(id);

        if (testCase is null)
            return NotFound();

        return testCase;
    }

    // PUT: api/TestCases/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TestCase>> PostTestCase(TestCase testCase)
    {
        context.TestCases.Add(testCase);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetTestCase", new { id = testCase.Id }, testCase);
    }

    // DELETE: api/TestCases/5
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
