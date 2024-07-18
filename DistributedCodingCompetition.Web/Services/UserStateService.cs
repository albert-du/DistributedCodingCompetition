namespace DistributedCodingCompetition.Web.Services;

using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

/// <inheritdoc/>
public sealed class UserStateService(IUsersService usersService, AuthenticationStateProvider authenticationStateProvider, IModalService modalService) : IUserStateService
{
    /// <inheritdoc/>
    public async Task<UserResponseDTO?> UserAsync()
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var claimsPrincipal = authState.User;
        if (claimsPrincipal.Identity?.IsAuthenticated != true)
            return null;
        var id = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return null;
        var (success, user) = await usersService.TryReadUserAsync(Guid.Parse(id));
        if (!success)
        {
            modalService.ShowError("Failed to fetch user", "An error occurred while trying to fetch current user");
            return null;
        }
        return user;
    }
}