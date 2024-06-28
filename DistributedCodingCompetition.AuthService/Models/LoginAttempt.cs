namespace DistributedCodingCompetition.AuthService.Models;

public class LoginAttempt
{
    public string IP { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public DateTime AttemptTime { get; set; }
    public bool Success { get; set; }
    public string? Error { get; set; }
}
