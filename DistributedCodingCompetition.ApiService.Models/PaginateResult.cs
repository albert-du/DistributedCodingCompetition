namespace DistributedCodingCompetition.ApiService.Models;

public sealed record PaginateResult<T>
{
    public required int Page { get; init; }
    public required int PageSize { get; init; }
    public required int TotalCount { get; init; }
    public required int TotalPages { get; init; }
    public required IReadOnlyList<T> Items { get; init; }
}