using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Constants;
using Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;
internal class EmployeeRepository : Repository<int, Employee>, IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext context)
        : base(context) { }

    public async Task<Employee?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Employees.FirstOrDefaultAsync(e => e.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<Employee>?> GetPagedListAsync(int page, int pageSize, string? searchTerm, string? sortColumn, string? sortOrder, CancellationToken cancellationToken = default)
    {
        IQueryable<Employee> query = dbContext.Employees.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(e => (e.FirstName.Contains(searchTerm)) ||

            (e.LastName.Contains(searchTerm)) ||

            (e.Email.Contains(searchTerm))
            );

        }

        if (sortOrder?.ToLower() == OrderBy.Desc)
        {
            query = query.OrderByDescending(GetSortProperty(sortColumn));
        }
        else
        {
            query = query.OrderBy(GetSortProperty(sortColumn));
        }

        query = query.Skip((page - 1) * pageSize)
                     .Take(pageSize);


        return await query.ToListAsync(cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
    {
        return !await dbContext.Employees.AnyAsync(e => e.Email == email, cancellationToken);
    }

    private Expression<Func<Employee, object>> GetSortProperty(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "firstname" => employee => employee.FirstName,
            "lastname" => employee => employee.LastName,
            "email" => employee => employee.Email,
            "salary" => employee => employee.Salary,

            _ => employee => employee.Id
        };
    }
}
