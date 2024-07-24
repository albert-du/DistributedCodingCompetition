namespace DistributedCodingCompetition.CodeExecution.Models;

using MongoDB.Bson.Serialization.Attributes;

/// <summary>
/// Data Model for exec runner
/// </summary>
public class ExecRunner
{
    /// <summary>
    /// Id of the Exec Runner
    /// </summary>
    [BsonId]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Given name of the ExecRunner
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Endpoint url for the ExecRunner
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// API Key for this Execution Runner
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Whether the execution runner is enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// The load balancing weight.
    /// </summary>
    public int Weight { get; set; } = 100;
}
