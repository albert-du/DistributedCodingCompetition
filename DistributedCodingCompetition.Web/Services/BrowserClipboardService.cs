namespace DistributedCodingCompetition.Web.Services;

using Microsoft.JSInterop;

public class BrowserClipboardService(IJSRuntime jsRuntime): IClipboardService
{
    public async Task SetClipboardAsync(string content)
    {
        await jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", content);
    }
}