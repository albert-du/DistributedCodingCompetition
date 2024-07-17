namespace DistributedCodingCompetition.Judge.Controllers;

using Microsoft.AspNetCore.Mvc;
using DistributedCodingCompetition.ExecutionShared;
using DistributedCodingCompetition.Judge.Services;
using Microsoft.EntityFrameworkCore;

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
                                  ISubmissionsService submissionService,
                                  IProblemsService problemService,
                                  ILiveReportingService liveReportingService,
                                  IContestsService problemPointValueService) : ControllerBase
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

        await JudgeAsync(submission);

        return Ok();
    }

    /// <summary>
    /// Reevaluates all existing submissions for a problem, does not rate limit against user.
    /// </summary>
    /// <param name="problemId"></param>
    /// <returns></returns>
    [HttpPost("problem")]
    public async Task<IActionResult> RejudgeProblemAsync(Guid problemId)
    {
        // Get all submissions for the problem
        var submissions = submissionService.ReadSubmissionsAsync(problemId);

        List<Task> tasks = [];

        // rejudge each submission
        await foreach (var submission in submissions)
            tasks.Add(JudgeAsync(submission));

        await Task.WhenAll(tasks);

        return Ok();
    }

    /// <summary>
    /// Reevaluates a submission, does not rate limit against user.
    /// </summary>
    /// <param name="submissionId"></param>
    /// <returns></returns>
    [HttpPost("rejudge")]
    public async Task<IActionResult> RejudgeAsync(Guid submissionId)
    {
        // Get the submission from the database
        var submission = await submissionService.ReadSubmissionAsync(submissionId);
        if (submission is null)
            return NotFound("submission not found");

        await JudgeAsync(submission);

        return Ok();
    }

    /// <summary>
    /// Judges a problem submission
    /// </summary>
    /// <param name="submission"></param>
    /// <returns></returns>
    private async Task JudgeAsync(Submission submission)
    {
        // log the start of the evaluation
        logger.LogInformation("Evaluating submission {SubmissionId} from {UserId} for problem {ProblemId}", submission.Id, submission.SubmitterId, submission.ProblemId);

        // record the start time of the evaluation
        var start = DateTime.UtcNow;

        // Get the problem from the database
        var testCases = await problemService.ReadTestCasesAsync(submission.ProblemId);

        // shouldn't happen but 
        testCases ??= [];

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
                SubmissionId = submission.Id,
                Output = execResults[i].Output,
                Passed = passed,
                Error = execResults[i].Error,
                ExecutionTime = (int)execResults[i].ExecutionTime.TotalMilliseconds,
            };
        }

        // Save the result
        await submissionService.UpdateSubmissionResults(submission.Id, results, maxScore: possibleScore, score: score);

        // report the results to the live reporting service

        var max = await problemPointValueService.GetPointMaxAsync(submission.ContestId!.Value, submission.ProblemId);

        await liveReportingService.ReportAsync(submission.ContestId!.Value, submission.SubmitterId, max * score / possibleScore);

        // log the time taken to evaluate the submission
        var end = DateTime.UtcNow;
        logger.LogInformation("Submission {SubmissionId} from {UserId} for problem {ProblemId} evaluated in {Time}ms", submission.Id, submission.SubmitterId, submission.ProblemId, (end - start).TotalMilliseconds);
    }
}
