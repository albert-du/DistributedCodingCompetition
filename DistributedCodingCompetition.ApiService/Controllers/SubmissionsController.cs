﻿namespace DistributedCodingCompetition.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistributedCodingCompetition.ApiService.Models;
using NuGet.Packaging;

[Route("api/[controller]")]
[ApiController]
public class SubmissionsController(ContestContext context) : ControllerBase
{
    // GET: api/Submissions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Submission>>> GetSubmissions() =>
        await context.Submissions.ToListAsync();

    // GET: api/Submissions/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Submission>> GetSubmission(Guid id)
    {
        var submission = await context.Submissions.FindAsync(id);
        return submission == null ? NotFound() : submission;
    }

    // PUT: api/Submissions/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Submission>> PostSubmission(Submission submission)
    {
        context.Submissions.Add(submission);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetSubmission", new { id = submission.Id }, submission);
    }

    // api/submissions/{submissionId}/results
    [HttpPost("{submissionId}/results")]
    public async Task<IActionResult> PostResults(Guid submissionId, IReadOnlyList<TestCaseResult> results)
    {
        var submission = await context.Submissions.FindAsync(submissionId);
        if (submission == null)
            return NotFound();

        // clean out outdated results
        context.TestCaseResults.RemoveRange(context.TestCaseResults.Where(x => x.SubmissionId == submissionId));

        // add new results
        submission.Results.AddRange(results);

        await context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Submissions/5
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
