namespace DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// Role of the user in the contest
/// </summary>
public enum ContestRole
{
    /// <summary>
    /// Admin of the contest
    /// </summary>
    Admin,

    /// <summary>
    /// Participant of the contest
    /// </summary>
    Participant,

    /// <summary>
    /// No role in the contest
    /// </summary>
    None,

    /// <summary>
    /// The owner of the contest
    /// </summary>
    Owner
}
