namespace DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// DTO for a test case request.
/// </summary>
public sealed record TestCaseRequestDTO
{
    /// <summary>
    /// The id of the test case.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// The id of the problem.
    /// </summary>
    public Guid? ProblemId { get; init; }

    /// <summary>
    /// The input for the test case.
    /// </summary>
    public string? Input { get; init; }

    /// <summary>
    /// The output for the test case.
    /// </summary>
    public string? Output { get; init; }

    /// <summary>
    /// The description of the test case.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// The sample status of the test case.
    /// </summary>
    public bool? Sample { get; init; }

    /// <summary>
    /// The active status of the test case.
    /// </summary>
    public bool? Active { get; init; }

    /// <summary>
    /// The weight of the test case.
    /// </summary>
    public int? Weight { get; init; }
}