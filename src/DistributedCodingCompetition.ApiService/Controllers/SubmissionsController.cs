﻿namespace DistributedCodingCompetition.ApiService.Controllers;

/// <summary>
/// Api controller for Submissions
/// </summary>
/// <param name="context"></param>
[Route("api/[controller]")]
[ApiController]
public sealed class SubmissionsController(ContestContext context) : ControllerBase
{
    // GET: api/Submissions
    /// <summary>
    /// Gets all submissions
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<PaginateResult<SubmissionResponseDTO>> GetSubmissionsAsync(int page = 1, int count = 50, Guid? contestId = null, Guid? problemId = null, Guid? userId = null)
    {
        IQueryable<Submission> query = context.Submissions.AsNoTracking();
        if (contestId != null)
            query = query.Where(x => x.ContestId == contestId);

        if (problemId != null)
            query = query.Where(x => x.ProblemId == problemId);

        if (userId != null)
            query = query.Where(x => x.SubmitterId == userId);

        return await query.OrderByDescending(x => x.EvaluationTime)
                          .PaginateAsync(page, count, x => x.ReadSubmissionsAsync());
    }

    // GET: api/Submissions/5
    /// <summary>
    /// Gets a submission by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<SubmissionResponseDTO>> GetSubmissionAsync(Guid id)
    {
        var submissions = await context.Submissions.AsNoTracking().Where(s => s.Id == id).ReadSubmissionsAsync();
        return submissions.Count == 0 ? NotFound() : submissions[0];
    }

    /// <summary>
    /// Validates a submission
    /// </summary>
    /// <param name="id"></param>
    /// <param name="valid"></param>
    /// <returns></returns>
    [HttpPost("{id}/validate")]
    public async Task<IActionResult> ValidateSubmissionAsync(Guid id, bool valid)
    {
        var submission = await context.Submissions.FindAsync(id);
        if (submission == null)
            return NotFound();

        submission.Invalidated = !valid;

        await context.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/Submissions
    /// <summary>
    /// Creates a submission
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Submission>> PostSubmissionAsync(SubmissionRequestDTO dto)
    {
        // make sure a contest exists with the problem and user
        var exists = await context.Contests.AsNoTracking().Where(c => c.Participants.Any(p => p.Id == dto.UserId) && c.Problems.Any(p => p.Id == dto.ProblemId)).AnyAsync();

        if (!exists)
            return BadRequest("Contest, problem, or user does not exist");

        Submission submission = new()
        {
            Id = dto.Id,
            ContestId = dto.ContestId,
            ProblemId = dto.ProblemId,
            SubmitterId = dto.UserId,
            Code = dto.Code,
            Language = dto.Language,
            SubmissionTime = DateTime.UtcNow
        };

        context.Submissions.Add(submission);
        await context.SaveChangesAsync();

        var responses = await context.Submissions
            .AsNoTracking()
            .Where(x => x.Id == submission.Id)
            .ReadSubmissionsAsync();

        return Created(submission.Id.ToString(), responses[0]);
    }

    /// <summary>
    /// Reads the results of a submission
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/results")]
    public async Task<IReadOnlyList<TestCaseResultDTO>> GetResultsAsync(Guid id) =>
        await context.TestCaseResults
            .AsNoTracking()
            .Where(x => x.SubmissionId == id)
            .ReadTestCaseResultsAsync();

    // api/submissions/{submissionId}/results
    /// <summary>
    /// Posts the results of a submission
    /// </summary>
    /// <param name="submissionId"></param>
    /// <param name="possible"></param>
    /// <param name="score"></param>
    /// <param name="dtos"></param>
    /// <returns></returns>
    [HttpPost("{submissionId}/results")]
    public async Task<IActionResult> PostResultsAsync(Guid submissionId, int possible, int score, [FromBody] IReadOnlyList<TestCaseResultDTO> dtos)
    {
        var submission = await context.Submissions.FindAsync(submissionId);
        if (submission == null)
            return NotFound();

        var results = dtos.Select(x => new TestCaseResult
        {
            SubmissionId = submissionId,
            TestCaseId = x.TestCaseId,
            Passed = x.Passed,
            Output = x.Output,
            Error = x.Error,
            ExecutionTime = x.ExecutionTime
        });

        // clean out outdated results
        context.TestCaseResults.RemoveRange(context.TestCaseResults.Where(x => x.SubmissionId == submissionId));

        // add new results
        await context.AddRangeAsync(results);

        // find if there is a point value for this problem and contest
        var problem = await context.ProblemPointValues.Where(x => x.ProblemId == submission.ProblemId && x.ContestId == submission.ContestId).FirstOrDefaultAsync();
        var maxPoints = problem?.Points;

        if (maxPoints == null)
        {
            var contest = await context.Contests.FindAsync(submission.ContestId);
            if (contest == null)
                return NotFound();
            maxPoints = contest.DefaultPointsForProblem;
        }

        var points = (int)((double)maxPoints * score / possible);

        submission.Points = points;
        submission.MaxPossibleScore = possible;
        submission.Score = score;
        submission.TotalTestCases = dtos.Count;
        submission.PassedTestCases = dtos.Count(x => x.Passed);
        submission.EvaluationTime = DateTime.UtcNow;

        await context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Submissions/5
    /// <summary>
    /// Deletes a submission by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubmissionAsync(Guid id)
    {
        var submission = await context.Submissions.FindAsync(id);
        if (submission == null)
            return NotFound();

        context.Submissions.Remove(submission);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
