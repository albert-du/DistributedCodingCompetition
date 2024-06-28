namespace DistributedCodingCompetition.Web.Models;

public class SMTPOptions
{
    public string Host { get; set; } = default!;
    public int Port { get; set; } = 587;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string From { get; set; } = default!;
    public bool EnableTLS { get; set; }
}
