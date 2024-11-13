using System.Data;

namespace Domain.Abstractions;

public interface IUnitOfWork
{
    IDbTransaction BeginTransaction();
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}