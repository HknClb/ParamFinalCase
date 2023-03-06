using Core.Application.Repositories;
using Core.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.UnitOfWorks.Base
{
    public abstract class EfUnitOfWork : UnitOfWorkBase
    {
        private readonly IEnumerable<DbContext> _contexts;

        public EfUnitOfWork(params DbContext[] contexts)
        {
            _contexts = contexts.AsEnumerable();
        }

        ~EfUnitOfWork() => Dispose(false);

        protected override void SaveChanges()
        {
            foreach (var context in _contexts)
                if (context.ChangeTracker.HasChanges())
                {
                    if (context.Database.CurrentTransaction is null)
                        context.Database.BeginTransaction();
                    context.SaveChanges();
                }
        }

        protected override async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var context in _contexts)
                if (context.ChangeTracker.HasChanges())
                {
                    if (context.Database.CurrentTransaction is null)
                        await context.Database.BeginTransactionAsync(cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }
        }

        protected override void BeginTransaction()
        {
            foreach (var context in _contexts)
                context.Database.BeginTransaction();
        }

        protected override async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            foreach (var context in _contexts)
                await context.Database.BeginTransactionAsync(cancellationToken);
        }

        protected override void CommitTransaction()
        {
            foreach (var context in _contexts)
                if (context.Database.CurrentTransaction is not null)
                    context.Database.CommitTransaction();
        }

        protected override async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            foreach (var context in _contexts)
                if (context.Database.CurrentTransaction is not null)
                    await context.Database.CommitTransactionAsync(cancellationToken);
        }

        protected override void RollbackTransaction()
        {
            foreach (var context in _contexts)
                if (context.Database.CurrentTransaction is not null)
                    context.Database.RollbackTransaction();
        }

        protected override async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            foreach (var context in _contexts)
                if (context.Database.CurrentTransaction is not null)
                    await context.Database.RollbackTransactionAsync(cancellationToken);
        }

        private DbContext? GetContextIfDbSetIsExist(Type type)
        {
            foreach (var context in _contexts)
                if (context.GetType().GetProperties().Where(x => x.PropertyType.Name == typeof(DbSet<>).Name)
                    .FirstOrDefault(x => x.PropertyType.GenericTypeArguments.Contains(type)) is not null)
                    return context;

            return null;
        }

        protected override IGenericReadRepository<TEntity>? GenerateReadRepository<TEntity>()
        {
            DbContext? context = GetContextIfDbSetIsExist(typeof(TEntity));
            if (context is null)
                return null;

            Type readRepositoryType = typeof(EfGenericReadRepository<,>).MakeGenericType(typeof(TEntity), context.GetType());

            return Activator.CreateInstance(readRepositoryType, new object[] { context }) as IGenericReadRepository<TEntity>;
        }

        protected override IGenericWriteRepository<TEntity>? GenerateWriteRepository<TEntity>()
        {
            DbContext? context = GetContextIfDbSetIsExist(typeof(TEntity));
            if (context is null)
                return null;

            Type writeRepositoryType = typeof(EfGenericWriteRepository<,>).MakeGenericType(typeof(TEntity), context.GetType());

            return Activator.CreateInstance(writeRepositoryType, new object[] { context }) as IGenericWriteRepository<TEntity>;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    foreach (var context in _contexts)
                        context.Dispose();

                disposedValue = true;
            }
        }
    }
}