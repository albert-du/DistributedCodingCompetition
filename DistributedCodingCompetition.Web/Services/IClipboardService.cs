namespace DistributedCodingCompetition.Web.Services;

public interface IClipboardService
{
    Task SetClipboardAsync(string content);
}