namespace DistributedCodingCompetition.AuthService.Models;

/// <summary>
/// Options for Argon2 password hashing
/// </summary>
public class ArgonOptions
{
    /// <summary>
    /// config string
    /// </summary>
    public const string Argon = nameof(Argon);

    /// <summary>
    /// Degree of parallelism to use for Argon2
    /// </summary>
    public int DegreeOfParallelism { get; set; }
    
    /// <summary>
    /// Memory size to use for Argon2, in KiB
    /// </summary>
    public int MemorySize { get; set; }

    /// <summary>
    /// Iterations to use for Argon2
    /// </summary>
    public int Iterations { get; set; }

    /// <summary>
    /// Salt size, bytes
    /// </summary>
    public int SaltSize { get; set; }

    /// <summary>
    /// Key size, bytes
    /// </summary>
    public int KeySize { get; set; }
}
