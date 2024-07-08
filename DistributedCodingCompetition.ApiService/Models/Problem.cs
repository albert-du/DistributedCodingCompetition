namespace DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// Problem to be solved.
/// </summary>
public class Problem
{
    /// <summary>
    /// Id of the problem.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Short name of the problem.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Id of the owner of the problem.
    /// </summary>
    public Guid OwnerId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Navigation Property for the owner of the problem.
    /// </summary>
    public User? Owner { get; set; }

    /// <summary>
    /// Markdown formatted description of the problem.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// HTML rendered description of the problem.
    /// </summary>
    public string RenderedDescription { get; set; } = string.Empty;

    /// <summary>
    /// Difficulty of the problem.
    /// </summary>
    public string? Difficulty { get; set; }

    /// <summary>
    /// Navigation to the test cases.
    /// </summary>
    public ICollection<TestCase> TestCases { get; set; } = [];
}
