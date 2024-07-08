namespace DistributedCodingCompetition.Web.Services;

using Microsoft.JSInterop;

/// <summary>
/// Clipboard service for browser
/// </summary>
/// <param name="jsRuntime"></param>
public sealed class BrowserClipboardService(IJSRuntime jsRuntime): IClipboardService
{
    /// <summary>
    /// Set clipboard content using Javascript interop
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public async Task SetClipboardAsync(string content)
    {
        await jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", content);
    }
}