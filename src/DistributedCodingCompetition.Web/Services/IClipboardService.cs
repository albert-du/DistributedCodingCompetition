namespace DistributedCodingCompetition.Web.Services;

/// <summary>
/// Represents a clipboard per user instance
/// </summary>
public interface IClipboardService
{
    /// <summary>
    /// Set the content of the clipboard
    /// </summary>
    /// <param name="content">text content</param>
    /// <returns></returns>
    Task SetClipboardAsync(string content);
}