namespace DistributedCodingCompetition.Judge.Services;

using Microsoft.Extensions.Caching.Distributed;
using System.Text;

/// <inheritdoc/>
public class RateLimitService(IDistributedCache distributedCache) : IRateLimitService
{
    /// <inheritdoc/>
    public async Task<IAsyncDisposable?> TryLockAsync(Guid id, TimeSpan duration)
    {
        var key = id.ToString();
        var val = distributedCache.GetAsync(key);
        if (val != null)
            return null;
        var options = new DistributedCacheEntryOptions().SetSlidingExpiration(duration);
        await distributedCache.SetAsync(key, Encoding.UTF8.GetBytes("1"), options);
        return new RateLimitLock(this, id);
    }

    /// <inheritdoc/>
    public Task ReleaseAsync(Guid id) =>
        distributedCache.RemoveAsync(id.ToString());
}
