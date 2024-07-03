namespace DistributedCodingCompetition.Web.Services;

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

    public async Task<(bool, Guid?)> TryCreateProblemAsync(Problem problem)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("api/problems", problem);
            response.EnsureSuccessStatusCode();
            return (true, problem.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create problem");
            return (false, null);
        }
    }

    public async Task<(bool, Contest?)> TryReadContestByJoinCodeAsync(string code)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/contests/joincode/{code}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return (true, null);
            response.EnsureSuccessStatusCode();
            return (true, await response.Content.ReadFromJsonAsync<Contest>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch contest by join code");
            return (false, null);
        }
    }

    public async Task<(bool, Contest?)> TryReadContestAsync(Guid id)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/contests/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return (true, null);
            response.EnsureSuccessStatusCode();
            return (true, await response.Content.ReadFromJsonAsync<Contest>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch contest by id");
            return (false, null);
        }
    }

    public async Task<(bool, IReadOnlyList<User>?)> TryReadContestAdminsAsync(Guid contestId)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/contests/{contestId}/admins");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return (true, null);
            response.EnsureSuccessStatusCode();
            return (true, await response.Content.ReadFromJsonAsync<IReadOnlyList<User>>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch contest admins");
            return (false, null);
        }
    }

    public async Task<(bool, IReadOnlyList<JoinCode>?)> TryReadJoinCodesAsync(Guid contestId)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/contests/{contestId}/joincodes");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return (true, null);
            response.EnsureSuccessStatusCode();
            return (true, await response.Content.ReadFromJsonAsync<IReadOnlyList<JoinCode>>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch join codes");
            return (false, null);
        }
    }

    public async Task<(bool, IReadOnlyList<Submission>?)> TryReadContestSubmissionsAsync(Guid contestId, int count, int page)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/contests/{contestId}/submissions?count={count}&page={page - 1}");
            response.EnsureSuccessStatusCode();
            return (true, await response.Content.ReadFromJsonAsync<IReadOnlyList<Submission>>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch contest submissions");
            return (false, null);
        }
    }

    public async Task<(bool, ContestRole?)> TryReadUserContestRoleAsync(Guid contestId, Guid userId)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/contests/{contestId}/role/{userId}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return (true, null);
            response.EnsureSuccessStatusCode();
            return (true, await response.Content.ReadFromJsonAsync<ContestRole>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch user contest role");
            return (false, null);
        }
    }

    public async Task<(bool, Contest?)> TryUpdateContestAsync(Contest contest)
    {
        try
        {
            var response = await httpClient.PutAsJsonAsync($"api/contests/{contest.Id}", contest);
            response.EnsureSuccessStatusCode();
            return (true, contest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update contest");
            return (false, null);
        }
    }

    public async Task<(bool, Guid?)> TryCreateContestAsync(Contest contest)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("api/contests", contest);
            response.EnsureSuccessStatusCode();
            return (true, contest.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create contest");
            return (false, null);
        }
    }

    public async Task<(bool, Problem?)> TryUpdateProblemAsync(Problem contest)
    {
        try
        {
            var response = await httpClient.PutAsJsonAsync($"api/problems/{contest.Id}", contest);
            response.EnsureSuccessStatusCode();
            return (true, contest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update problem");
            return (false, null);
        }
    }

    public async Task<(bool, Problem?)> TryReadProblemAsync(Guid id)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/problems/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return (true, null);
            response.EnsureSuccessStatusCode();
            return (true, await response.Content.ReadFromJsonAsync<Problem>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch problem by id");
            return (false, null);
        }
    }

    public async Task<(bool, IReadOnlyList<TestCase>?)> TryReadProblemTestCases(Guid problemId)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/problems/{problemId}/testcases");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return (true, null);
            response.EnsureSuccessStatusCode();
            return (true, await response.Content.ReadFromJsonAsync<IReadOnlyList<TestCase>>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch problem test cases");
            return (false, null);
        }
    }

    public async Task<bool> TryUpdateUserContestRoleAsync(Guid contestId, Guid userId, ContestRole role)
    {
        try
        {
            var response = await httpClient.PutAsJsonAsync($"api/contests/{contestId}/role/{userId}", role);
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update user contest role");
            return false;
        }
    }

    public async Task<(bool, IReadOnlyList<User>?)> TryReadContestParticipantsAsync(Guid contestId, int count, int page)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/contests/{contestId}/participants?count={count}&page={page}");
            response.EnsureSuccessStatusCode();
            return (true, await response.Content.ReadFromJsonAsync<IReadOnlyList<User>>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch contest participants");
            return (false, null);
        }
    }

    public async Task<(bool, IReadOnlyList<User>?)> TryReadContestBannedAsync(Guid contestId, int count, int page)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/contests/{contestId}/banned?count={count}&page={page}");
            response.EnsureSuccessStatusCode();
            return (true, await response.Content.ReadFromJsonAsync<IReadOnlyList<User>>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch contest banned users");
            return (false, null);
        }
    }

    public async Task<(bool, IReadOnlyList<Contest>?)> TryReadUserAdministratedContestsAsync(Guid userId, int count, int page)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/users/{userId}/administered?count={count}&page={page}");
            response.EnsureSuccessStatusCode();
            return (true, await response.Content.ReadFromJsonAsync<IReadOnlyList<Contest>>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch user administered contests");
            return (false, null);
        }
    }

    public async Task<(bool, IReadOnlyList<Contest>?)> TryReadUserEnteredContestsAsync(Guid userId, int count, int page)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/users/{userId}/entered?count={count}&page={page}");
            response.EnsureSuccessStatusCode();
            return (true, await response.Content.ReadFromJsonAsync<IReadOnlyList<Contest>>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch user entered contests");
            return (false, null);
        }
    }

    public async Task<(bool, IReadOnlyList<Contest>?)> TryReadPublicContestsAsync(int count, int page)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/contests/public?count={count}&page={page}");
            response.EnsureSuccessStatusCode();
            return (true, await response.Content.ReadFromJsonAsync<IReadOnlyList<Contest>>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch public contests");
            return (false, null);
        }
    }

    public async Task<(bool, IReadOnlyList<JoinCode>?)> TryReadContestJoinCodesAsync(Guid id)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/contests/{id}/joincodes");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return (true, null);
            response.EnsureSuccessStatusCode();
            return (true, await response.Content.ReadFromJsonAsync<IReadOnlyList<JoinCode>>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch contest join codes");
            return (false, null);
        }
    }
}
