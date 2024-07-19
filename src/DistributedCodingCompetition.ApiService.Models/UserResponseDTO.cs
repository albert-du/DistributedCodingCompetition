namespace DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// DTO for a user response.
/// </summary>
public sealed record UserResponseDTO
{
    /// <summary>
    /// The id of the user.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// The username of the user.
    /// </summary>
    public required string Username { get; init; }

    /// <summary>
    /// The email of the user.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// The full name of the user.
    /// </summary>
    public required string FullName { get; init; }

    /// <summary>
    /// The created at date of the user.
    /// </summary>
    public required DateTime CreatedAt { get; init; }

    /// <summary>
    /// The banned status of the user.
    /// </summary>
    public required bool Banned { get; init; }

    /// <summary>
    /// The number of owned contests.
    /// </summary>
    public required int OwnedContests { get; init; }

    /// <summary>
    /// The number of participated contests.
    /// </summary>
    public required int ParticipatedContests { get; init; }

    /// <summary>
    /// The number of banned contests.
    /// </summary>
    public required int BannedContests { get; init; }

    /// <summary>
    /// The number of administered contests.
    /// </summary>
    public required int AdministeredContests { get; init; }

    /// <summary>
    /// The birthday of the user.
    /// </summary>
    public required DateTime Birthday { get; init; }
}