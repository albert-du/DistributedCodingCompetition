namespace DistributedCodingCompetition.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// Api controller for Submissions
/// </summary>
/// <param name="context"></param>
[Route("api/[controller]")]
[ApiController]
public class SubmissionsController(ContestContext context) : ControllerBase
{
    // GET: api/Submissions
    /// <summary>
    /// Gets all submissions
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Submission>>> GetSubmissions() =>
        await context.Submissions.ToListAsync();

    // GET: api/Submissions/5
    /// <summary>
    /// Gets a submission by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Submission>> GetSubmission(Guid id)
    {
        var submission = await context.Submissions.FindAsync(id);
        return submission == null ? NotFound() : submission;
    }

    // PUT: api/Submissions/5
    /// <summary>
    /// Updates a submission
    /// </summary>
    /// <param name="id"></param>
    /// <param name="submission"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSubmission(Guid id, Submission submission)
    {
        if (id != submission.Id)
            return BadRequest();

        context.Entry(submission).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SubmissionExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Submissions
    /// <summary>
    /// Creates a submission
    /// </summary>
    /// <param name="submission"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Submission>> PostSubmission(Submission submission)
    {
        context.Submissions.Add(submission);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetSubmission", new { id = submission.Id }, submission);
    }

    // api/submissions/{submissionId}/results
    /// <summary>
    /// Posts the results of a submission
    /// </summary>
    /// <param name="submissionId"></param>
    /// <param name="possible"></param>
    /// <param name="score"></param>
    /// <param name="results"></param>
    /// <returns></returns>
    [HttpPost("{submissionId}/results")]
    public async Task<IActionResult> PostResults(Guid submissionId, int possible, int score, [FromBody] IReadOnlyList<TestCaseResult> results)
    {
        var submission = await context.Submissions.FindAsync(submissionId);
        if (submission == null)
            return NotFound();

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
    public async Task<IActionResult> DeleteSubmission(Guid id)
    {
        var submission = await context.Submissions.FindAsync(id);
        if (submission == null)
            return NotFound();

        context.Submissions.Remove(submission);
        await context.SaveChangesAsync();

        return NoContent();
    }
    private bool SubmissionExists(Guid id) =>
        context.Submissions.Any(e => e.Id == id);
}
