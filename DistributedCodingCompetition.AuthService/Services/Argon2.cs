namespace DistributedCodingCompetition.AuthService.Services;

using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Konscious.Security.Cryptography;
using DistributedCodingCompetition.AuthService.Models;

// Format:
// argon2id:<parrallelism>;<memory>;<iterations>;<salt>;<key>

public class Argon2(IOptions<ArgonOptions> options) : IPasswordService
{
    private readonly ArgonOptions _options = options.Value;
    public string HashPassword(string password)
    {
        var salt = GenerateSalt();
        using Argon2id argon2 = new(Encoding.UTF8.GetBytes(password))
        {
            DegreeOfParallelism = _options.DegreeOfParallelism,
            MemorySize = _options.MemorySize,
            Iterations = _options.Iterations,
            Salt = salt
        };
        var key = argon2.GetBytes(_options.KeySize);
        return $"{_options.DegreeOfParallelism}:{_options.MemorySize}:{_options.Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(key)}";
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

        var needsRehash =
            parallelism != _options.DegreeOfParallelism ||
            memory != _options.MemorySize ||
            iterations != _options.Iterations ||
            saltSize != _options.SaltSize ||
            keySize != _options.KeySize;

        using Argon2id argon2 = new(Encoding.UTF8.GetBytes(password))
        {
            DegreeOfParallelism = parallelism,
            MemorySize = memory,
            Iterations = iterations,
            Salt = salt
        };
        return (argon2.GetBytes(keySize).SequenceEqual(key), needsRehash ? HashPassword(password) : null);
    }

    private byte[] GenerateSalt() =>
        RandomNumberGenerator.GetBytes(_options.SaltSize);
}
