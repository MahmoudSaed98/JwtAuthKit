using Application.DTOs.Requests;
using Domain.Abstractions;
using FluentValidation;

namespace Application.Validations;

public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
{
    private readonly IEmployeeRepository _employeeRepository;

    public CreateEmployeeRequestValidator(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;

        RuleFor(e => e.FirstName).NotEmpty();

        RuleFor(e => e.LastName).NotEmpty();

        RuleFor(e => e.Email).NotEmpty();

        RuleFor(e => e.Email).MustAsync(async (email, cancellationToken) =>
        {
            return await _employeeRepository.IsEmailUniqueAsync(email, cancellationToken);

        }).WithMessage("Email must be unique.");
    }
}
