namespace DistributedCodingCompetition.ApiService.Models;

public class Submission
{
    public Guid Id { get; set; }
    public User Submitter { get; set; } = default!;
    public Guid ProblemId { get; set; }
    public Problem Problem { get; set; } = default!;
    public string Code { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public DateTime SubmissionTime { get; set; }
    public ICollection<TestCaseResult> Results { get; set; } = [];
}
