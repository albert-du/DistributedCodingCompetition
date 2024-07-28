namespace DistributedCodingCompetition.CodeExecution.Models;

/// <summary>
/// Interface for weighted objects.
/// </summary>
public interface IWeighted
{
    /// <summary>
    /// Weight of the object.
    /// </summary>
    int Weight { get; }
}
