using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using Domain.Abstractions;

namespace Application.Services;

public class UserService(IUserRepository userRepository, IUnitOfWork unitOfWork
    , IPasswordHasher passwordHasher) : IUserService
{
    public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user == null)
        {
            return false;
        }

        if (!passwordHasher.Verify(request.CurrentPassword, user.PasswordHash))
        {
            return false;
        }

        string newHashedPassword = passwordHasher.Hash(request.NewPassword);

        user.SetPassword(newHashedPassword);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<UserResponse?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

        var user = await userRepository.GetByEmailAsync(email, cancellationToken);

        if (user == null)
        {
            return null;
        }

        return new UserResponse(user.Id, user.Username, user.Email);
    }

    public async Task<UserResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);

        if (user == null)
        {
            return null;
        }

        return new UserResponse(user.Id, user.Username, user.Email);
    }

    public async Task<UserResponse?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(username, nameof(username));

        var user = await userRepository.GetByUsernameAsync(username, cancellationToken);

        if (user == null)
        {
            return null;
        }

        return new UserResponse(user.Id, user.Username, user.Email);
    }






}
