namespace DistributedCodingCompetition.AuthService.Services;

using Konscious.Security.Cryptography;
using System.Text;

public class Argon2(IConfiguration configuration) : IPasswordService
{
    public string HashPassword(string password)
    {
        using Argon2id argon2 = new(Encoding.UTF8.GetBytes(password)) { DegreeOfParallelism = _parallelism, MemorySize = _memorySize, Iterations = _iterations, Salt = salt };
        return argon2Id.Hash(password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        using Argon2id argon2 = new(Encoding.UTF8.GetBytes(password)) { DegreeOfParallelism = _parallelism, MemorySize = _memorySize, Iterations = _iterations, Salt = salt };
        return argon2.Verify(hash);
    }
}
