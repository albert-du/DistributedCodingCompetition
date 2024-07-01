namespace DistributedCodingCompetition.Web.Services;

/// <summary>
/// Time zone provider interface from which the local date and time can be retrieved.
/// </summary>
public interface ITimeZoneProvider
{
    /// <summary>
    /// Gets the local date and time from the provided date and time.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    Task<DateTimeOffset> GetLocalDateTimeAsync(DateTimeOffset dateTime);

    /// <summary>
    /// Gets the UTC date and time from the provided date and time.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    Task<DateTimeOffset> GetUtcDateTimeAsync(DateTimeOffset dateTime);
}