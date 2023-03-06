using Core.Application.DynamicQuery;
using Core.Application.Paging;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Core.Application.Repositories.Base.Reads
{
    public interface IReadRepository<TEntity> where TEntity : Entity
    {
        bool Any(Expression<Func<TEntity, bool>> predicate);

        TEntity? Get(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool enableTracking = true);

        IList<TEntity> GetList(Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool enableTracking = true);

        IPaginate<TEntity> GetListAsPaginate(Expression<Func<TEntity, bool>>? predicate = null, int index = 0, int size = 10,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool enableTracking = true);

        IList<TEntity> GetListByDynamic(Dynamic dynamic, Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool enableTracking = true);

        IPaginate<TEntity> GetListByDynamicAsPaginate(Dynamic dynamic, Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            int index = 0, int size = 10, bool enableTracking = true);
    }
}