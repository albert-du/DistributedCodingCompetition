using DistributedCodingCompetition.Judge.Services;
namespace DistributedCodingCompetition.Judge.Controllers;

using DistributedCodingCompetition.ApiService.Models;
using DistributedCodingCompetition.ExecutionShared;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class EvaluationController(ILogger<EvaluationController> logger,
                                  ICodeExecutionService codeExecutionService,
                                  IRateLimitService rateLimitService,
                                  ISubmissionService submissionService,
                                  IProblemService problemService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PostAsync(Guid submissionId)
    {
        // Get the submission from the database
        var submission = await submissionService.ReadSubmissionAsync(submissionId);
        if (submission is null)
            return NotFound("submission not found");

        if (!await rateLimitService.TryLockAsync(submission.SubmitterId, TimeSpan.FromSeconds(3)))
            return StatusCode(429);

        // Get the problem from the database
        var testCases = await problemService.ReadTestCasesAsync(submission.ProblemId);

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

        var results = new TestCaseResult[execResults.Count];

        for (var i = 0; i < execResults.Count; i++)
        {
            results[i] = new TestCaseResult
            {
                Id = Guid.NewGuid(),
                TestCaseId = testCases[i].Id,
                SubmissionId = submissionId,
                Output = execResults[i].Output,
                Passed = execResults[i].ExitCode == 0 && CodeOutputChecker.CheckOutput(testCases[i].Output, execResults[i].Output),
                Error = execResults[i].Error,
                ExecutionTime = (int)execResults[i].ExecutionTime.TotalMilliseconds,
            };
        }

        // Save the result
        await submissionService.UpdateSubmissionResults(submissionId, results);

        return Ok();
    }
}
