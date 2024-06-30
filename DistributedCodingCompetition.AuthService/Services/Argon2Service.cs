namespace DistributedCodingCompetition.AuthService.Services;

using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Konscious.Security.Cryptography;
using DistributedCodingCompetition.AuthService.Models;

// Format:
// argon2id:<parallelism>;<memory>;<iterations>;<salt>;<key>

/// <summary>
/// Service for hashing and verifying passwords using Argon2 algorithm.
/// </summary>
/// <param name="options"></param>
public class Argon2Service(IOptions<ArgonOptions> options) : IPasswordService
{
    // options filled from environment, default is fine
    private readonly ArgonOptions _options = options.Value;

    /// <summary>
    /// Hashes a password using Argon2 algorithm.
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public string HashPassword(string password)
    {
        // fill salt
        var salt = GenerateSalt();

        // create Argon2 instance
        using Argon2id argon2 = new(Encoding.UTF8.GetBytes(password))
        {
            DegreeOfParallelism = _options.DegreeOfParallelism,
            MemorySize = _options.MemorySize,
            Iterations = _options.Iterations,
            Salt = salt
        };

        // get key
        var key = argon2.GetBytes(_options.KeySize);
        
        // encode
        return $"argon2id:{_options.DegreeOfParallelism};{_options.MemorySize};{_options.Iterations};{Convert.ToBase64String(salt)};{Convert.ToBase64String(key)}";
    }

    public (bool, string?) VerifyPassword(string password, string hash)
    {
        var parts = hash.Split(':');
        if (parts[0] != "argon2id")
            throw new ArgumentException("Invalid hash format, expected \"argon2id\"");
        parts = parts[1].Split(';');
        var parallelism = int.Parse(parts[0]);
        var memory = int.Parse(parts[1]);
        var iterations = int.Parse(parts[2]);
        var salt = Convert.FromBase64String(parts[3]);
        var key = Convert.FromBase64String(parts[4]);
        var saltSize = salt.Length;
        var keySize = key.Length;

        // check if rehash is needed
        var needsRehash =
            parallelism != _options.DegreeOfParallelism ||
            memory != _options.MemorySize ||
            iterations != _options.Iterations ||
            saltSize != _options.SaltSize ||
            keySize != _options.KeySize;

        // create Argon2 instance
        using Argon2id argon2 = new(Encoding.UTF8.GetBytes(password))
        {
            DegreeOfParallelism = parallelism,
            MemorySize = memory,
            Iterations = iterations,
            Salt = salt
        };

        // get key, compare, ?rehash, return
        return (argon2.GetBytes(keySize).SequenceEqual(key), needsRehash ? HashPassword(password) : null);
    }

    /// <summary>
    /// Fill RNG bytes.
    /// </summary>
    /// <returns></returns>
    private byte[] GenerateSalt() =>
        RandomNumberGenerator.GetBytes(_options.SaltSize);
}
