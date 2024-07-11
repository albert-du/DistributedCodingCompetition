namespace DistributedCodingCompetition.Web.Services;

using Microsoft.JSInterop;

public class SelectedLanguageService(IJSRuntime jSRuntime, IUserStateService userStateService) : ISelectedLanguageService
{
    public async Task<string?> GetSelectedLanguage()
    {
        var user = await userStateService.UserAsync();
        if (user is null)
            return null;

        return await jSRuntime.InvokeAsync<string>("localStorage.getItem", GetKey(user.Id));
    }

    public async Task ReportLanguageSwitch(string language)
    {
        var user = await userStateService.UserAsync();
        if (user is null)
            return;

        await jSRuntime.InvokeVoidAsync("localStorage.setItem", GetKey(user.Id), language);
    }

    private string GetKey(Guid userId) =>
        $"language-{userId}";
}