namespace DistributedCodingCompetition.Web.Services;

public interface ITimeZoneProvider
{
    Task<DateTimeOffset> GetLocalDateTime(DateTimeOffset dateTime);
}