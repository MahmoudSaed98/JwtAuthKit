using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Interfaces;

public interface IEmployeeService
{
    public Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request, CancellationToken cancellationToken = default);

    public Task<EmployeeResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    public Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    public Task UpdateAsync(UpdateEmployeeRequest request, CancellationToken cancellationToken = default);

    public Task<EmployeeResponse?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    public Task<IEnumerable<EmployeeResponse>?> GetEmployeesPagedList(GetEmployeesRequest request, CancellationToken cancellationToken
        = default);

}
