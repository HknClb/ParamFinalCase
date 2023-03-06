using Core.Domain.Entities;

namespace Core.Application.Repositories.Base.Writes
{
    public interface IAsyncWriteRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task AddRangeAsync(IList<TEntity> entities);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity> DeleteAsync(TEntity entity);
        Task DeleteRangeAsync(IList<TEntity> entities);
        Task<TEntity?> DeleteByIdAsync(string id);
    }
}