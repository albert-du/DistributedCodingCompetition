namespace DistributedCodingCompetition.ApiService;

using Microsoft.EntityFrameworkCore;

internal static class QueryExtensions
{
    internal static async Task<PaginateResult<TR>> PaginateAsync<T, TR>(this IQueryable<T> query, int page, int count, Func<IQueryable<T>, Task<IReadOnlyList<TR>>> transform)
    {
        if (page < 1)
            throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than 0.");

        if (count < 1)
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than 0.");

        var querySection =
            query.Skip((page - 1) * count)
                 .Take(count);

        var length = await query.CountAsync();

        var items = await transform(querySection);

        return new PaginateResult<TR>
        {
            Page = page,
            PageSize = count,
            TotalCount = length,
            TotalPages = (int)Math.Ceiling((double)length / count),
            Items = items
        };
    }

    internal static async Task<IReadOnlyList<ContestResponseDTO>> ReadContestsAsync(this IQueryable<Contest> contests) =>
        await contests
            .Select(contest => new
            {
                contest.Id,
                contest.Name,
                contest.OwnerId,
                OwnerName = contest.Owner.Username,
                contest.Description,
                contest.RenderedDescription,
                contest.StartTime,
                contest.EndTime,
                contest.Active,
                contest.Public,
                contest.Open,
                contest.MinimumAge,
                contest.DefaultPointsForProblem
            })
            .ToAsyncEnumerable()
            .Select(contest => new ContestResponseDTO
            {
                Id = contest.Id,
                Name = contest.Name,
                OwnerId = contest.OwnerId,
                OwnerName = contest.OwnerName,
                Description = contest.Description,
                RenderedDescription = contest.RenderedDescription,
                StartTime = contest.StartTime,
                EndTime = contest.EndTime,
                Active = contest.Active,
                Public = contest.Public,
                Open = contest.Open,
                MinimumAge = contest.MinimumAge,
                DefaultPointsForProblem = contest.DefaultPointsForProblem
            })
            .ToArrayAsync();
}