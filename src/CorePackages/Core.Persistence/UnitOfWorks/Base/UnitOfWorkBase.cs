using Core.Application.Repositories;
using Core.Application.UnitOfWorks.Base;
using Core.Domain.Entities;

namespace Core.Persistence.UnitOfWorks.Base
{
    public abstract class UnitOfWorkBase : IUnitOfWorkBase
    {
        private readonly Dictionary<Type, object> readRepositories = new();
        private readonly Dictionary<Type, object> writeRepositories = new();
        protected bool disposedValue;

        protected abstract void BeginTransaction();
        protected abstract Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        protected abstract void SaveChanges();
        protected abstract Task SaveChangesAsync(CancellationToken cancellationToken = default);
        protected abstract void CommitTransaction();
        protected abstract Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        protected abstract void RollbackTransaction();
        protected abstract Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        protected abstract IGenericReadRepository<TEntity>? GenerateReadRepository<TEntity>() where TEntity : Entity;
        protected abstract IGenericWriteRepository<TEntity>? GenerateWriteRepository<TEntity>() where TEntity : Entity;

        public void Complete()
        {
            try
            {
                SaveChanges();
                CommitTransaction();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                throw new Exception("Transaction", ex);
            }
        }

        public async Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await SaveChangesAsync(cancellationToken);
                await CommitTransactionAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync(cancellationToken);
                throw new Exception("Transaction", ex);
            }
        }

        public IGenericReadRepository<TEntity> ReadRepository<TEntity>() where TEntity : Entity
        {
            if (readRepositories.TryGetValue(typeof(TEntity), out object? repository))
                return (repository as IGenericReadRepository<TEntity>)!;

            repository = GenerateReadRepository<TEntity>() ?? throw new NotImplementedException($"{typeof(TEntity).Name} Repository Not Found!");
            readRepositories.Add(typeof(TEntity), repository);

            return (repository as IGenericReadRepository<TEntity>)!;
        }

        public IGenericWriteRepository<TEntity> WriteRepository<TEntity>() where TEntity : Entity
        {
            if (writeRepositories.TryGetValue(typeof(TEntity), out object? repository))
                return (repository as IGenericWriteRepository<TEntity>)!;

            repository = GenerateWriteRepository<TEntity>() ?? throw new NotImplementedException($"{typeof(TEntity).Name} Repository Not Found!");
            writeRepositories.Add(typeof(TEntity), repository);

            return (repository as IGenericWriteRepository<TEntity>)!;
        }

        protected abstract void Dispose(bool disposing);

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}