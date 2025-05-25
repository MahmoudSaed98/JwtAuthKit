using Application.DTOs.Requests;
using Domain.Abstractions;
using FluentValidation;

namespace Application.Validations;

internal sealed class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    private readonly IUserRepository _userRepository;

    public RegisterUserRequestValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public RegisterUserRequestValidator()
    {
        RuleFor(request => request.Email)
               .NotEmpty()
               .EmailAddress()
               .MustAsync(_userRepository!.IsEmailUniqueAsync);
    }
}
