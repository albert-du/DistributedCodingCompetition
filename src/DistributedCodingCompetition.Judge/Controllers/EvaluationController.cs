namespace DistributedCodingCompetition.Judge.Controllers;

using Microsoft.AspNetCore.Mvc;
using DistributedCodingCompetition.ExecutionShared;
using DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// Controller for evaluating submissions
/// </summary>
/// <param name="logger"></param>
/// <param name="codeExecutionService"></param>
/// <param name="rateLimitService"></param>
/// <param name="submissionService"></param>
/// <param name="problemService"></param>
/// <param name="contestsService"></param>
/// <param name="liveReportingService"></param>
[ApiController]
[Route("[controller]")]
public class EvaluationController(ILogger<EvaluationController> logger,
                                  ICodeExecutionService codeExecutionService,
                                  IRateLimitService rateLimitService,
                                  ISubmissionsService submissionService,
                                  IProblemsService problemService,
                                  ILiveReportingService liveReportingService,
                                  IContestsService contestsService) : ControllerBase
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
        var (success, submission) = await submissionService.TryReadSubmissionAsync(submissionId);
        if (!success || submission is null)
            return NotFound("submission not found");

        // if we can't obtain the lock, return immediately.
        await using var execLock = await rateLimitService.TryLockAsync(submission.UserId, TimeSpan.FromSeconds(3));

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
        PaginateResult<SubmissionResponseDTO>? submissions;
        do
        {
            // Get all submissions for the problem
            (var success, submissions) = await problemService.TryReadProblemSubmissionsAsync(problemId, 1, 100);

            if (!success || submissions is null)
                return NotFound("problem not found");

            List<Task> tasks = [];

            // rejudge each submission
            foreach (var submission in submissions.Items)
                tasks.Add(JudgeAsync(submission));

            await Task.WhenAll(tasks);
        }
        while (submissions?.Items.Count == submissions?.PageSize);

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
        var (success, submission) = await submissionService.TryReadSubmissionAsync(submissionId);
        if (!success || submission is null)
            return NotFound("submission not found");

        await JudgeAsync(submission);

        return Ok();
    }

    /// <summary>
    /// Judges a problem submission
    /// </summary>
    /// <param name="submission"></param>
    /// <returns></returns>
    private async Task JudgeAsync(SubmissionResponseDTO submission)
    {
        // log the start of the evaluation
        logger.LogInformation("Evaluating submission {SubmissionId} from {UserId} for problem {ProblemId}", submission.Id, submission.UserId, submission.ProblemId);

        // record the start time of the evaluation
        var start = DateTime.UtcNow;

        // Get the problem from the database
        // If there are more than 10k test cases that's a different problem
        var (success, testCases) = await problemService.TryReadProblemTestCasesAsync(submission.ProblemId, 1, 10_000);

        if (!success || testCases is null)
        {
            logger.LogError("Problem {ProblemId} not found", submission.ProblemId);
            return;
        }
        // Execute the submission
        var execResults = await codeExecutionService.TryExecuteBatchAsync(testCases.Items.Select(testcase =>
            new ExecutionRequest
            {
                Code = submission.Code,
                Language = submission.Language,
                Input = testcase.Input
            }));

        // start recording the scores.
        var possibleScore = 0;
        var score = 0;
        Console.WriteLine("TestCases: " + testCases.Items.Count);
        Console.WriteLine("ExecResults: " + execResults?.Count);
        if (execResults?.Count != testCases.Items.Count)
        {
            logger.LogError("Submission {SubmissionId} from {UserId} for problem {ProblemId} did not return the expected number of results", submission.Id, submission.UserId, submission.ProblemId);
            return;
        }

        // setup results.
        var results = new TestCaseResultDTO[execResults.Count];

        for (var i = 0; i < execResults.Count; i++)
        {
            // pass is defied as matching code and exit code of 0
            var passed = execResults[i].ExitCode == 0 && CodeOutputChecker.CheckOutput(testCases.Items[i].Output, execResults[i].Output);

            // increment the score if the test case passed, increment the possible score regardless.
            var w = testCases.Items[i].Weight;
            possibleScore += w;
            if (passed)
                score += w;

            // record the result
            results[i] = new TestCaseResultDTO
            {
                TestCaseId = testCases.Items[i].Id,
                SubmissionId = submission.Id,
                Output = execResults[i].Output,
                Passed = passed,
                Error = execResults[i].Error,
                ExecutionTime = (int)execResults[i].ExecutionTime.TotalMilliseconds,
            };
        }

        // Save the result
        await submissionService.TryUpdateSubmissionResultsAsync(submission.Id, results, possible: possibleScore, score: score);

        // report the results to the live reporting service
        (success, var ppv) = await contestsService.TryReadContestProblemPointValueAsync(submission.ContestId!.Value, submission.ProblemId, true);

        if (!success || ppv is null)
        {
            logger.LogError("Points for Problem {ProblemId} not found", submission.ProblemId);
            return;
        }
        var max = ppv.Points;

        await liveReportingService.ReportAsync(submission.ContestId.Value, submission.UserId, max * score / possibleScore);

        // log the time taken to evaluate the submission
        var end = DateTime.UtcNow;
        logger.LogInformation("Submission {SubmissionId} from {UserId} for problem {ProblemId} evaluated in {Time}ms", submission.Id, submission.UserId, submission.ProblemId, (end - start).TotalMilliseconds);
    }
}
