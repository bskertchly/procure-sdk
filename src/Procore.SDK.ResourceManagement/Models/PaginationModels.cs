using System.Collections.Generic;
using System.Linq;

namespace Procore.SDK.ResourceManagement.Models;

/// <summary>
/// Pagination models for ResourceManagement client operations
/// </summary>

public class PaginationOptions
{
    public int Page { get; set; } = 1;
    public int PerPage { get; set; } = 100;
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
}

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