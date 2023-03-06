using Core.Application.Repositories.Base.Writes;
using Core.Domain.Entities;

namespace Core.Application.Repositories
{
    public interface IGenericWriteRepository<TEntity> : IWriteRepository<TEntity>, IAsyncWriteRepository<TEntity> where TEntity : Entity
    {
    }
}
