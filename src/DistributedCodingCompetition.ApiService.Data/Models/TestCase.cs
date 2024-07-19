namespace DistributedCodingCompetition.ApiService.Data.Models;

/// <summary>
/// Test case for a problem
/// </summary>
public class TestCase
{
    /// <summary>
    /// Id of the test case
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Problem Id
    /// </summary>
    public Guid ProblemId { get; set; }

    /// <summary>
    /// Navigation Property for the problem
    /// </summary> 
    public Problem Problem { get; set; } = null!;

    /// <summary>
    /// Input for the test case
    /// </summary>
    public string Input { get; set; } = string.Empty;

    /// <summary>
    /// Expected output for the test case
    /// </summary>
    public string Output { get; set; } = string.Empty;

    /// <summary>
    /// Description of the test case
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// If the test case is a sample
    /// </summary>
    public bool Sample { get; set; }

    /// <summary>
    /// If the test case is active
    /// </summary>
    public bool Active { get; set; } = true;

    /// <summary>
    /// Weight of the test case
    /// </summary>
    public int Weight { get; set; } = 100;
}
