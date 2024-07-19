namespace DistributedCodingCompetition.ApiService.Data.Models;

/// <summary>
/// Result of a test case
/// </summary>
public class TestCaseResult
{
    /// <summary>
    /// Id of the test case result
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Id of the test case
    /// </summary>
    public Guid TestCaseId { get; set; }

    /// <summary>
    /// Navigation property for the test case
    /// </summary>
    public TestCase TestCase { get; set; } = null!;

    /// <summary>
    /// Id of the submission
    /// </summary>
    public Guid SubmissionId { get; set; }

    /// <summary>
    /// Navigation property for the submission
    /// </summary>
    public Submission? Submission { get; set; }

    /// <summary>
    /// Input for the test case
    /// </summary>
    public string Output { get; set; } = string.Empty;

    /// <summary>
    /// Output for the test case
    /// </summary>
    public bool Passed { get; set; }

    /// <summary>
    /// Error message if the test case failed
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// Execution time in milliseconds
    /// </summary>
    public int ExecutionTime { get; set; }
}
