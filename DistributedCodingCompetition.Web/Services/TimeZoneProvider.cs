namespace DistributedCodingCompetition.Web.Services;

using Microsoft.JSInterop;

public class TimeZoneProvider(IJSRuntime jsRuntime) : ITimeZoneProvider
{
    private TimeSpan? _userOffset;
    public async Task<DateTimeOffset> GetLocalDateTime(DateTimeOffset dateTime)
    {
        if (_userOffset is null)
        {
            int offsetInMinutes = await jsRuntime.InvokeAsync<int>("getTimezoneOffset");
            _userOffset = TimeSpan.FromMinutes(-offsetInMinutes);
        }

        return dateTime.ToOffset(_userOffset.Value);
    }
}