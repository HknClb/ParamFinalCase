using Core.Application.DynamicQuery;
using Core.Application.Paging;
using Core.Application.Repositories;
using Core.Domain.Entities;
using Core.Persistence.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Core.Persistence.Repositories
{
    public class EfGenericReadRepository<TEntity, TContext> : IGenericReadRepository<TEntity> where TEntity : Entity where TContext : DbContext
    {
        protected TContext Context { get; }

        public EfGenericReadRepository(TContext context)
        {
            Context = context;
        }

        public DbSet<TEntity> Table => Context.Set<TEntity>();

        public TEntity? Get(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool enableTracking = true)
        {
            IQueryable<TEntity> queryable = Table.AsQueryable();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include is not null)
                queryable = include(queryable);
            return queryable.FirstOrDefault(predicate);
        }

        public Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Table.AsQueryable();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include is not null)
                queryable = include(queryable);
            return queryable.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public IList<TEntity> GetList(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool enableTracking = true)
        {
            IQueryable<TEntity> queryable = Table.AsQueryable();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include is not null)
                queryable = include(queryable);
            if (predicate is not null)
                queryable = queryable.Where(predicate);
            if (orderBy is not null)
                return orderBy(queryable).ToList();
            return queryable.ToList();
        }

        public async Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Table.AsQueryable();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include is not null)
                queryable = include(queryable);
            if (predicate is not null)
                queryable = queryable.Where(predicate);
            if (orderBy is not null)
                return await orderBy(queryable).ToListAsync(cancellationToken: cancellationToken);
            return await queryable.ToListAsync(cancellationToken: cancellationToken);
        }

        public IList<TEntity> GetListByDynamic(Dynamic dynamic, Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool enableTracking = true)
        {
            IQueryable<TEntity> queryable = Table.AsQueryable();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include != null)
                queryable = include(queryable);
            queryable = queryable.ToDynamic(dynamic);
            return queryable.ToList();
        }

        public async Task<IList<TEntity>> GetListByDynamicAsync(Dynamic dynamic, Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Table.AsQueryable();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include != null)
                queryable = include(queryable);
            queryable = queryable.ToDynamic(dynamic);
            return await queryable.ToListAsync();
        }

        public IPaginate<TEntity> GetListAsPaginate(Expression<Func<TEntity, bool>>? predicate = null, int index = 0, int size = 10,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool enableTracking = true)
        {
            IQueryable<TEntity> queryable = Table.AsQueryable();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include is not null)
                queryable = include(queryable);
            if (predicate is not null)
                queryable = queryable.Where(predicate);
            if (orderBy is not null)
                return orderBy(queryable).ToPaginate(index, size, 0);
            return queryable.ToPaginate(index, size, 0);
        }

        public async Task<IPaginate<TEntity>> GetListAsPaginateAsync(Expression<Func<TEntity, bool>>? predicate = null, int index = 0, int size = 10,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>,
                IIncludableQueryable<TEntity, object>>? include = null, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Table.AsQueryable();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include is not null)
                queryable = include(queryable);
            if (predicate is not null)
                queryable = queryable.Where(predicate);
            if (orderBy is not null)
                return await orderBy(queryable).ToPaginateAsync(index, size, 0, cancellationToken);
            return await queryable.ToPaginateAsync(index, size, 0, cancellationToken);
        }

        public IPaginate<TEntity> GetListByDynamicAsPaginate(Dynamic dynamic, Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10,
            bool enableTracking = true)
        {
            IQueryable<TEntity> queryable = Table.AsQueryable();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include != null)
                queryable = include(queryable);
            queryable = queryable.ToDynamic(dynamic);
            return queryable.ToPaginate(index, size, 0);
        }

        public async Task<IPaginate<TEntity>> GetListByDynamicAsPaginateAsync(Dynamic dynamic, Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10,
            bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Table.AsQueryable();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include != null)
                queryable = include(queryable);
            queryable = queryable.ToDynamic(dynamic);
            return await queryable.ToPaginateAsync(index, size, 0, cancellationToken);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
            => Table.Any(predicate);

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
            => await Table.AnyAsync(predicate, cancellationToken);
    }
}