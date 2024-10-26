using Domain.Entities;

namespace Domain.Abstractions;

public interface IRepository<TPrimaryKey, TEntity> where TEntity : Entity<TPrimaryKey>
{
    void Delete(TEntity entity);

    Task<TEntity?> GetByIdAsync(TPrimaryKey key, CancellationToken cancellationToken = default);

    void Insert(TEntity entity);

    void Update(TEntity entity);
}