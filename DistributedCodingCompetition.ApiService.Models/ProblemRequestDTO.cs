namespace DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// DTO for a problem request.
/// </summary>
public sealed record ProblemRequestDTO
{
    /// <summary>
    /// The id of the problem.
    /// Required for updates.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();
    
    /// <summary>
    /// The name of the problem.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// The id of the owner of the problem.
    /// Required for creation.
    /// </summary>
    public Guid? OwnerId { get; init; }
 
    /// <summary>
    /// The tag line of the problem.
    /// </summary>
    public string? TagLine { get; init; }

    /// <summary>
    /// The description of the problem.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// The rendered description of the problem.
    /// </summary>
    public string? RenderedDescription { get; init; }

    /// <summary>
    /// The input description of the problem.
    /// </summary>
    public string? Difficulty { get; init; }
}