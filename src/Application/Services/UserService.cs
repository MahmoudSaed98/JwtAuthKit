using Application.Common;
using Application.Common.Constants;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using Domain.Abstractions;

namespace Application.Services;

public class UserService(IUserRepository userRepository, IUnitOfWork unitOfWork
    , IPasswordHasher passwordHasher, IPermissionManager permissionManager) : IUserService
{
    public async Task<Result<string>> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user == null)
        {
            return Result<string>.Failure($"no user found with email '{request.Email}'");
        }

        if (!passwordHasher.Verify(request.CurrentPassword, user.PasswordHash))
        {
            return Result<string>.Failure(ErrorMessages.PasswordMismatch);
        }

        string newHashPassword = passwordHasher.Hash(request.NewPassword);

        user.SetPassword(newHashPassword);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success(SuccessMessages.PasswordChangedSuccessfully);
    }

    public async Task<Result<string>> DeleteUserAsync(string username, CancellationToken cancellationToken = default)
    {
        var existingUser = await userRepository.GetByUsernameAsync(username, cancellationToken);

        if (existingUser is null)
        {
            return Result<string>.Failure($"user with username '{username}' does not exist.");
        }

        var isAdmin = existingUser.Roles
                    .Any(x => permissionManager.IsAdmin(x.Permissions));

        if (isAdmin)
        {
            return Result<string>.Failure(ErrorMessages.CannotDeleteAdmin);
        }

        userRepository.Update(existingUser);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success($"user with username '{username}' has been deleted.");
    }

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await userRepository.GetAllAsync(cancellationToken);

        if (users is null)
        {
            return Enumerable.Empty<UserResponse>();
        }


        return users.Select(x => new UserResponse(x.Id, x.Username, x.Email,
            x.Roles.Select(r => r.Name).ToList()));

    }

    public async Task<UserResponse?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

        var user = await userRepository.GetByEmailAsync(email, cancellationToken);

        if (user == null)
        {
            return null;
        }

        var roles = user.Roles.Select(x => x.Name).ToList();

        return new UserResponse(user.Id, user.Username, user.Email, roles);
    }

    public async Task<UserResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);

        if (user == null)
        {
            return null;
        }

        var roles = user.Roles.Select(x => x.Name).ToList();

        return new UserResponse(user.Id, user.Username, user.Email, roles);
    }

    public async Task<UserResponse?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByUsernameAsync(username, cancellationToken);

        if (user == null)
        {
            return null;
        }

        var roles = user.Roles.Select(x => x.Name).ToList();

        return new UserResponse(user.Id, user.Username, user.Email, roles);
    }
}