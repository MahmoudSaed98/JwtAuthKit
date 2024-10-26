using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Data.DbContexts;

namespace Infrastructure.Repositories;

public class Repository<TPrimaryKey, TEntity> :
    IRepository<TPrimaryKey, TEntity> where TEntity : Entity<TPrimaryKey>
{
    protected ApplicationDbContext dbContext;

    public Repository(ApplicationDbContext context) =>
        dbContext = context;

    public void Delete(TEntity entity)
    {
        dbContext.Set<TEntity>().Remove(entity);
    }

    public async Task<TEntity?> GetByIdAsync(TPrimaryKey key, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<TEntity>().FindAsync(key, cancellationToken);
    }

    public void Insert(TEntity entity)
    {
        dbContext.Set<TEntity>().Add(entity);
    }

    public void Update(TEntity entity)
    {
        dbContext.Set<TEntity>().Update(entity);
    }
}
