using Core.Application.Paging;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.Paging;

public static class IQueryablePaginateExtensions
{
    public static async Task<IPaginate<TEntity>> ToPaginateAsync<TEntity>(this IQueryable<TEntity> source, int index, int size, int from = 0, CancellationToken cancellationToken = default)
    {
        if (from > index) throw new ArgumentException($"From: {from} > Index: {index}, must from <= Index");

        int count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
        List<TEntity> items = await source.Skip((index - from) * size).Take(size).ToListAsync(cancellationToken)
                                    .ConfigureAwait(false);
        Paginate<TEntity> list = new()
        {
            Index = index,
            Size = size,
            From = from,
            Count = count,
            Items = items,
            Pages = (int)Math.Ceiling(count / (double)size)
        };
        return list;
    }


    public static IPaginate<TEntity> ToPaginate<TEntity>(this IQueryable<TEntity> source, int index, int size, int from = 0)
    {
        if (from > index) throw new ArgumentException($"From: {from} > Index: {index}, must from <= Index");

        int count = source.Count();
        List<TEntity> items = source.Skip((index - from) * size).Take(size).ToList();
        Paginate<TEntity> list = new()
        {
            Index = index,
            Size = size,
            From = from,
            Count = count,
            Items = items,
            Pages = (int)Math.Ceiling(count / (double)size)
        };
        return list;
    }
}