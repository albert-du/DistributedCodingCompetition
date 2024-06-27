namespace DistributedCodingCompetition.ApiService.Models;

public class Problem
{
    public Guid Id { get; set; }

    /// <summary>
    /// Short name of the problem.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    /// <summary>
    /// Markdown formatted description of the problem.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Difficulty of the problem.
    /// </summary>
    public string? Difficulty { get; set; }

    public ICollection<TestCase> TestCases { get; set; } = [];
}
