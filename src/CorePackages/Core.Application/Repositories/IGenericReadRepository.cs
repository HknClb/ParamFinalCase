using Core.Application.Repositories.Base.Reads;
using Core.Domain.Entities;

namespace Core.Application.Repositories
{
    public interface IGenericReadRepository<TEntity> : IReadRepository<TEntity>, IAsyncReadRepository<TEntity> where TEntity : Entity
    {
    }
}
