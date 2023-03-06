using Core.Domain.Entities;

namespace Core.Application.Repositories.Base.Writes
{
    public interface IWriteRepository<TEntity> where TEntity : Entity
    {
        TEntity Add(TEntity entity);
        void AddRange(IList<TEntity> entities);
        TEntity Update(TEntity entity);
        TEntity Delete(TEntity entity);
        void DeleteRange(IList<TEntity> entities);
        TEntity? DeleteById(string id);
    }
}