namespace DistributedCodingCompetition.CodeExecution.Models;

public class ExecRunner
{
    public Guid Id { get; set; }
    public string Endpoint { get; set; } = string.Empty;
    public string Version { get; set; } = "Unknown";
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = "Unknown";
    public List<string> Languages { get; set; } = [];
    public List<string> Packages { get; set; } = [];
    public string SystemInfo { get; set; } = "Unknown";
    public string Status { get; set; } = "Unknown";
    public bool Available { get; set; }
    public bool Live { get; set; }
    public bool Enabled { get; set; } = true;
    public bool Authenticated { get; set; }
    public int Weight { get; set; } = 100;
}
