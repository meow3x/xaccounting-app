using Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueryParser;

namespace Api.Features.Shared;

// Offset pagination
public record PageOptions(int PageId, int Limit);

public enum SortDirection
{
    Asc,
    Desc
}

public record SortCriteria(
    string SortBy, // Column name
    SortDirection SortOrder = SortDirection.Asc// asc|desc
);

/* 
 * Read: https://learn.microsoft.com/en-us/ef/core/modeling/shadow-properties#foreign-key-shadow-properties
 */
public static class FilterUtils
{
    public static IQueryable<T> Search<T>(this IQueryable<T> q, IEnumerable<SearchTerm>? terms)
    {
        if (terms == null) { return q; }
        foreach (var term in terms)
        {
            q = term.Operator switch
            {
                Operator.Contains => q.Where(
                    e => EF.Property<string>(e, term.Field)
                        .ToLower()
                        .Contains(
                            term.ValueAs<string>().ToLower()
                        )),
                Operator.In => q = q.Where(e => term.ValueAs<int[]>().Contains(EF.Property<int>(e, term.Field))),
                _ => throw new NotImplementedException("Operator not implemented")
            };
        }

        return q;
    }

    public static IQueryable<T> Skip<T>(this IQueryable<T> q, PageOptions? pageOptions)
    {
        if (pageOptions == null) { return q; }
        return q.Skip((pageOptions.PageId - 1) * pageOptions.Limit);
    }
}

