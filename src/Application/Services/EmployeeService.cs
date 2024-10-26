using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Services;

public sealed class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public EmployeeService(IEmployeeRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request, CancellationToken cancellationToken = default)
    {

        var newEmployee = Employee.Create(request.FirstName,
            request.LastName, request.Email, request.Salary);

        _repository.Insert(newEmployee);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new EmployeeResponse(newEmployee.Id,
            newEmployee.FirstName,
            newEmployee.LastName,
            newEmployee.Email,
            newEmployee.Salary);

    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));

        var existingEmployee = await _repository.GetByIdAsync(id, cancellationToken);

        if (existingEmployee is null)
        {
            throw new Exception($"Employee with '{id}' not found.");
        }

        _repository.Delete(existingEmployee);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<EmployeeResponse?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

        var employee = await _repository.GetByEmailAsync(email, cancellationToken);

        return employee == null ? null :

            new EmployeeResponse(employee.Id, employee.FirstName, employee.LastName,
            employee.Email
            , employee.Salary);
    }


    public async Task<EmployeeResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));

        var employee = await _repository.GetByIdAsync(id, cancellationToken);

        return employee == null ? null :

           new EmployeeResponse(employee.Id, employee.FirstName, employee.LastName,
           employee.Email
           , employee.Salary);
    }

    public async Task<IEnumerable<EmployeeResponse>?> GetEmployeesPagedList(GetEmployeesRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetPagedListAsync(request.Page, request.PageSize,
            request.SearchTerm, request.SortColumn, request.SortOrder, cancellationToken);

        return result?.Select(e => new EmployeeResponse(e.Id,
            e.FirstName,
            e.LastName,
            e.Email,
            e.Salary));
    }

    public async Task UpdateAsync(UpdateEmployeeRequest request, CancellationToken cancellationToken = default)
    {

        var selectedEmployee = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (selectedEmployee is null)
        {
            throw new Exception($"Employee with '{request.Id}'not found.");
        }

        selectedEmployee.Update(request.FirstName,
            request.LastName,
            request.Email,
            request.Salary);

        _repository.Update(selectedEmployee);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }



}
