using System;
using System.Collections.Generic;
using System.Linq;

namespace Procore.SDK.Core.Models;

/// <summary>
/// Options for paginated requests.
/// </summary>
public class PaginationOptions
{
    public int Page { get; set; } = 1;
    public int PerPage { get; set; } = 100;
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; } = "asc";
    public Dictionary<string, object>? Filters { get; set; }
}

/// <summary>
/// Represents a paginated result.
/// </summary>
/// <typeparam name="T">The type of items in the result.</typeparam>
public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PerPage { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}