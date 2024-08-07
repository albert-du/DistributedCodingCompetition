namespace DistributedCodingCompetition.ApiService.Models;

using System.Text.Json.Serialization;

/// <summary>
/// Pagination result representation.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed record PaginateResult<T>
{
    /// <summary>
    /// The current page number. Starting at 1.
    /// </summary>
    public required int Page { get; init; }

    /// <summary>
    /// The number of items per page.
    /// </summary>
    public required int PageSize { get; init; }

    /// <summary>
    /// The total number of items.
    /// </summary>
    public required int TotalCount { get; init; }

    /// <summary>
    /// The total number of pages.
    /// </summary>
    public required int TotalPages { get; init; }

    /// <summary>
    /// The items.
    /// </summary>
    public required IReadOnlyList<T> Items { get; init; }

    /// <summary>
    /// Whether there is a next page.
    /// </summary>
    [JsonIgnore]
    public bool NextEnabled => Page < TotalPages;

    /// <summary>
    /// Whether there is a previous page.
    /// </summary>
    [JsonIgnore]
    public bool PreviousEnabled => Page > 1;

    /// <summary>
    /// An empty pagination result.
    /// </summary>
    public static PaginateResult<T> Empty => new()
    {
        Page = 1,
        PageSize = 0,
        TotalCount = 0,
        TotalPages = 1,
        Items = []
    };
}