namespace DistributedCodingCompetition.ApiService.Models;

public class Submission
{
    public Guid Id { get; set; }
    public Guid SubmitterId { get; set; }
    public User? Submitter { get; set; }
    public Guid? ContestId { get; set; }
    public Guid ProblemId { get; set; }
    public Problem? Problem { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public DateTime SubmissionTime { get; set; }
    public ICollection<TestCaseResult> Results { get; set; } = [];
    public int Score { get; set; }
    public int MaxPossibleScore { get; set; }
    public int Points { get; set; }
    public DateTime EvaluationTime { get; set; }
}
