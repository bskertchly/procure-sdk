using System.Collections.Generic;
using System.Linq;

namespace Procore.SDK.ConstructionFinancials.Models;

/// <summary>
/// Pagination models for ConstructionFinancials client operations
/// </summary>

/// <summary>
/// Represents options for paginating results.
/// </summary>
public class PaginationOptions
{
    /// <summary>
    /// Gets or sets the page number to retrieve (1-based).
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    public int PerPage { get; set; } = 100;

    /// <summary>
    /// Gets or sets the property name to sort by.
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Gets or sets the sort direction (asc or desc).
    /// </summary>
    public string? SortDirection { get; set; }
}

/// <summary>
/// Represents a paginated result set containing items and pagination metadata.
/// </summary>
/// <typeparam name="T">The type of items in the result set.</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// Gets or sets the items in the current page.
    /// </summary>
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

    /// <summary>
    /// Gets or sets the total number of items across all pages.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the current page number (1-based).
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    public int PerPage { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether there is a next page available.
    /// </summary>
    public bool HasNextPage { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether there is a previous page available.
    /// </summary>
    public bool HasPreviousPage { get; set; }
}