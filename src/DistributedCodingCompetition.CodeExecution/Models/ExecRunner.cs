namespace DistributedCodingCompetition.CodeExecution.Models;

/// <summary>
/// Data Model for exec runner
/// </summary>
public class ExecRunner
{
    /// <summary>
    /// Id of the Exec Runner
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Endpoint url for the ExecRunner
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// Reported Version of the Execution Runner
    /// </summary>
    public string Version { get; set; } = "Unknown";

    /// <summary>
    /// API Key for this Execution Runner
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Reported Name of this execution runner
    /// </summary>
    public string Name { get; set; } = "Unknown";

    /// <summary>
    /// available lanaugages of the exec runner
    /// </summary>
    public List<string> Languages { get; set; } = [];

    /// <summary>
    /// available packages of the exec runner
    /// </summary>
    public List<string> Packages { get; set; } = [];

    /// <summary>
    /// Reported SystemInfo of the execrunner
    /// </summary>
    public string SystemInfo { get; set; } = "Unknown";

    /// <summary>
    /// Reported Status of the Execution Runner
    /// </summary>
    public string Status { get; set; } = "Unknown";

    /// <summary>
    /// Reported availability of the Execution Runner.
    /// </summary>
    public bool Available { get; set; }

    /// <summary>
    /// Whether the Execution runner's status endpoint is reachable
    /// </summary>
    public bool Live { get; set; }

    /// <summary>
    /// Whether the execution runner is enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Whether the API Key was accepted
    /// </summary>
    public bool Authenticated { get; set; }

    /// <summary>
    /// The load balancing weight.
    /// </summary>
    public int Weight { get; set; } = 100;
}
