namespace DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// DTO for a problem response.
/// </summary>
public sealed record ProblemResponseDTO
{
    /// <summary>
    /// The id of the problem.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// The name of the problem.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The id of the owner of the problem.
    /// </summary>
    public required Guid OwnerId { get; init; }

    /// <summary>
    /// The name of the owner of the problem.
    /// </summary>
    public required string OwnerName { get; init; }

    /// <summary>
    /// The tag line of the problem.
    /// </summary>
    public required string TagLine { get; init; }

    /// <summary>
    /// The description of the problem.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// The rendered description of the problem.
    /// </summary>
    public required string RenderedDescription { get; init; }

    /// <summary>
    /// The input description of the problem.
    /// </summary>
    public required string? Difficulty { get; init; }

    /// <summary>
    /// The number of test cases for the problem.
    /// </summary>
    public required int TestCaseCount { get; init; }
}