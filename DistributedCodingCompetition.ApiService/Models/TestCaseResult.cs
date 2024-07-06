namespace DistributedCodingCompetition.ApiService.Models;

public class TestCaseResult
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TestCaseId { get; set; }
    public TestCase? TestCase { get; set; }
    public Guid SubmissionId { get; set; }
    public Submission Submission { get; set; } = null!;
    public string Output { get; set; } = string.Empty;
    public bool Passed { get; set; }
    public string? Error { get; set; }
    public int ExecutionTime { get; set; }
}
