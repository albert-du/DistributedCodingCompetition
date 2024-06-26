﻿namespace DistributedCodingCompetition.Web.Services;

using DistributedCodingCompetition.ApiService.Models;
using System.Collections.Generic;
using System.Net;

public class ApiService(HttpClient httpClient, ILogger<ApiService> logger) : IApiService
{
    public async Task<(bool, User?)> TryReadUserByEmailAsync(string email)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/users/email/{email}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return (true, null);
            response.EnsureSuccessStatusCode();
            return (true, await response.Content.ReadFromJsonAsync<User>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch user by email");
            return (false, null);
        }
    }

    public async Task<bool> TryCreateUserAsync(User user)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("api/users", user);
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create user");
            return false;
        }
    }

    public async Task<(bool, User?)> TryReadUserAsync(Guid id)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/users/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return (true, null);
            response.EnsureSuccessStatusCode();
            return (true, await response.Content.ReadFromJsonAsync<User>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch user by id");
            return (false, null);
        }
    }

    public async Task<bool> TryUpdateUserAsync(User user)
    {
        try
        {
            var response = await httpClient.PutAsJsonAsync($"api/users/{user.Id}", user);
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update user");
            return false;
        }
    }

    public async Task<(bool, User?)> TryReadUserByUsername(string username)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/users/username/{username}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return (true, null);
            response.EnsureSuccessStatusCode();
            return (true, await response.Content.ReadFromJsonAsync<User>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch user by email");
            return (false, null);
        }
    }

    public Task<(bool, Contest?)> TryReadContestByJoinCodeAsync(string code)
    {
        throw new NotImplementedException();
    }

    public Task<(bool, Contest?)> TryReadContestAsync(Guid id)
    {
        throw new NotImplementedException();
    }


    public Task<(bool, IReadOnlyList<User>?)> TryReadContestAdminsAsync(Guid contestId)
    {
        throw new NotImplementedException();
    }

    public Task<(bool, IReadOnlyList<JoinCode>?)> TryReadJoinCodesAsync(Guid contestId)
    {
        throw new NotImplementedException();
    }

    public Task<(bool, Contest?)> TryUpdateContestAsync(Contest contest)
    {
        throw new NotImplementedException();
    }

    public Task<(bool, IReadOnlyList<Submission>?)> TryReadContestSubmissionsAsync(Guid contestId)
    {
        throw new NotImplementedException();
    }

    public Task<(bool, ContestRole?)> TryReadUserContestRoleAsync(Guid contestId, Guid userId)
    {
        throw new NotImplementedException();
    }
}
