namespace DistributedCodingCompetition.ExecutionShared;

/// <summary>
/// DTO for the request of an exec runner
/// </summary>
/// <param name="Name"></param>
/// <param name="Endpoint"></param>
/// <param name="Enabled"></param>
/// <param name="Weight"></param>
/// <param name="Key"></param>
public record ExecRunnerRequestDTO(string Name, string Endpoint, bool Enabled, int Weight, string Key);
