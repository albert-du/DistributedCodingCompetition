namespace DistributedCodingCompetition.ExecutionShared;

/// <summary>
/// DTO for the response of an exec runner
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="Endpoint"></param>
/// <param name="Enabled"></param>
/// <param name="Status"></param>
public record ExecRunnerResponseDTO(Guid Id, string Name, string Endpoint, bool Enabled, RunnerStatus? Status);
