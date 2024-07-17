namespace DistributedCodingCompetition.ApiService.Controllers;

/// <summary>
/// Api controller for TestCases
/// </summary>
/// <param name="context"></param>
[Route("api/[controller]")]
[ApiController]
public sealed class TestCasesController(ContestContext context) : ControllerBase
{
    // GET: api/TestCases
    /// <summary>
    /// Gets all test cases
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<PaginateResult<TestCaseResponseDTO>> GetTestCasesAsync(int page, int count) =>
        await context.TestCases
            .AsNoTracking()
            .PaginateAsync(page, count, q => q.ReadTestCasesAsync());

    // GET: api/TestCases/5
    /// <summary>
    /// Gets a test case by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TestCaseResponseDTO>> GetTestCaseAsync(Guid id)
    {
        var testCases = await context.TestCases.AsNoTracking().Where(t => t.Id == id).ReadTestCasesAsync();
        return testCases.Count == 0 ? NotFound() : testCases[0];
    }

    // PUT: api/TestCases/5
    /// <summary>
    /// Updates a test case
    /// </summary>
    /// <param name="id"></param>
    /// <param name="testCase"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTestCaseAsync(Guid id, TestCaseRequestDTO dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var testCase = await context.TestCases.FindAsync(id);
        if (testCase is null)
            return NotFound();

        testCase.Input = dto.Input ?? testCase.Input;
        testCase.Output = dto.Output ?? testCase.Output;
        testCase.Description = dto.Description ?? testCase.Description;
        testCase.Sample = dto.Sample ?? testCase.Sample;
        testCase.Active = dto.Active ?? testCase.Active;
        testCase.Weight = dto.Weight ?? testCase.Weight;

        context.Entry(testCase).State = EntityState.Modified;

        var prob = await context.Problems.FindAsync(testCase.ProblemId);
        if (prob is not null)
            prob.ScoringFactorsChanged = DateTime.UtcNow;

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
    public async Task<ActionResult<TestCase>> PostTestCaseAsync(TestCaseRequestDTO dto)
    {
        if (dto.ProblemId is null)
            return BadRequest("ProblemId is required");

        TestCase testCase = new()
        {
            Id = dto.Id,
            ProblemId = dto.ProblemId!.Value,
            Input = dto.Input ?? string.Empty,
            Output = dto.Output ?? string.Empty,
            Description = dto.Description ?? string.Empty,
            Sample = dto.Sample ?? false,
            Active = dto.Active ?? true,
            Weight = dto.Weight ?? 100
        };

        // add testcase to problem
        var problem = await context.Problems.FindAsync(testCase.ProblemId);
        if (problem is null)
            return NotFound();

        context.TestCases.Add(testCase);

        testCase.Problem = problem;

        await context.SaveChangesAsync();

        TestCaseResponseDTO response = new()
        {
            Id = testCase.Id,
            ProblemId = testCase.ProblemId,
            ProblemName = problem.Name,
            Input = testCase.Input,
            Output = testCase.Output,
            Description = testCase.Description,
            Sample = testCase.Sample,
            Active = testCase.Active,
            Weight = testCase.Weight
        };

        return CreatedAtAction(nameof(GetTestCaseAsync), new { id = testCase.Id }, response);
    }

    // DELETE: api/TestCases/5
    /// <summary>
    /// Deletes a test case by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTestCaseAsync(Guid id)
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
