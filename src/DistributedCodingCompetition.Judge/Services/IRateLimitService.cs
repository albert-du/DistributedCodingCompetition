namespace DistributedCodingCompetition.Judge.Services;

/// <summary>
/// Rate limit provider
/// </summary>
public interface IRateLimitService
{
    /// <summary>
    /// Try to lock the specified id for the specified duration.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="duration"></param>
    /// <returns>A lock object to be disposed to end early if successful</returns>
    Task<IAsyncDisposable?> TryLockAsync(Guid id, TimeSpan duration);

    /// <summary>
    /// Release the lock
    /// </summary>
    /// <returns></returns>
    Task ReleaseAsync(Guid id);
}
