namespace DistributedCodingCompetition.ApiService.Controllers;

/// <summary>
/// Api controller for Problems
/// </summary>
/// <param name="context"></param>
[Route("api/[controller]")]
[ApiController]
public sealed class ProblemsController(ContestContext context) : ControllerBase
{
    // GET: api/Problems
    /// <summary>
    /// Gets all problems
    /// </summary>
    /// <param name="page">page number starting at 1</param>
    /// <param name="count">count of problems</param>
    /// <returns></returns>
    [HttpGet]
    public Task<PaginateResult<ProblemResponseDTO>> GetProblemsAsync(int page, int count) =>
        context.Problems
            .AsNoTracking()
            .PaginateAsync(page, count, q => q.ReadProblemsAsync());

    // GET: api/Problems/5
    /// <summary>
    /// Gets a problem by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProblemResponseDTO>> GetProblemAsync(Guid id)
    {
        var problems = await context.Problems.AsNoTracking().Where(p => p.Id == id).ReadProblemsAsync();
        return problems.Count == 0 ? NotFound() : problems[0];
    }

    /// <summary>
    /// Gets all submissions for a problem
    /// </summary>
    /// <param name="id"></param>
    /// <param name="page"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    [HttpGet("{id}/submissions")]
    public Task<PaginateResult<SubmissionResponseDTO>> GetSubmissionsForProblemAsync(Guid id, int page, int count) =>
        context.Submissions
            .AsNoTracking()
            .Where(s => s.ProblemId == id)
            .PaginateAsync(page, count, q => q.ReadSubmissionsAsync());

    // PUT: api/Problems/5
    /// <summary>
    /// Updates a problem
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProblemAsync(Guid id, ProblemRequestDTO dto)
    {
        if (id != dto.Id)
            return BadRequest();

        // find the problem
        var problem = await context.Problems.FindAsync(id);

        if (problem is null)
            return NotFound();

        // update the problem

        problem.Name = dto.Name ?? problem.Name;
        problem.OwnerId = dto.OwnerId ?? problem.OwnerId;
        problem.TagLine = dto.TagLine ?? problem.TagLine;
        problem.Description = dto.Description ?? problem.Description;
        problem.RenderedDescription = dto.RenderedDescription ?? problem.RenderedDescription;
        problem.Difficulty = dto.Difficulty ?? problem.Difficulty;

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
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Problem>> PostProblemAsync(ProblemRequestDTO dto)
    {
        if (!dto.OwnerId.HasValue)
            return BadRequest("Owner Id is required");

        Problem problem = new()
        {
            Id = dto.Id,
            Name = dto.Name ?? "New Problem",
            OwnerId = dto.OwnerId.Value,
            TagLine = dto.TagLine ?? "New Problem",
            Description = dto.Description ?? "New Problem",
            RenderedDescription = dto.RenderedDescription ?? "New Problem",
            Difficulty = dto.Difficulty
        };

        context.Problems.Add(problem);
        await context.SaveChangesAsync();

        // read it back

        var dtos = await context.Problems.AsNoTracking().Where(p => p.Id == problem.Id).Take(1).ReadProblemsAsync();

        return Created(problem.Id.ToString(), dtos[0]);
    }

    // DELETE: api/Problems/5
    /// <summary>
    /// Deletes a problem by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProblemAsync(Guid id)
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
    public Task<PaginateResult<TestCaseResponseDTO>> GetTestCasesForProblemAsync(Guid problemId) =>
        context.Problems
            .AsNoTracking()
            .Where(p => p.Id == problemId)
            .SelectMany(p => p.TestCases)
            .PaginateAsync(1, 50, q => q.ReadTestCasesAsync());

    // POST: api/problems/{problemId}/testcases
    /// <summary>
    /// Adds a test case to a problem
    /// </summary>
    /// <param name="problemId"></param>
    /// <param name="testCaseId"></param>
    /// <returns></returns>
    [HttpPost("{problemId}/testcases/{testCaseId}")]
    public async Task<IActionResult> AddTestCaseToProblemAsync(Guid problemId, Guid testCaseId)
    {
        var problem = await context.Problems.FindAsync(problemId);
        if (problem is null)
            return NotFound();

        var testCase = await context.TestCases.FindAsync(testCaseId);

        if (testCase is null)
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
