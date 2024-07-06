namespace DistributedCodingCompetition.Judge.Services;

using Microsoft.Extensions.Caching.Distributed;
using System.Text;
public class RateLimitService(IDistributedCache distributedCache) : IRateLimitService
{
    public async Task<bool> TryLockAsync(Guid id, TimeSpan duration)
    {
        var key = id.ToString();
        var val = distributedCache.GetAsync(key);
        if (val != null)
            return false;
        var options = new DistributedCacheEntryOptions().SetSlidingExpiration(duration);
        await distributedCache.SetAsync(key, Encoding.UTF8.GetBytes("1"), options);
        return true;
    }
}
