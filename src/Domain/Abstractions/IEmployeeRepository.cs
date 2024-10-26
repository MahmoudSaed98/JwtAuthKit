using Domain.Entities;

namespace Domain.Abstractions;
public interface IEmployeeRepository : IRepository<int, Employee>
{

    Task<Employee?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default);

    Task<IEnumerable<Employee>?> GetPagedListAsync(int page, int pageSize, string? searchTerm,
        string? sortColumn, string? sortOrder,
        CancellationToken cancellationToken = default);
}