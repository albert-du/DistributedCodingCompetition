namespace DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// DTO for a test case response.
/// </summary>
public sealed record TestCaseResponseDTO
{
    /// <summary>
    /// The id of the test case.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// The id of the problem.
    /// </summary>
    public required Guid ProblemId { get; init; }

    /// <summary>
    /// The name of the problem.
    /// </summary>
    public required string ProblemName { get; init; }

    /// <summary>
    /// The input of the test case.
    /// </summary>
    public required string Input { get; init; }

    /// <summary>
    /// The output of the test case.
    /// </summary>
    public required string Output { get; init; }

    /// <summary>
    /// The description of the test case.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// The sample status of the test case.
    /// </summary>
    public required bool Sample { get; init; }

    /// <summary>
    /// The active status of the test case.
    /// </summary>
    public required bool Active { get; init; }

    /// <summary>
    /// The weight of the test case.
    /// </summary>
    public required int Weight { get; init; }
}