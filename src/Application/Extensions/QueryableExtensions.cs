using CleanBlazor.Application.Exceptions;
using CleanBlazor.Application.Specifications.Base;
using CleanBlazor.Domain.Abstractions;
using CleanBlazor.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace CleanBlazor.Application.Extensions;

public static class QueryableExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(
        this IQueryable<T> source,
        int pageNumber,
        int pageSize) where T : class
    {
        if (source == null)
        {
            throw new ApiException();
        }

        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        pageSize = pageSize == 0 ? 10 : pageSize;
        var count = await source.CountAsync();
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return PaginatedResult<T>.Ok(items, count, pageNumber, pageSize);
    }

    public static IQueryable<T> Specify<T>(this IQueryable<T> query, ISpecification<T> spec) where T : class, IEntity
    {
        IQueryable<T> queryableResultWithIncludes = spec.Includes
            .Aggregate(query,
                (current, include) => current.Include(include));
        IQueryable<T> secondaryResult = spec.IncludeStrings
            .Aggregate(queryableResultWithIncludes,
                (current, include) => current.Include(include));
        return secondaryResult.Where(spec.Criteria);
    }
}
