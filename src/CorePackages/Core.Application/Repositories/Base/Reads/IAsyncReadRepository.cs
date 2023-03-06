using Core.Application.DynamicQuery;
using Core.Application.Paging;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Core.Application.Repositories.Base.Reads
{
    public interface IAsyncReadRepository<TEntity> where TEntity : Entity
    {
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool enableTracking = true, CancellationToken cancellationToken = default);

        Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool enableTracking = true, CancellationToken cancellationToken = default);

        Task<IPaginate<TEntity>> GetListAsPaginateAsync(Expression<Func<TEntity, bool>>? predicate = null, int index = 0, int size = 10,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool enableTracking = true, CancellationToken cancellationToken = default);

        Task<IList<TEntity>> GetListByDynamicAsync(Dynamic dynamic, Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool enableTracking = true, CancellationToken cancellationToken = default);

        Task<IPaginate<TEntity>> GetListByDynamicAsPaginateAsync(Dynamic dynamic, Expression<Func<TEntity, bool>>? predicate = null,
           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10,
           bool enableTracking = true, CancellationToken cancellationToken = default);
    }
}