namespace DistributedCodingCompetition.Web.Services;

using Microsoft.JSInterop;

/// <summary>
/// Time zone provider for the browser.
/// </summary>
/// <param name="jsRuntime"></param>
public sealed class TimeZoneProvider(IJSRuntime jsRuntime) : ITimeZoneProvider
{
    private TimeSpan? _userOffset;

    /// <summary>
    /// Gets the local date and time from the provided date and time.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public async Task<DateTimeOffset> GetLocalDateTimeAsync(DateTimeOffset dateTime)
    {
        if (_userOffset is null)
        {
            int offsetInMinutes = await jsRuntime.InvokeAsync<int>("getTimezoneOffset");
            _userOffset = TimeSpan.FromMinutes(-offsetInMinutes);
        }
        // offset the date time by the user's offset
        return dateTime.ToOffset(_userOffset.Value);
    }

    /// <summary>
    /// Gets the time zone offset.
    /// </summary>
    /// <returns></returns>
    public async Task<TimeSpan> GetTimeZoneOffsetAsync()
    {
        if (_userOffset is null)
        {
            int offsetInMinutes = await jsRuntime.InvokeAsync<int>("getTimezoneOffset");
            _userOffset = TimeSpan.FromMinutes(-offsetInMinutes);
        }
        // return the offset
        return _userOffset.Value;
    }

    /// <summary>
    /// Gets the UTC date and time from the provided date and time.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public async Task<DateTimeOffset> GetUtcDateTimeAsync(DateTimeOffset dateTime)
    {
        if (_userOffset is null)
        {
            int offsetInMinutes = await jsRuntime.InvokeAsync<int>("getTimezoneOffset");
            _userOffset = TimeSpan.FromMinutes(-offsetInMinutes);
        }
        // convert the date time to UTC
        return dateTime.ToOffset(_userOffset.Value).ToUniversalTime();
    }
}