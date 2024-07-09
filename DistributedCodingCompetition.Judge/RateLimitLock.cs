using DistributedCodingCompetition.Judge.Services;

namespace DistributedCodingCompetition.Judge;

/// <summary>
/// Rate limit lock
/// Dispose to release the lock
/// </summary>
internal sealed class RateLimitLock(IRateLimitService rateLimitService, Guid id) : IAsyncDisposable
{
    /// <summary>
    /// Release the lock
    /// </summary>
    /// <returns></returns>
    public ValueTask DisposeAsync() =>
        new(rateLimitService.ReleaseAsync(id));
}
