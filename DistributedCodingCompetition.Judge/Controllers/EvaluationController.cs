namespace DistributedCodingCompetition.Judge.Controllers;

using Microsoft.AspNetCore.Mvc;
using DistributedCodingCompetition.ApiService.Models;
using DistributedCodingCompetition.ExecutionShared;
using DistributedCodingCompetition.Judge.Services;

/// <summary>
/// Controller for evaluating submissions
/// </summary>
/// <param name="logger"></param>
/// <param name="codeExecutionService"></param>
/// <param name="rateLimitService"></param>
/// <param name="submissionService"></param>
/// <param name="problemService"></param>
[ApiController]
[Route("[controller]")]
public class EvaluationController(ILogger<EvaluationController> logger,
                                  ICodeExecutionService codeExecutionService,
                                  IRateLimitService rateLimitService,
                                  ISubmissionService submissionService,
                                  IProblemService problemService) : ControllerBase
{
    /// <summary>
    /// Evaluate a submission
    /// </summary>
    /// <param name="submissionId"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> PostAsync(Guid submissionId)
    {
        // Get the submission from the database
        var submission = await submissionService.ReadSubmissionAsync(submissionId);
        if (submission is null)
            return NotFound("submission not found");

        // if we can't obtain the lock, return immediately.
        await using var execLock = await rateLimitService.TryLockAsync(submission.SubmitterId, TimeSpan.FromSeconds(3));

        if (execLock is null)
            return StatusCode(429);

        // log the start of the evaluation
        logger.LogInformation("Evaluating submission {SubmissionId} from {UserId} for problem {ProblemId}", submissionId, submission.SubmitterId, submission.ProblemId);

        // record the start time of the evaluation
        var start = DateTime.UtcNow;

        // Get the problem from the database
        var testCases = await problemService.ReadTestCasesAsync(submission.ProblemId);

        // unlikely scenario where the problem is not found, likely deleted
        if (testCases is null)
            return NotFound("problem not found");

        // Execute the submission
        var execResults = await codeExecutionService.ExecuteBatchAsync(testCases.Select(testcase =>
            new ExecutionRequest
            {
                Code = submission.Code,
                Language = submission.Language,
                Input = testcase.Input
            }));

        // start recording the scores.
        var possibleScore = 0;
        var score = 0;

        // setup results.
        var results = new TestCaseResult[execResults.Count];

        for (var i = 0; i < execResults.Count; i++)
        {
            // pass is defied as matching code and exit code of 0
            var passed = execResults[i].ExitCode == 0 && CodeOutputChecker.CheckOutput(testCases[i].Output, execResults[i].Output);

            // increment the score if the test case passed, increment the possible score regardless.
            var w = testCases[i].Weight;
            possibleScore += w;
            if (passed)
                score += w;

            // record the result
            results[i] = new TestCaseResult
            {
                Id = Guid.NewGuid(),
                TestCaseId = testCases[i].Id,
                SubmissionId = submissionId,
                Output = execResults[i].Output,
                Passed = passed,
                Error = execResults[i].Error,
                ExecutionTime = (int)execResults[i].ExecutionTime.TotalMilliseconds,
            };
        }

        // Save the result
        await submissionService.UpdateSubmissionResults(submissionId, results, maxScore: possibleScore, score: score);

        // log the time taken to evaluate the submission
        var end = DateTime.UtcNow;
        logger.LogInformation("Submission {SubmissionId} from {UserId} for problem {ProblemId} evaluated in {Time}ms", submissionId, submission.SubmitterId, submission.ProblemId, (end - start).TotalMilliseconds);

        return Ok();
    }
}
