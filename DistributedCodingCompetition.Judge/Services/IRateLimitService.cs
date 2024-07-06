namespace DistributedCodingCompetition.Judge.Services;

public interface IRateLimitService
{
    /// <summary>
    /// Try to lock the specified id for the specified duration.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="duration"></param>
    /// <returns>true if successful</returns>
    Task<bool> TryLockAsync(Guid id, TimeSpan duration);
}
