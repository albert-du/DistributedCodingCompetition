namespace DistributedCodingCompetition.Models;

/// <summary>
/// Point value for a problem in a contest
/// </summary>
public class ProblemPointValue
{
    /// <summary>
    /// Id of the point value
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Id of the problem
    /// </summary>
    public Guid ProblemId { get; set; }

    /// <summary>
    /// Id of the contest
    /// </summary>
    public Guid ContestId { get; set; }

    /// <summary>
    /// Points for the problem
    /// </summary>
    public int Points { get; set; } = 100;
}
