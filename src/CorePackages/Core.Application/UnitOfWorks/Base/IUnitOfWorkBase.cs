using Core.Application.Repositories;
using Core.Domain.Entities;

namespace Core.Application.UnitOfWorks.Base
{
    public interface IUnitOfWorkBase : IDisposable
    {
        IGenericReadRepository<TEntity> ReadRepository<TEntity>() where TEntity : Entity;
        IGenericWriteRepository<TEntity> WriteRepository<TEntity>() where TEntity : Entity;
        public void Complete();
        public Task CompleteAsync(CancellationToken cancellationToken = default);
    }
}
