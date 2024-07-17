namespace DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// DTO for a user request.
/// </summary>
public sealed record UserRequestDTO
{
    /// <summary>
    /// The id of the user.
    /// Required for updates.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// The username of the user.
    /// </summary>
    public string? Username { get; init; }

    /// <summary>
    /// The email of the user.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// The full name of the user.
    /// </summary>
    public string? FullName { get; init; }
}