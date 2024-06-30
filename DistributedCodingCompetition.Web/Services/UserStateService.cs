namespace DistributedCodingCompetition.Web.Services;

using System.Security.Claims;
using DistributedCodingCompetition.ApiService.Models;
using Microsoft.AspNetCore.Components.Authorization;

public class UserStateService(IApiService apiService, AuthenticationStateProvider authenticationStateProvider, IModalService modalService) : IUserStateService
{
    public async Task<User?> UserAsync()
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var claimsPrincipal = authState.User;
        if (claimsPrincipal.Identity?.IsAuthenticated != true)
            return null;
        var id = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return null;
        var (success, user) = await apiService.TryReadUserAsync(Guid.Parse(id));
        if (!success)
        {
            modalService.ShowError("Failed to fetch user", "An error occurred while trying to fetch current user");
            return null;
        }
        return user;
    }

    public async Task UpdateUserAsync(User user)
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var claimsPrincipal = authState.User;
        if (claimsPrincipal.Identity?.IsAuthenticated != true)
            return;
        var id = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return;
        if (Guid.Parse(id) != user.Id)
        {
            modalService.ShowError("Failed to update user", "User ID does not match current user");
            return;
        }
        var success = await apiService.TryUpdateUserAsync(user);
        if (!success)
            modalService.ShowError("Failed to update user", "An error occurred while trying to update current user");
        else
            OnChange?.Invoke(this, user);
    }
    
    public event EventHandler<User?>? OnChange;
}