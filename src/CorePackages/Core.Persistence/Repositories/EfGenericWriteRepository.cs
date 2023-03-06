using Core.Application.Repositories;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.Repositories
{
    public class EfGenericWriteRepository<TEntity, TContext> : IGenericWriteRepository<TEntity> where TEntity : Entity where TContext : DbContext
    {
        protected TContext Context { get; }

        public EfGenericWriteRepository(TContext context)
        {
            Context = context;
        }

        public DbSet<TEntity> Table => Context.Set<TEntity>();

        public TEntity Add(TEntity entity)
        {
            Table.Add(entity);
            return entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await Table.AddAsync(entity);
            return entity;
        }

        public void AddRange(IList<TEntity> entities)
        {
            Table.AddRangeAsync(entities);
        }

        public async Task AddRangeAsync(IList<TEntity> entities)
        {
            await Table.AddRangeAsync(entities);
        }

        public TEntity Update(TEntity entity)
        {
            Table.Update(entity);
            return entity;
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            Table.Update(entity);
            return Task.FromResult(entity);
        }

        public TEntity Delete(TEntity entity)
        {
            Table.Remove(entity);
            return entity;
        }

        public Task<TEntity> DeleteAsync(TEntity entity)
        {
            Table.Remove(entity);
            return Task.FromResult(entity);
        }

        public TEntity? DeleteById(string id)
        {
            TEntity? entity = Table.Find(id);
            if (entity is not null)
                Table.Remove(entity);
            return entity;
        }

        public async Task<TEntity?> DeleteByIdAsync(string id)
        {
            TEntity? entity = await Table.FindAsync(id);
            if (entity is not null)
                Table.Remove(entity);
            return entity;
        }

        public void DeleteRange(IList<TEntity> entities)
        {
            Table.RemoveRange(entities);
        }

        public Task DeleteRangeAsync(IList<TEntity> entities)
        {
            Table.RemoveRange(entities);
            return Task.CompletedTask;
        }
    }
}
